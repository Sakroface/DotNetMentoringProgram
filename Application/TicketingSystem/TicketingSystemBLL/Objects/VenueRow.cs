namespace TicketingSystemBLL.Objects
{
    public class VenueRow
    {
        /// <summary>
        /// Unique Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the section.
        /// </summary>
        public int SectionId { get; set; }

        /// <summary>
        /// Name of the row.
        /// </summary>
        public string Name { get; set; }
    }
}
