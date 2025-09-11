using System;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystemDAL.Entities.Base
{
    public class BaseGuidEntity
    {
        /// <summary>
        /// Unique identifier for the entity.
        /// </summary>
        [Key]
        public Guid Id { get; set; }
    }
}
