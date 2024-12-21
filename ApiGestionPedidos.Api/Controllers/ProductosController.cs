using ApiGestionPedidos.Api.DTOs;
using ApiGestionPedidos.Aplicacion.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ApiGestionPedidos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoServicios _productoServicios;

        public ProductosController(ProductoServicios productoServicios)
        {
            _productoServicios = productoServicios;
        }

        //Crear un nuevo producto
        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] CrearProductoDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _productoServicios.CrearProductoAsync(dto.Nombre, dto.Precio, dto.Stock);
                return Ok("Producto creado exitosamente.");
            }
            catch (System.InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        //Obtener un producto por su id
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerProducto(int id)
        {
            var productos = await _productoServicios.ObtenerProductoAsync(id);

            if (productos == null)
                return NotFound("Producto no encontrado.");

            var productoDto = new ProductoDto
            {
                Id = productos.Id,
                Nombre = productos.Nombre,
                Precio = productos.Precio,
                Stock = productos.Stock
            };

            return Ok(productoDto);
        }

        //Listar todos los producto.
        [HttpGet]
        public async Task<IActionResult> ListarProductos()
        {
            var productos = await _productoServicios.ListarProductosAsync();
            var ListaDto = productos.Select(p => new ProductoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock
            });

            return Ok(ListaDto);
        }

        //Actualizar un producto Existente
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] CrearProductoDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _productoServicios.ActualizarProductoAsync(id, dto.Nombre, dto.Precio, dto.Stock);
                return Ok("Producto actualizado correctamente.");
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        //Eliminar un producto por id
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            await _productoServicios.EliminarProductoAsync(id);
            return Ok("Producto eliminado correctamente.");
        }
    }
}
