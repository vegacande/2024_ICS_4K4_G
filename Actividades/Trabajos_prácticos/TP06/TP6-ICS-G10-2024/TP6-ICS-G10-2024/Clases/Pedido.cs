using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP6_ICS_G10_2024.Clases
{
    public class Pedido
    {
        public int Id { get; set; }

        public DateTime FechaRetiro { get; set; }

        public DateTime FechaEntrega { get; set; }

        public Domicilio DomicilioEntrega { get; set; }

        public int DomicilioEntregaId { get; set; }

        public TipoCarga TipoCarga { get; set; }

        public byte[]? Imagen { get; set; }

        public async Task ConvertirFotoAsync(IFormFile foto)
        {
            // Cargar la imagen y convertirla en bytes
            if (foto != null && foto.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await foto.CopyToAsync(memoryStream);
                    this.Imagen = memoryStream.ToArray();
                }
            }
            else
            {
                this.Imagen = null;
            }
        }

    }
}
