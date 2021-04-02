using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace microSGE.APIConsumer
{
    public static class ViaCEP
    {
        public static string API_End_Base = "https://viacep.com.br/ws/";
        public static string API_Tipo = "/json/";

        public static string[] ViaCEP_Obter(string cep)
        {
            List<string> LocalFinal = new List<string>();
            string[] LocalPreparado = { "" };
            string LocalRaw = "";
            using (WebClient obt = new WebClient())
            {
                try
                {
                    LocalRaw = obt.DownloadString(API_End_Base + cep + API_Tipo);
                }
                catch
                {

                }
            }
            
            LocalPreparado = LocalRaw.Split(
            new[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
            );

            foreach (string i in LocalPreparado)
            {
                if (i.Contains("\"bairro\":"))
                {
                    string item;
                    item = i;
                    item = item.Replace("\"", "");
                    item = item.Replace(":", "");
                    item = item.Replace(",", "");
                    item = item.Replace("bairro", "");
                    LocalFinal.Add(item.Trim());
                }

                if (i.Contains("\"localidade\":"))
                {
                    string item;
                    item = i;
                    item = item.Replace("\"", "");
                    item = item.Replace(":", "");
                    item = item.Replace(",", "");
                    item = item.Replace("localidade", "");
                    LocalFinal.Add(item.Trim());
                }

                if (i.Contains("\"uf\":"))
                {
                    string item;
                    item = i;
                    item = item.Replace("\"", "");
                    item = item.Replace(":", "");
                    item = item.Replace(",", "");
                    item = item.Replace("uf", "");
                    LocalFinal.Add(item.Trim());
                }
            }

            return LocalFinal.ToArray();
        }
    }
}
