using ApiGestionPedidos.Aplicacion.Repositorios;
using ApiGestionPedidos.Aplicacion.Servicios;
using ApiGestionPedidos.Dominio.Models;
using Moq;

namespace ApiGestionPedidos.Tests.Aplicacion
{
    public class PedidoServicioTests
    {
        [Fact]
        public async Task AgregarProducto_ProductosSinStock_DeberiaLanzarExcepcion()
        {
            // Arrange
            var mockPedidoRepo = new Mock<IPedidoRepositorio>();
            var mockProductoRepo = new Mock<IProductoRepositorio>();
            var mockClienteRepo = new Mock<IClienteRepositorio>();

            // Simulamos un pedido existente
            var pedidoExistente = new Pedido { Id = 1, Estado = EstadoPedido.Pendiente };
            mockPedidoRepo.Setup(r => r.ObtenerPorIdAsync(1))
                          .ReturnsAsync(pedidoExistente);

            // Simulamos un producto con stock = 0
            var productoSinStock = new Producto { Id = 10, Stock = 0, Precio = 50m };
            mockProductoRepo.Setup(r => r.ObtenerPorIdAsync(10))
                            .ReturnsAsync(productoSinStock);

            var servicio = new PedidoServicio(mockPedidoRepo.Object, mockProductoRepo.Object, mockClienteRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<System.InvalidOperationException>(async () =>
            {
                await servicio.AgregarProductoAsync(1, 10, 2);
            });
        }

        [Fact]
        public async Task AgregarProducto_DeberiaAgregarDetalleAlPedido()
        {
            //Arrage
            var mockPedidoRepo = new Mock<IPedidoRepositorio>();
            var mockProductoRepo = new Mock<IProductoRepositorio>();
            var mockClienteRepo = new Mock<IClienteRepositorio>();

            var pedido = new Pedido { Id = 1, Estado = EstadoPedido.Pendiente, Detalles = new List<DetallePedido>() };
            mockPedidoRepo.Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(pedido);

            var producto = new Producto { Id = 10, Stock = 5, Precio = 50m };
            mockProductoRepo.Setup(p => p.ObtenerPorIdAsync(10))
                .ReturnsAsync(producto);

            var servicio = new PedidoServicio(mockPedidoRepo.Object, mockProductoRepo.Object, mockClienteRepo.Object);

            //Act
            await servicio.AgregarProductoAsync(1, 10, 3);

            //Assert
            //Verificamos que el pedido ahora tiene un detalle con Cantidad = 3, ProductoId = 10, PedidoId = 1
            Assert.Single(pedido.Detalles);
            Assert.Equal(3, pedido.Detalles[0].Cantidad);
            Assert.Equal(10, pedido.Detalles[0].ProductoId);

            //Verificar que se llamo al repositorio para actualizar
            mockPedidoRepo.Verify(r => r.ActualizarAsync(It.IsAny<Pedido>()), Times.Once());
        }
        [Fact]
        public async Task ConfirmarPedido_PendienteConStockSuficiente_DeberiaCambiarEstadoYDescontarStock()
        {
            // Arrange
            var mockPedidoRepo = new Mock<IPedidoRepositorio>();
            var mockProductoRepo = new Mock<IProductoRepositorio>();
            var mockClienteRepo = new Mock<IClienteRepositorio>();

            // Pedido en estado Pendiente con un detalle
            var pedido = new Pedido
            {
                Id = 1,
                Estado = EstadoPedido.Pendiente,
                Detalles = new List<DetallePedido>
        {
            new DetallePedido { ProductoId = 10, Cantidad = 2, PrecioUnitario = 50m }
        }
            };

            mockPedidoRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(pedido);

            // Producto con stock suficiente
            var producto = new Producto { Id = 10, Stock = 5, Precio = 50m };
            mockProductoRepo.Setup(r => r.ObtenerPorIdAsync(10)).ReturnsAsync(producto);

            var servicio = new PedidoServicio(mockPedidoRepo.Object, mockProductoRepo.Object, mockClienteRepo.Object);

            // Act
            await servicio.ConfirmarPedidoAsync(1);

            // Assert
            Assert.Equal(EstadoPedido.Confirmado, pedido.Estado);
            // Verificar que el stock se haya descontado
            Assert.Equal(3, producto.Stock); // 5 - 2 = 3

            // Verificar que se llamara al método ActualizarAsync
            mockProductoRepo.Verify(r => r.ActualizarAsync(It.IsAny<Producto>()), Times.Once);
            mockPedidoRepo.Verify(r => r.ActualizarAsync(It.IsAny<Pedido>()), Times.Once);
        }

        [Fact]
        public async Task ConfirmarPedido_PendienteSinStock_DeberiaLanzarExcepcion()
        {
            // Arrange
            var mockPedidoRepo = new Mock<IPedidoRepositorio>();
            var mockProductoRepo = new Mock<IProductoRepositorio>();
            var mockClienteRepo = new Mock<IClienteRepositorio>();

            var pedido = new Pedido
            {
                Id = 1,
                Estado = EstadoPedido.Pendiente,
                Detalles = new List<DetallePedido>
        {
            new DetallePedido { ProductoId = 10, Cantidad = 2, PrecioUnitario = 50m }
        }
            };

            mockPedidoRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(pedido);

            // Producto con stock insuficiente
            var producto = new Producto { Id = 10, Stock = 1, Precio = 50m };
            mockProductoRepo.Setup(r => r.ObtenerPorIdAsync(10)).ReturnsAsync(producto);

            var servicio = new PedidoServicio(mockPedidoRepo.Object, mockProductoRepo.Object, mockClienteRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => servicio.ConfirmarPedidoAsync(1));
            // Verificamos que NO se llame a ActualizarAsync(pedido)
            mockPedidoRepo.Verify(r => r.ActualizarAsync(It.IsAny<Pedido>()), Times.Never);
        }

        [Fact]
        public async Task EntregarPedido_Confirmado_DeberiaCambiarEstadoAEntregado()
        {
            // Arrange
            var mockPedidoRepo = new Mock<IPedidoRepositorio>();
            var mockProductoRepo = new Mock<IProductoRepositorio>();
            var mockClienteRepo = new Mock<IClienteRepositorio>();

            var pedido = new Pedido { Id = 1, Estado = EstadoPedido.Confirmado };
            mockPedidoRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(pedido);

            var servicio = new PedidoServicio(mockPedidoRepo.Object, mockProductoRepo.Object, mockClienteRepo.Object);

            // Act
            await servicio.EntregarPedidoAsync(1);

            // Assert
            Assert.Equal(EstadoPedido.Entregado, pedido.Estado);
            mockPedidoRepo.Verify(r => r.ActualizarAsync(It.IsAny<Pedido>()), Times.Once);
        }

        [Fact]
        public async Task EntregarPedido_Pendiente_DeberiaLanzarExcepcion()
        {
            // Arrange
            var mockPedidoRepo = new Mock<IPedidoRepositorio>();
            var mockProductoRepo = new Mock<IProductoRepositorio>();
            var mockClienteRepo = new Mock<IClienteRepositorio>();

            var pedido = new Pedido { Id = 1, Estado = EstadoPedido.Pendiente };
            mockPedidoRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(pedido);

            var servicio = new PedidoServicio(mockPedidoRepo.Object, mockProductoRepo.Object, mockClienteRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => servicio.EntregarPedidoAsync(1));
            mockPedidoRepo.Verify(r => r.ActualizarAsync(It.IsAny<Pedido>()), Times.Never);
        }

    }
}
