namespace TP6_ICS_G10_2024.Models.Email
{
    public class EmailDto
    {
        public string To { get; set; } = string.Empty;
       
        public string Contenido { get; set; }=  string.Empty ;

        public string Asunto { get; set; } = string.Empty;

        public byte[] Documento { get; internal set; } 
    }
}
