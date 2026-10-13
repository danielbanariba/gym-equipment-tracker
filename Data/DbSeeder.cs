using GymEquipmentTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace GymEquipmentTracker.Data;

// Clase "static": no se instancia nunca con "new", solo se llama su método
// directamente (DbSeeder.SeedAsync(...)). Se usa para sembrar datos de
// ejemplo la primera vez que corre la app contra una base vacía, así la
// demo no arranca con tablas en blanco.
public static class DbSeeder
{
    public static async Task SeedAsync(GymDbContext db)
    {
        // Si ya hay equipo cargado, no se vuelve a sembrar nada: esto hace
        // que SeedAsync se pueda llamar en cada arranque de la app sin
        // duplicar datos en corridas sucesivas.
        if (await db.Equipment.AnyAsync())
        {
            return;
        }

        var equipment = new List<Equipment>
        {
            new() { Name = "Caminadora #1", Category = "Cardio", Location = "Área de cardio", Status = EquipmentStatus.Operational },
            new() { Name = "Caminadora #2", Category = "Cardio", Location = "Área de cardio", Status = EquipmentStatus.Operational },
            new() { Name = "Caminadora #3", Category = "Cardio", Location = "Área de cardio", Status = EquipmentStatus.UnderMaintenance },
            new() { Name = "Bicicleta estática #1", Category = "Cardio", Location = "Área de cardio", Status = EquipmentStatus.Operational },
            new() { Name = "Bicicleta estática #2", Category = "Cardio", Location = "Área de cardio", Status = EquipmentStatus.OutOfService },
            new() { Name = "Elíptica #1", Category = "Cardio", Location = "Área de cardio", Status = EquipmentStatus.Operational },
            new() { Name = "Press de banca", Category = "Pesas libres", Location = "Zona de pesas libres", Status = EquipmentStatus.Operational },
            new() { Name = "Rack de sentadillas", Category = "Pesas libres", Location = "Zona de pesas libres", Status = EquipmentStatus.Operational },
            new() { Name = "Set de mancuernas (5-40 lb)", Category = "Pesas libres", Location = "Zona de pesas libres", Status = EquipmentStatus.Operational },
            new() { Name = "Máquina de poleas (cable crossover)", Category = "Máquinas", Location = "Zona de máquinas", Status = EquipmentStatus.UnderMaintenance },
            new() { Name = "Prensa de piernas", Category = "Máquinas", Location = "Zona de máquinas", Status = EquipmentStatus.Operational },
            new() { Name = "Máquina de remo", Category = "Máquinas", Location = "Zona de máquinas", Status = EquipmentStatus.Operational },
            new() { Name = "Máquina de extensión de piernas", Category = "Máquinas", Location = "Zona de máquinas", Status = EquipmentStatus.OutOfService },
            new() { Name = "Multiestación funcional", Category = "Funcional", Location = "Zona funcional", Status = EquipmentStatus.Operational },
        };

        db.Equipment.AddRange(equipment);

        // Primer SaveChangesAsync: hace falta que EF Core le asigne un Id
        // real (autoincremental en Postgres) a cada Equipment antes de
        // poder usar esos Id como EquipmentId en los tickets de abajo.
        await db.SaveChangesAsync();

        Equipment Find(string name) => equipment.Single(e => e.Name == name);
        var now = DateTime.UtcNow;

        var tickets = new List<MaintenanceTicket>
        {
            new()
            {
                EquipmentId = Find("Caminadora #3").Id,
                ProblemDescription = "La banda hace un ruido fuerte y vibra al superar los 8 km/h.",
                ReportedBy = "Carlos Martínez",
                Status = TicketStatus.InProgress,
                ReportedAt = now.AddDays(-6)
            },
            new()
            {
                EquipmentId = Find("Bicicleta estática #2").Id,
                ProblemDescription = "La pantalla no enciende; probablemente la fuente de poder.",
                ReportedBy = "Recepción",
                Status = TicketStatus.Reported,
                ReportedAt = now.AddDays(-2)
            },
            new()
            {
                EquipmentId = Find("Máquina de poleas (cable crossover)").Id,
                ProblemDescription = "El cable del lado derecho está deshilachado, riesgo de que se rompa.",
                ReportedBy = "Entrenador Josué",
                Status = TicketStatus.InProgress,
                ReportedAt = now.AddDays(-4)
            },
            new()
            {
                EquipmentId = Find("Máquina de extensión de piernas").Id,
                ProblemDescription = "El pin de ajuste de peso está trabado y no se puede cambiar la carga.",
                ReportedBy = "Recepción",
                Status = TicketStatus.Reported,
                ReportedAt = now.AddDays(-1)
            },
            new()
            {
                EquipmentId = Find("Elíptica #1").Id,
                ProblemDescription = "Pedal izquierdo suelto, se mueve de lado al pisar.",
                ReportedBy = "Ana López (miembro)",
                Status = TicketStatus.Resolved,
                ReportedAt = now.AddDays(-10),
                ResolvedAt = now.AddDays(-7)
            },
            new()
            {
                EquipmentId = Find("Rack de sentadillas").Id,
                ProblemDescription = "Falta un seguro en los ganchos de seguridad del lado izquierdo.",
                ReportedBy = "Entrenador Josué",
                Status = TicketStatus.Resolved,
                ReportedAt = now.AddDays(-15),
                ResolvedAt = now.AddDays(-12)
            },
            new()
            {
                EquipmentId = Find("Prensa de piernas").Id,
                ProblemDescription = "Fuga leve de aceite hidráulico en el pistón principal.",
                ReportedBy = "Mantenimiento",
                Status = TicketStatus.Resolved,
                ReportedAt = now.AddDays(-20),
                ResolvedAt = now.AddDays(-18)
            },
        };

        db.MaintenanceTickets.AddRange(tickets);
        await db.SaveChangesAsync();
    }
}
