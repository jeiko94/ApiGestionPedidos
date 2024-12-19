using ApiGestionPedidos.Dominio.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionPedidos.Infraestructura.Data
{
    //DbContext principal que mapeara nuestras entidades de dominio a tablas sql server
    public class AplicacionDbContext :  DbContext
    {
        //Constructor que recibe opciones de configuración para el DbContext
        public AplicacionDbContext(DbContextOptions<AplicacionDbContext> options)
            :base(options)
        {
        }

        //Tabla de clientes
        public DbSet<Cliente> Clientes { get; set; }

        //Tabla productos
        public DbSet<Producto> Productos { get; set; }

        //Tabla Pedidos
        public DbSet<Pedido> Pedidos { get; set; }

        //Tabla detalles de pedidos
        public DbSet<DetallePedido> DetallesPedido { get; set; }

        //Configuraciones especificas del modelo (relaciones, contrains)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Configurar llaves primarias compuestas, relaciones, etc

            //Detallepedido tiene una llave compuesta {PedidoId, ProductoId}.
            modelBuilder.Entity<DetallePedido>()
                .HasKey(d => new { d.PedidoId, d.ProductoId });

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetallePedido>()
                .Property(d => d.PrecioUnitario)
                .HasPrecision(18, 2);


            // Relación uno a muchos: Pedido -> DetallePedido
            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Pedido)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.PedidoId);


            //Relacion uno a muchos: Producto -> DetallePedido
            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId);
        }
    }
}
