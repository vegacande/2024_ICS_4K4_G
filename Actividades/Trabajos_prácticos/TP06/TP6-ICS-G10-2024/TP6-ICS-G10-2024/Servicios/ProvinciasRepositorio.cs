using Microsoft.EntityFrameworkCore;
using TP6_ICS_G10_2024.Clases;

namespace TP6_ICS_G10_2024.Servicios
{
    public interface IRepositorioProvincias
    {
        Provincia ObtenerProvinciaPorId(int provinciaId);
        Task<IEnumerable<Provincia>> ObtenerProvincias();
    }

    public class ProvinciasRepositorio : IRepositorioProvincias
    {
        private readonly TPDBcontext dbContext;

        public ProvinciasRepositorio(TPDBcontext DbContext)
        {
            dbContext = DbContext;
        }

        public Provincia ObtenerProvinciaPorId(int provinciaId)
        {
            //select del nombre de la pronciaId indicada
            return dbContext.Provincias.FirstOrDefault(x => x.Id == provinciaId);
        }

        public async Task<IEnumerable<Provincia>> ObtenerProvincias()
        {
            return await dbContext.Provincias.ToListAsync();
        }
    }
}
