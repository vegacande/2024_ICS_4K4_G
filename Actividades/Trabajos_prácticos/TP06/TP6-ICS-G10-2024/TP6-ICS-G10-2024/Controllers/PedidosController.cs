using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using TP6_ICS_G10_2024.Models;
using TP6_ICS_G10_2024.Servicios;
using Microsoft.Extensions.Options;
using TP6_ICS_G10_2024.Models.Email;
using MailKit.Net.Smtp;

namespace TP6_ICS_G10_2024.Controllers
{
    public class PedidosController : Controller
    {
        private readonly IRepositorioTipoCargas repositorioTipoCargas;
        private readonly IRepositorioPaises repositorioPaises;
        private readonly IRepositorioProvincias repositorioProvincias;
        private readonly IRepositorioLocalidades repositorioLocalidades;
        private readonly IRepositorioPedidos repositorioPedidos;
        private readonly IConfiguration configuration;
        private readonly SmtpSettings emailSettings;

        public PedidosController(IRepositorioTipoCargas repositorioTipoCargas,
            IRepositorioPaises repositorioPaises,
            IRepositorioProvincias repositorioProvincias, IRepositorioLocalidades repositorioLocalidades, IRepositorioPedidos repositorioPedidos, IConfiguration configuration, IOptions<SmtpSettings> options)
        {
            this.repositorioTipoCargas = repositorioTipoCargas;
            this.repositorioPaises = repositorioPaises;
            this.repositorioProvincias = repositorioProvincias;
            this.repositorioLocalidades = repositorioLocalidades;
            this.repositorioPedidos = repositorioPedidos;
            this.configuration = configuration;
            this.emailSettings = options.Value;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Publicar()
        {
            var modelo = new PedidoCreacionViewModel();
            modelo.TipoCargas = await repositorioTipoCargas.ObtenerTipoCargas();
            modelo.Paises = await repositorioPaises.ObtenerPaises();
            modelo.Provincias = await repositorioProvincias.ObtenerProvincias();
            modelo.Localidades = await repositorioLocalidades.ObtenerLocalidades();

            return View(modelo);

        }

        [HttpPost]
        public async Task<IActionResult> Publicar(PedidoCreacionViewModel pedido)
        {
            if (pedido.LocalidadId <= 0 || pedido.PaisId <= 0 || pedido.ProvinciaId <= 0 || pedido.DomicilioEntrega.Calle == null || pedido.DomicilioEntrega.Numero <= 0)
            {
                TempData["error"] = "El pedido no se ha publicado con éxito, hay algunos datos de la dirección de entrega inválidos o incorrectos";
            }
            else if (pedido.FechaRetiro < DateTime.Today || pedido.FechaRetiro == null)
            {
                TempData["error"] = "El pedido no se ha publicado con éxito, la fecha del retiro debe ser mayor o igual a la fecha de hoy";
            }
            else if (pedido.FechaRetiro > pedido.FechaEntrega || pedido.FechaEntrega == null)
            {
                TempData["error"] = "El pedido no se ha publicado con éxito, la fecha de entrega debe ser mayor o igual a la fecha de retiro";
            }
            else if (pedido.TipoCargaId <= 0)
            {
                TempData["error"] = "El pedido no se ha publicado con éxito, el tipo de carga no se ha seleccionado";
            }

            if (TempData["error"] != null)
            {
                pedido.TipoCargas = await repositorioTipoCargas.ObtenerTipoCargas();
                pedido.Paises = await repositorioPaises.ObtenerPaises();
                pedido.Provincias = await repositorioProvincias.ObtenerProvincias();
                pedido.Localidades = await repositorioLocalidades.ObtenerLocalidades();
                return View(pedido);
            }

            // Código adicional en caso de que todas las condiciones sean falsas



            await pedido.ConvertirFotoAsync(pedido.ImagenFile);
           pedido.DomicilioEntrega.Localidad = repositorioLocalidades.ObtenerLocalidadPorId(pedido.LocalidadId);
           pedido.TipoCarga = await repositorioTipoCargas.ObtenerCargaPorId(pedido.TipoCargaId);
            await repositorioPedidos.Crear(pedido);


            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(configuration.GetSection("EmailSettings:DisplayName").Value, configuration.GetSection("EmailSettings:Email").Value));
            email.To.Add(MailboxAddress.Parse("tomas.marro10@gmail.com"));
            email.Subject = "Pedido nuevo publicado en tu zona";
            email.Body = new TextPart(TextFormat.Html) { Text = $"Nuevo pedido con Fecha de retiro:{pedido.FechaRetiro}, fecha de entrega: {pedido.FechaEntrega} en el domicilio: Buenos Aires 450," +
                $" Córdoba, Córdoba, Argentina, para entregar en la direccion: {pedido.DomicilioEntrega.Calle}{pedido.DomicilioEntrega.Numero}, el tipo de carga es: {pedido.TipoCarga.Nombre}" };

            

            using (SmtpClient smtp = new SmtpClient())
            {
                var server = configuration.GetSection("EmailSettings:MailServer").Value;
                var port = Convert.ToInt32(configuration.GetSection("EmailSettings:Port").Value);
                smtp.CheckCertificateRevocation = false;
                await smtp.ConnectAsync(emailSettings.Host, emailSettings.Port);

                await smtp.AuthenticateAsync(configuration.GetSection("EmailSettings:Email").Value, configuration.GetSection("EmailSettings:Password").Value);

                // Adjuntar el PDF al correo electrónico

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
