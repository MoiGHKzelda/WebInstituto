namespace WebInstituto.ViewModels.Horarios
{
    public class HorariosViewModel
    {
        public IList<HorarioViewModel> Horarios { get; set; }
        public int AsignaturaId { get; set; }
        public string PageTitle { get; set; }
    }
}
