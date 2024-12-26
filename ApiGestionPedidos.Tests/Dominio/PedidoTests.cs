using ApiGestionPedidos.Dominio.Models;

namespace ApiGestionPedidos.Tests.Dominio
{
    public class PedidoTests
    {
        [Fact]
        public void CalcularTotal_DeberiaSumarSubtotalDeDetalles()
        {
            //Arrange: crear un pedido con detalles
            var pedido = new Pedido
            {
                Detalles = new List<DetallePedido>
                {
                    new DetallePedido { Cantidad = 2, PrecioUnitario = 10m },
                    new DetallePedido { Cantidad = 1, PrecioUnitario = 20m }
                }
            };

            //Act: invocar CalcularTotal
            decimal total = pedido.CalcularTotal();

            //Assert: validar el resultado esperado
            Assert.Equal(40m, total); //2*10 + 1*20 = 40
        }

        [Fact]
        public void CalcularTotal_SinDetaller_DeberiaRetornarCero()
        {
            //Arrange
            var pedido = new Pedido(); //Sin detalles

            //Act
            decimal total = pedido.CalcularTotal();

            //Assert
            Assert.Equal(0, total);
        }
    }
}