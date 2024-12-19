using System.ComponentModel.DataAnnotations;

namespace ApiGestionPedidos.Dominio.Models
{
    //Representa un cliente en el sistema, con sus datos basicos
    public class Cliente
    {
        //Identificador unico del cliente
        public int Id { get; set; }

        //Nombre completo del cliente
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string? Nombre { get; set; }

        //El email debe ser unico
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es valido.")]
        public string? Email { get; set; }

        //Contraseña hasheada del cliente para su autenticación
        //No se guarda la contraseña en texto plano
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string? PasswordHashed { get; set; }

        //Fecha de resgistro del cliente en el sistema
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;


    }
}
