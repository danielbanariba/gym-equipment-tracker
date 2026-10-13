using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymEquipmentTracker.Models;

public class MaintenanceTicket
{
    public int Id { get; set; }

    // Clave foránea explícita hacia Equipment. EF Core también acepta que
    // solo exista la propiedad de navegación "Equipment", pero declarar el
    // Id aparte hace mucho más simple el formulario (un <select> que postea
    // un entero) y el filtro por equipo en las páginas.
    [Required]
    public int EquipmentId { get; set; }

    // Propiedad de navegación hacia el equipo relacionado. [ForeignKey] le
    // dice a EF Core con qué columna emparejarla; sin la anotación también
    // la infiere por convención de nombres, pero se deja explícita por
    // claridad para quien lea el modelo sin conocer EF Core.
    [ForeignKey(nameof(EquipmentId))]
    public Equipment? Equipment { get; set; }

    [Required]
    [StringLength(500)]
    public string ProblemDescription { get; set; } = string.Empty;

    public TicketStatus Status { get; set; } = TicketStatus.Reported;

    // Quién reportó el problema: texto simple, sin autenticación. El aviso
    // del gimnasio no pide login de usuarios, así que no se modela como FK
    // a una tabla de empleados que no existe en este alcance.
    [Required]
    [StringLength(80)]
    public string ReportedBy { get; set; } = string.Empty;

    public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

    // Nullable: un ticket recién reportado no tiene fecha de resolución
    // todavía. El "?" después del tipo (DateTime?) es exactamente el mismo
    // concepto que Optional[datetime] en Python o "Date | null" en TS.
    public DateTime? ResolvedAt { get; set; }
}
