using Microsoft.EntityFrameworkCore;
using TP6_ICS_G10_2024.Clases;

namespace TP6_ICS_G10_2024.Servicios
{

    public interface IRepositorioTipoCargas
    {
        Task<TipoCarga> ObtenerCargaPorId(int tipoCargaId);
        Task<IEnumerable<TipoCarga>> ObtenerTipoCargas();
    }
    public class TipoCargasRepositorio : IRepositorioTipoCargas
    {
        private readonly TPDBcontext dbContext;

        public TipoCargasRepositorio(TPDBcontext DbContext)
        {
            dbContext = DbContext;
        }

        public Task<TipoCarga> ObtenerCargaPorId(int tipoCargaId)
        {
            return dbContext.TipoCargas.FirstOrDefaultAsync(x => x.Id == tipoCargaId);
        }

        public async Task<IEnumerable<TipoCarga>> ObtenerTipoCargas()
        {
            return await dbContext.TipoCargas.ToListAsync();
        }
    }
}
