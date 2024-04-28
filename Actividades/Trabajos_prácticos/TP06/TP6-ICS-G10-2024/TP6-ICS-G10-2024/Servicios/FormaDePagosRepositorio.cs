using Microsoft.EntityFrameworkCore;
using TP6_ICS_G10_2024.Clases;

namespace TP6_ICS_G10_2024.Servicios
{

    public interface IRepositorioFormaDePago
    {
        Task<IEnumerable<FormaDePago>> ObtenerFormasDepago();
        FormaDePago ObtenerFormaDepagoPorId(int fomaDePagoId);
    }
    public class FormaDePagosRepositorio : IRepositorioFormaDePago
    {
        private readonly TPDBcontext dbContext;

        public FormaDePagosRepositorio(TPDBcontext dbContext)
        {
            this.dbContext = dbContext;
        }
        public FormaDePago ObtenerFormaDepagoPorId(int fomaDePagoId)
        {
            return dbContext.FormaDePagos.FirstOrDefault(f => f.Id == fomaDePagoId);
        }

        public async Task<IEnumerable<FormaDePago>> ObtenerFormasDepago()
        {
            return await dbContext.FormaDePagos.ToListAsync();
        }
    }
}
