using LanchoMac.Areas.Admin.Services;
using LanchoMac.Data;
using LanchoMac.Models;
using LanchoMac.Repositories;
using LanchoMac.Repositories.Interfaces;
using LanchoMac.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LanchesContexto>(options => options.UseMySql("Server=localhost;Port=3306;Database=LanchesDb;User=root;Password=123456;"
, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<LanchesContexto>().AddDefaultTokenProviders();
builder.Services.AddTransient<ILancheRepository, LanchesRepository>();
builder.Services.AddTransient<ICategotiaRepository, CategoriaRepository>();
builder.Services.AddTransient<IPedidoRepository, PedidoRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
builder.Services.AddScoped<RelatorioVendasService>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
        politica =>
        {   
            politica.RequireRole("Admin");
        });
});
builder.Services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));
builder.Services.AddScoped<GraficoVendasService>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddPaging(option =>
{
    option.ViewName = "Bootstrap4";
    option.PageParameterName = "pageindex";
});
builder.Services.Configure<ConfigurationImagens>(builder.Configuration.GetSection("ConfigurationPastaImagens"));

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
app.UseSession();
CriarPerfisUsuarios(app);

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
    );
});
app.MapControllerRoute(
    name: "categoriaFiltro",
    pattern: "Lanche/{action}/{Categoria?}",
    defaults: new { Controller = "Lanche", action = "List"}
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
void CriarPerfisUsuarios(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<ISeedUserRoleInitial>();
        service.SeedRoles();
        service.SeedUsers();

    }
}