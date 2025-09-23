using MDK.Demo.ITHelpDesk.Model.Dtos;
using MDK.Demo.ITHelpDesk.Service.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MDK.Demo.ITHelpDesk.Service.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelpDeskController : ControllerBase
    {
        private readonly ILogger<HelpDeskController> _logger;
        private readonly ITicketOperation _operation;

        public HelpDeskController(ILogger<HelpDeskController> logger, ITicketOperation operation)
        {
            _logger = logger;
            _operation = operation;
        }

        [HttpGet("Ticket/All")]
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _operation.GetTickets();
            return Ok(tickets);
        }

        [HttpGet("Ticket/FilterBy")]
        public async Task<IActionResult> GetTickets([FromQuery] TicketStatus? status, [FromQuery] int? assignedUserId, [FromQuery] int? createdUserId)
        {
            var tickets = await _operation.GetTicketsFilterBy(status, assignedUserId, createdUserId);
            return Ok(tickets);
        }

        [HttpPost("Ticket/Create")]
        public async Task<IActionResult> CreateTicket([FromBody] TicketInfo ticketInfo)
        {
            var ticketId = await _operation.CreateTicket(ticketInfo);
            return Ok(ticketId);
        }

        [HttpPut("Ticket/Update/{id:int}")]
        public async Task<IActionResult> UpdateTicket([FromBody] TicketInfo ticketInfo, [FromRoute] int id)
        {
            if (id != ticketInfo.Id) return BadRequest();

            var rows = await _operation.UpdateTicket(ticketInfo);
            if (rows == 0) return NotFound();

            return Ok(rows);
        }

        [HttpPut("Ticket/Assign/{userId:int}")]
        public async Task<IActionResult> AssignTicket([FromBody] TicketInfo ticketInfo, [FromRoute] int userId)
        {
            ticketInfo.AssignedToUserId = userId;

            var rows = await _operation.UpdateTicket(ticketInfo);
            if (rows == 0) return NotFound();

            return Ok(rows);
        }

        [HttpDelete("Ticket/Delete/{id:int}")]
        public async Task<IActionResult> DeleteTicket([FromRoute] int id)
        {
            var rows = await _operation.DeleteTicket(id);
            if (rows == 0) return NotFound();

            return Ok(rows);
        }
    }
}
