using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web.Mvc;
using TP6_ICS_G10_2024.Clases;

namespace TP6_ICS_G10_2024.Models
{
    public class PedidoCreacionViewModel: Pedido
    {
        public IEnumerable<Localidad> Localidades { get; set; }
        public IEnumerable<Provincia> Provincias { get; set; }
        public IEnumerable<Pais> Paises { get; set; }
        public IEnumerable<TipoCarga> TipoCargas { get; set; }

        public IFormFile? ImagenFile { get; set; }

        public int ProvinciaId { get; set; }

        public int TipoCargaId { get; set; }

        public int LocalidadId { get; set; }

        public int PaisId { get; set; }

    }
}
