namespace ApiGestionPedidos.Api.DTOs
{
    public class PedidoDto
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public string? Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public List<DetallePedidoDto> Detalles { get; set; } = new List<DetallePedidoDto>();

        public decimal Total { get; set; }
    }
}
