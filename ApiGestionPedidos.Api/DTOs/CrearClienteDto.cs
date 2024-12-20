using System.ComponentModel.DataAnnotations;

namespace ApiGestionPedidos.Api.DTOs
{
    //DTO para crear un nuevo cliente. Se utiliza en solicitudes POST /clientes.
    public class CrearClienteDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato email invalido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string? Password { get; set; }
    }
}
