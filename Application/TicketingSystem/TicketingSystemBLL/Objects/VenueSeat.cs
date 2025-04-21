using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystemBLL.Objects
{
    public class VenueSeat
    {
        /// <summary>
        /// Id of the row it belongs to.
        /// </summary>
        public int RowId { get; set; }

        /// <summary>
        /// Seat number in the row.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Type of the seat.
        /// </summary>
        public int SeatsTypeId { get; set; }
    }
}
