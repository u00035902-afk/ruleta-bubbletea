using Microsoft.EntityFrameworkCore;
using RuletanBublee.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Configurar CORS (Permite que cualquier teléfono o navegador consuma la API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Middlewares para servir la interfaz web (index.html)
app.UseDefaultFiles(); // Busca automáticamente index.html en wwwroot
app.UseStaticFiles();  // Permite servir archivos HTML, CSS, JS e imágenes

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();//

// 4. Activar CORS antes del ruteo de controladores
app.UseCors("PermitirTodo");

app.UseAuthorization();

app.MapControllers();



using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.Run();