var builder = WebApplication.CreateBuilder(args);

// Configuração da porta para o Render
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://*:{port}");
}

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<Projis.ApontamentoProducao.Data.Database>();

builder.Services.AddScoped<Projis.ApontamentoProducao.Services.UsuarioService>();
builder.Services.AddScoped<Projis.ApontamentoProducao.Services.ApontamentoService>();

// Session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Habilita Session
app.UseSession();

app.UseAuthorization();

// Rota inicial será a tela de Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();