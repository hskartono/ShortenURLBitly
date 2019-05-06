using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace ShortenURLBitly
{
    class Program
    {
        static void Main(string[] args)
        {
            String myURL = "http://long/url/to/be/shortened/";
            String shortURL = BitlyShortURL.ShortenURL(myURL);
            Console.WriteLine(shortURL);
            Console.ReadKey();
        }
    }

    public class BitlyShortURL
    {
        const string APIKey = "YOUR_BITLY_APIKEY";
        const string Username = "YOUR_BITLY_USERNAME";

        public static string ShortenURL(String url)
        {
            string statusCode = string.Empty;
            string statusText = string.Empty;
            string shortUrl = string.Empty;
            string longUrl = string.Empty;

            XmlDocument xmlDoc = new XmlDocument();
            WebRequest request = WebRequest.Create("http://api.bitly.com/v3/shorten");
            byte[] data = Encoding.UTF8.GetBytes(
                string.Format("login={0}&apiKey={1}&longUrl={2}&format={3}",
                Username,
                APIKey,
                HttpUtility.UrlEncode(url),
                "xml"));

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (Stream ds = request.GetRequestStream())
            {
                ds.Write(data, 0, data.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using(StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    xmlDoc.LoadXml(sr.ReadToEnd());
                }
            }

            statusCode = xmlDoc.GetElementsByTagName("status_code")[0].InnerText;
            statusText = xmlDoc.GetElementsByTagName("status_txt")[0].InnerText;
            shortUrl = xmlDoc.GetElementsByTagName("url")[0].InnerText;
            longUrl = xmlDoc.GetElementsByTagName("long_url")[0].InnerText;

            return shortUrl;
        }
    }
}
