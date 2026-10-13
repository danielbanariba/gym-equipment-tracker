namespace GymEquipmentTracker.Models;

// Un "enum" en C# es un conjunto fijo y nombrado de valores enteros.
// Es el equivalente más cercano a un Literal de TypeScript ("Operational" | "UnderMaintenance" | ...)
// o a una clase Enum de Python, pero vive en el sistema de tipos del compilador:
// no se puede asignar un valor fuera de esta lista, ni comparar con un string a secas.
public enum EquipmentStatus
{
    Operational,
    UnderMaintenance,
    OutOfService
}
