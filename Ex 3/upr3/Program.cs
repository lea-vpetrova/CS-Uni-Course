using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Linq;

namespace upr3
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            while (true)
            {
                Console.WriteLine("\n1. Whois\n2. Local Time\n3. Scrape News\n0. Exit");
                string input = Console.ReadLine();

                if (input == "0") return;
                else if (input == "1") await Task1();
                else if (input == "2") await Task2();
                else if (input == "3") await Task3();
            }
        }

        static async Task Task1()
        {
            Console.Write("IP: ");
            string ip = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(ip)) return;
            try
            {
                string result = await client.GetStringAsync($"https://ipapi.co/{ip}/country_name/");
                Console.WriteLine(result.Trim());
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        static async Task Task2()
        {
            try
            {
                string html = await client.GetStringAsync("https://www.timeanddate.com/worldclock/bulgaria/sofia");
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                var node = doc.DocumentNode.SelectSingleNode("//span[@id='ct']");
                if (node != null) Console.WriteLine(node.InnerText);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        static async Task Task3()
        {
            string[] bad = { "Covid", "корона", "пандемия", "virus" };
            try
            {
                var bytes = await client.GetByteArrayAsync("https://www.mediapool.bg/");
                string html = Encoding.UTF8.GetString(bytes);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                var nodes = doc.DocumentNode.SelectNodes("//a");

                if (nodes == null) return;

                int count = 0;
                foreach (var node in nodes)
                {
                    string text = System.Net.WebUtility.HtmlDecode(node.InnerText.Trim());
                    if (text.Length < 15) continue;

                    bool skip = false;
                    foreach (var b in bad)
                        if (text.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0) skip = true;

                    if (!skip)
                    {
                        Console.WriteLine("- " + text);
                        if (++count >= 10) break;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
