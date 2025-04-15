document.addEventListener("DOMContentLoaded", () => {

    // --- Editar Asignatura ---
    document.querySelectorAll('img[id^="editar-"]').forEach(function (btn) {
        btn.addEventListener('click', function () {
            let id = this.id.split('-')[1];
            window.location.href = `/Asignaturas/FormAsignatura?idAsignatura=${id}`;
        });
    });

    // --- Eliminar Asignatura ---
    document.querySelectorAll('img[id^="eliminar-"]').forEach(function (btn) {
        btn.addEventListener('click', function () {
            let id = this.id.split('-')[1];

            if (confirm("¿Estás seguro de que deseas eliminar esta asignatura?")) {
                fetch(`/Asignaturas/EliminarAsignatura?id=${id}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(response => {
                        if (response.ok) {
                            const fila = btn.closest('tr');
                            fila.remove();
                        } else {
                            alert('Error al eliminar la asignatura.');
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
