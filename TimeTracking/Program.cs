using Microsoft.EntityFrameworkCore;
using Data;


    var builder = WebApplication.CreateBuilder(args);
    
    // Подключение к базе данных SQL Server.
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultConnection")));

    // Настройка котроллеров и параметров JSON.
    builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Игнорирование цуклических ссылок для EF.
        options.JsonSerializerOptions.ReferenceHandler = 
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

        //Форматирование JSON.
        options.JsonSerializerOptions.WriteIndented = true;
    });

   builder.Services.AddEndpointsApiExplorer();

   builder.Services.AddSwaggerGen();

   var app = builder.Build();

   // Конфигрурация HTTP-запроса.
   if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(); 
    }

    // Настройка политики CORS.
    app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());


    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseDefaultFiles();

    app.UseStaticFiles();

    app.MapControllers();

    app.Run();
