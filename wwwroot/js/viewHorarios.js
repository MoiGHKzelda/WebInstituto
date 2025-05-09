﻿document.addEventListener("DOMContentLoaded", () => {

    // --- Editar Horario ---
    document.querySelectorAll('img[id^="editar-h-"]').forEach(function (btn) {
        btn.addEventListener('click', function () {
            let parts = this.id.split('-'); 
            let horarioId = parts[2];
            let asignaturaId = this.dataset.asignaturaId;
            window.location.href = `/Horarios/FormularioCrearHorario?asignaturaId=${asignaturaId}&horarioId=${horarioId}`;
        });
    });
    // --- Eliminar Horario ---
    document.querySelectorAll('img[id^="eliminar-h-"]').forEach(function (btn) {
        btn.addEventListener('click', function (e) {
            e.preventDefault();

            let horarioId = this.id.split('-')[2];
            let asignaturaId = this.closest('a').href.split('asignaturaId=')[1].split('&')[0];

            if (confirm("¿Estás seguro de que deseas eliminar este horario?")) {
                let url = `/Horarios/EliminarHorario?asignaturaId=${asignaturaId}&horarioId=${horarioId}`;

                fetch(url, {
                    method: 'GET'
                })
                    .then(response => {
                        if (response.ok) {
                            window.location.href = `/Asignaturas/VistaAsignatura/${asignaturaId}`;
                        } else {
                            alert('Error al eliminar el horario.');
                        }
                    })
                    .catch(err => {
                        console.error('Error en la solicitud:', err);
                        alert('Error al intentar eliminar.');
                    });
            }
        });
    });
});
