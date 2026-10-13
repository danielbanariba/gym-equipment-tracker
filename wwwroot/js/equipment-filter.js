// Filtro de la tabla de equipo, 100% en el cliente: no hace ningún request
// nuevo al servidor, solo muestra/oculta filas del <tbody> ya renderizado
// por el PageModel. Es intencionalmente JS "de toda la vida" (sin
// framework), igual que el resto del proyecto.
(function () {
    const searchInput = document.getElementById("equipment-search");
    const statusFilter = document.getElementById("equipment-status-filter");
    const table = document.getElementById("equipment-table");
    const emptyMessage = document.getElementById("equipment-empty-message");

    if (!searchInput || !statusFilter || !table) {
        return;
    }

    const rows = Array.from(table.querySelectorAll("tbody tr"));

    function applyFilters() {
        const searchTerm = searchInput.value.trim().toLowerCase();
        const statusTerm = statusFilter.value;
        let visibleCount = 0;

        rows.forEach(function (row) {
            const rowText = row.textContent.toLowerCase();
            const matchesSearch = searchTerm === "" || rowText.includes(searchTerm);
            const matchesStatus = statusTerm === "" || row.dataset.status === statusTerm;
            const isVisible = matchesSearch && matchesStatus;

            row.classList.toggle("d-none", !isVisible);
            if (isVisible) {
                visibleCount += 1;
            }
        });

        if (emptyMessage) {
            emptyMessage.classList.toggle("d-none", visibleCount > 0);
        }
    }

    searchInput.addEventListener("input", applyFilters);
    statusFilter.addEventListener("change", applyFilters);
})();
