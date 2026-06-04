// RF04 - Verificación de disponibilidad del lado del cliente.
// Consulta el endpoint de la WebAPI (GET api/Equipo/disponibilidad/{id}) y, si el
// equipo seleccionado no tiene disponibilidad, muestra un mensaje y evita el envío
// del formulario sin recargar la página.
(function () {
    const form = document.getElementById("formPrestamo");
    if (!form) return;

    const apiBase = form.getAttribute("data-api-base");
    const selects = form.querySelectorAll(".chequear-disponibilidad");
    const alerta = document.getElementById("alertaDisponibilidad");
    const btnGuardar = document.getElementById("btnGuardar");

    // Estado de disponibilidad por cada select (true = disponible / sin selección).
    const estado = {};
    selects.forEach(s => estado[s.id] = true);

    function refrescarBoton() {
        const hayNoDisponible = Object.values(estado).some(v => v === false);
        btnGuardar.disabled = hayNoDisponible;
        if (hayNoDisponible) {
            alerta.textContent = "Hay equipos seleccionados sin disponibilidad. Corrija la selección antes de continuar.";
            alerta.classList.remove("d-none");
        } else {
            alerta.classList.add("d-none");
        }
    }

    function mostrarFeedback(select, mensaje, ok) {
        const feedback = document.getElementById(select.getAttribute("data-feedback"));
        if (!feedback) return;
        feedback.textContent = mensaje;
        feedback.classList.remove("text-success", "text-danger");
        feedback.classList.add(ok ? "text-success" : "text-danger");
    }

    function verificar(select) {
        const id = select.value;
        if (!id) {
            estado[select.id] = true;
            mostrarFeedback(select, "", true);
            refrescarBoton();
            return;
        }

        fetch(apiBase + "Equipo/disponibilidad/" + id)
            .then(respuesta => {
                if (!respuesta.ok) throw new Error("No se pudo consultar la disponibilidad.");
                return respuesta.json();
            })
            .then(datos => {
                if (datos.disponible) {
                    estado[select.id] = true;
                    mostrarFeedback(select, "Disponible (stock: " + datos.cantidad + ").", true);
                } else {
                    estado[select.id] = false;
                    mostrarFeedback(select, "Sin disponibilidad para este equipo.", false);
                }
                refrescarBoton();
            })
            .catch(error => {
                estado[select.id] = true;
                mostrarFeedback(select, error.message, false);
                refrescarBoton();
            });
    }

    selects.forEach(s => s.addEventListener("change", () => verificar(s)));

    form.addEventListener("submit", function (evento) {
        const hayNoDisponible = Object.values(estado).some(v => v === false);
        if (hayNoDisponible) {
            evento.preventDefault();
            refrescarBoton();
        }
    });
})();
