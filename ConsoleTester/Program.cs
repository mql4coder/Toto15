using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpWebRequest myHttpWebRequest1 = (HttpWebRequest)WebRequest.Create("http://fb.oddsportal.com/feed/match/1-4-Stqrz7hH-5-2-yj642.dat");
            myHttpWebRequest1.KeepAlive = true;
            //  myHttpWebRequest1.Accept = "*/*";
            // var cookie = new CookieContainer();
            //  cookie.Add(new Cookie("_ga", "GA1.2.1897164523.1536495329", "/", "fb.oddsportal.com"));
            //  cookie.Add(new Cookie("_gid", "GA1.2.131495071.1536495329", "/", "fb.oddsportal.com"));
            //myHttpWebRequest1.CookieContainer = cookie;
              myHttpWebRequest1.Referer = "http://www.oddsportal.com/hockey/russia/khl/dinamo-riga-severstal-cherepovets-IsGvmN6n/";
            myHttpWebRequest1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";

            var myHttpWebResponse1 = (HttpWebResponse)myHttpWebRequest1.GetResponse();

            Stream streamResponse = myHttpWebResponse1.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);
            Char[] readBuff = new Char[256];
            int count = streamRead.Read(readBuff, 0, 256);
            Console.WriteLine("The contents of the Html page are.......\n");
            while (count > 0)
            {
                String outputData = new String(readBuff, 0, count);
                Console.Write(outputData);
                count = streamRead.Read(readBuff, 0, 256);
            }
            Console.WriteLine();
            // Close the Stream object.
            streamResponse.Close();
            streamRead.Close();
            // Release the resources held by response object.
            myHttpWebResponse1.Close();
            Console.ReadLine();

        }
    }
}
