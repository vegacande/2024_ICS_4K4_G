using TP6_ICS_G10_2024.Clases;

namespace TP6_ICS_G10_2024.Models
{
    public class PedidosVM : Pedido
    {
        public Pais Pais { get; set; }

        public Provincia Provincia { get; set; }

        public string Localidad { get; set; }

        public Localidad LocalidadUsuario { get; set; }


    }
}
