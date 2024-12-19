namespace ApiGestionPedidos.Dominio.Models
{
    //Representa un pedido realizado por un cliente
    public class Pedido
    {
        //Identificador unico del pedido
        public int Id { get; set; }

        //Identificador del cliente que realizo el pedido
        public int ClienteId { get; set; }

        //Estado actual del pedido
        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

        //Fecha de creación del pedido
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        //Lista de detalles (productos y cantidades) asociados a este pedido
        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();

        //Propiedad de navegación al cliente
        public Cliente? Cliente { get; set; }

        //Calcular el total del pedido sumando cantidad * precio unitario de cada detalle.
        public decimal CalcularTotal()
        {
            decimal total = 0;
            foreach (var item in Detalles)
            {
                total += item.Cantidad * item.PrecioUnitario;
            }

            return total;
        }
    }
}
