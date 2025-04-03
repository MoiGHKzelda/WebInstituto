
let nombreProfesores = document.getElementById("nombreProfesores")
let apellidosProfesores = document.getElementById("apellidosProfesores")
let tablaProfesores = document.querySelectorAll("#tablaProfesores tr");

let ordenNombre = true;
let ordenApellidos = true;
nombreProfesores.addEventListener("click", () => {
    let datos = [];

    // Obtener las filas y almacenar la información td
    for (let i = 1; i < tablaProfesores.length; i++) {
        let celdas = tablaProfesores[i].getElementsByTagName("td");

        let nombre = celdas[0].textContent.trim();
        let apellidos = celdas[1].textContent.trim();
        datos.push([nombre, apellidos, tablaProfesores[i]])
    }

    // Si orden es true es ascendente, si es false descendente
    if (ordenNombre) {
        datos.sort((a, b) => a[0].localeCompare(b[0]));
    } else {
        datos.sort((a, b) => b[0].localeCompare(a[0]));
    }
    ordenNombre = !ordenNombre;

    // Reinsertar las filas ordenadas en la tabla
    let tbody = document.getElementById("tablaProfesores");
    datos.forEach(d => tbody.appendChild(d[2]));
})

apellidosProfesores.addEventListener("click", () => {
    let datos = [];

    // Obtener las filas y almacenar la información td
    for (let i = 1; i < tablaProfesores.length; i++) {
        let celdas = tablaProfesores[i].getElementsByTagName("td");

        let nombre = celdas[0].textContent.trim();
        let apellidos = celdas[1].textContent.trim();
        datos.push([nombre, apellidos, tablaProfesores[i]]);

    }

    // Si orden es true es ascendente, si es false descendente
    if (ordenApellidos) {
        datos.sort((a, b) => a[1].localeCompare(b[1]));
    } else {
        datos.sort((a, b) => b[1].localeCompare(a[1]));
    }
    ordenApellidos = !ordenApellidos;

    // Reinsertar las filas ordenadas en la tabla
    let tbody = document.getElementById("tablaProfesores");
    datos.forEach(d => tbody.appendChild(d[2]));
})