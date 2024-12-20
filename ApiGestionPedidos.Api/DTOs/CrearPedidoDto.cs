using System.ComponentModel.DataAnnotations;

namespace ApiGestionPedidos.Api.DTOs
{
    public class CrearPedidoDto
    {
        [Required(ErrorMessage = "El clienteId es obligatorio.")]
        public int ClienteId { get; set; }
    }
}
