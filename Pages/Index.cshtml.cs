using GymEquipmentTracker.Data;
using GymEquipmentTracker.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GymEquipmentTracker.Pages;

public class IndexModel : PageModel
{
    private readonly GymDbContext _db;

    public IndexModel(GymDbContext db)
    {
        _db = db;
    }

    public int OperationalCount { get; set; }
    public int UnderMaintenanceCount { get; set; }
    public int OutOfServiceCount { get; set; }

    public int OpenTicketCount { get; set; }
    public int ResolvedTicketCount { get; set; }

    public async Task OnGetAsync()
    {
        // CountAsync traduce cada llamada a un "SELECT COUNT(*) ... WHERE"
        // ejecutado en Postgres — no se traen todas las filas a memoria
        // para contarlas en C#.
        OperationalCount = await _db.Equipment.CountAsync(e => e.Status == EquipmentStatus.Operational);
        UnderMaintenanceCount = await _db.Equipment.CountAsync(e => e.Status == EquipmentStatus.UnderMaintenance);
        OutOfServiceCount = await _db.Equipment.CountAsync(e => e.Status == EquipmentStatus.OutOfService);

        OpenTicketCount = await _db.MaintenanceTickets.CountAsync(t => t.Status != TicketStatus.Resolved);
        ResolvedTicketCount = await _db.MaintenanceTickets.CountAsync(t => t.Status == TicketStatus.Resolved);
    }
}
