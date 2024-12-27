using ApiGestionPedidos.Api.DTOs;
using ApiGestionPedidos.Aplicacion.Servicios;
using ApiGestionPedidos.Dominio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGestionPedidos.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly PedidoServicio _pedidoServicio;

        public PedidosController(PedidoServicio pedidoServicio)
        {
            _pedidoServicio = pedidoServicio;
        }

        //Crea un pedido para un cliente
        [HttpPost]
        public async Task<IActionResult> CreaPedido([FromBody] CrearPedidoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                int pedidoId = await _pedidoServicio.CrearPedidoAsync(dto.ClienteId);
                return Ok($"Pedido creado con Id: {pedidoId}");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        //agrega un producto al pedido
        //POST /api/pedidos/{pedidoId}/productos
        [HttpPost("{pedidoId}/productos")]
        public async Task<IActionResult> AgregarProductoAlPedido(int pedidoId, [FromBody] AgregarProductoPedidoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _pedidoServicio.AgregarProductoAsync(pedidoId, dto.ProductoId, dto.Cantidad);
                return Ok("Producto agregado al pedido.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // Confirma un pedido (descuenta stock, cambia estado a Confirmado).
        // POST /api/pedidos/{pedidoId}/confirmar
        [HttpPost("{pedidoId}/confirmar")]
        public async Task<IActionResult> ConfirmarPedido(int pedidoId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _pedidoServicio.ConfirmarPedidoAsync(pedidoId);
                return Ok("Pedido confirmado.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // Marca un pedido como Entregado.
        // POST /api/pedidos/{pedidoId}/entregar
        [HttpPost("{pedidoId}/entregar")]
        public async Task<IActionResult> EntregarPedido(int pedidoId)
        {
            if (ModelState.IsValid)
                return Ok(ModelState);

            try
            {
                await _pedidoServicio.EntregarPedidoAsync(pedidoId);
                return Ok("Pedido entregado.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// Obtiene un pedido por Id.
        /// GET /api/pedidos/{pedidoId}
        [HttpGet("{pedidoId}")]
        public async Task<IActionResult> ObtenerPedido(int pedidoId)
        {
            var pedido = await _pedidoServicio.ObtenerPorIdAsync(pedidoId);

            if(pedido == null)
                return NotFound("Pedido no encontrado.");

            var pedidoDto = MapearPedidoDto(pedido);
            return Ok(pedidoDto);
        }

        // Obtiene todos los pedidos de un cliente.
        // GET /api/pedidos/cliente/{clienteId}
        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> ObtenerPedidosDeCliente(int clienteId)
        {
            var pedidos = await _pedidoServicio.ObtenerPedidoDeClienteAsync(clienteId);
            var listadoDto = pedidos.Select(p => MapearPedidoDto(p)).ToList();
            return Ok(listadoDto);
        }

        // Elimina un pedido (opcional).
        // DELETE /api/pedidos/{pedidoId}
        [HttpDelete("{pedidoId}")]
        public async Task<IActionResult> EliminarPedido(int pedidoId)
        {
            await _pedidoServicio.EliminarPedidoAsync(pedidoId);
            return Ok("Producto eliminado.");
        }

        private PedidoDto MapearPedidoDto(Pedido pedido)
        {
            var detallesDto = pedido.Detalles.Select(d => new DetallePedidoDto
            {
                ProductoId = d.ProductoId,
                NombreProducto = d.Producto?.Nombre,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            }).ToList();

            return new PedidoDto
            {
                Id = pedido.Id,
                ClienteId = pedido.ClienteId,
                Estado = pedido.Estado.ToString(),
                FechaCreacion = pedido.FechaCreacion,
                Detalles = detallesDto,
                Total = pedido.CalcularTotal(),
            };

        }
    }


}
