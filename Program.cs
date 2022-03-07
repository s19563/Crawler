using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler
{
    class Program
    {
        public async static Task Main(string[] args)
        {
            if (args.Length == 0 || args == null) throw new ArgumentNullException();

            var websiteUrl = args[0];

            Uri uriResult;
            bool result = Uri.TryCreate(websiteUrl, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!result) throw new ArgumentException();

            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(websiteUrl);

            if (((int)response.StatusCode) != 200) Console.WriteLine("Blad w czasie pobierania strony");

            var content = await response.Content.ReadAsStringAsync();

            var regex = new Regex(@"[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+[a-zA-Z0-9-.]+");

            if (regex.Matches(content).Count == 0) Console.WriteLine("Nie znaleziono adresow email");
            else
            {
                HashSet<string> uniqueMatches = new HashSet<string>();
                foreach (Match match in regex.Matches(content))
                {
                    uniqueMatches.Add(match.ToString());
                }
                Console.WriteLine(string.Join(" ", uniqueMatches));
            }
            httpClient.Dispose();
            response.Dispose();
        }
    }
}