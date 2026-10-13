using System.ComponentModel.DataAnnotations;
using GymEquipmentTracker.Data;
using GymEquipmentTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GymEquipmentTracker.Pages.Tickets;

public class ReportModel : PageModel
{
    private readonly GymDbContext _db;

    public ReportModel(GymDbContext db)
    {
        _db = db;
    }

    // [BindProperty] le dice a Razor Pages que, en un POST, llene esta
    // propiedad automáticamente a partir de los campos del formulario que
    // comparten nombre con sus propiedades (model binding). Se usa un tipo
    // de entrada propio (ReportTicketInput) en vez de bindear directo
    // contra la entidad MaintenanceTicket para no exponer campos como
    // Status o ResolvedAt a lo que el usuario puede enviar desde el form
    // ("overposting") — esos los decide el servidor, no el cliente.
    [BindProperty]
    public ReportTicketInput NewTicket { get; set; } = new();

    public List<SelectListItem> EquipmentOptions { get; set; } = new();

    public async Task OnGetAsync()
    {
        await LoadEquipmentOptionsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // ModelState.IsValid evalúa todas las [Required]/[StringLength] de
        // ReportTicketInput. Si algo falla, se vuelve a mostrar la misma
        // página con los mensajes de validación, sin tocar la base.
        if (!ModelState.IsValid)
        {
            await LoadEquipmentOptionsAsync();
            return Page();
        }

        var ticket = new MaintenanceTicket
        {
            EquipmentId = NewTicket.EquipmentId,
            ProblemDescription = NewTicket.ProblemDescription,
            ReportedBy = NewTicket.ReportedBy,
            Status = TicketStatus.Reported,
            ReportedAt = DateTime.UtcNow
        };

        _db.MaintenanceTickets.Add(ticket);
        await _db.SaveChangesAsync();

        // TempData sobrevive exactamente un redirect: es la forma estándar
        // en Razor Pages de mostrar un mensaje de éxito en la página de
        // destino después de un POST-redirect-GET.
        TempData["SuccessMessage"] = "Ticket reportado correctamente.";
        return RedirectToPage("/Tickets/Index");
    }

    private async Task LoadEquipmentOptionsAsync()
    {
        EquipmentOptions = await _db.Equipment
            .OrderBy(e => e.Name)
            .Select(e => new SelectListItem(e.Name, e.Id.ToString()))
            .ToListAsync();
    }
}

public class ReportTicketInput
{
    [Required(ErrorMessage = "Elegí el equipo con el problema.")]
    public int EquipmentId { get; set; }

    [Required(ErrorMessage = "Describí el problema.")]
    [StringLength(500)]
    public string ProblemDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "Decinos quién lo reporta.")]
    [StringLength(80)]
    public string ReportedBy { get; set; } = string.Empty;
}
