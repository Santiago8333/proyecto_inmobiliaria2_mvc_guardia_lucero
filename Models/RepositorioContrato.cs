using MySql.Data.MySqlClient;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class RepositorioContrato : RepositorioBase
{ 


    public RepositorioContrato(IConfiguration configuration) : base(configuration)
    {

    }
    public List<Contratos> ObtenerPaginados(int pagina, int tamanoPagina)
    {
        List<Contratos> contratos = new List<Contratos>();
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"SELECT   
                            Id_contrato,
                            Id_inquilino,
                            Id_inmueble,
                            Contrato_completado,
                            Monto,
                            Monto_total,
                            Monto_a_pagar,
                            Fecha_creacion,
                            Fecha_desde,
                            Fecha_hasta,
                            Fecha_final,
                            Meses,
                            Creado_por,
                            Terminado_por,
                            Estado
                        FROM contratos
                        ORDER BY Id_contrato
                        LIMIT @limit OFFSET @offset";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@limit", tamanoPagina);
                command.Parameters.AddWithValue("@offset", (pagina - 1) * tamanoPagina);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contratos.Add(new Contratos
                    {
                        Id_contrato = reader.GetInt32(nameof(Contratos.Id_contrato)),
                        Id_inquilino = reader.GetInt32(nameof(Contratos.Id_inquilino)),
                        Id_inmueble = reader.GetInt32(nameof(Contratos.Id_inmueble)),
                        Contrato_completado = reader.GetBoolean(nameof(Contratos.Contrato_completado)),
                        Monto = reader.GetDecimal(nameof(Contratos.Monto)),
                        Monto_total = reader.GetDecimal(nameof(Contratos.Monto_total)),
                        Monto_a_pagar = reader.GetDecimal(nameof(Contratos.Monto_a_pagar)),
                        Fecha_creacion = reader.GetDateTime(nameof(Contratos.Fecha_creacion)),
                        Fecha_desde = reader.GetDateTime(nameof(Contratos.Fecha_desde)),
                        Fecha_hasta = reader.GetDateTime(nameof(Contratos.Fecha_hasta)),
                        Fecha_final = reader.GetDateTime(nameof(Contratos.Fecha_final)),
                        Meses = reader.GetInt32(nameof(Contratos.Meses)),
                        Estado = reader.GetBoolean(nameof(Contratos.Estado))
                    });
                }
            }
        }
        return contratos;
    }
    public int ContarContrato()
    {
        using (var connection = new MySqlConnection(ConectionString))
        {
            var query = "SELECT COUNT(*) FROM contratos";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }

}