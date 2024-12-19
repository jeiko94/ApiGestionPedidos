using ApiGestionPedidos.Dominio.Models;

namespace ApiGestionPedidos.Aplicacion.Repositorios
{
    //Define las operaciones de persistencia relacionadas con la entidad pedido y sus detalles
    public interface IPedidoRepositorio
    {
        Task CrearAsync(Pedido pedido);

        Task<Pedido> ObtenerPorIdAsync(int id);

        Task<IEnumerable<Pedido>> ObtenerPorClienteAsync(int clienteId);

        Task ActualizarAsync(Pedido pedido);

        Task EliminarAsync(int Id);
    }
}
