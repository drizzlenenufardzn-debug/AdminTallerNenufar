using System;

namespace AdminTallerNenufar.Models
{
    // Ahora es público y compatible con los estándares actuales de .NET
    public class Cliente
    {
        // El ID es autoincremental, así que no hay problema
        public int IdCliente { get; set; }

        // Agregamos 'string.Empty' para evitar la advertencia CS8618
        // Esto le dice al programa: "Si no hay datos, empieza con un texto vacío"
        public string Cedula { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        // Constructor vacío (necesario para SQLite y otros procesos)
        public Cliente() { }

        // Opcional: Para mostrar el nombre en listas o etiquetas
        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}