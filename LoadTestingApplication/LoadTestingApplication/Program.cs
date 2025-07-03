using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace LoadTestingApplication
{
    class Program
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string baseUrl = "https://localhost:44336"; // Replace with your API URL
        private static readonly object lockObject = new object();
        private static List<TestResult> results = new List<TestResult>();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Cart Booking Concurrent Test Started");
            Console.WriteLine("=====================================");

            // Configure HttpClient
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            // Test parameters
            const int numberOfRequests = 10;
            const int eventId = 2; // Replace with actual event ID
            const int seatId = 13; // Replace with actual seat ID that should be booked
            Guid priceId = new Guid("A8EE3779-6A79-436D-1B93-08DDB7FA304C");

            var stopwatch = Stopwatch.StartNew();

            // Create tasks for parallel execution
            var tasks = new List<Task>();

            for (int i = 0; i < numberOfRequests; i++)
            {
                int requestId = i + 1;
                tasks.Add(BookSeatAsync(requestId, eventId, seatId, priceId));
            }

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);

            stopwatch.Stop();

            // Analyze results
            AnalyzeResults(stopwatch.Elapsed);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static async Task BookSeatAsync(int requestId, int eventId, int seatId, Guid priceId)
        {
            var result = new TestResult
            {
                RequestId = requestId,
                StartTime = DateTime.UtcNow
            };

            try
            {
                // First create a cart
                var cartId = new Guid("3c2cbcac-7077-4d19-be66-b3ab3c2c1745");

                if (cartId != Guid.Empty)
                {
                    // Then try to add the seat to the cart
                    var success = await AddSeatToCartAsync(requestId, cartId, seatId, eventId, priceId);

                    result.Success = success;
                    result.CartId = cartId;
                    result.StatusCode = success ? 200 : 400;
                }
                else
                {
                    result.Success = false;
                    result.StatusCode = 500;
                    result.ErrorMessage = "Failed to create cart";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.StatusCode = 500;
            }
            finally
            {
                result.EndTime = DateTime.UtcNow;
                result.Duration = result.EndTime - result.StartTime;

                lock (lockObject)
                {
                    results.Add(result);

                    // Log progress every 100 requests
                    if (results.Count % 100 == 0)
                    {
                        Console.WriteLine($"Completed {results.Count}/1000 requests...");
                    }
                }
            }
        }

        private static async Task<Guid> CreateCartAsync(int requestId, int eventId, Guid priceId)
        {
            try
            {
                var cartData = new
                {
                    eventId = eventId,
                    statusId = 1, // Assuming 1 is "Created" status
                    priceId = priceId, 
                    userId = new Guid("4F7D55E3-C404-4DFB-1EFD-08DDB7FA2FB8"), 
                    amount = 50.00m
                };

                var json = JsonSerializer.Serialize(cartData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{baseUrl}/orders/carts", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response to extract cartId
                    using var document = JsonDocument.Parse(responseContent);
                    if (document.RootElement.TryGetProperty("cartId", out var cartIdElement))
                    {
                        if (Guid.TryParse(cartIdElement.GetString(), out Guid cartId))
                        {
                            Console.WriteLine($"[{requestId:D4}] Cart created: {cartId}");
                            return cartId;
                        }
                    }
                }

                Console.WriteLine($"[{requestId:D4}] Failed to create cart: {response.StatusCode}");
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[{requestId:D4}] Error details: {errorContent}");
                return Guid.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{requestId:D4}] Exception creating cart: {ex.Message}");
                return Guid.Empty;
            }
        }


        private static async Task<bool> AddSeatToCartAsync(int requestId, Guid cartId, int seatId, int eventId, Guid priceId)
        {
            try
            {
                var seatData = new
                {
                    seatId = seatId,
                    eventId = eventId,
                    priceId = priceId
                };

                var json = JsonSerializer.Serialize(seatData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{baseUrl}/orders/carts/{cartId}", content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"[{requestId:D4}] ✅ SUCCESS - Seat booked in cart {cartId}");
                    return true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotModified)
                {
                    Console.WriteLine($"[{requestId:D4}] = Not Modified - Seat is already booked in cart {cartId}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"[{requestId:D4}] ❌ FAILED - {response.StatusCode}: {responseContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{requestId:D4}] ❌ EXCEPTION - {ex.Message}");
                return false;
            }
        }

        private static void AnalyzeResults(TimeSpan totalTime)
        {
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("TEST RESULTS ANALYSIS");
            Console.WriteLine(new string('=', 50));

            var successfulRequests = results.Where(r => r.Success).ToList();
            var failedRequests = results.Where(r => !r.Success).ToList();

            Console.WriteLine($"Total Requests: {results.Count}");
            Console.WriteLine($"Successful Requests: {successfulRequests.Count}");
            Console.WriteLine($"Failed Requests: {failedRequests.Count}");
            Console.WriteLine($"Total Execution Time: {totalTime.TotalSeconds:F2} seconds");
            Console.WriteLine($"Average Request Duration: {results.Average(r => r.Duration.TotalMilliseconds):F2} ms");

            // Critical Analysis
            Console.WriteLine("\n" + new string('-', 30));
            Console.WriteLine("RACE CONDITION ANALYSIS");
            Console.WriteLine(new string('-', 30));

            if (successfulRequests.Count > 1)
            {
                Console.WriteLine($"🚨 RACE CONDITION DETECTED!");
                Console.WriteLine($"   Expected: 1 successful booking");
                Console.WriteLine($"   Actual: {successfulRequests.Count} successful bookings");
                Console.WriteLine($"   This indicates a concurrency issue in your booking logic!");

                Console.WriteLine("\nSuccessful bookings timeline:");
                foreach (var success in successfulRequests.OrderBy(r => r.StartTime))
                {
                    Console.WriteLine($"   Request {success.RequestId}: {success.StartTime:HH:mm:ss.fff} - Cart: {success.CartId}");
                }

                Console.WriteLine("\n📋 DEBUGGING RECOMMENDATIONS:");
                Console.WriteLine("1. Add database-level constraints (unique indexes)");
                Console.WriteLine("2. Implement optimistic locking with version fields");
                Console.WriteLine("3. Use database transactions with proper isolation levels");
                Console.WriteLine("4. Add distributed locking (Redis, etc.) for seat booking");
                Console.WriteLine("5. Implement idempotency checks");
            }
            else if (successfulRequests.Count == 1)
            {
                Console.WriteLine("✅ GOOD: Only 1 successful booking detected");
                Console.WriteLine("   Your concurrency control is working correctly!");
            }
            else
            {
                Console.WriteLine("⚠️  WARNING: No successful bookings");
                Console.WriteLine("   Check your API endpoint and test data");
            }

            // Error analysis
            if (failedRequests.Any())
            {
                Console.WriteLine("\nCommon Error Messages:");
                var errorGroups = failedRequests
                    .GroupBy(r => r.ErrorMessage ?? "Unknown")
                    .OrderByDescending(g => g.Count());

                foreach (var group in errorGroups.Take(5))
                {
                    Console.WriteLine($"   {group.Key}: {group.Count()} occurrences");
                }
            }

            // Performance metrics
            Console.WriteLine($"\nPerformance Metrics:");
            Console.WriteLine($"   Fastest Request: {results.Min(r => r.Duration.TotalMilliseconds):F2} ms");
            Console.WriteLine($"   Slowest Request: {results.Max(r => r.Duration.TotalMilliseconds):F2} ms");
            Console.WriteLine($"   Requests per second: {results.Count / totalTime.TotalSeconds:F2}");
        }
    }

    public class TestResult
    {
        public int RequestId { get; set; }
        public bool Success { get; set; }
        public Guid CartId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
