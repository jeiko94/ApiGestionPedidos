using ApiGestionPedidos.Dominio.Models;
using ApiGestionPedidos.Infraestructura.Data;
using ApiGestionPedidos.Infraestructura.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionPedidos.Tests.Infraestructura
{
    public class ProductoRepositorioTest
    {
        [Fact]
        public async Task CrearProducto_DeberiaGuardarEnDbInMemory()
        {
            //Arrange: Configurar un DbContext in memory
            var options = new DbContextOptionsBuilder<AplicacionDbContext>()
                .UseInMemoryDatabase(databaseName: "testDb_Producto")
                .Options;

            using var context = new AplicacionDbContext(options);
            var repo = new ProductoRepositorio(context);

            //Act
            var producto = new Producto { Nombre = "Test Prod", Precio = 10, Stock = 5 };
            await repo.CrearAsync(producto);

            //Assert
            var productoGuardado = await context.Productos.FindAsync(producto.Id);
            Assert.NotNull(productoGuardado);
            Assert.Equal("Test Prod", productoGuardado.Nombre);
        }
    }
}
