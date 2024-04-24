using Microsoft.EntityFrameworkCore;
using TP6_ICS_G10_2024.Clases;
using TP6_ICS_G10_2024.Models.Email;
using TP6_ICS_G10_2024.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<TPDBcontext>(optionsAction: options =>
options.UseSqlServer(builder.Configuration.GetConnectionString(name: "DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IRepositorioLocalidades, LocalidadesRepositorio>();
builder.Services.AddScoped<IRepositorioPaises, PaisesRepositorio>();
builder.Services.AddScoped<IRepositorioProvincias, ProvinciasRepositorio>();
builder.Services.AddScoped<IRepositorioTipoCargas, TipoCargasRepositorio>();
builder.Services.AddScoped<IRepositorioPedidos, PedidosRepositorio>();
builder.Services.AddScoped<IRepositorioDomicilios, DomiciliosRepositorio>();

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("EmailSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
