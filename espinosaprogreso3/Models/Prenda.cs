using SQLite;
using System.ComponentModel.DataAnnotations;

namespace espinosaprogreso3.Models
{
    public class Prenda
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required(ErrorMessage = "La prenda es requerida")]
        public string NombrePrenda { get; set; } = string.Empty;

        [Required(ErrorMessage = "El color es requerido")]
        public string Color { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "La talla debe ser mayor a 0")]
        public int Talla { get; set; }

        public bool EnInventario { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
