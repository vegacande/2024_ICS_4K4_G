using TP6_ICS_G10_2024.Clases;

namespace TP6_ICS_G10_2024.Models
{
    public class PedidosVM : Pedido
    {
        public Pais Pais { get; set; }

        public Pais PaisRetiro { get; set; }

        public Provincia Provincia { get; set; }

        public Provincia ProvinciaRetiro { get; set; }

        public string Localidad { get; set; }

        public Localidad LocalidadRetiro { get; set; }


    }
}
