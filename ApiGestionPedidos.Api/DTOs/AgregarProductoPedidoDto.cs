using System.ComponentModel.DataAnnotations;

namespace ApiGestionPedidos.Api.DTOs
{
    public class AgregarProductoPedidoDto
    {
        [Required(ErrorMessage = "El productoId es obligatorio.")]
        public int ProductoId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad debe ser almenos 1.")]
        public int Cantidad { get; set; }
    }
}
