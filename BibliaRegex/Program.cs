using HtmlAgilityPack;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace BibliaRegex
{
    public class Program
    {
        #region Fields



        #endregion

        /// <summary>
        /// Entry point of the application.
        /// </summary>
        /// <param name="args">Arguments of the application.</param>
        static void Main(string[] args)
        {
            args = new string[] {
                "http://biblia.com.br/joaoferreiraalmeidarevistaatualizada/page/{0}/",
                "1250"
                };

            WebClient webClient = new WebClient();

            string url = args[0];
            string pageBibleUrl;
            string htmlString;
            string pattern;

            int limitPage = Convert.ToInt32(args[1]);

            Regex regex;
            Match match;
            GroupCollection groups;

            for (int i = 1; i <= limitPage; i++)
            {
                pageBibleUrl = string.Format(url, i);

                htmlString = webClient.DownloadString(pageBibleUrl);

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlString);

                HtmlNode tituloPost = htmlDocument.DocumentNode.SelectSingleNode("//a[@class='titulo-post']");

                HtmlNode versiculosNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='versiculos']");

                HtmlNodeCollection versiculos = versiculosNode.ChildNodes;

                foreach (HtmlNode versiculo in versiculos)
                {
                    
                }
            }
        }
    }
}