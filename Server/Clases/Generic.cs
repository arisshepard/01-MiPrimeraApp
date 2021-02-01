using System.Net;
using System.Net.Mail;

namespace _01_MiPrimeraApp.Server.Clases
{
    public static class Generic
    {
        public static int EnviarCorreo(string nombreCorreo, string asunto, string contenido)
        {
            int respuesta;
            try
            {
                string correo = "correo";
                string clave = "clave";
                string servidor = "servidor";
                int puerto = 25;

                MailMessage mail = new MailMessage
                {
                    Subject = asunto,
                    IsBodyHtml = true,
                    Body = contenido,
                    From = new MailAddress(correo)
                };
                mail.To.Add(new MailAddress(nombreCorreo));

                SmtpClient smtp = new SmtpClient
                {
                    Host = servidor,
                    EnableSsl = true,
                    Port = puerto,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(correo, clave)
                };

                smtp.Send(mail);

                respuesta = 1;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return respuesta;
        }
    }
}