﻿using Microsoft.AspNetCore.Mvc.Rendering;
using WebInstituto.Models;

namespace WebInstituto.ViewModels.Asignaturas
{
    public class CrearEditarAsignaturasViewModel
    {
        public Asignatura Asignatura { get; set; }

        public List<SelectListItem> Cursos { get; set; }

        public bool Editar { get; set; }

        public CrearEditarAsignaturasViewModel()
        {
            // Inicializar por seguridad para evitar null reference
            Asignatura = new Asignatura();
            Cursos = new List<SelectListItem>();
        }
    }
}
