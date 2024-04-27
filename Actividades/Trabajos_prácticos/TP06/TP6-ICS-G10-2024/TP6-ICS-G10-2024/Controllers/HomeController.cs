using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TP6_ICS_G10_2024.Clases;
using TP6_ICS_G10_2024.Models;
using TP6_ICS_G10_2024.Servicios;

namespace TP6_ICS_G10_2024.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositorioPedidos repositorioPedidos;
        private readonly IRepositorioProvincias repositorioProvincias;
        private readonly IRepositorioPaises repositorioPaises;
        private readonly IRepositorioLocalidades repositorioLocalidades;
        private readonly IRepositorioDomicilios repositorioDomicilios;

        public HomeController(ILogger<HomeController> logger, IRepositorioPedidos repositorioPedidos,IRepositorioProvincias repositorioProvincias
            , IRepositorioPaises repositorioPaises,IRepositorioLocalidades repositorioLocalidades, IRepositorioDomicilios repositorioDomicilios)
        {
            _logger = logger;
            this.repositorioPedidos = repositorioPedidos;
            this.repositorioProvincias = repositorioProvincias;
            this.repositorioPaises = repositorioPaises;
            this.repositorioLocalidades = repositorioLocalidades;
            this.repositorioDomicilios = repositorioDomicilios;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<PedidosVM> pedidos = await repositorioPedidos.ObtenerTodos();

            // Obtener todas las provincias, localidades y domicilios
            var provincias = await repositorioProvincias.ObtenerProvincias();
            var localidades = await repositorioLocalidades.ObtenerLocalidades();
            IEnumerable<Domicilio> domicilios = await repositorioDomicilios.ObtenerDomicilios();
            var paises = await repositorioPaises.ObtenerPaises();

            // Mapear los datos de los pedidos
            foreach (var item in pedidos)
            {
                // Obtener la localidad de retiro del pedido
                var localidadRetiro = localidades.FirstOrDefault(l => l.Id == item.DomicilioRetiro.LocalidadId);
                if (localidadRetiro != null)
                {
                    // Obtener la provincia de retiro
                    item.LocalidadRetiro = localidadRetiro;
                    item.ProvinciaRetiro = provincias.FirstOrDefault(p => p.Id == localidadRetiro.ProvinciaId);
                    if (item.ProvinciaRetiro != null)
                    {
                        // Obtener el país de retiro
                        item.PaisRetiro = paises.FirstOrDefault(pa => pa.Id == item.ProvinciaRetiro.PaisId);
                    }
                }

                // Obtener el domicilio de entrega
                item.DomicilioEntrega = domicilios.FirstOrDefault(d => d.Id == item.DomicilioEntregaId);
                if (item.DomicilioEntrega != null)
                {
                    // Obtener la localidad de entrega
                    item.DomicilioEntrega.Localidad = localidades.FirstOrDefault(l => l.Id == item.DomicilioEntrega.LocalidadId);
                    if (item.DomicilioEntrega.Localidad != null)
                    {
                        // Obtener la provincia de entrega
                        item.Provincia = provincias.FirstOrDefault(p => p.Id == item.DomicilioEntrega.Localidad.ProvinciaId);
                        if (item.Provincia != null)
                        {
                            // Obtener el país de entrega
                            item.Pais = paises.FirstOrDefault(pa => pa.Id == item.Provincia.PaisId);
                        }
                    }
                }
            }

            return View(pedidos);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
