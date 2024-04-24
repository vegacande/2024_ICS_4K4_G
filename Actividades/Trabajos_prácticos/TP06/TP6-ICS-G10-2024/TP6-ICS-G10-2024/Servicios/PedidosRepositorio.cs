using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TP6_ICS_G10_2024.Clases;
using TP6_ICS_G10_2024.Models;

namespace TP6_ICS_G10_2024.Servicios
{
    public interface IRepositorioPedidos
    {
        Task Crear(Pedido pedido);
        Task<IEnumerable<PedidosVM>> ObtenerTodos();
    }

    public class PedidosRepositorio : IRepositorioPedidos
    {
        private readonly TPDBcontext dbContext;
        private readonly IMapper mapper;

        public PedidosRepositorio(TPDBcontext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task Crear(Pedido pedido)
        {
            EntityEntry<Pedido> productoInsertado = await dbContext.AddAsync(pedido);
            await dbContext.SaveChangesAsync();
        }


        public async Task<IEnumerable<PedidosVM>> ObtenerTodos()
        {
            var pedidosVMList = await dbContext.Pedidos
                .ProjectTo<PedidosVM>(mapper.ConfigurationProvider)
                .ToListAsync();

            return pedidosVMList;
        }
    }
}
