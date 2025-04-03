using Microsoft.AspNetCore.Mvc.Rendering;

public class CrearAsignaturasViewModel
{
    public string Name { get; set; }
    public List<SelectListItem> Cursos { get; set; }
}