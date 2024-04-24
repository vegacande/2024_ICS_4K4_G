using Org.BouncyCastle.Asn1.Crmf;

namespace TP6_ICS_G10_2024.Models.Email
{
    public class SmtpSettings
    {

        public string Host { get; set; }
        public int Port { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}
