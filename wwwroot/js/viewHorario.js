let tablaHorario = document.querySelectorAll("#tablaHorario tr");
let diaHorario = document.getElementById("diaHorario");
let inicioHorario = document.getElementById("inicioHorario");
let finHorario = document.getElementById("finHorario");

let ordenDia = true;
let ordenInicio = true;
let ordenFin = true;

diaHorario.addEventListener("click", () => {
    let datos = [];
    const diasOrden = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"];

    // Obtener las filas y almacenar la información
    for (let i = 1; i < tablaHorario.length; i++) {
        let celdas = tablaHorario[i].getElementsByTagName("td");
        let dia = celdas[0].textContent.trim();
        datos.push([dia, tablaHorario[i]]);
    }

    // Ordenar según el array de días de la semana
    datos.sort((a, b) => {
        return ordenDia
            ? diasOrden.indexOf(a[0]) - diasOrden.indexOf(b[0])
            : diasOrden.indexOf(b[0]) - diasOrden.indexOf(a[0]);
    });

    ordenDia = !ordenDia;

    // Reinsertar las filas ordenadas en la tabla
    let tbody = document.getElementById("tablaHorario");
    datos.forEach(d => tbody.appendChild(d[1]));
});


inicioHorario.addEventListener("click", () => {
    let datos = [];


    // Obtener las filas y almacenar la información td
    for (let i = 1; i < tablaHorario.length; i++) {
        let celdas = tablaHorario[i].getElementsByTagName("td");
        let dia = celdas[0].textContent.trim();
        let inicio = celdas[1].textContent.trim();
        let fin = celdas[2].textContent.trim();
        datos.push([dia, inicio, fin, tablaHorario[i]])
    }

    // Si orden es true es ascendente, si es false descendente
    if (ordenInicio) {
        datos.sort((a, b) => a[1].localeCompare(b[1]));
    } else {
        datos.sort((a, b) => b[1].localeCompare(a[1]));
    }
    ordenInicio = !ordenInicio;

    // Reinsertar las filas ordenadas en la tabla
    let tbody = document.getElementById("tablaHorario");
    datos.forEach(d => tbody.appendChild(d[3]));

})

finHorario.addEventListener("click", () => {
    let datos = [];


    // Obtener las filas y almacenar la información td
    for (let i = 1; i < tablaHorario.length; i++) {
        let celdas = tablaHorario[i].getElementsByTagName("td");
        let dia = celdas[0].textContent.trim();
        let inicio = celdas[1].textContent.trim();
        let fin = celdas[2].textContent.trim();
        datos.push([dia, inicio, fin, tablaHorario[i]])
    }

    // Si orden es true es ascendente, si es false descendente
    if (ordenFin) {
        datos.sort((a, b) => a[2].localeCompare(b[2]));
    } else {
        datos.sort((a, b) => b[2].localeCompare(a[2]));
    }
    ordenFin = !ordenFin;

    // Reinsertar las filas ordenadas en la tabla
    let tbody = document.getElementById("tablaHorario");
    datos.forEach(d => tbody.appendChild(d[3]));

})