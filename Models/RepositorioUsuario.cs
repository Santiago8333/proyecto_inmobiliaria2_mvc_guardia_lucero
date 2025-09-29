using MySql.Data.MySqlClient;

namespace proyecto_inmobiliaria2_mvc_guardia_lucero.Models
{
    public class RepositorioUsuario : RepositorioBase
    {
        public RepositorioUsuario(IConfiguration configuration) : base(configuration)
        {

        }

        public List<Usuarios> ObtenerPaginados(int pagina, int tamanoPagina)
        {
            List<Usuarios> usuarios = new List<Usuarios>();
            using (MySqlConnection connection = new MySqlConnection(ConectionString))
            {
                var query = @"SELECT Id_usuario, Nombre, Apellido, Email, AvatarUrl, Rol, RolNombre,Fecha_creacion, Estado
                FROM usuarios
                ORDER BY Id_usuario
                LIMIT @limit OFFSET @offset";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@limit", tamanoPagina);
                    command.Parameters.AddWithValue("@offset", (pagina - 1) * tamanoPagina);

                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuarios
                        {
                            Id_usuario = reader.GetInt32(nameof(Usuarios.Id_usuario)),
                            Nombre = reader.GetString(nameof(Usuarios.Nombre)),
                            Apellido = reader.GetString(nameof(Usuarios.Apellido)),
                            Email = reader.GetString(nameof(Usuarios.Email)),
                            AvatarUrl = reader.GetString(nameof(Usuarios.AvatarUrl)),
                            Rol = reader.GetInt32(nameof(Usuarios.Rol)),
                            RolNombre = reader.GetString(nameof(Usuarios.RolNombre)),
                            Fecha_creacion = reader.GetDateTime(nameof(Usuarios.Fecha_creacion)),
                            Estado = reader.GetBoolean(nameof(Usuarios.Estado))
                        });
                    }
                }
            }
            return usuarios;
        }

        public int ContarUsuarios()
        {
            using (var connection = new MySqlConnection(ConectionString))
            {
                var query = "SELECT COUNT(*) FROM usuarios";
                using (var command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }


        public async Task<Usuarios?> ObtenerPorEmailAsync(string email)
        {
            Usuarios? res = null;

            using (var connection = new MySqlConnection(ConectionString))
            {
                var query = @"SELECT Id_usuario, Nombre, Apellido, Email, RolNombre, Clave , AvatarUrl
                      FROM usuarios
                      WHERE Email = @Email AND Estado = true";

                using (var command = new MySqlCommand(query, connection))
                {
                    // Agrega el parámetro Email
                    command.Parameters.AddWithValue("@Email", email);

                    await connection.OpenAsync(); // Abre la conexión de manera asincrónica

                    using (var reader = await command.ExecuteReaderAsync()) // Ejecuta el lector asincrónicamente
                    {
                        if (await reader.ReadAsync()) // Lee de forma asincrónica
                        {
                            res = new Usuarios
                            {
                                Id_usuario = reader.GetInt32(reader.GetOrdinal("Id_usuario")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                RolNombre = reader.GetString(reader.GetOrdinal("RolNombre")),
                                Clave = reader.GetString(reader.GetOrdinal("Clave")),
                                AvatarUrl = reader.GetString(reader.GetOrdinal("AvatarUrl"))
                            };
                        }
                    }
                }
            }

            return res; // Retorna el usuario o null si no se encontró
        }


        public void AgregarUsuario(Usuarios nuevoUsuario)
        {
            using (MySqlConnection connection = new MySqlConnection(ConectionString))
            {
                var query = $@"INSERT INTO usuarios ({nameof(Usuarios.Nombre)}, {nameof(Usuarios.Apellido)}, {nameof(Usuarios.Email)}, {nameof(Usuarios.Clave)}, {nameof(Usuarios.Rol)}, {nameof(Usuarios.RolNombre)},{nameof(Usuarios.Estado)},{nameof(Usuarios.AvatarUrl)})
                    VALUES (@Nombre, @Apellido, @Email,@Clave,@Rol, @RolNombre, @Estado,@AvatarFile)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Nombre", nuevoUsuario.Nombre);
                    command.Parameters.AddWithValue("@Apellido", nuevoUsuario.Apellido);
                    command.Parameters.AddWithValue("@Email", nuevoUsuario.Email);
                    command.Parameters.AddWithValue("@Clave", nuevoUsuario.Clave);
                    command.Parameters.AddWithValue("@Rol", nuevoUsuario.Rol);
                    command.Parameters.AddWithValue("@RolNombre", nuevoUsuario.RolNombre);
                    command.Parameters.AddWithValue("@AvatarFile", nuevoUsuario.AvatarUrl);
                    command.Parameters.AddWithValue("@Estado", true);

                    connection.Open();
                    command.ExecuteNonQuery(); // Ejecuta la consulta de inserción
                    connection.Close();
                }
            }
        }


        public void ActualizarUsuario(Usuarios actualizarUsuario)
        {
            using (MySqlConnection connection = new MySqlConnection(ConectionString))
            {
                var query = $@"UPDATE usuarios
                    SET 
                        {nameof(Usuarios.Nombre)} = @Nombre, 
                        {nameof(Usuarios.Apellido)} = @Apellido, 
                        {nameof(Usuarios.Email)} = @Email, 
                        {nameof(Usuarios.Rol)} = @Rol, 
                        {nameof(Usuarios.RolNombre)} = @RolNombre
                    WHERE Id_usuario = @Id AND Estado = true";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Id", actualizarUsuario.Id_usuario);
                    command.Parameters.AddWithValue("@Nombre", actualizarUsuario.Nombre);
                    command.Parameters.AddWithValue("@Apellido", actualizarUsuario.Apellido);
                    command.Parameters.AddWithValue("@Email", actualizarUsuario.Email);
                    command.Parameters.AddWithValue("@Rol", actualizarUsuario.Rol);
                    command.Parameters.AddWithValue("@RolNombre", actualizarUsuario.RolNombre);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        public void ActualizarUsuarioPerfil(Usuarios actualizarUsuario)
        {
            using (MySqlConnection connection = new MySqlConnection(ConectionString))
            {
                var query = $@"UPDATE usuarios
                    SET 
                        {nameof(Usuarios.Nombre)} = @Nombre, 
                        {nameof(Usuarios.Apellido)} = @Apellido, 
                        {nameof(Usuarios.Email)} = @Email
                    WHERE Id_usuario = @Id AND Estado = true";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Id", actualizarUsuario.Id_usuario);
                    command.Parameters.AddWithValue("@Nombre", actualizarUsuario.Nombre);
                    command.Parameters.AddWithValue("@Apellido", actualizarUsuario.Apellido);
                    command.Parameters.AddWithValue("@Email", actualizarUsuario.Email);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        public void ActualizarEditarClave(Usuarios actualizarUsuario)
        {
            using (MySqlConnection connection = new MySqlConnection(ConectionString))
            {
                var query = $@"UPDATE usuarios
                    SET 
                        {nameof(Usuarios.Clave)} = @Clave
                    WHERE Id_usuario = @Id AND Estado = true";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Id", actualizarUsuario.Id_usuario);
                    command.Parameters.AddWithValue("@Clave", actualizarUsuario.Clave);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        public void ActualizarEditarAvatar(Usuarios actualizarUsuario)
        {
            using (MySqlConnection connection = new MySqlConnection(ConectionString))
            {
                var query = $@"UPDATE usuarios
                    SET 
                        {nameof(Usuarios.AvatarUrl)} = @AvatarUrl
                    WHERE Id_usuario = @Id AND Estado = true";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Id", actualizarUsuario.Id_usuario);
                    command.Parameters.AddWithValue("@AvatarUrl", actualizarUsuario.AvatarUrl);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        public Usuarios? ObtenerPorID(int id)
        {
            Usuarios? res = null;
            using (MySqlConnection connection = new MySqlConnection(ConectionString))
            {
                var query = @"SELECT Id_usuario,Apellido, Nombre, Email, Clave,AvatarUrl,Rol,RolNombre,Estado
                      FROM usuarios
                      WHERE Id_usuario = @Id AND Estado = 1";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Agrega el parámetro id
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            res = new Usuarios
                            {
                                Id_usuario = reader.GetInt32(nameof(Usuarios.Id_usuario)),
                                Apellido = reader.GetString(nameof(Usuarios.Apellido)),
                                Nombre = reader.GetString(nameof(Usuarios.Nombre)),
                                Email = reader.GetString(nameof(Usuarios.Email)),
                                Clave = reader.GetString(nameof(Usuarios.Clave)),
                                Rol = reader.GetInt32(nameof(Usuarios.Rol)),
                                RolNombre = reader.GetString(nameof(Usuarios.RolNombre)),
                                AvatarUrl = reader.GetString(nameof(Usuarios.AvatarUrl)),
                                Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Usuarios.Estado)))
                            };
                        }
                    }
                }
            }
            return res;
        }

        public void EliminarUsuario(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(ConectionString))
            {
                var query = "DELETE FROM usuarios WHERE Id_usuario = @Id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        
        
    }
}