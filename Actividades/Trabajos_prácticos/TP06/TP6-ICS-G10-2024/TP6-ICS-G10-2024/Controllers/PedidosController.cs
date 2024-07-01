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
        private readonly IRepositorioDomicilios repositorioDomicilios;
        private readonly IRepositorioProvincias repositorioProvincias;
        private readonly IRepositorioLocalidades repositorioLocalidades;
        private readonly IRepositorioFormaDePago repositorioFormaDePago;
        private readonly IRepositorioPedidos repositorioPedidos;
        private readonly IConfiguration configuration;
        private readonly SmtpSettings emailSettings;

        public PedidosController(IRepositorioTipoCargas repositorioTipoCargas,
            IRepositorioPaises repositorioPaises, IRepositorioDomicilios repositorioDomicilios,
            IRepositorioProvincias repositorioProvincias, IRepositorioLocalidades repositorioLocalidades, IRepositorioFormaDePago repositorioFormaDePago, 
            IRepositorioPedidos repositorioPedidos, IConfiguration configuration, IOptions<SmtpSettings> options)
        {
            this.repositorioTipoCargas = repositorioTipoCargas;
            this.repositorioPaises = repositorioPaises;
            this.repositorioDomicilios = repositorioDomicilios;
            this.repositorioProvincias = repositorioProvincias;
            this.repositorioLocalidades = repositorioLocalidades;
            this.repositorioFormaDePago = repositorioFormaDePago;
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
            //Carga todos los datos necesarios para la vista de publicar pedido
            var modelo = new PedidoCreacionViewModel();
            modelo.TipoCargas = await repositorioTipoCargas.ObtenerTipoCargas();
            modelo.Paises = await repositorioPaises.ObtenerPaises();
            modelo.Provincias = await repositorioProvincias.ObtenerProvincias();
            modelo.Localidades = await repositorioLocalidades.ObtenerLocalidades();
            modelo.FormasDePago = await repositorioFormaDePago.ObtenerFormasDepago();

            return View(modelo);

        }

        [HttpPost]
        public async Task<IActionResult> Publicar(PedidoCreacionViewModel pedido)
        {

            //Valida en el caso que no se ingrese algun dato del pedido tanto como la fechas ,direcciones, tipo de carga
            if (pedido.LocalidadId <= 0 || pedido.PaisId <= 0 || pedido.ProvinciaId <= 0 || pedido.DomicilioEntrega.Calle == null || pedido.DomicilioEntrega.Numero <= 0)
            {
                TempData["error"] = "El pedido no se ha publicado con éxito, hay algunos datos de la dirección de entrega y/o retiro inválidos o incorrectos";
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
            else if (pedido.FormaDePagoId <= 0)
            {
                TempData["error"] = "El pedido no se ha publicado con éxito, la forma de pago Preferida no se ha seleccionado";
            }

            if (!pedido.DomiciolioDeUsuario)
            {
                if (pedido.DomicilioRetiro.Calle == null || pedido.DomicilioRetiro.Numero <= 0 || pedido.ProvinciaRetiroId <= 0 || pedido.ProvinciaRetiroId <= 0)
                {
                    TempData["error"] = "El pedido no se ha publicado con éxito, hay algunos datos de la dirección de entrega y/o retiro inválidos o incorrectos";
                }
            }
            else
            {
               
            }

            if (TempData["error"] != null)
            {
                pedido.TipoCargas = await repositorioTipoCargas.ObtenerTipoCargas();
                pedido.Paises = await repositorioPaises.ObtenerPaises();
                pedido.Provincias = await repositorioProvincias.ObtenerProvincias();
                pedido.Localidades = await repositorioLocalidades.ObtenerLocalidades();
                pedido.FormasDePago = await repositorioFormaDePago.ObtenerFormasDepago();
                return View(pedido);
            }


            if (pedido.DomiciolioDeUsuario)
            {
                pedido.DomicilioRetiroId = 5;
                pedido.DomicilioRetiro = repositorioDomicilios.ObtenerDomicilioPorId(5);
                pedido.DomicilioRetiro.Localidad = repositorioLocalidades.ObtenerLocalidadPorId(pedido.DomicilioRetiro.LocalidadId);
                pedido.ProvinciaRetiroId = pedido.DomicilioRetiro.Localidad.ProvinciaId;
                pedido.LocalidadRetiroId = pedido.DomicilioRetiro.LocalidadId;
                pedido.PaisRetiroId = 1;
            }

            await pedido.ConvertirFotoAsync(pedido.ImagenFile);
            pedido.DomicilioEntrega.Localidad = repositorioLocalidades.ObtenerLocalidadPorId(pedido.LocalidadId);
            pedido.DomicilioRetiro.Localidad = repositorioLocalidades.ObtenerLocalidadPorId(pedido.LocalidadRetiroId);
            pedido.FormaDePago = repositorioFormaDePago.ObtenerFormaDepagoPorId(pedido.FormaDePagoId);
            pedido.TipoCarga = await repositorioTipoCargas.ObtenerCargaPorId(pedido.TipoCargaId);
            await repositorioPedidos.Crear(pedido);

            var provinciaRetiro = repositorioProvincias.ObtenerProvinciaPorId(pedido.DomicilioRetiro.Localidad.ProvinciaId);
            var provinciaEntrega = repositorioProvincias.ObtenerProvinciaPorId(pedido.DomicilioEntrega.Localidad.ProvinciaId); 
            
            
            var paisRetiro = repositorioPaises.ObtenerPaisPorId(provinciaRetiro.PaisId);
            var paisEntrega = repositorioPaises.ObtenerPaisPorId(provinciaEntrega.PaisId);

            //Crea el mail y lo enviara al correo de los proveedores activos
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(configuration.GetSection("EmailSettings:DisplayName").Value, configuration.GetSection("EmailSettings:Email").Value));
            email.To.Add(MailboxAddress.Parse("tomas.marro10@gmail.com"));
            email.Subject = "Pedido nuevo publicado en tu zona";
            email.Body = new TextPart(TextFormat.Html)
            {
                            Text = $@"
                                <h1>pedido</h1>
                    <h2>Usuario: Tomas Marro</h2>
                    <h3>Datos de Envío:</h3>
                    <table border=""1"">
                      <tr>
                        <th>Fecha de retiro</th>
                        <th>Dirección de retiro</th>
                        <th>Fecha de entrega</th>
                        <th>Dirección de entrega</th>
                        <th>Tipo de carga</th>
                        <th>Observaciones</th>
                        <th>Forma de pago preferida</th>
                      </tr>
                      <tr>
                        <td>{pedido.FechaRetiro}</td>
                        <td>{pedido.DomicilioRetiro.Calle} {pedido.DomicilioRetiro.Numero}, {pedido.DomicilioRetiro.Localidad.Nombre}, {provinciaRetiro.Nombre}, {paisRetiro.Nombre}</td>
                        <td>{pedido.FechaEntrega}</td>
                        <td>{pedido.DomicilioEntrega.Calle}{pedido.DomicilioEntrega.Numero},{provinciaEntrega.Nombre},{paisEntrega.Nombre}</td>
                        <td>{pedido.TipoCarga.Nombre}</td>
                        <td>{pedido.Observaciones}</td>
                        <td>{pedido.FormaDePago.Nombre}</td>
                      </tr>
                    </table>
                "
            };


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
            TempData["exito"] = "El pedido se publico conn exito";
            //Vuelve al inicio
            return RedirectToAction("Index", "Home");
        }
    }
}
