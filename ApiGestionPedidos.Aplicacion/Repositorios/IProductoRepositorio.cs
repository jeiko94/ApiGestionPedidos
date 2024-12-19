using ApiGestionPedidos.Dominio.Models;

namespace ApiGestionPedidos.Aplicacion.Repositorios
{
    //Define las operaciones de persistencia relacionadas con la entidad producto
    public interface IProductoRepositorio
    {
        Task CrearAsync(Producto producto);

        Task<Producto> ObtenerPorIdAsync(int id);

        Task<IEnumerable<Producto>> ObtenerTodosAsync();

        Task ActualizarAsync(Producto producto);

        Task EliminarAsync(int Id);
    }
}
