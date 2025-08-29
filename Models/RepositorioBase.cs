using Microsoft.Extensions.Configuration;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models
{
    public abstract class RepositorioBase
    {
        protected readonly IConfiguration configuration;
        protected readonly string ConectionString;

        protected RepositorioBase(IConfiguration configuration)
        {
            this.configuration = configuration;
            // lee desde appsettings.json
            ConectionString = configuration.GetConnectionString("MySql")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'MySql' en appsettings.json");
        }
    }
}
