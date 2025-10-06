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
                            c.Id_contrato,
                            c.Id_inquilino,
                            c.Id_inmueble,
                            c.Contrato_completado,
                            c.Monto,
                            c.Monto_total,
                            c.Monto_a_pagar,
                            c.Fecha_creacion,
                            c.Fecha_desde,
                            c.Fecha_hasta,
                            c.Fecha_final,
                            c.Meses,
                            c.Creado_por,
                            c.Terminado_por,
                            c.Estado,
                            i.Direccion AS DireccionInmueble,
                            q.Email AS EmailInquilino
                        FROM contratos c
                        JOIN inmuebles i ON c.Id_inmueble = i.Id_inmueble
                        JOIN inquilinos q ON c.Id_inquilino = q.Id_inquilino
                        ORDER BY c.Id_contrato
                        LIMIT @limit OFFSET @offset;";

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
                        Fecha_final = reader.IsDBNull(reader.GetOrdinal("Fecha_final"))
                                        ? (DateTime?)null
                                        : reader.GetDateTime("Fecha_final"),
                        Creado_por = reader.IsDBNull(reader.GetOrdinal(nameof(Contratos.Creado_por))) 
                            ? null 
                            : reader.GetString(nameof(Contratos.Creado_por)),

                        Terminado_por = reader.IsDBNull(reader.GetOrdinal(nameof(Contratos.Terminado_por))) 
                            ? null 
                            : reader.GetString(nameof(Contratos.Terminado_por)),
                        Meses = reader.GetInt32(nameof(Contratos.Meses)),
                        Estado = reader.GetBoolean(nameof(Contratos.Estado)),
                        DireccionInmueble = reader.GetString("DireccionInmueble"),
                        EmailInquilino = reader.GetString("EmailInquilino")
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
    public List<Contratos> ObtenerPaginadosFiltrados(DateTime? fechaInicio, DateTime? fechaFin, string? email,int? expiracion, int pagina, int tamanoPagina)
{
    List<Contratos> contratos = new List<Contratos>();
    using (MySqlConnection connection = new MySqlConnection(ConectionString))
    {
        var sqlBuilder = new System.Text.StringBuilder(@"
            SELECT c.Id_contrato, c.Id_inquilino, c.Id_inmueble, c.Contrato_completado, 
                   c.Monto, c.Monto_total, c.Monto_a_pagar, c.Fecha_creacion, c.Fecha_desde, 
                   c.Fecha_hasta, c.Fecha_final, c.Meses, c.Creado_por, c.Terminado_por, c.Estado,
                   i.Direccion AS DireccionInmueble, q.Email AS EmailInquilino
            FROM contratos c
            JOIN inmuebles i ON c.Id_inmueble = i.Id_inmueble
            JOIN inquilinos q ON c.Id_inquilino = q.Id_inquilino");

        var whereClauses = new List<string>();
        
        using (MySqlCommand command = new MySqlCommand())
        {

           
            if (!string.IsNullOrEmpty(email))
            {
                whereClauses.Add("q.Email LIKE @email");
                command.Parameters.AddWithValue("@email", $"%{email}%");
            }

            
            if (fechaInicio.HasValue)
            {
    
                whereClauses.Add("c.Fecha_hasta >= @fechaInicio");
                command.Parameters.AddWithValue("@fechaInicio", fechaInicio.Value);
            }

          
            if (fechaFin.HasValue)
            {
              
                whereClauses.Add("c.Fecha_desde <= @fechaFin");
                command.Parameters.AddWithValue("@fechaFin", fechaFin.Value);
            }
            if (expiracion.HasValue && expiracion > 0)
            {
                // La cláusula WHERE para buscar contratos que:
                // 1. Terminen DESPUÉS de hoy.
                // 2. Terminen ANTES de "hoy + X días".
                whereClauses.Add("c.Fecha_hasta BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL @expiracion DAY)");
                command.Parameters.AddWithValue("@expiracion", expiracion.Value);
            }

            if (whereClauses.Any())
                {
                    sqlBuilder.Append(" WHERE ").Append(string.Join(" AND ", whereClauses));
                }

            sqlBuilder.Append(" ORDER BY c.Id_contrato LIMIT @limit OFFSET @offset");
            command.Parameters.AddWithValue("@limit", tamanoPagina);
            command.Parameters.AddWithValue("@offset", (pagina - 1) * tamanoPagina);
            
            command.CommandText = sqlBuilder.ToString();
            command.Connection = connection;

            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
      
                contratos.Add(new Contratos { Id_contrato = reader.GetInt32(nameof(Contratos.Id_contrato)),
                        Id_inquilino = reader.GetInt32(nameof(Contratos.Id_inquilino)),
                        Id_inmueble = reader.GetInt32(nameof(Contratos.Id_inmueble)),
                        Contrato_completado = reader.GetBoolean(nameof(Contratos.Contrato_completado)),
                        Monto = reader.GetDecimal(nameof(Contratos.Monto)),
                        Monto_total = reader.GetDecimal(nameof(Contratos.Monto_total)),
                        Monto_a_pagar = reader.GetDecimal(nameof(Contratos.Monto_a_pagar)),
                        Fecha_creacion = reader.GetDateTime(nameof(Contratos.Fecha_creacion)),
                        Fecha_desde = reader.GetDateTime(nameof(Contratos.Fecha_desde)),
                        Fecha_hasta = reader.GetDateTime(nameof(Contratos.Fecha_hasta)),
                        Fecha_final = reader.IsDBNull(reader.GetOrdinal("Fecha_final"))
                                        ? (DateTime?)null
                                        : reader.GetDateTime("Fecha_final"),
                        Creado_por = reader.IsDBNull(reader.GetOrdinal(nameof(Contratos.Creado_por))) 
                            ? null 
                            : reader.GetString(nameof(Contratos.Creado_por)),

                        Terminado_por = reader.IsDBNull(reader.GetOrdinal(nameof(Contratos.Terminado_por))) 
                            ? null 
                            : reader.GetString(nameof(Contratos.Terminado_por)),
                        Meses = reader.GetInt32(nameof(Contratos.Meses)),
                        Estado = reader.GetBoolean(nameof(Contratos.Estado)),
                        DireccionInmueble = reader.GetString("DireccionInmueble"),
                        EmailInquilino = reader.GetString("EmailInquilino")});
            }
        }
    }
    return contratos;
}


public int ContarFiltrados(DateTime? fechaInicio, DateTime? fechaFin, string? email,int? expiracion)
{
    using (var connection = new MySqlConnection(ConectionString))
    {
        var sqlBuilder = new System.Text.StringBuilder(@"
            SELECT COUNT(*) FROM contratos c
            JOIN inquilinos q ON c.Id_inquilino = q.Id_inquilino");

        var whereClauses = new List<string>();
        using (var command = new MySqlCommand())
        {
            
            if (!string.IsNullOrEmpty(email))
            {
                whereClauses.Add("q.Email LIKE @email");
                command.Parameters.AddWithValue("@email", $"%{email}%");
            }
            if (fechaInicio.HasValue)
            {
                whereClauses.Add("c.Fecha_hasta >= @fechaInicio");
                command.Parameters.AddWithValue("@fechaInicio", fechaInicio.Value);
            }
            if (fechaFin.HasValue)
            {
                whereClauses.Add("c.Fecha_desde <= @fechaFin");
                command.Parameters.AddWithValue("@fechaFin", fechaFin.Value);
            }
            if (expiracion.HasValue && expiracion > 0)
            {
                whereClauses.Add("c.Fecha_hasta BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL @expiracion DAY)");
                command.Parameters.AddWithValue("@expiracion", expiracion.Value);
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
    public void AgregarContrato(Contratos nuevoContrato)
    {

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"INSERT INTO contratos ({nameof(Contratos.Id_inquilino)}, {nameof(Contratos.Id_inmueble)}, {nameof(Contratos.Monto)}, {nameof(Contratos.Monto_total)}, {nameof(Contratos.Monto_a_pagar)},{nameof(Contratos.Fecha_desde)},{nameof(Contratos.Fecha_hasta)},{nameof(Contratos.Fecha_final)},{nameof(Contratos.Meses)},{nameof(Contratos.Creado_por)})
                    VALUES (@Id_inquilino, @Id_inmueble, @Monto,@Monto_total,@Monto_a_pagar,@Fecha_desde,@Fecha_hasta,@Fecha_final,@Meses,@Creado_por)";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                // Agrega los parámetros
                command.Parameters.AddWithValue("@Id_inquilino", nuevoContrato.Id_inquilino);
                command.Parameters.AddWithValue("@Id_inmueble", nuevoContrato.Id_inmueble);
                command.Parameters.AddWithValue("@Monto", nuevoContrato.Monto);
                command.Parameters.AddWithValue("@Monto_total", nuevoContrato.Monto_total);
                command.Parameters.AddWithValue("@Monto_a_pagar", nuevoContrato.Monto_a_pagar);
                command.Parameters.AddWithValue("@Fecha_desde", nuevoContrato.Fecha_desde);
                command.Parameters.AddWithValue("@Fecha_hasta", nuevoContrato.Fecha_hasta);
                command.Parameters.AddWithValue("@Fecha_final", nuevoContrato.Fecha_final);
                command.Parameters.AddWithValue("@Meses", nuevoContrato.Meses);
                command.Parameters.AddWithValue("@Creado_por", nuevoContrato.Creado_por);
                connection.Open();
                command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
                connection.Close();
            }
        }
    }
    public Contratos? BuscarPorId(int id)
    {
        Contratos? contrato = null;

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"
            SELECT 
                c.Id_contrato,
                c.Id_inquilino,
                c.Id_inmueble,
                c.Monto,
                c.Monto_total,
                c.Monto_a_pagar,
                c.Fecha_desde,
                c.Fecha_hasta,
                c.Fecha_final,
                c.Meses,
                c.Estado,
                c.Contrato_completado,
                c.Fecha_creacion,
                i.Email AS EmailInquilino,
                m.Direccion AS DireccionInmueble
            FROM contratos c
            INNER JOIN inquilinos i ON c.Id_inquilino = i.Id_inquilino
            INNER JOIN inmuebles m ON c.Id_inmueble = m.Id_inmueble
            WHERE c.Id_contrato = @Id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        contrato = new Contratos
                        {
                            Id_contrato = reader.GetInt32("Id_contrato"),
                            Id_inquilino = reader.GetInt32("Id_inquilino"),
                            Id_inmueble = reader.GetInt32("Id_inmueble"),
                            Monto = reader.GetDecimal("Monto"),
                            Monto_total = reader.GetDecimal("Monto_total"),
                            Monto_a_pagar = reader.GetDecimal("Monto_a_pagar"),
                            Fecha_desde = reader.GetDateTime("Fecha_desde"),
                            Fecha_hasta = reader.GetDateTime("Fecha_hasta"),
                            Fecha_final = reader.IsDBNull(reader.GetOrdinal("Fecha_final"))
                                        ? (DateTime?)null
                                        : reader.GetDateTime("Fecha_final"),
                            Meses = reader.GetInt32("Meses"),
                            Estado = reader.GetBoolean("Estado"),
                            Contrato_completado = reader.GetBoolean("Contrato_completado"),
                            Fecha_creacion = reader.GetDateTime("Fecha_creacion"),

                            // Campos extras traídos con JOIN
                            EmailInquilino = reader.GetString("EmailInquilino"),
                            DireccionInmueble = reader.GetString("DireccionInmueble")
                        };
                    }
                }
            }
        }

        return contrato;
    }
    public void ActualizarContrato(Contratos contrato)
    {
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"
            UPDATE contratos SET
                {nameof(Contratos.Id_inquilino)} = @Id_inquilino,
                {nameof(Contratos.Id_inmueble)} = @Id_inmueble,
                {nameof(Contratos.Monto)} = @Monto,
                {nameof(Contratos.Monto_total)} = @Monto_total,
                {nameof(Contratos.Monto_a_pagar)} = @Monto_a_pagar,
                {nameof(Contratos.Fecha_desde)} = @Fecha_desde,
                {nameof(Contratos.Fecha_hasta)} = @Fecha_hasta,
                {nameof(Contratos.Fecha_final)} = @Fecha_final,
                {nameof(Contratos.Meses)} = @Meses,
                {nameof(Contratos.Estado)} = @Estado
            WHERE {nameof(Contratos.Id_contrato)} = @Id_contrato";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id_contrato", contrato.Id_contrato);
                command.Parameters.AddWithValue("@Id_inquilino", contrato.Id_inquilino);
                command.Parameters.AddWithValue("@Id_inmueble", contrato.Id_inmueble);
                command.Parameters.AddWithValue("@Monto", contrato.Monto);
                command.Parameters.AddWithValue("@Monto_total", contrato.Monto_total);
                command.Parameters.AddWithValue("@Monto_a_pagar", contrato.Monto_a_pagar);
                command.Parameters.AddWithValue("@Fecha_desde", contrato.Fecha_desde);
                command.Parameters.AddWithValue("@Fecha_hasta", contrato.Fecha_hasta);
                command.Parameters.AddWithValue("@Fecha_final", (object?)contrato.Fecha_final ?? DBNull.Value);
                command.Parameters.AddWithValue("@Meses", contrato.Meses);
                command.Parameters.AddWithValue("@Estado", contrato.Estado);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
public void EliminarContrato(int id)
{
    using (var connection = new MySqlConnection(ConectionString))
    {
        connection.Open();
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                // 1. Eliminar multas asociadas
                var queryMultas = "DELETE FROM multas WHERE Id_contrato = @Id";
                using (var cmdMultas = new MySqlCommand(queryMultas, connection, transaction))
                {
                    cmdMultas.Parameters.AddWithValue("@Id", id);
                    cmdMultas.ExecuteNonQuery();
                }

                // 2. Eliminar pagos asociados
                var queryPagos = "DELETE FROM pagos WHERE Id_contrato = @Id";
                using (var cmdPagos = new MySqlCommand(queryPagos, connection, transaction))
                {
                    cmdPagos.Parameters.AddWithValue("@Id", id);
                    cmdPagos.ExecuteNonQuery();
                }

                // 3. Finalmente, eliminar el contrato
                var queryContrato = "DELETE FROM contratos WHERE Id_contrato = @Id";
                using (var cmdContrato = new MySqlCommand(queryContrato, connection, transaction))
                {
                    cmdContrato.Parameters.AddWithValue("@Id", id);
                    cmdContrato.ExecuteNonQuery();
                }

                transaction.Commit(); // Confirmar todos los cambios
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Deshacer todo si algo falló
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}

    public List<Pagos> ObtenerPagosPaginados(int id, int pagina, int tamanoPagina)
    {
        List<Pagos> pagos = new List<Pagos>();
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"SELECT
                            p.Id_pago,
                            p.Id_contrato,
                            p.Detalle,
                            p.Fecha_creacion,
                            p.Creado_por,
                            p.Anulado_por,
                            p.Estado,
                            p.Monto
                        FROM pagos p
                        JOIN contratos c
                            ON p.Id_contrato = c.Id_contrato
                        WHERE
                            c.Id_contrato = @id
                        ORDER BY p.Fecha_creacion DESC
                        LIMIT @limit OFFSET @offset;";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@limit", tamanoPagina);
                command.Parameters.AddWithValue("@offset", (pagina - 1) * tamanoPagina);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                int creadoPorIndex = reader.GetOrdinal("Creado_por");
                int anuladoPorIndex = reader.GetOrdinal("Anulado_por");
                while (reader.Read())
                {
                    pagos.Add(new Pagos
                    {
                        Id_contrato = reader.GetInt32(nameof(Pagos.Id_contrato)),
                        Id_pago = reader.GetInt32(nameof(Pagos.Id_pago)),
                        Detalle = reader.GetString(nameof(Pagos.Detalle)),
                        Fecha_creacion = reader.GetDateTime(nameof(Pagos.Fecha_creacion)),
                        Creado_por = !reader.IsDBNull(creadoPorIndex) ? reader.GetString("Creado_por") : null,
                        Anulado_por = !reader.IsDBNull(anuladoPorIndex) ? reader.GetString("Anulado_por") : null,
                        Monto = reader.GetDecimal(nameof(Pagos.Monto)),
                        Estado = reader.GetBoolean(nameof(Pagos.Estado))

                    });
                }
            }
        }
        return pagos;
    }
    public int ContarPagos(int id)
    {
        using (var connection = new MySqlConnection(ConectionString))
        {
            var query = "SELECT COUNT(*) FROM  pagos WHERE Id_contrato = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
    public void AgregarPago(Pagos nuevoPago)
    {

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            connection.Open();

            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {

                    var queryUpdate = @"UPDATE contratos 
                                        SET Monto_a_pagar = Monto_a_pagar - @Monto 
                                        WHERE Id_contrato = @Id_contrato";

                    using (MySqlCommand command = new MySqlCommand(queryUpdate, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@Monto", nuevoPago.Monto);
                        command.Parameters.AddWithValue("@Id_contrato", nuevoPago.Id_contrato);
                        command.ExecuteNonQuery();
                    }


                    var queryInsert = $@"INSERT INTO pagos ({nameof(Pagos.Id_contrato)}, {nameof(Pagos.Detalle)}, {nameof(Pagos.Monto)},{nameof(Pagos.Creado_por)})
                                        VALUES (@Id_contrato, @Detalle, @Monto,@Creado_por)";

                    using (MySqlCommand command = new MySqlCommand(queryInsert, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@Id_contrato", nuevoPago.Id_contrato);
                        command.Parameters.AddWithValue("@Detalle", nuevoPago.Detalle);
                        command.Parameters.AddWithValue("@Monto", nuevoPago.Monto);
                        command.Parameters.AddWithValue("@Creado_por", nuevoPago.Creado_por);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    throw;
                }
                finally
                {

                }
            }
        }
    }
    public void ActualizarContratoCompletado(int Id)
    {
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"UPDATE contratos 
                      SET Contrato_completado = true 
                      WHERE Id_contrato = @Id_contrato";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id_contrato", Id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    public void AnularPago(int Id_pago, int Id_contrato, decimal Monto,string Anulado_por)
    {

        if (Monto <= 0)
        {

            return;
        }

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            connection.Open();

            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    var queryPago = @"UPDATE pagos SET Estado = false ,Anulado_por = @Anulado_por	WHERE Id_pago = @Id_pago";
                    using (MySqlCommand cmdPago = new MySqlCommand(queryPago, connection, transaction))
                    {
                        cmdPago.Parameters.AddWithValue("@Id_pago", Id_pago);
                        cmdPago.Parameters.AddWithValue("@Anulado_por", Anulado_por);
                        cmdPago.ExecuteNonQuery();
                    }


                    var queryContrato = @"UPDATE contratos 
                                      SET Monto_a_pagar = Monto_a_pagar + @Monto,
                                          Contrato_completado = false 
                                      WHERE Id_contrato = @Id_contrato";
                    using (MySqlCommand cmdContrato = new MySqlCommand(queryContrato, connection, transaction))
                    {
                        cmdContrato.Parameters.AddWithValue("@Monto", Monto);
                        cmdContrato.Parameters.AddWithValue("@Id_contrato", Id_contrato);
                        cmdContrato.ExecuteNonQuery();
                    }


                    transaction.Commit();
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    throw;
                }
            }
        }
    }
    public Pagos? BuscarPagoPorId(int id)
    {
        Pagos? pago = null;

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = @"SELECT
                            Id_pago,
                            Id_contrato,
                            Detalle,
                            Monto,
                            Fecha_creacion,
                            Estado
                        FROM
                            pagos
                        WHERE Id_pago = @Id";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pago = new Pagos
                        {
                            Id_pago = reader.GetInt32("Id_pago"),
                            Id_contrato = reader.GetInt32("Id_contrato"),
                            Detalle = reader.GetString("Detalle"),
                            Monto = reader.GetDecimal("Monto"),
                            Fecha_creacion = reader.GetDateTime("Fecha_creacion"),
                            Estado = reader.GetBoolean("Estado")

                        };
                    }
                }
            }
        }

        return pago;
    }
    public void EliminarPago(int id)
    {
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            var query = $@"DELETE FROM pagos WHERE {nameof(Pagos.Id_pago)} = @Id AND Estado = false";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
    public List<Multas> ObtenerMultasPaginados(int idContrato, int pagina, int tamanoPagina)
    {
        List<Multas> multas = new List<Multas>();
        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            
            var query = @"SELECT
                        m.Id_multa,
                        m.Id_contrato,
                        m.Razon_multa,
                        m.Monto,
                        m.Fecha
                    FROM multas m
                    WHERE m.Id_contrato = @idContrato
                    ORDER BY m.Fecha DESC -- Ordenamos por fecha, de más nueva a más vieja
                    LIMIT @limit OFFSET @offset;";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                int offset = (pagina - 1) * tamanoPagina;
                command.Parameters.AddWithValue("@idContrato", idContrato);
                command.Parameters.AddWithValue("@limit", tamanoPagina);
                command.Parameters.AddWithValue("@offset", offset);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                       
                        multas.Add(new Multas
                        {
                            Id_multa = reader.GetInt32("Id_multa"),
                            Id_contrato = reader.GetInt32("Id_contrato"),
                            Razon_multa = reader.GetString("Razon_multa"),
                            Monto = reader.GetDecimal("Monto"),
                            Fecha = reader.GetDateTime("Fecha")
                        });
                    }
                }
            }
        }
        return multas;
    }
    public int ContarMultas(int idContrato)
    {
        using (var connection = new MySqlConnection(ConectionString))
        {

            var query = "SELECT COUNT(*) FROM multas WHERE Id_contrato = @idContrato";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idContrato", idContrato);
                connection.Open();


                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
    public void AnularContrato(Contratos contrato, Multas multa)
    {

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            connection.Open();

            MySqlTransaction transaction = connection.BeginTransaction();

            try
            {

                var queryContrato = $@"
                UPDATE contratos SET
                    {nameof(Contratos.Monto_total)} = @Monto_total,
                    {nameof(Contratos.Monto_a_pagar)} = @Monto_a_pagar,
                    {nameof(Contratos.Fecha_final)} = @Fecha_final,
                    {nameof(Contratos.Meses)} = @Meses,
                    {nameof(Contratos.Terminado_por)} = @Terminado_por,
                    {nameof(Contratos.Estado)} = @Estado
                WHERE {nameof(Contratos.Id_contrato)} = @Id_contrato";

                using (MySqlCommand commandContrato = new MySqlCommand(queryContrato, connection, transaction))
                {
                    commandContrato.Parameters.AddWithValue("@Id_contrato", contrato.Id_contrato);
                    commandContrato.Parameters.AddWithValue("@Monto_total", contrato.Monto_total);
                    commandContrato.Parameters.AddWithValue("@Monto_a_pagar", contrato.Monto_a_pagar);
                    commandContrato.Parameters.AddWithValue("@Fecha_final", (object?)contrato.Fecha_final ?? DBNull.Value);
                    commandContrato.Parameters.AddWithValue("@Meses", contrato.Meses);
                    commandContrato.Parameters.AddWithValue("@Terminado_por", contrato.Terminado_por);
                    commandContrato.Parameters.AddWithValue("@Estado", contrato.Estado);

                    commandContrato.ExecuteNonQuery();
                }


                var queryMulta = @"
                INSERT INTO multas (Id_contrato, Monto, Fecha, Razon_multa)
                VALUES (@Id_contrato, @MontoMulta, @FechaMulta, @Razon_multa)";

                using (MySqlCommand commandMulta = new MySqlCommand(queryMulta, connection, transaction))
                {

                    commandMulta.Parameters.AddWithValue("@Id_contrato", contrato.Id_contrato);
                    commandMulta.Parameters.AddWithValue("@MontoMulta", multa.Monto);
                    commandMulta.Parameters.AddWithValue("@FechaMulta", multa.Fecha);
                    commandMulta.Parameters.AddWithValue("@Razon_multa", multa.Razon_multa);

                    commandMulta.ExecuteNonQuery();
                }


                transaction.Commit();
            }
            catch (Exception ex)
            {

                transaction.Rollback();
                Console.WriteLine("Error en la transacción: " + ex.Message);

                throw;
            }

        }
    }
    public List<Contratos> ObtenerContratosActivosPorInmueble(int idInmueble)
    {
        var listaContratos = new List<Contratos>();

        using (MySqlConnection connection = new MySqlConnection(ConectionString))
        {
            // La consulta ahora filtra por Id_inmueble y por Estado = 1 (o true)
            var query = @"
            SELECT Fecha_desde, Fecha_hasta
            FROM contratos
            WHERE Id_inmueble = @IdInmueble AND Estado = 1";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdInmueble", idInmueble);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    // Usamos un 'while' porque pueden existir múltiples contratos
                    while (reader.Read())
                    {
                        listaContratos.Add(new Contratos
                        {
                            Fecha_desde = reader.GetDateTime("Fecha_desde"),
                            Fecha_hasta = reader.GetDateTime("Fecha_hasta"),
                        });
                    }
                }
            }
        }
        return listaContratos;
    }

}