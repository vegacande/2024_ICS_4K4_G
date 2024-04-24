using Microsoft.EntityFrameworkCore;
using TP6_ICS_G10_2024.Clases;

namespace TP6_ICS_G10_2024.Servicios
{
    public interface IRepositorioPaises
    {
        Task<IEnumerable<Pais>> ObtenerPaises();
        Pais ObtenerPaisPorId(int paisId);
    }

    public class PaisesRepositorio : IRepositorioPaises
    {
        private readonly TPDBcontext dbContext;

        public PaisesRepositorio(TPDBcontext DbContext)
        {
            dbContext = DbContext;
        }
        public async Task<IEnumerable<Pais>> ObtenerPaises()
        {
            return await dbContext.Paises.ToListAsync();
        }

        public Pais ObtenerPaisPorId(int paisId)
        {
            return dbContext.Paises.FirstOrDefault(ObtenerPaisPorId => ObtenerPaisPorId.Id == paisId);  
        }
    }
}
