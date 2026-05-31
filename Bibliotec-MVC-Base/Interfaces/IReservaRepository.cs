using Bibliotec_MVC_Base.Entities;

namespace Bibliotec_MVC_Base.Interfaces;

public interface IReservaRepository : IRepository<Reserva>
{
    Task<IEnumerable<Reserva>> GetAllWithDetailsAsync();
    Task<IEnumerable<Reserva>> GetReservasByUsuarioIdAsync(int usuarioId);
}
