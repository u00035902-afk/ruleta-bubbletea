using System.ComponentModel.DataAnnotations;

namespace RuletanBublee.Models
{
    public class GiroResultado
    {
        public int Id { get; set; }
        public string Premio { get; set; }
        public int IndiceVisual { get; set; }
        public bool Entregado { get; set; }
    }
}
