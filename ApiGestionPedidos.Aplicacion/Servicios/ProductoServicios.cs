using ApiGestionPedidos.Aplicacion.Repositorios;
using ApiGestionPedidos.Dominio.Models;

namespace ApiGestionPedidos.Aplicacion.Servicios
{
    //Servicio de apliacion para manejar logica relacionada con productos
    public class ProductoServicios
    {
        private readonly IProductoRepositorio _productoRepositorio;

        public ProductoServicios(IProductoRepositorio productoRepositorio)
        {
            _productoRepositorio = productoRepositorio;
        }

        //Crear un nuevo producto en el sistema
        public async Task CrearProductoAsync(string nombre, decimal precio, int stock)
        {
            var nuevoProducto = new Producto
            {
                Nombre = nombre,
                Precio = precio,
                Stock = stock
            };

            await _productoRepositorio.CrearAsync(nuevoProducto);
        }

        //Obtiene un producto por su id
        public async Task<Producto> ObtenerProductoAsync(int id)
        {
            return await _productoRepositorio.ObtenerPorIdAsync(id);
        }

        //Listar todos los productos del sistema
        public async Task<IEnumerable<Producto>> ListarProductosAsync()
        {
            return await _productoRepositorio.ObtenerTodosAsync();
        }

        //Actualizar datos de un producto
        public async Task ActualizarProductoAsync(int id, string nombre, decimal precio, int stock)
        {
            var producto = await _productoRepositorio.ObtenerPorIdAsync(id);

            if (producto == null)
                throw new KeyNotFoundException("Producto no encontrado.");

            producto.Nombre = nombre;
            producto.Precio = precio;
            producto.Stock = stock;

            await _productoRepositorio.ActualizarAsync(producto);
        }

        public async Task EliminarProductoAsync(int id)
        {
            await _productoRepositorio.EliminarAsync(id);
        }
    }
}
