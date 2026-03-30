using System;
using System.Data;
using System.Data.SQLite;
using AdminTallerNenufar.Models;

namespace AdminTallerNenufar.Data
{
    public class EquipoRepository
    {
        /// <summary>
        /// Guarda los datos técnicos del equipo vinculado a un cliente.
        /// </summary>
        public bool GuardarEquipo(Equipo equipo)
        {
            try
            {
                using (var conexion = ConexionDB.ObtenerConexion())
                {
                    conexion.Open();
                    string sql = @"INSERT INTO Equipos (IdCliente, NombreEquipo, Marca, Modelo, Categoria, Subcategoria, Descripcion) 
                                   VALUES (@idC, @nom, @mar, @mod, @cat, @sub, @desc)";

                    using (var comando = new SQLiteCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@idC", equipo.IdCliente);
                        comando.Parameters.AddWithValue("@nom", equipo.NombreEquipo ?? string.Empty);
                        comando.Parameters.AddWithValue("@mar", equipo.Marca ?? string.Empty);
                        comando.Parameters.AddWithValue("@mod", equipo.Modelo ?? string.Empty);
                        comando.Parameters.AddWithValue("@cat", equipo.Categoria ?? string.Empty);
                        comando.Parameters.AddWithValue("@sub", equipo.Subcategoria ?? string.Empty);
                        comando.Parameters.AddWithValue("@desc", equipo.Descripcion ?? string.Empty);

                        return comando.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtiene la lista completa de equipos con el nombre del cliente asociado.
        /// </summary>
        public DataTable ObtenerTodosLosEquipos()
        {
            DataTable tabla = new DataTable();
            try
            {
                using (var conexion = ConexionDB.ObtenerConexion())
                {
                    conexion.Open();
                    // Usamos un JOIN para combinar tablas y traer el nombre legible del cliente
                    string sql = @"SELECT E.IdEquipo, 
                                          (C.Nombre || ' ' || C.Apellido) AS Cliente, 
                                          E.NombreEquipo, E.Marca, E.Modelo, E.Categoria, 
                                          E.Descripcion, E.FechaIngreso 
                                   FROM Equipos E
                                   INNER JOIN Clientes C ON E.IdCliente = C.IdCliente
                                   ORDER BY E.FechaIngreso DESC";

                    using (var comando = new SQLiteCommand(sql, conexion))
                    {
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(comando))
                        {
                            adapter.Fill(tabla);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // En caso de error, devolvemos la tabla vacía para no romper la UI
            }
            return tabla;
        }
    }
}