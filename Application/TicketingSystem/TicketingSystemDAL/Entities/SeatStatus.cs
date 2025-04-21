using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystemDAL.Entities
{
    public class SeatStatus : BaseEntity
    {
        /// <summary>
        /// Constructor by default.
        /// </summary>
        public SeatStatus()
        {
            VenueSeats = new HashSet<VenueSeat>();
        }

        /// <summary>
        /// Nameof the seat type.
        /// </summary>
        [Required]
        public string Name { get; set; }

        #region Navigational properties for EF

        public virtual ICollection<VenueSeat> VenueSeats { get; set; }

        #endregion
    }
}
