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

            foreach (var item in pedidos)
            {
                item.LocalidadRetiro = repositorioLocalidades.ObtenerLocalidadPorId(item.DomicilioRetiro.LocalidadId);
                item.ProvinciaRetiro = repositorioProvincias.ObtenerProvinciaPorId(item.LocalidadRetiro.ProvinciaId);
                item.PaisRetiro = repositorioPaises.ObtenerPaisPorId(item.ProvinciaRetiro.PaisId);
                item.DomicilioEntrega = repositorioDomicilios.ObtenerDomicilioPorId(item.DomicilioEntregaId);
                item.DomicilioEntrega.Localidad = repositorioLocalidades.ObtenerLocalidadPorId(item.DomicilioEntrega.LocalidadId);
                item.Provincia = repositorioProvincias.ObtenerProvinciaPorId(item.DomicilioEntrega.Localidad.ProvinciaId);
                item.Pais = repositorioPaises.ObtenerPaisPorId(item.Provincia.PaisId);
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
