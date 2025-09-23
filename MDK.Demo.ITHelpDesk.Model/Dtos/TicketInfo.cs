using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDK.Demo.ITHelpDesk.Model.Dtos
{
    public class TicketInfo
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public TicketStatus Status { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CreatedByUserName { get; set; } = string.Empty;
        public string AssignedToUserName { get; set; } = string.Empty;
        public int CreatedByUserId { get; set; }
        public int? AssignedToUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
