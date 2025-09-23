using MDK.Demo.ITHelpDesk.Model.Dtos;
using MDK.Demo.ITHelpDesk.Model.Entities;

namespace MDK.Demo.ITHelpDesk.Service.Data
{
    public interface ITicketOperation
    {
        Task<IEnumerable<TicketInfo>?> GetTickets();
        Task<IEnumerable<TicketInfo>?> GetTicketsFilterBy(TicketStatus? status, int? assignedUserId, int? createdUserId);
        Task<User?> GetUser(int userId);
        Task<int> CreateTicket(TicketInfo ti);
        Task<int> UpdateTicket(TicketInfo ti);
        Task<int> DeleteTicket(int id);

    }
}