using System.ComponentModel.DataAnnotations;

namespace GymEquipmentTracker.Models;

// Esta clase es un "modelo" (o "entidad"): una clase C# normal que Entity
// Framework Core mapea 1:1 a una tabla de Postgres. No hay SQL escrito a mano
// para crear la tabla: EF Core genera el CREATE TABLE a partir de esta clase
// (ver carpeta Migrations/).
public class Equipment
{
    // Clave primaria por convención: EF Core reconoce una propiedad llamada
    // "Id" (o "<NombreDeClase>Id") como primary key sin configuración extra,
    // e int la mapea como SERIAL/IDENTITY autoincremental en Postgres.
    public int Id { get; set; }

    // [Required] es un "atributo" (data annotation): metadata que decora la
    // propiedad y que tanto EF Core (columna NOT NULL) como la validación de
    // formularios de ASP.NET Core (ModelState.IsValid) leen en tiempo de
    // ejecución. Es parecido a un decorator de Python/TS, pero declarativo:
    // no envuelve la propiedad, solo la etiqueta.
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    // Categoría/tipo de equipo: "Cardio", "Pesas libres", "Máquinas", etc.
    // Se deja como texto libre simple en vez de otro enum porque el gimnasio
    // puede querer agregar categorías nuevas sin tocar código.
    [Required]
    [StringLength(60)]
    public string Category { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Location { get; set; } = string.Empty;

    public EquipmentStatus Status { get; set; } = EquipmentStatus.Operational;

    // Propiedad de navegación: no es una columna propia, es la forma en que
    // EF Core expone "todos los tickets que apuntan a este equipo" cuando se
    // le pide explícitamente con .Include(). La inicializamos como lista
    // vacía para no tener que chequear null en las vistas.
    public List<MaintenanceTicket> MaintenanceTickets { get; set; } = new();
}
