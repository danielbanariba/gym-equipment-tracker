using GymEquipmentTracker.Data;
using GymEquipmentTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GymEquipmentTracker.Pages.Tickets;

public class IndexModel : PageModel
{
    private readonly GymDbContext _db;

    public IndexModel(GymDbContext db)
    {
        _db = db;
    }

    public List<MaintenanceTicket> TicketList { get; set; } = new();

    public async Task OnGetAsync()
    {
        // Include() trae también la fila de Equipment relacionada en la
        // misma consulta (un JOIN), para poder mostrar el nombre del equipo
        // en la tabla sin disparar una query extra por cada ticket (lo que
        // se conoce como el problema "N+1").
        TicketList = await _db.MaintenanceTickets
            .Include(t => t.Equipment)
            .OrderByDescending(t => t.ReportedAt)
            .ToListAsync();
    }

    // El nombre del método (OnPost + "UpdateStatus") es lo que el Tag
    // Helper asp-page-handler="UpdateStatus" invoca desde el formulario de
    // cada fila. ticketId y newStatus se bindean desde los <input>/<select>
    // del form que tengan esos mismos nombres.
    public async Task<IActionResult> OnPostUpdateStatusAsync(int ticketId, TicketStatus newStatus)
    {
        var ticket = await _db.MaintenanceTickets.FindAsync(ticketId);
        if (ticket is null)
        {
            return NotFound();
        }

        ticket.Status = newStatus;
        ticket.ResolvedAt = newStatus == TicketStatus.Resolved ? DateTime.UtcNow : null;

        await _db.SaveChangesAsync();
        return RedirectToPage();
    }
}
