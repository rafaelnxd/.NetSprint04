using Challenge_Sprint03.Data;
using Challenge_Sprint03.Models;
using Challenge_Sprint03.Repositories;
using Challenge_Sprint03.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ---------- Infra -----------------------------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddControllers();

builder.Services.AddSingleton<YoloService>();

// AppSettings (caso use em outras partes)
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton<SettingsService>(sp =>
{
    var cfg = sp.GetRequiredService<IOptions<AppSettings>>().Value;
    return SettingsService.GetInstance(cfg);
});

// ---------- Configuração do OpenAI ------------------------------------------
builder.Services.Configure<OpenAIOptions>(builder.Configuration.GetSection("OpenAI"));
builder.Services.AddHttpClient<RecomendacaoService>();

// ---------- Repositórios e Serviços de domínio ------------------------------
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRegistroHabitoRepository, RegistroHabitoRepository>();
builder.Services.AddScoped<IUnidadeRepository, UnidadeRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<HabitoService>();
builder.Services.AddScoped<RegistroHabitoService>();
builder.Services.AddScoped<UnidadesService>();
builder.Services.AddScoped<UsuariosService>();

// ---------- YOLO Service ----------------------------------------------------
builder.Services.AddSingleton<YoloService>();

// ---------- HTTP Client para Brevo + serviço de e-mail ----------------------
builder.Services.AddHttpClient("Brevo", c =>
{
    c.BaseAddress = new Uri("https://api.brevo.com/v3/");
    // Content-Type e Accept são definidos no EmailService.
});

builder.Services.AddScoped<EmailService>();

// ---------- CORS ------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ---------- Swagger ---------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Challenge Sprint 03 API",
        Version = "v1",
        Description = "Documentação da API para o desafio de sprint 03"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

// ---------- Ajuste Kestrel para requisições grandes -------------------------
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 50 * 1024 * 1024; // 50 MB
});

var app = builder.Build();

// ---------- Pipeline ---------------------------------------------------------
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenge Sprint 03 API V1");
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
