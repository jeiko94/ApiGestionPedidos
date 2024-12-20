using ApiGestionPedidos.Aplicacion.Repositorios;
using ApiGestionPedidos.Dominio.Models;
using ApiGestionPedidos.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionPedidos.Infraestructura.Repositorios
{
    //Implementacion concreta de IPedidoRepositorio usando con EF core
    public class PedidoRepositorio : IPedidoRepositorio
    {
        private readonly AplicacionDbContext _context;

        public PedidoRepositorio(AplicacionDbContext context)
        {
            _context = context;
        }

        public async Task CrearAsync(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<Pedido> ObtenerPorIdAsync(int id)
        {
            //Incluir detalles y productos para tener info completa
            return await _context.Pedidos
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pedido>> ObtenerPorClienteAsync(int clienteId)
        {
            //Retornar los pedidos de un cliente, incluyendo detalles
            return await _context.Pedidos
                .Where(p => p.ClienteId == clienteId)
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
                .ToListAsync();
        }

        public async Task ActualizarAsync(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int Id)
        {
            var pedido = await ObtenerPorIdAsync(Id);

            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
        }
    }
}
