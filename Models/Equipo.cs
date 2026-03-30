using System;

namespace AdminTallerNenufar.Models
{
    public class Equipo
    {
        // Identificadores numéricos
        public int IdEquipo { get; set; }
        public int IdCliente { get; set; } // Llave foránea (Relación con el dueño)

        // Inicializamos con string.Empty para eliminar advertencias CS8618
        public string NombreEquipo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Subcategoria { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        // Fecha de ingreso (por defecto la de hoy)
        public string FechaIngreso { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public Equipo() { }
    }
}