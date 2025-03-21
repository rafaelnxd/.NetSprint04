using Challenge_Sprint03.Data;
using Challenge_Sprint03.Models;
using Challenge_Sprint03.Repositories;
using Challenge_Sprint03.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar o DbContext para Oracle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Configuração dos serviços
builder.Services.AddControllers();

// Adicionar o serviço Singleton de configurações
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton<SettingsService>(serviceProvider =>
{
    var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
    return SettingsService.GetInstance(appSettings);
});

// Registrar o repositório genérico
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Registrar repositórios específicos
builder.Services.AddScoped<IRegistroHabitoRepository, RegistroHabitoRepository>();
builder.Services.AddScoped<IUnidadeRepository, UnidadeRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Registrar serviços de negócio
builder.Services.AddScoped<HabitoService>();
builder.Services.AddScoped<RegistroHabitoService>();
builder.Services.AddScoped<UnidadesService>();
builder.Services.AddScoped<UsuariosService>();

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Challenge Sprint 03 API",
        Version = "v1",
        Description = "Documentação da API para o desafio de sprint 03"
    });
});

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
// Ativa o Swagger para todos os ambientes:
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenge Sprint 03 API V1");
});

// Redirecionamento HTTPS
app.UseHttpsRedirection();

// Autenticação/Autorização (se houver)
app.UseAuthorization();

// Mapear os controladores
app.MapControllers();

// Iniciar a aplicação
app.Run();
