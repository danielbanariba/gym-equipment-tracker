# GymEquipmentTracker

## Qué es esto y por qué existe

Tracker interno de equipo y mantenimiento para un gimnasio: lista el equipo
(caminadoras, máquinas, pesas) con su estado, permite reportar un problema
como ticket de mantenimiento, y seguir ese ticket hasta que se resuelve. Hay
un dashboard con conteos simples de ambas cosas.

Se construyó como proyecto de muestra para la vacante de **Analista de
Soporte TI y Desarrollo Aplicativo** de Platinum Gym and Fitness. El aviso
pide C#, .NET, ASP.NET Core, SQL/PostgreSQL, HTML/CSS/JavaScript, Git y
Linux/redes básico — este proyecto toca cada una de esas piezas de forma
real (sin mocks, sin SQLite de reemplazo), sobre un caso de uso que es
literalmente el trabajo del puesto: dar soporte al equipo del gimnasio y
desarrollar una aplicación chica para gestionarlo.

## Cómo correrlo localmente

Requisitos: .NET 8 SDK y PostgreSQL corriendo en `localhost:5432`.

1. Restaurar dependencias:

   ```bash
   dotnet restore
   ```

2. Crear el rol y la base de datos (una sola vez; ajustar la contraseña si
   se quiere otra):

   ```bash
   psql -U postgres -h localhost -c "CREATE ROLE gym_tracker WITH LOGIN PASSWORD 'gym_tracker_dev_local';"
   psql -U postgres -h localhost -c "CREATE DATABASE gym_equipment_tracker OWNER gym_tracker;"
   ```

3. Copiar la plantilla de configuración local y ajustar la contraseña si se
   cambió en el paso anterior:

   ```bash
   cp appsettings.Development.json.example appsettings.Development.json
   ```

4. Aplicar las migraciones de Entity Framework Core (crea las tablas):

   ```bash
   dotnet tool install --global dotnet-ef   # si no está instalado
   dotnet ef database update
   ```

5. Correr el sitio:

   ```bash
   dotnet run
   ```

   La primera vez que levanta, siembra automáticamente 14 equipos y 7
   tickets de ejemplo (ver `Data/DbSeeder.cs`) si la tabla de equipo está
   vacía — no hace falta cargar nada a mano para ver la demo poblada.

   La URL exacta la imprime la consola (perfil `http` de
   `Properties/launchSettings.json`, normalmente `http://localhost:5051`).

## Mapa de arquitectura

- `Program.cs` — arranque de la app: registra Razor Pages, registra el
  `DbContext` contra Postgres, siembra datos de ejemplo y configura el
  pipeline HTTP.
- `Models/Equipment.cs`, `Models/MaintenanceTicket.cs` — las dos entidades
  del dominio; cada clase es una tabla.
- `Models/EquipmentStatus.cs`, `Models/TicketStatus.cs` — los enums de
  estado de cada entidad.
- `Data/GymDbContext.cs` — el objeto de EF Core que representa la conexión
  a la base y expone las tablas como `DbSet<T>`.
- `Data/DbSeeder.cs` — carga el equipo y los tickets de ejemplo si la base
  está vacía.
- `Migrations/` — el historial versionado de cambios al esquema de la base,
  generado por EF Core a partir de los modelos.
- `Pages/Index.cshtml(.cs)` — el dashboard con los conteos.
- `Pages/Equipment/Index.cshtml(.cs)` — listado de equipo, con filtro en
  cliente.
- `Pages/Tickets/Report.cshtml(.cs)` — formulario para crear un ticket.
- `Pages/Tickets/Index.cshtml(.cs)` — listado de tickets y cambio de estado.
- `wwwroot/js/equipment-filter.js` — filtra la tabla de equipo sin recargar
  la página.
- `wwwroot/js/ticket-status.js` — pide confirmación antes de marcar un
  ticket como resuelto.
- `appsettings.Development.json.example` — plantilla de la connection
  string local; la copia real con la contraseña (`appsettings.Development.json`)
  no se versiona.

## Cómo explicar esto en la entrevista

- **Un "modelo"** (`Equipment`, `MaintenanceTicket`) es una clase C# normal
  que representa una fila de una tabla; cada propiedad pública es una
  columna. No hay SQL escrito a mano para crear las tablas.
- **Una "migración"** es un archivo versionado que EF Core genera comparando
  los modelos actuales contra el estado anterior de la base, y que sabe
  aplicar (`dotnet ef database update`) o revertir. Es el mismo concepto que
  una migración de Django/Alembic en Python o de Prisma/TypeORM en Node,
  trasladado a C#.
- **El `DbContext`** (`GymDbContext`) es la puerta de entrada a la base:
  se le pide con LINQ (`_db.Equipment.Where(...)`) en vez de escribir SQL
  suelto, y él lo traduce a SQL real contra Postgres.
- **Una Razor Page** es un archivo `.cshtml` con HTML + código embebido para
  renderizar la vista; su **`PageModel`** (el `.cshtml.cs` al lado) es el
  controlador de esa página puntual: `OnGet` decide qué datos mostrar,
  `OnPost...` qué pasa cuando se envía un formulario. Es más simple que un
  controller de MVC porque cada página tiene su propia clase, sin rutas que
  mapear a mano.
- **`[Required]` y los otros atributos sobre las propiedades** son
  validaciones declarativas: tanto EF Core (para la columna NOT NULL) como
  el framework de validación de formularios las leen en tiempo de
  ejecución, sin si-entonces repetidos en cada handler.
- **La inyección de dependencias** es lo que pone el `DbContext` disponible
  en cada `PageModel` con solo pedirlo en el constructor; `Program.cs` lo
  registra una sola vez con `AddDbContext`.
- **El JavaScript es deliberadamente simple**: filtra una tabla ya
  renderizada por el servidor y confirma antes de una acción destructiva
  (cerrar un ticket). No hay un framework de frontend de por medio — encaja
  con lo que pide el aviso (HTML/CSS/JS, no una SPA).
- **La base es PostgreSQL real**, con su propio rol de aplicación
  (`gym_tracker`) separado del superusuario, y la contraseña de desarrollo
  queda fuera de git (`appsettings.Development.json` está en `.gitignore`;
  se versiona solo la plantilla `.example`).
