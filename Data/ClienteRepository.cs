using System;
using System.Data.SQLite;
using AdminTallerNenufar.Models;

namespace AdminTallerNenufar.Data
{
    public class ClienteRepository
    {
        /// <summary>
        /// Guarda un nuevo cliente y retorna el ID generado.
        /// </summary>
        public int GuardarCliente(Cliente cliente)
        {
            try
            {
                using (var conexion = ConexionDB.ObtenerConexion())
                {
                    conexion.Open();

                    // Insertamos y pedimos el ID en la misma transacción
                    string sql = @"INSERT INTO Clientes (Cedula, Nombre, Apellido, Telefono) 
                                   VALUES (@cedula, @nombre, @apellido, @telefono);
                                   SELECT last_insert_rowid();";

                    using (var comando = new SQLiteCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@cedula", cliente.Cedula ?? string.Empty);
                        comando.Parameters.AddWithValue("@nombre", cliente.Nombre ?? string.Empty);
                        comando.Parameters.AddWithValue("@apellido", cliente.Apellido ?? string.Empty);
                        comando.Parameters.AddWithValue("@telefono", cliente.Telefono ?? string.Empty);

                        object resultado = comando.ExecuteScalar();

                        if (resultado != null && resultado != DBNull.Value)
                        {
                            return Convert.ToInt32(resultado);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Si hay un error (ej. cédula duplicada), lanzamos la excepción para que la vista la capture
                throw;
            }
            return 0;
        }

        /// <summary>
        /// Busca un cliente por su cédula. Retorna null si no se encuentra.
        /// </summary>
        // El '?' soluciona la advertencia CS8603 al permitir explícitamente el retorno de un null
        public Cliente? BuscarPorCedula(string cedula)
        {
            using (var conexion = ConexionDB.ObtenerConexion())
            {
                conexion.Open();
                string sql = "SELECT * FROM Clientes WHERE Cedula = @cedula LIMIT 1";

                using (var comando = new SQLiteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@cedula", cedula);
                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cliente
                            {
                                IdCliente = Convert.ToInt32(reader["IdCliente"]),
                                Cedula = reader["Cedula"]?.ToString() ?? string.Empty,
                                Nombre = reader["Nombre"]?.ToString() ?? string.Empty,
                                Apellido = reader["Apellido"]?.ToString() ?? string.Empty,
                                Telefono = reader["Telefono"]?.ToString() ?? string.Empty
                            };
                        }
                    }
                }
            }
            return null; // Ahora el compilador sabe que este null es intencional
        }
    }
}