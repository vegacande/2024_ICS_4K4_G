using TP6_ICS_G10_2024.Clases;

namespace TP6_ICS_G10_2024.Servicios
{
    public interface IRepositorioDomicilios
    {
        Domicilio ObtenerDomicilioPorId(int domicilioEntregaId);
    }

    public class DomiciliosRepositorio : IRepositorioDomicilios
    {
        private readonly TPDBcontext dbContext;

        public DomiciliosRepositorio(TPDBcontext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Domicilio ObtenerDomicilioPorId(int domicilioEntregaId)
        {
            return dbContext.Domicilios.FirstOrDefault(d => d.Id == domicilioEntregaId);
        }
    }
}
