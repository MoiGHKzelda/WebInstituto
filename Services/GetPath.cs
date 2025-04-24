namespace Proyecto.Services
{
    public static class GetPath
    {
        public static string GetDatabasePath()
        {
            var projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                                       ?.Parent?.Parent?.Parent?.FullName;

            var path = Path.Combine(projectRoot, "BBDD", "institutoEF.sqlite");

            return path ?? throw new Exception("No se pudo determinar la ruta de la base de datos.");
        }
    }
}
