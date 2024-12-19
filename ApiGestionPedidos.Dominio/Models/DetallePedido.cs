using System.ComponentModel.DataAnnotations;

namespace ApiGestionPedidos.Dominio.Models
{
    //Detalle de un pedido, asociado a un producto con su cantidad y precio unitario en el momento del pedido
    public class DetallePedido
    {
        //Identificador del pedido al que pertenece este detalle
        public int PedidoId { get; set; }

        //Identificador del producto.
        public int ProductoId { get; set; }

        //Cantidad del producto en este pedido.
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; }

        //Precio unitario del producto al momento de crear el pedido.
        //Esto permite mantener el historico de precios.
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal PrecioUnitario { get; set; }

        //Propiedad de navegación al pedido
        public Pedido? Pedido { get; set; }

        //Propiedad de navegación al producto
        public Producto? Producto { get; set; }
    }
}
