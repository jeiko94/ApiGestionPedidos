using ApiGestionPedidos.Api.DTOs;
using ApiGestionPedidos.Aplicacion.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGestionPedidos.Api.Controllers
{
    //Controlador para gestionar clientes.
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController :ControllerBase
    {
        private readonly ClienteServicios _clienteServicios;

        //El servicio de aplicacion se inyecta a traves del constructor
        public ClientesController(ClienteServicios clienteServicios)
        {
            _clienteServicios = clienteServicios;
        }

        //Crear un nuevo cliente
        //POST /api/clientes
        [HttpPost]
        public async Task<IActionResult> CrearClientes([FromBody] CrearClienteDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _clienteServicios.RegistrarClienteAsync(dto.Nombre, dto.Email, dto.Password);
                return Ok("Cliente creado exitosamente.");
            }
            catch (System.InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize]
        //Obtener un cliente por id
        //GET api/clientes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerCliente(int id)
        {
            var cliente = await _clienteServicios.ObtenerClienteAsync(id);

            if(cliente == null)
                return NotFound("Cliente no encontrado.");

            var clienteDto = new ClienteDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Email = cliente.Email,
            };

            return Ok(clienteDto);
        }

        //Actualizar datos de un cliente
        //PUT /api/clientes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] ActualizarClienteDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _clienteServicios.ActualizarClienteAsync(id, dto.Nombre, dto.Email);
                return Ok("Cliente actualizado correctamente.");
            }
            catch (System.InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        //Elimina un cliente por id
        //DELETE /api/cliente/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            await _clienteServicios.EliminarClienteAsync(id);
            return Ok("Cliente eliminado correctamente.");
        }
    }
}
