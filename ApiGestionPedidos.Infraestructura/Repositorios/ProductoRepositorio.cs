using ApiGestionPedidos.Aplicacion.Repositorios;
using ApiGestionPedidos.Dominio.Models;
using ApiGestionPedidos.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionPedidos.Infraestructura.Repositorios
{
    //Implementacion concreta de IProductoRepositorio
    public class ProductoRepositorio : IProductoRepositorio
    {
        private readonly AplicacionDbContext _context;

        public ProductoRepositorio(AplicacionDbContext context)
        {
            _context = context;
        }

        public async Task CrearAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task<Producto> ObtenerPorIdAsync(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task ActualizarAsync(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int Id)
        {
            var producto = await ObtenerPorIdAsync(Id);

            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
