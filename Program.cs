//using WebInstituto.BBDD;

using WebInstituto.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar IHttpContextAccessor para que sea inyectado en el SessionService
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Registrar SessionService
builder.Services.AddSingleton<SessionService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
/*IniciadorDatos iniciadorDatos = new IniciadorDatos();
iniciadorDatos.LimpiarDatos();
iniciadorDatos.IniciarPersonas();
iniciadorDatos.IniciarAsignaturas();
iniciadorDatos.IniciarHorarios();*/

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
