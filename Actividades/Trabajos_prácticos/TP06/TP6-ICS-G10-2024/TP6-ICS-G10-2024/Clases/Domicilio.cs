namespace TP6_ICS_G10_2024.Clases
{
    public class Domicilio
    {
        public int Id { get; set; }

        public string Calle { get; set; }

        public int Numero { get; set; }

        public Localidad Localidad { get; set; }

        public int LocalidadId { get; set; }
    }
}