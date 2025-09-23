using Dapper;
using MDK.Demo.ITHelpDesk.Model.Dtos;
using MDK.Demo.ITHelpDesk.Model.Entities;
using System.Collections.Generic;
using System.Diagnostics;

namespace MDK.Demo.ITHelpDesk.Service.Data
{
    public class TicketOperation(IDBConnection connection) : ITicketOperation
    {
        private readonly IDBConnection _connection = connection;

        public async Task<IEnumerable<TicketInfo>?> GetTickets()
        {
            using var conn = _connection.GetConnection();

            var sql = "SELECT * FROM Ticket";
            
            var tickets = await conn.QueryAsync<Ticket>(sql);

            if (tickets == null)
            {
                return null;
            }

            var ticketInfos = new List<TicketInfo>();
            foreach (var ticket in tickets)
            {
                var ticketInfo = new TicketInfo()
                {
                    AssignedToUserId = ticket.AssignedUserId,
                    CreatedByUserId = ticket.CreatedUserId,
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    CreatedAt = ticket.CreatedAt,
                };

                var userAssigned = await GetUser(ticket.AssignedUserId ?? 0);
                if (userAssigned != null)
                {
                    ticketInfo.AssignedToUserName = userAssigned.FirstName + " " + userAssigned.LastName;
                }

                var userCreated = await GetUser(ticket.CreatedUserId);
                if (userCreated != null)
                {
                    ticketInfo.CreatedByUserName = userCreated.FirstName + " " + userCreated.LastName;
                }

                ticketInfo.Status = (TicketStatus)ticket.StatusId;
                ticketInfos.Add(ticketInfo);
            }
            return ticketInfos;
        }

        public async Task<IEnumerable<TicketInfo>?> GetTicketsFilterBy(TicketStatus? status, int? assignedUserId, int? createdUserId)
        {
            using var conn = _connection.GetConnection();

            var sql = "SELECT * FROM Ticket WHERE 1=1";
            var parameters = new DynamicParameters();

            if (status != null)
            {
                sql += " AND StatusId=@statusId";
                parameters.Add("statusId", (short)status);
            }

            if (assignedUserId != null)
            {
                sql += " AND AssignedUserId=@assignedUserId";
                parameters.Add("assignedUserId", assignedUserId);
            }

            if (createdUserId != null)
            {
                sql += " AND CreatedUserId=@createdUserId";
                parameters.Add("createdUserId", createdUserId);
            }

            var tickets = await conn.QueryAsync<Ticket>(sql,parameters);

            if (tickets == null)
            {
                return null;
            }

            var ticketInfos = new List<TicketInfo>();
            foreach (var ticket in tickets)
            {
                var ticketInfo = new TicketInfo()
                {
                    AssignedToUserId = ticket.AssignedUserId,
                    CreatedByUserId = ticket.CreatedUserId,
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Description = ticket.Description,
                    CreatedAt = ticket.CreatedAt,
                };

                var userAssigned = await GetUser(ticket.AssignedUserId?? 0);
                if (userAssigned != null)
                {
                    ticketInfo.AssignedToUserName = userAssigned.FirstName + " " + userAssigned.LastName;
                }

                var userCreated = await GetUser(ticket.CreatedUserId);
                if (userCreated != null)
                {
                    ticketInfo.CreatedByUserName = userCreated.FirstName + " " + userCreated.LastName;
                }

                ticketInfo.Status = (TicketStatus)ticket.StatusId;
                ticketInfos.Add(ticketInfo);
            }
            return ticketInfos;
        }

        public async Task<User?> GetUser(int userId)
        {
            using var conn = _connection.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM User WHERE Id = @Id", new { Id = userId });
        }

        public async Task<IEnumerable<UserInfo>?> GetUsers()
        {
            using var conn = _connection.GetConnection();

            var sql = "SELECT * FROM User";
            var entities = await conn.QueryAsync<User>(sql);

            if (entities == null) return null;

            var userInfos = new List<UserInfo>();
            foreach(var entity in entities)
            {
                var userInfo = new UserInfo()
                {
                    Email = entity.Email,
                    Name = entity.FirstName + " " + entity.LastName,
                    Id = entity.Id
                };
                userInfos.Add(userInfo);
            }
            return userInfos;
        }

        public async Task<int> CreateTicket(TicketInfo ti)
        {
            using var conn = _connection.GetConnection();

            var ticket = new Ticket()
            {
                AssignedUserId = ti.AssignedToUserId != null ? (short)ti.AssignedToUserId : null,
                CreatedAt = ti.CreatedAt,
                CreatedUserId = (short)ti.CreatedByUserId,
                Description = ti.Description,
                StatusId = (short)ti.Status,
                Title = ti.Title
            };
            var sql = "INSERT INTO Ticket(Title, Description, StatusId, AssignedUserId, CreatedUserId, CreatedAt) VALUES(@Title, @Description, @StatusId, @AssignedUserId, @CreatedUserId, @CreatedAt); SELECT CAST(SCOPE_IDENTITY() as int)";
            return await conn.ExecuteScalarAsync<int>(sql, ticket);
        }

        public async Task<int> UpdateTicket(TicketInfo ti)
        {
            using var conn = _connection.GetConnection();

            var ticket = await conn.QueryFirstOrDefaultAsync<Ticket>("SELECT * FROM Ticket WHERE Id=@Id", new { ti.Id });
            if (ticket != null)
            {
                ticket.Title = ti.Title;
                ticket.Description = ti.Description;
                ticket.CreatedUserId = (short)ti.CreatedByUserId;
                ticket.StatusId = (short)ti.Status;
                ticket.AssignedUserId = ti.AssignedToUserId != null ? (short)ti.AssignedToUserId : null;
                ticket.ModifiedAt = DateTime.Now;
            }

            var sql = "UPDATE SET Title=@Title, Description=@Description, CreatedUserId=@CreatedUserId, AssignedUserId=@AssignedUserId, StatusId=@StatusId, ModifiedAt=@ModifiedAt WHERE Id=@Id";

            return await conn.ExecuteAsync(sql, ticket);
        }

        public async Task<int> DeleteTicket(int id)
        {
            using var conn = _connection.GetConnection(); ;
            return await conn.ExecuteAsync("DELETE FROM Ticket WHERE Id=@Id", new { Id = id });
        }
    }
}
