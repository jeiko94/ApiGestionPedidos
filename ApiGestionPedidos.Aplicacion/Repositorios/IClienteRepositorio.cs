using ApiGestionPedidos.Dominio.Models;

namespace ApiGestionPedidos.Aplicacion.Repositorios
{
    //Define las operaciones de percistencia relacionadas con la entidad cliente
    public interface IClienteRepositorio
    {
        //Crea un nuevo cliente en la base de datos.
        Task CrearAsync(Cliente cliente);

        //Obtiene un cliente por su Id
        Task<Cliente> ObtenerPorIdAsync(int id);

        //Obtiene un cliente por su email
        //email = del cliente
        Task<Cliente> ObtenerPorEmailAsync(string email);

        //Actualiza los datos de un cliente existente
        //cliente = con los nuevo datos
        Task ActualizarAsync(Cliente cliente);

        //Elimina un cliente por su Id
        Task EliminarAsync(int id);
    }
}
