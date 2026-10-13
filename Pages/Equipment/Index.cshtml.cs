using GymEquipmentTracker.Data;
using GymEquipmentTracker.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GymEquipmentTracker.Pages.Equipment;

// En Razor Pages cada página ".cshtml" tiene una clase "PageModel" asociada
// (este archivo ".cshtml.cs") que hace las veces de controlador para esa
// sola página: resuelve qué datos necesita la vista (OnGet) y qué pasa
// cuando se envía un formulario (OnPost...). Es el equivalente a un
// controller + una vista de MVC, pero atado 1:1 a una página en vez de a
// una ruta genérica.
public class IndexModel : PageModel
{
    private readonly GymDbContext _db;

    public IndexModel(GymDbContext db)
    {
        _db = db;
    }

    // Las vistas Razor leen propiedades públicas del PageModel con
    // "@Model.NombreDeLaPropiedad". No hace falta devolver nada desde
    // OnGet: basta con dejar el dato en esta propiedad antes de que la
    // página se renderice.
    public List<Models.Equipment> EquipmentList { get; set; } = new();

    public async Task OnGetAsync()
    {
        EquipmentList = await _db.Equipment
            .OrderBy(e => e.Category)
            .ThenBy(e => e.Name)
            .ToListAsync();
    }
}
