using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;

namespace Mirante.Util
{
    public static class Utility
    {
        public static string EncryptText(string p)
        {
            return p;
        }

        public static string DecryptText(string p)
        {
            return p;
        }
        
        #region Propriedades
        
        public static string CampanhaComode
        {
            get { return Convert.ToString(WebConfigurationManager.AppSettings["CampanhaComode"]); }
        }
        
        #endregion

        #region Metodos

        public static void saveFile(string fileStream, string oldFilePath, string newFileName, string newPath)
        {
            if (fileStream != null && fileStream.Length > 28)
            {
                System.IO.Directory.CreateDirectory(newPath);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                byte[] sPDFDecoded = Convert.FromBase64String(fileStream.Substring(28));

                var fullPath = Path.Combine(newPath, newFileName);

                File.WriteAllBytes(fullPath, sPDFDecoded);
            }
        }

        public static void saveImage(string fileStream, string oldFilePath, string newFileName, string newPath)
        {
            if (fileStream != null && fileStream.Length > 37)
            {
                System.IO.Directory.CreateDirectory(newPath);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                var t = fileStream.Substring(37);  // remove data:image/png;base64,

                byte[] bytes = Convert.FromBase64String(t);

                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                }

                var fullPath = Path.Combine(newPath, newFileName);
                image.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }


        public static string MD5Hash(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static string GetIpAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public static SmtpClient GetSmtpClient(MailSettingsSectionGroup settings)
        {
            MailSettingsSectionGroup mssg = settings;
            SmtpClient smtp = new SmtpClient(mssg.Smtp.Network.Host);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Host = mssg.Smtp.Network.Host;
            smtp.Port = mssg.Smtp.Network.Port;
            smtp.EnableSsl = false;
            NetworkCredential c = new NetworkCredential();
            c.UserName = mssg.Smtp.Network.UserName;
            c.Password = mssg.Smtp.Network.Password;
            smtp.Credentials = c;
            return smtp;
        }

        public static MailSettingsSectionGroup GetMailSettingsSectionGroup()
        {
            HttpContext ctx = HttpContext.Current;
            System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(ctx.Request.ApplicationPath);
            MailSettingsSectionGroup mssg = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            return mssg;
        }

        public static string LoadTemplate(string path)
        {
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(path)))
            {
                string template = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                return template;
            }
        }

        public static bool SendEmail(string to, string subject, string body, bool isHtml)
        {
            MailSettingsSectionGroup mssg = GetMailSettingsSectionGroup();

            using (MailMessage mail = new MailMessage())
            {
                mail.IsBodyHtml = isHtml;
                mail.Body = body;
                SmtpClient smtp = GetSmtpClient(mssg);
                string emailDe = mssg.Smtp.From;
                mail.From = new MailAddress(emailDe);
                mail.To.Add(new MailAddress(to));
                mail.Subject = subject;
                mail.BodyEncoding = UTF8Encoding.UTF8;
                mail.Bcc.Add(new MailAddress(emailDe));
                try
                {
                    smtp.Send(mail);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static void ShowBootstrapMessage(string title, string message, Page p)
        {
            p.ClientScript.RegisterStartupScript(p.GetType(), "messsage", "$(document).ready(function(){ShowMessageWithTitle('" + title + "','" + message + "');});", true);
        }

        public static char GetRequestDevice()
        {
            return 'C';
        }

        public static string ClearString(string s)
        {
            return Regex.Replace(s, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        internal static string GeneratePassword()
        {
            return Membership.GeneratePassword(6, 0);
        }


        public static string GetCurrentUrl()
        {
            string sPagePath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo oFileInfo = new System.IO.FileInfo(sPagePath);
            string sPageName = oFileInfo.Name;
            return sPageName;
        }

        public static string ConnectionString
        {
            get
            {
                return WebConfigurationManager.ConnectionStrings["HavaianasConnectionString"].ConnectionString;
            }
        }
        
        public static bool ValidaCPF(string vrCPF)
        {

            string valor = vrCPF.Replace(".", "");

            valor = valor.Replace("-", "");


            if (valor.Length != 11)
            {
                return false;
            }

            bool igual = true;

            int i = 1;

            while (i < 11 && igual)
            {

                if (valor[i] != valor[0])
                {
                    igual = false;
                }
                i += 1;
            }



            if (igual || valor == "12345678909")
            {
                return false;
            }


            int[] numeros = new int[11];



            for (int x = 0; x <= 10; x++)
            {
                numeros[x] = int.Parse(valor[x].ToString());
            }



            int soma = 0;


            for (int x = 0; x <= 8; x++)
            {
                soma += (10 - x) * numeros[x];
            }



            int resultado = soma % 11;



            if (resultado == 1 || resultado == 0)
            {

                if (numeros[9] != 0)
                {
                    return false;

                }

            }
            else if (numeros[9] != 11 - resultado)
            {
                return false;
            }



            soma = 0;


            for (int x = 0; x <= 9; x++)
            {
                soma += (11 - x) * numeros[x];
            }



            resultado = soma % 11;




            if (resultado == 1 || resultado == 0)
            {


                if (numeros[10] != 0)
                {
                    return false;

                }



            }
            else if (numeros[10] != 11 - resultado)
            {
                return false;
            }
            return true;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
       (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string XmlTransfer(string json)
        {
            var clearJson = json.Substring(1, json.Length - 2).Split(',');
            //Perfil
            var perfil = XmlSerialize(clearJson[0], "Perfils");
            //Regiao
            var regiao = XmlSerialize(clearJson[1], "Regiaos");
            //Aes
            var aes = XmlSerialize(clearJson[2], "AEs");
            //Campanhas
            var campanhas = XmlSerialize(clearJson[3], "Campanhas");
            //Segmento
            var segmento = XmlSerialize(clearJson[4], "Segmentos");
            var usuario = string.Empty;
            if (clearJson.Length > 5)
                //Usuario
                usuario = XmlSerialize(clearJson[5], "Usuarios");
            return string.Format("<?xml version=\"1.0\"?><Visibilidade>{0}{1}{2}{3}{4}{5}</Visibilidade>", aes, campanhas, perfil, regiao, segmento, usuario);
        }

        public static string XmlSerialize(string str, string types)
        {
            var clearStr = string.Empty;
            if (str.Contains("\""))
                clearStr = str.Substring(1, str.Length - 2);
            else
                clearStr = str;
            var xmlSerialize = string.Empty;
            if (!string.IsNullOrEmpty(clearStr))
            {
                var strSplit = clearStr.Split('|');
                xmlSerialize = string.Format("<{0}>", types);
                var type = types.Substring(0, types.Length - 1);
                foreach (var item in strSplit)
                {
                    xmlSerialize += string.Format("<{0} ID=\"{1}\"/>", type, item);
                }
                xmlSerialize += string.Format("</{0}>", types);
            }
            return xmlSerialize;
        }

        public static bool validarEmail(string Email)
        {
            bool ValidEmail = false;
            int indexArr = Email.IndexOf("@");
            if (indexArr > 0)
            {
                int indexDot = Email.IndexOf(".", indexArr);
                if (indexDot - 1 > indexArr)
                {
                    if (indexDot + 1 < Email.Length)
                    {
                        string indexDot2 = Email.Substring(indexDot + 1, 1);
                        if (indexDot2 != ".")
                        {
                            ValidEmail = true;
                        }
                    }
                }
            }
            return ValidEmail;
        }

        public enum TipoLogEmail
        {
            FaleConosco = 1,
            BoasVindas = 2,
            EsqueciSenha = 3,
            BoasVindasGanhador = 4

        }

        public static string JsonSerialize(object obj)
        {
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            String sJSON = oSerializer.Serialize(obj);
            return string.Format("{0}", sJSON);
        }

        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        public static string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
        #endregion

        #region Cookies
        public static class Cookies
        {
            public const string USUARIO = "USER";
       
            //CLEAN COOKIES
            public static void Clean()
            {
                Clean(USUARIO);
            }
            public static void Clean(string cookieName)
            {
                HttpCookie cookie;

                if (HttpContext.Current.Response.Cookies[cookieName] != null)
                {
                    cookie = new HttpCookie(cookieName);
                    cookie.Expires = DateTime.Now.AddDays(-1d);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
            public static void Clean(List<string> lista)
            {
                if (lista.Count > 0)
                {
                    foreach (var item in lista)
                    {
                        Clean(item);
                    }
                }
            }




            //NEW COOKIE
            public static void New(string name, int value)
            {
                //Limpa o cookie caso já exista
                Clean(name);

                //Cria o cookie novo
                HttpCookie cookie = new HttpCookie(name, Utility.EncryptText(value.ToString()));
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            public static void New(string name, string value)
            {
                //Limpa o cookie caso já exista
                Clean(name);

                //Cria o cookie novo
                HttpCookie cookie = new HttpCookie(name, Utility.EncryptText(value));
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            public static void New(string name, bool value)
            {
                //Limpa o cookie caso já exista
                Clean(name);

                //Cria o cookie novo
                HttpCookie cookie = new HttpCookie(name, Utility.EncryptText(value.ToString()));
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            public static void New(string name, double value)
            {
                //Limpa o cookie caso já exista
                Clean(name);

                //Cria o cookie novo
                HttpCookie cookie = new HttpCookie(name, Utility.EncryptText(value.ToString()));
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            //GET COOKIE
            public static string Get(string name, bool Decrypt = true)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
                if (cookie == null)
                {
                    return null;
                }
                else
                {
                    if (Decrypt == true)
                        return Utility.DecryptText(cookie.Value);
                    else
                        return cookie.Value;
                }
            }
        }
        #endregion

    }
}