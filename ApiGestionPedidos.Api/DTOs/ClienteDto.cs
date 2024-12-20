namespace ApiGestionPedidos.Api.DTOs
{
    //DTO para representar datos de un cliente al devolver en respuestas GET.
    //No se incluyen contraseñas, por obvia seguridad
    public class ClienteDto
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public string? Email { get; set; }
    }
}
