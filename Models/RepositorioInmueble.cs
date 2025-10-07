using MySql.Data.MySqlClient;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models;

public class RepositorioInmueble : RepositorioBase
{

    //private readonly string ConectionString = "Server=localhost;User=root;Password=;Database=proyecto_inmobiliaria_guardia_lucero;SslMode=none";
    public RepositorioInmueble(IConfiguration configuration) : base(configuration)
    {

    }
    public List<Inmuebles> ObtenerPaginados(int pagina, int tamanoPagina)
    {
        List<Inmuebles> inmuebles = new List<Inmuebles>();
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"SELECT i.Id_inmueble,
                            i.Id_propietario,
                            p.Email,
                            i.Uso,
                            i.Tipo,
                            i.Direccion,
                            i.Ambiente,
                            i.Precio,
                            i.Longitud,
                            i.Latitud,
                            i.Fecha_creacion,
                            i.Estado,
                            i.Portada,
                            i.Creado_por,
                            i.Desactivado_por
                        FROM inmuebles i
                        JOIN propietarios p ON i.Id_propietario = p.Id_propietario
                        ORDER BY i.Id_inmueble
                        LIMIT @limit OFFSET @offset";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@limit", tamanoPagina);
                command.Parameters.AddWithValue("@offset", (pagina - 1) * tamanoPagina);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inmuebles.Add(new Inmuebles
                    {
                        Id_inmueble = reader.GetInt32(nameof(Inmuebles.Id_inmueble)),
                        Id_propietario = reader.GetInt32(nameof(Inmuebles.Id_propietario)),
                        EmailPropietario = reader.GetString("Email"),
                        Uso = reader.GetString(nameof(Inmuebles.Uso)),
                        Tipo = reader.GetString(nameof(Inmuebles.Tipo)),
                        Direccion = reader.GetString(nameof(Inmuebles.Direccion)),
                        Ambiente = reader.GetInt32(nameof(Inmuebles.Ambiente)),
                        Precio = reader.GetDecimal(nameof(Inmuebles.Precio)),
                        Longitud = reader.GetDouble(nameof(Inmuebles.Longitud)),
                        Latitud = reader.GetDouble(nameof(Inmuebles.Latitud)),
                        Fecha_creacion = reader.GetDateTime(nameof(Inmuebles.Fecha_creacion)),
                        Portada = reader.IsDBNull(reader.GetOrdinal(nameof(Inmuebles.Portada))) 
                                    ? null 
                                    : reader.GetString(nameof(Inmuebles.Portada)),
                        Estado = reader.GetBoolean(nameof(Inmuebles.Estado)),
                        Creado_por = reader.IsDBNull(reader.GetOrdinal(nameof(Inmuebles.Creado_por))) 
                            ? null 
                            : reader.GetString(nameof(Inmuebles.Creado_por)),

                        Desactivado_por = reader.IsDBNull(reader.GetOrdinal(nameof(Inmuebles.Desactivado_por))) 
                            ? null 
                            : reader.GetString(nameof(Inmuebles.Desactivado_por)),
                    });
                }
            }
        }
        return inmuebles;
    }
    public List<Inmuebles> ObtenerPaginadosFiltrados(string? email, bool? estado,DateTime? fechaInicio,DateTime? fechaFin,int pagina, int tamanoPagina)
{
    List<Inmuebles> inmuebles = new List<Inmuebles>();
    using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        // Usamos StringBuilder para construir la consulta dinámicamente
        var sqlBuilder = new System.Text.StringBuilder(@"
            SELECT i.Id_inmueble, i.Id_propietario, p.Email, i.Uso, i.Tipo, 
                   i.Direccion, i.Ambiente, i.Precio, i.Longitud, i.Latitud, 
                   i.Fecha_creacion, i.Estado, i.Portada, i.Creado_por, i.Desactivado_por
            FROM inmuebles i
            JOIN propietarios p ON i.Id_propietario = p.Id_propietario");

        var whereClauses = new List<string>();
        
        
        using (MySqlCommand command = new MySqlCommand())
        {
            
            if (!string.IsNullOrEmpty(email))
            {
                whereClauses.Add("p.Email LIKE @email");
                command.Parameters.AddWithValue("@email", $"%{email}%"); 
            }

           
            if (estado.HasValue)
            {
                whereClauses.Add("i.Estado = @estado");
                command.Parameters.AddWithValue("@estado", estado.Value);
            }

            // Filtro por Disponibilidad de Fechas
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                whereClauses.Add(@"i.Id_inmueble NOT IN (
                    SELECT DISTINCT c.Id_inmueble 
                    FROM contratos c
                    WHERE c.Estado = 1 
                      AND (c.Fecha_desde <= @fechaFin) 
                      AND (c.Fecha_hasta >= @fechaInicio)
                )");
                command.Parameters.AddWithValue("@fechaInicio", fechaInicio.Value);
                command.Parameters.AddWithValue("@fechaFin", fechaFin.Value);
            }
            if (whereClauses.Any())
            {
                sqlBuilder.Append(" WHERE ").Append(string.Join(" AND ", whereClauses));
            }

            // Añadimos el orden y la paginación al final
            sqlBuilder.Append(" ORDER BY i.Id_inmueble LIMIT @limit OFFSET @offset");
            command.Parameters.AddWithValue("@limit", tamanoPagina);
            command.Parameters.AddWithValue("@offset", (pagina - 1) * tamanoPagina);
            
            
            command.CommandText = sqlBuilder.ToString();
            command.Connection = connection;

            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                inmuebles.Add(new Inmuebles
                {
                    Id_inmueble = reader.GetInt32(nameof(Inmuebles.Id_inmueble)),
                        Id_propietario = reader.GetInt32(nameof(Inmuebles.Id_propietario)),
                        EmailPropietario = reader.GetString("Email"),
                        Uso = reader.GetString(nameof(Inmuebles.Uso)),
                        Tipo = reader.GetString(nameof(Inmuebles.Tipo)),
                        Direccion = reader.GetString(nameof(Inmuebles.Direccion)),
                        Ambiente = reader.GetInt32(nameof(Inmuebles.Ambiente)),
                        Precio = reader.GetDecimal(nameof(Inmuebles.Precio)),
                        Longitud = reader.GetDouble(nameof(Inmuebles.Longitud)),
                        Latitud = reader.GetDouble(nameof(Inmuebles.Latitud)),
                        Fecha_creacion = reader.GetDateTime(nameof(Inmuebles.Fecha_creacion)),
                        Portada = reader.IsDBNull(reader.GetOrdinal(nameof(Inmuebles.Portada))) 
                                    ? null 
                                    : reader.GetString(nameof(Inmuebles.Portada)),
                        Estado = reader.GetBoolean(nameof(Inmuebles.Estado)),
                        Creado_por = reader.IsDBNull(reader.GetOrdinal(nameof(Inmuebles.Creado_por))) 
                            ? null 
                            : reader.GetString(nameof(Inmuebles.Creado_por)),

                        Desactivado_por = reader.IsDBNull(reader.GetOrdinal(nameof(Inmuebles.Desactivado_por))) 
                            ? null 
                            : reader.GetString(nameof(Inmuebles.Desactivado_por)),
                });
            }
        }
    }
    return inmuebles;
}
    public int ContarInmuebles()
    {
        using (var connection = new MySqlConnection(ConectionString))
        {
            var query = "SELECT COUNT(*) FROM inmuebles";
            using (var command = new MySqlCommand(query, connection))
            {
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
public int ContarFiltrados(string? email, bool? estado, DateTime? fechaInicio, DateTime? fechaFin)
{
    using (var connection = new MySqlConnection(ConectionString))
    {
        var sqlBuilder = new System.Text.StringBuilder(@"
            SELECT COUNT(*)
            FROM inmuebles i
            JOIN propietarios p ON i.Id_propietario = p.Id_propietario");

        var whereClauses = new List<string>();
        using (var command = new MySqlCommand())
        {
            // --- LA MISMA LÓGICA DE FILTROS QUE EL MÉTODO ANTERIOR ---
            if (!string.IsNullOrEmpty(email))
            {
                whereClauses.Add("p.Email LIKE @email");
                command.Parameters.AddWithValue("@email", $"%{email}%");
            }
            if (estado.HasValue)
            {
                whereClauses.Add("i.Estado = @estado");
                command.Parameters.AddWithValue("@estado", estado.Value);
            }
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                whereClauses.Add(@"i.Id_inmueble NOT IN (
                    SELECT DISTINCT c.Id_inmueble FROM contratos c
                    WHERE c.Estado = 1 AND (c.Fecha_desde <= @fechaFin) AND (c.Fecha_hasta >= @fechaInicio)
                )");
                command.Parameters.AddWithValue("@fechaInicio", fechaInicio.Value);
                command.Parameters.AddWithValue("@fechaFin", fechaFin.Value);
            }

            if (whereClauses.Any())
            {
                sqlBuilder.Append(" WHERE ").Append(string.Join(" AND ", whereClauses));
            }

            command.CommandText = sqlBuilder.ToString();
            command.Connection = connection;
            connection.Open();
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
}

    public List<Propietarios> ObtenerTodosPropietarios()
    {
        List<Propietarios> propietarios = new List<Propietarios>();
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"SELECT {nameof(Propietarios.Id_propietario)}, {nameof(Propietarios.Dni)}, {nameof(Propietarios.Apellido)}, {nameof(Propietarios.Nombre)}, {nameof(Propietarios.Email)}, {nameof(Propietarios.Telefono)}, {nameof(Propietarios.Fecha_creacion)}
                      FROM propietarios WHERE 1";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    propietarios.Add(new Propietarios
                    {
                        Id_propietario = reader.GetInt32(nameof(Propietarios.Id_propietario)),
                        Dni = reader.GetString(nameof(Propietarios.Dni)),
                        Apellido = reader.GetString(nameof(Propietarios.Apellido)),
                        Nombre = reader.GetString(nameof(Propietarios.Nombre)),
                        Email = reader.GetString(nameof(Propietarios.Email)),
                        Telefono = reader.GetString(nameof(Propietarios.Telefono)),
                        Fecha_creacion = reader.GetDateTime(nameof(Propietarios.Fecha_creacion))

                    });
                }
                connection.Close();
            }
            return propietarios;
        }
    }
    public void AgregarInmueble(Inmuebles nuevoInmuebles)
    {

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"INSERT INTO inmuebles ({nameof(Inmuebles.Id_propietario)}, {nameof(Inmuebles.Uso)}, {nameof(Inmuebles.Tipo)},{nameof(Inmuebles.Direccion)}, {nameof(Inmuebles.Ambiente)}, {nameof(Inmuebles.Precio)}, {nameof(Inmuebles.Longitud)}, {nameof(Inmuebles.Latitud)},{nameof(Inmuebles.Portada)},{nameof(Inmuebles.Creado_por)})
                    VALUES (@Id_propietario, @Uso, @Tipo,@Direccion,@Ambiente,@Precio,@Longitud,@Latitud,@Portada,@Creado_por)";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Agrega los parámetros
                command.Parameters.AddWithValue("@Id_propietario", nuevoInmuebles.Id_propietario);
                command.Parameters.AddWithValue("@Uso", nuevoInmuebles.Uso);
                command.Parameters.AddWithValue("@Tipo", nuevoInmuebles.Tipo);
                command.Parameters.AddWithValue("@Direccion", nuevoInmuebles.Direccion);
                command.Parameters.AddWithValue("@Ambiente", nuevoInmuebles.Ambiente);
                command.Parameters.AddWithValue("@Precio", nuevoInmuebles.Precio);
                command.Parameters.AddWithValue("@Longitud", nuevoInmuebles.Longitud);
                command.Parameters.AddWithValue("@Latitud", nuevoInmuebles.Latitud);
                command.Parameters.AddWithValue("@Portada", nuevoInmuebles.Portada);
                command.Parameters.AddWithValue("@Creado_por", nuevoInmuebles.Creado_por);

                connection.Open();
                command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
                connection.Close();
            }
        }
    }
    
    public void EliminarInmueble(int id)
    {
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = "DELETE FROM inmuebles WHERE Id_inmueble = @Id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
    
    
    public Inmuebles? ObtenerPorID(int id)
    {
        Inmuebles? res = null;

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"
            SELECT i.{nameof(Inmuebles.Id_inmueble)},
                   i.{nameof(Inmuebles.Id_propietario)},
                   i.{nameof(Inmuebles.Uso)},
                   i.{nameof(Inmuebles.Tipo)},
                   i.{nameof(Inmuebles.Direccion)},
                   i.{nameof(Inmuebles.Ambiente)},
                   i.{nameof(Inmuebles.Precio)},
                   i.{nameof(Inmuebles.Longitud)},
                   i.{nameof(Inmuebles.Latitud)},
                   i.{nameof(Inmuebles.Estado)},
                   i.{nameof(Inmuebles.Portada)},
                   p.Email AS EmailPropietario
            FROM inmuebles i
            JOIN propietarios p ON i.Id_propietario = p.Id_propietario
            WHERE i.Id_inmueble = @Id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Agrega el parámetro id
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new Inmuebles
                        {
                            Id_inmueble = reader.GetInt32(nameof(Inmuebles.Id_inmueble)),
                            Id_propietario = reader.GetInt32(nameof(Inmuebles.Id_propietario)),
                            Uso = reader.GetString(nameof(Inmuebles.Uso)),
                            Tipo = reader.GetString(nameof(Inmuebles.Tipo)),
                            Direccion = reader.GetString(nameof(Inmuebles.Direccion)),
                            Ambiente = reader.GetInt32(nameof(Inmuebles.Ambiente)),
                            Precio = reader.GetDecimal(nameof(Inmuebles.Precio)),
                            Longitud = reader.GetDouble(nameof(Inmuebles.Longitud)),
                            Latitud = reader.GetDouble(nameof(Inmuebles.Latitud)),
                            Estado = reader.GetBoolean(nameof(Inmuebles.Estado)),
                            Portada = reader.IsDBNull(reader.GetOrdinal(nameof(Inmuebles.Portada)))
                                    ? null
                                    : reader.GetString(nameof(Inmuebles.Portada)),
                            EmailPropietario = reader.GetString("EmailPropietario")
                        };
                    }
                }
            }
        }

        return res;
    }
    public void ActualizarInmueble(Inmuebles actualizarPropietario)
    {
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"UPDATE inmuebles
               SET 
                   {nameof(Inmuebles.Uso)} = @Uso, 
                   {nameof(Inmuebles.Tipo)} = @Tipo, 
                   {nameof(Inmuebles.Direccion)} = @Direccion, 
                   {nameof(Inmuebles.Ambiente)} = @Ambiente, 
                   {nameof(Inmuebles.Precio)} = @Precio,
                   {nameof(Inmuebles.Longitud)} = @Longitud,
                   {nameof(Inmuebles.Latitud)} = @Latitud,
                   {nameof(Inmuebles.Id_propietario)} = @Id_propietario,
                   {nameof(Inmuebles.Portada)} = @Portada
               WHERE Id_inmueble = @Id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Agrega los parámetros
                command.Parameters.AddWithValue("@Uso", actualizarPropietario.Uso);
                command.Parameters.AddWithValue("@Tipo", actualizarPropietario.Tipo);
                command.Parameters.AddWithValue("@Direccion", actualizarPropietario.Direccion);
                command.Parameters.AddWithValue("@Ambiente", actualizarPropietario.Ambiente);
                command.Parameters.AddWithValue("@Precio", actualizarPropietario.Precio);
                command.Parameters.AddWithValue("@Longitud", actualizarPropietario.Longitud);
                command.Parameters.AddWithValue("@Latitud", actualizarPropietario.Latitud);
                command.Parameters.AddWithValue("@Id_propietario", actualizarPropietario.Id_propietario);
                command.Parameters.AddWithValue("@Id", actualizarPropietario.Id_inmueble);
                command.Parameters.AddWithValue("@Portada", actualizarPropietario.Portada);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
    public List<Inmuebles> BuscarPorDireccion(string direccionParcial)
    {
        var lista = new List<Inmuebles>();

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"SELECT Id_inmueble, Id_propietario ,Uso, Tipo, Direccion,Ambiente,Precio,Longitud,Latitud,Fecha_creacion,Estado
                            FROM inmuebles
                            WHERE LOWER(Direccion) LIKE LOWER(@Direccion) AND Estado = 1";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {

                command.Parameters.AddWithValue("@Direccion", $"%{direccionParcial}%");

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Inmuebles
                        {
                            Id_inmueble = reader.GetInt32(reader.GetOrdinal("Id_inmueble")),
                            Id_propietario = reader.GetInt32(reader.GetOrdinal("Id_propietario")),
                            Uso = reader.GetString(reader.GetOrdinal("Uso")),
                            Tipo = reader.GetString(reader.GetOrdinal("Tipo")),
                            Direccion = reader.GetString(reader.GetOrdinal("Direccion")),
                            Ambiente = reader.GetInt32(reader.GetOrdinal("Ambiente")),
                            Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                            Longitud = reader.GetDouble(reader.GetOrdinal("Longitud")),
                            Latitud = reader.GetDouble(reader.GetOrdinal("Latitud")),
                            Fecha_creacion = reader.GetDateTime(reader.GetOrdinal("Fecha_creacion")),
                            Estado = reader.GetBoolean(reader.GetOrdinal("Estado"))
                        });
                    }
                }
            }
        }

        return lista;
    }
    public void DesactivarInmueble(int idInmueble,string Name)
{
    using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var query = @"UPDATE inmuebles
                      SET Estado = 0,Desactivado_por = @Desactivado_por
                      WHERE Id_inmueble = @Id";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", idInmueble);
            command.Parameters.AddWithValue("@Desactivado_por", Name);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
    public void ActivarInmueble(int idInmueble)
    {
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"UPDATE inmuebles
                      SET Estado = 1,Desactivado_por = @Desactivado_por
                      WHERE Id_inmueble = @Id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", idInmueble);
                command.Parameters.AddWithValue("@Desactivado_por", "");
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
    public List<Inmuebles> ObtenerInmueblesDisponibles(DateTime fechaInicio, DateTime fechaFin)
    {
        var lista = new List<Inmuebles>();
        using (var connection = new MySqlConnection(ConectionString))
        {

            var query = @"
            SELECT * FROM inmuebles
            WHERE Activo = 1 AND Id_inmueble NOT IN (
                -- ... que no estén en esta subconsulta de inmuebles ocupados.
                SELECT DISTINCT Id_inmueble 
                FROM contratos
                WHERE Estado = 1 
                  AND (Fecha_desde <= @fechaFin) 
                  AND (Fecha_hasta >= @fechaInicio)
            )";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                command.Parameters.AddWithValue("@fechaFin", fechaFin);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        lista.Add(new Inmuebles
                        {
                            Id_inmueble = reader.GetInt32("Id_inmueble"),
                            Direccion = reader.GetString("Direccion"),
                            Precio = reader.GetDecimal("Precio"),

                        });
                    }
                }
            }
        }
        return lista;
    }

}