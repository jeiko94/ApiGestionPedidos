using ApiGestionPedidos.Aplicacion.Repositorios;
using ApiGestionPedidos.Dominio.Models;

namespace ApiGestionPedidos.Aplicacion.Servicios
{
    //Servicio de aplicacion para manejar la ligica relacionada con pedidos
    public class PedidoServicio
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly IProductoRepositorio _productoRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;

        public PedidoServicio(IPedidoRepositorio pedidoRepositorio, IProductoRepositorio productoRepositorio, IClienteRepositorio clienteRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
            _productoRepositorio = productoRepositorio;
            _clienteRepositorio = clienteRepositorio;
        }

        //Crear un pedido vacio para un cliente especifico (estado pendiente)
        public async Task<int> CrearPedidoAsync(int clienteId)
        {
            //verificar que el cliente exite
            var cliente = await _clienteRepositorio.ObtenerPorIdAsync(clienteId);

            if (cliente == null)
                throw new InvalidOperationException("EL cliente no existe.");

            var pedido = new Pedido
            {
                ClienteId = clienteId,
                FechaCreacion = DateTime.UtcNow,
                Estado = EstadoPedido.Pendiente
            };

            await _pedidoRepositorio.CrearAsync(pedido);
            return pedido.Id;
        }

        //Agregar un producto al pedido con la cantidad especifica.
        //Asume que el pedido esta en estado pendiente.
        public async Task AgregarProductoAsync(int pedidoId, int productoId, int cantidad)
        {
            //Verificar pedido existe
            var pedido = await _pedidoRepositorio.ObtenerPorIdAsync(pedidoId);

            if (pedido == null)
                throw new InvalidOperationException("Pedido no encontrado.");

            if (pedido.Estado != EstadoPedido.Pendiente)
                throw new InvalidOperationException("No se puede agregar productos un pedido no pendiente.");

            //verificar producto
            var producto = await _productoRepositorio.ObtenerPorIdAsync(productoId);

            if (producto == null)
                throw new InvalidOperationException("Producto no encontrado.");

            if (cantidad < 0)
                throw new ArgumentException("la cantidad debe de ser mayor a cero.");

            //Crear detalle del pedido
            var detalle = new DetallePedido
            {
                PedidoId = pedidoId,
                ProductoId = productoId,
                Cantidad = cantidad,
                PrecioUnitario = producto.Precio
            };

            pedido.Detalles.Add(detalle);

            //Actualizar pedido en repositorio
            await _pedidoRepositorio.CrearAsync(pedido);
        }

        //Cambiar el estado del pedido a confirmado y ajustar el stock de los productos.
        public async Task ConfirmarPedidoAsync(int pedidoId)
        {
            var pedido = await _pedidoRepositorio.ObtenerPorIdAsync(pedidoId);

            if (pedido == null)
                throw new InvalidOperationException("Pedido no encontrado.");

            if (pedido.Estado != EstadoPedido.Pendiente)
                throw new InvalidOperationException("Solo un producto en estado pendiente se puede confirmar.");

            //Verificar stock de cada producto
            foreach (var detalle in pedido.Detalles)
            {
                var producto = await _productoRepositorio.ObtenerPorIdAsync(detalle.ProductoId);

                if (producto.Stock < detalle.Cantidad)
                {
                    throw new InvalidOperationException($"Stock insuficiente para el producto: {producto.Nombre}");
                }
            }

            //Descontar del stock
            foreach (var detalle in pedido.Detalles)
            {
                var producto = await _productoRepositorio.ObtenerPorIdAsync(detalle.ProductoId);
                producto.Stock -= detalle.Cantidad;
                await _productoRepositorio.ActualizarAsync(producto);
            }

            //Cambiar estado
            pedido.Estado = EstadoPedido.Confirmado;

            await _pedidoRepositorio.ActualizarAsync(pedido);
        }

        //Cambiar el estado dl pedido a entregado
        public async Task EntregarPedidoAsync(int pedidoId)
        {
            var pedido = await _pedidoRepositorio.ObtenerPorIdAsync(pedidoId);

            if (pedido == null)
                throw new InvalidOperationException("Pedido no encontrado.");

            if (pedido.Estado != EstadoPedido.Confirmado)
                throw new InvalidOperationException("Solo un producto confirmado se puede entregar.");

            pedido.Estado = EstadoPedido.Entregado;
            await _pedidoRepositorio.ActualizarAsync(pedido);
        }

        //Obtener los pedidos de un cliente especifico
        public async Task<IEnumerable<Pedido>> ObtenerPedidoDeClienteAsync(int clienteId)
        {
            return await _pedidoRepositorio.ObtenerPorClienteAsync(clienteId);
        }

        //Obtener un pedido por su Id
        public async Task<Pedido> ObtenerPorIdAsync(int pedidoId)
        {
            return await _pedidoRepositorio.ObtenerPorIdAsync(pedidoId);
        }

        //Eliminar un pedido
        public async Task EliminarPedidoAsync(int id)
        {
            await _pedidoRepositorio.EliminarAsync(id);
        }
    }
}
