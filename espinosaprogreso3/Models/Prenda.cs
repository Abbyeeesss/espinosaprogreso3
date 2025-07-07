using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace espinosaprogreso3.Models
{
    public class Prenda
    {
        public int Id { get; set; }

        public string PrendaNombre { get; set; }
        public string Color { get; set; }
        public int Talla { get; set; }
        public bool EnInventario { get; set; }
    }
}