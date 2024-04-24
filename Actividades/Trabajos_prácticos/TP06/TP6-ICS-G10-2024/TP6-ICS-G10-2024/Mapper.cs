using AutoMapper;
using TP6_ICS_G10_2024.Clases;
using TP6_ICS_G10_2024.Models;


namespace NegocioRodarSRL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Pedido, PedidoCreacionViewModel>().ReverseMap();
            CreateMap<Pedido, PedidosVM>().ReverseMap();

        }
    }
}
