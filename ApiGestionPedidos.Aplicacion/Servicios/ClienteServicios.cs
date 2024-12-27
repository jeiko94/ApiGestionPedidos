using ApiGestionPedidos.Aplicacion.Repositorios;
using ApiGestionPedidos.Dominio.Models;
using System.Security.Cryptography;
using System.Text;

namespace ApiGestionPedidos.Aplicacion.Servicios
{
    //Servico de aplicación para manejar la logica relacionado con Clientes
    public class ClienteServicios
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        //Constructor que recive el repositorio Clientes (puerto).
        public ClienteServicios(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        //Registrar un nuevo cliente en el sistema, validando duplicados y encriptando la contraseña.
        public async Task RegistrarClienteAsync(string nombre, string email, string password)
        {
            //Verificar si el email ya existe
            var clienteExistente = await _clienteRepositorio.ObtenerPorEmailAsync(email);

            if (clienteExistente != null)
            {
                throw new InvalidOperationException("El email ya esta registrado.");
            }

            //Hashear la contraseña
            string passwordHashed = HashPassword(password);

            //Crear objeto cliente
            var nuevoCliente = new Cliente
            {
                Nombre = nombre,
                Email = email,
                PasswordHashed = passwordHashed,
                FechaRegistro = DateTime.UtcNow
            };

            //Guardar en repositorio
            await _clienteRepositorio.CrearAsync(nuevoCliente);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        //Obtener un cliente por id
        public async Task<Cliente> ObtenerClienteAsync(int id)
        {
            return await _clienteRepositorio.ObtenerPorIdAsync(id);
        }

        //Actualiza los datos de un cliente
        //Se pueden considerar validaciones adicionales
        public async Task ActualizarClienteAsync(int id, string nuevoNombre, string nuevoEmail)
        {
            var cliente = await _clienteRepositorio.ObtenerPorIdAsync(id);

            if (cliente == null)
                throw new InvalidOperationException("Cliente no encontrado.");

            //verificar si el nuevo email ya esta en uso por otro cliente
            var clienteOtro = await _clienteRepositorio.ObtenerPorEmailAsync(nuevoEmail);

            if (clienteOtro != null && clienteOtro.Id != id)
            {
                throw new InvalidOperationException("El email ya esta en uso por otro cliente.");
            }

            cliente.Nombre = nuevoNombre;
            cliente.Email = nuevoEmail;

            await _clienteRepositorio.ActualizarAsync(cliente);
        }

        //Eliminar un cliente por su id
        public async Task EliminarClienteAsync(int id)
        {
            await _clienteRepositorio.EliminarAsync(id);
        }

        //Autenticacion de cliente JWT
        public async Task<Cliente> AutenticarClienteAsync(string email, string passwordClaro)
        {
            //Hashear la contraseña
            string passwordHashed = HashPassword(passwordClaro);
            //Buscar cliente por email y contraseña
            var cliente = await _clienteRepositorio.ObtenerPorEmailAsync(email);
            if (cliente == null || cliente.PasswordHashed != passwordHashed)
            {
                throw new InvalidOperationException("Email o contraseña incorrectos.");
            }
            return cliente;
        }
    }
}
