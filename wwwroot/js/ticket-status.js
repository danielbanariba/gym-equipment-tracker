// Confirmación en el navegador antes de cerrar (marcar "Resuelto") un
// ticket de mantenimiento. No reemplaza ninguna validación del servidor:
// es solo una red de seguridad para que un clic accidental en "Actualizar"
// no cierre un ticket que seguía abierto.
(function () {
    const forms = document.querySelectorAll(".status-update-form");

    forms.forEach(function (form) {
        form.addEventListener("submit", function (event) {
            const select = form.querySelector("select[name='newStatus']");
            if (!select || select.value !== "Resolved") {
                return;
            }

            const description = form.dataset.ticketDescription || "este ticket";
            const confirmed = window.confirm(
                "¿Confirmás que el problema \"" + description + "\" ya está resuelto?"
            );

            if (!confirmed) {
                event.preventDefault();
            }
        });
    });
})();
