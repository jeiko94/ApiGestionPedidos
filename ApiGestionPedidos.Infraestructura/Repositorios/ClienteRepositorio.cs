using ApiGestionPedidos.Aplicacion.Repositorios;
using ApiGestionPedidos.Dominio.Models;
using ApiGestionPedidos.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionPedidos.Infraestructura.Repositorios
{
    //Implementacion congreta de IClienteRepositorio usando EF core y SQL server.
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly AplicacionDbContext _context;

        public ClienteRepositorio(AplicacionDbContext context)
        {
            _context = context;
        }

        //Crea un nuevo cliente en la base de datos.
        public async Task CrearAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        //Obtiene un cliente por su Id
        public async Task<Cliente> ObtenerPorIdAsync(int id){

            return await _context.Clientes.FindAsync(id);
        }

        //Obtiene un cliente por su email
        //email = del cliente
        public async Task<Cliente> ObtenerPorEmailAsync(string email)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        //Actualiza los datos de un cliente existente
        //cliente = con los nuevo datos
        public async Task ActualizarAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        //Elimina un cliente por su Id
        public async Task EliminarAsync(int id)
        {
            var cliente = await ObtenerPorIdAsync(id);

            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
}
