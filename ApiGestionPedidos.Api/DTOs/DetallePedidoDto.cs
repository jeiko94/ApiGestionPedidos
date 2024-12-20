namespace ApiGestionPedidos.Api.DTOs
{
    public class DetallePedidoDto
    {
        public int ProductoId { get; set; }

        public string? NombreProducto { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }
    }
}
