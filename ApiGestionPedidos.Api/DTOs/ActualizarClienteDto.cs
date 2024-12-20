using System.ComponentModel.DataAnnotations;

namespace ApiGestionPedidos.Api.DTOs
{
    //DTO para actualizar datos de un cliente (PUT /clientes/{id})
    public class ActualizarClienteDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatori.")]
        [EmailAddress(ErrorMessage = "Formato de email invalido.")]
        public string? Email { get; set; }
    }
}
