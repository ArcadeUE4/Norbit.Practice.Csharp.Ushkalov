using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using Data;
using System.Reflection;


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

    builder.Services.AddSwaggerGen(c =>
        { 
        c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "TimeTracking API",
        Version = "v1" });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        if (File.Exists(xmlPath))
        {
        c.IncludeXmlComments(xmlPath);
        }
    });


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
