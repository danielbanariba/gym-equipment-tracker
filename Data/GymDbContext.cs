using GymEquipmentTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace GymEquipmentTracker.Data;

// Un DbContext es el objeto central de Entity Framework Core: representa
// una sesión contra la base de datos y es la puerta de entrada a las tablas
// mapeadas (cada DbSet<T> es una tabla). Es parecido a una "Session" de
// SQLAlchemy en Python o a un cliente de Prisma/TypeORM en TypeScript: se
// inyecta donde se necesite, se le consulta con LINQ en vez de SQL a mano, y
// SaveChangesAsync() hace el commit real de los cambios pendientes.
public class GymDbContext : DbContext
{
    // El constructor recibe las "options" (incluyendo la connection string)
    // configuradas en Program.cs vía inyección de dependencias — ASP.NET
    // Core resuelve e inyecta esto automáticamente en cada PageModel que lo
    // pida en su propio constructor, sin que el código de la página tenga
    // que crear la instancia a mano.
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
    {
    }

    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<MaintenanceTicket> MaintenanceTickets => Set<MaintenanceTicket>();
}
