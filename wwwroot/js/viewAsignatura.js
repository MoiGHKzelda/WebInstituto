const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

document.addEventListener("DOMContentLoaded", () => {
    const btnImpartir = document.getElementById("btnImpartir");
    const btnDejarDeImpartir = document.getElementById("btnDejarDeImpartir");
    const btnMatricular = document.getElementById("btnMatricular");

    if (btnImpartir) {
        btnImpartir.addEventListener("click", () => cambiarEstadoImpartir(btnImpartir));
    }

    if (btnDejarDeImpartir) {
        btnDejarDeImpartir.addEventListener("click", () => cambiarEstadoImpartir(btnDejarDeImpartir));
    }

    if (btnMatricular) {
        btnMatricular.addEventListener("click", () => cambiarEstadoMatricular(btnMatricular));
    }
});

//Cambiar estado de impartir
function cambiarEstadoImpartir(boton) {
    const activar = boton.classList.contains("btn-outline-success");
    const asignaturaId = boton.dataset.asignaturaId;

    fetch('/Asignaturas/CambiarEstadoImpartir', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({
            asignaturaId: parseInt(asignaturaId),
            activar: activar,
            usuarioEmail: userEmail
        })
    }).then(res => res.json()).then(() => {
        boton.classList.toggle("btn-outline-success");
        boton.classList.toggle("btn-outline-danger");
        boton.textContent = activar
            ? "Dejar de impartir"
            : "Impartir";
    }).catch(err => {
        console.error("Error:", err);
    });
}

//Cambiar estado de matriculación 
function cambiarEstadoMatricular(boton) {
    const activar = boton.classList.contains("btn-outline-success");
    const asignaturaId = boton.dataset.asignaturaId;

    fetch('/Asignaturas/CambiarEstadoMatricular', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({
            asignaturaId: parseInt(asignaturaId),
            activar: activar,
            usuarioEmail: userEmail
        })
    }).then(res => res.json()).then((data) => {
        if (data.success) {
            boton.classList.toggle("btn-outline-success");
            boton.classList.toggle("btn-outline-danger");
            boton.textContent = activar
                ? "Quitar matrícula"
                : "Matricularse";
        } else {
            alert(data.message || "No se pudo actualizar la matrícula.");
        }
    }).catch(err => {
        console.error("Error:", err);
        alert("Ocurrió un error al procesar tu solicitud.");
    });
}