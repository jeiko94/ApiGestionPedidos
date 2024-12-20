using System.ComponentModel.DataAnnotations;

namespace ApiGestionPedidos.Api.DTOs
{
    public class CrearProductoDto
    {
        [Required(ErrorMessage = "El nombre del producto es obligatoria.")]
        public string? Nombre { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El Stock debe ser mayor a cero.")]
        public int Stock { get; set; }
    }
}
