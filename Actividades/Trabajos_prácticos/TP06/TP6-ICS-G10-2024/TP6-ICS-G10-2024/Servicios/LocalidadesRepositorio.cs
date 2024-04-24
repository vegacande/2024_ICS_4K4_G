using Microsoft.EntityFrameworkCore;
using TP6_ICS_G10_2024.Clases;

namespace TP6_ICS_G10_2024.Servicios
{
    public interface IRepositorioLocalidades
    {
        Task<IEnumerable<Localidad>> ObtenerLocalidades();
        Localidad ObtenerLocalidadPorId(int localidadId);
    }
    public class LocalidadesRepositorio : IRepositorioLocalidades
    {
        private readonly TPDBcontext dbContext;

        public LocalidadesRepositorio(TPDBcontext DbContext)
        {
            dbContext = DbContext;
        }
        public async Task<IEnumerable<Localidad>> ObtenerLocalidades()
        {
            return await dbContext.Localidades.ToListAsync();
        }

        public  Localidad ObtenerLocalidadPorId(int localidadId)
        {
            return dbContext.Localidades.FirstOrDefault(x => x.Id == localidadId);
        }
    }
}
