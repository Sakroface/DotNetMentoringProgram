using System.ComponentModel.DataAnnotations;

namespace TicketingSystemDAL.Entities.Base
{
    public class BaseDictionary : BaseEntity
    {
        /// <summary>
        /// Value with the description.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
