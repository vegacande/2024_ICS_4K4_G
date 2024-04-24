using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP6_ICS_G10_2024.Clases
{
    public class Provincia
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public IEnumerable<Localidad> Localidades { get; set; }


        public int PaisId { get; set; }

    }
}
