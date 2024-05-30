using DependencyInjection.Context; // Importa el espacio de nombres que contiene el contexto de base de datos.
using DependencyInjection.Repositories; // Importa el espacio de nombres que contiene los repositorios.
using Microsoft.EntityFrameworkCore; // Importa el espacio de nombres para usar Entity Framework Core.

var builder = WebApplication.CreateBuilder(args); // Crea un nuevo objeto WebApplicationBuilder con los argumentos de l�nea de comandos.

// A�adir servicios al contenedor.

builder.Services.AddControllers(); // Agrega los servicios de los controladores al contenedor de dependencias.
// Aprende m�s sobre c�mo configurar Swagger/OpenAPI en https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // Agrega servicios para explorar los endpoints de la API.
builder.Services.AddSwaggerGen(); // Agrega servicios para generar la documentaci�n de Swagger.

// Las siguientes l�neas configuran la inyecci�n de dependencias para los repositorios.
//builder.Services.AddSingleton<ICountryRepository, InMemoryRepository>(); // Descomentar para una �nica instancia a lo largo de la aplicaci�n.
//builder.Services.AddScoped<ICountryRepository, FileCountryRepository>(); // Descomentar para una instancia por cada alcance (por ejemplo, una instancia por cada solicitud en un controlador).
builder.Services.AddTransient<ICountryRepository, DbCountryRepository>(); // Agrega el repositorio DbCountryRepository con una nueva instancia cada vez que se solicita.

builder.Services.AddDbContext<CountryDbContext>((context) => // Configura el contexto de base de datos.
{
    context.UseSqlServer(builder.Configuration.GetConnectionString("CountryDb")); // Configura el contexto para usar SQL Server con la cadena de conexi�n especificada.
});

var app = builder.Build(); // Construye la aplicaci�n.

// Configuraci�n del pipeline de manejo de solicitudes HTTP.
if (app.Environment.IsDevelopment()) // Si el entorno es de desarrollo,
{
    app.UseSwagger(); // Habilita Swagger para generar la documentaci�n de la API.
    app.UseSwaggerUI(); // Habilita la interfaz de usuario de Swagger.
}

app.UseHttpsRedirection(); // Fuerza el uso de HTTPS en las solicitudes.

app.UseAuthorization(); // Habilita la autorizaci�n en la aplicaci�n.

app.MapControllers(); // Mapea los endpoints de los controladores.

app.Run(); // Ejecuta la aplicaci�n.
