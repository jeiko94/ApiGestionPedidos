using System.ComponentModel.DataAnnotations;

namespace ApiGestionPedidos.Api.DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }
        
        public string? Nombre { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }
    }
}
