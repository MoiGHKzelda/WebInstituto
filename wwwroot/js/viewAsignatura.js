let nombreAsignatura = document.getElementById("nombreAsignatura");
let cursoAsignatura = document.getElementById("cursoAsignatura");
let tablaAsignatura = document.querySelectorAll("#tablaAsignatura tr");

let ordenNombre = true;
let ordenCurso = true;

nombreAsignatura.addEventListener("click", () => {
    let datos = [];

    // Obtener las filas y extraer el texto dentro de <a>
    for (let i = 1; i < tablaAsignatura.length; i++) {
        let celdas = tablaAsignatura[i].getElementsByTagName("td");

        let enlaceElemento = celdas[0].querySelector("a"); //Primera celda extraemos el <a> y su contenido
        let textoEnlace = enlaceElemento ? enlaceElemento.textContent.trim() : "";
        let curso = celdas[1].textContent.trim();

        datos.push([textoEnlace, tablaAsignatura[i]]);
    }

    // Si orden es true es ascendente, si es false descendente
    if (ordenNombre) {
        datos.sort((a, b) => a[0].localeCompare(b[0]));
    } else {
        datos.sort((a, b) => b[0].localeCompare(a[0]));
    }
    ordenNombre = !ordenNombre;

    // Reinsertar las filas ordenadas en la tabla
    let tbody = document.getElementById("tablaAsignatura");
    datos.forEach(d => tbody.appendChild(d[1]));
});


cursoAsignatura.addEventListener("click", () => {
    let datos = [];

    // Obtener las filas y almacenar la información td
    for (let i = 1; i < tablaAsignatura.length; i++) {
        let celdas = tablaAsignatura[i].getElementsByTagName("td");

        let enlace = celdas[0].innerHTML.trim();
        let curso = celdas[1].textContent.trim();
        datos.push([enlace, curso, tablaAsignatura[i]]);
    }

    // Si orden es true es ascendente, si es false descendente
    if (ordenCurso) {
        datos.sort((a, b) => a[1].localeCompare(b[1]));
    } else {
        datos.sort((a, b) => b[1].localeCompare(a[1]));
    }
    ordenCurso = !ordenCurso;

    // Reinsertar las filas ordenadas en la tabla
    let tbody = document.getElementById("tablaAsignatura");
    datos.forEach(d => tbody.appendChild(d[2]));
})

