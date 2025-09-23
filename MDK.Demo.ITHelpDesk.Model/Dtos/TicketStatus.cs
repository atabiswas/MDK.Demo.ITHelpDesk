using System.ComponentModel;

namespace MDK.Demo.ITHelpDesk.Model.Dtos
{
    public enum TicketStatus
    {
        [Description("Open")]
        Open = 1,
        [Description("In Progress")]
        InProgress = 2,
        [Description("Closed")]
        Closed = 3
    }
}
