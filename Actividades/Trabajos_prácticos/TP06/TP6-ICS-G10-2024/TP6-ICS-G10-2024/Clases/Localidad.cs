using System.Collections.Generic;

namespace TP6_ICS_G10_2024.Clases
{
    public class Localidad
    {
        public int Id { get; set; }

        public string Nombre { get; set; }  

        public int CodigoPostal { get; set; }

        public int  ProvinciaId { get; set; }

        public IEnumerable<Domicilio> Domicilios { get; set; }
    }
}