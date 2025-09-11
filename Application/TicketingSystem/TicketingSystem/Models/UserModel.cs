using System;

namespace TicketingSystem.Models
{
    public class UserModel
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// First name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Midddle name of the user.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// UserName for the user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password for the user.
        /// </summary>
        public string Password { get; set; }
    }
}
