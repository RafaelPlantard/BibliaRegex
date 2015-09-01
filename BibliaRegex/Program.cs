using BibliaRegex.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
                "1189",
                "pt-BR",
                @"D:\Documents\Extensions\Json\"
                };

            WebClient webClient = new WebClient();

            string url = args[0];
            string pageBibleUrl;
            string htmlString;
            string pattern;

            IList<string> oldTestament = new List<string>();
            IList<string> newTestament = new List<string>();

            Bible bible = new Bible()
            {
                OldTestament = new List<Book>(),
                NewTestament = new List<Book>()
            };

            int limitPage = Convert.ToInt32(args[1]);

            Regex regex;
            Match match;
            GroupCollection groups;

            for (int i = 1; i <= limitPage; i++)
            {
                pageBibleUrl = string.Format(url, i);

                HtmlDocument htmlDocument = new HtmlDocument()
                {
                    OptionDefaultStreamEncoding = Encoding.UTF8,
                };

                htmlDocument.Load(webClient.OpenRead(pageBibleUrl), Encoding.UTF8);

                if (oldTestament.Count <= 0)
                {
                    HtmlNode oldTestamentNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='grid_4 old_test omega']");
                    IEnumerable<HtmlNode> booksOldTestamentNodeEnumerable = oldTestamentNode.ChildNodes.Where(n => n.Name == "ul");

                    foreach (HtmlNode node in booksOldTestamentNodeEnumerable)
                    {
                        IEnumerable<HtmlNode> booksLi = node.ChildNodes.Where(b => b.Name == "li");
                        foreach (HtmlNode li in booksLi)
                        {
                            oldTestament.Add(li.InnerText);
                        }
                    }
                }

                if (newTestament.Count <= 0)
                {
                    HtmlNode newTestamentNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='grid_4 push_1 new_test alpha']");
                    IEnumerable<HtmlNode> booksNewTestamentNodeCollection = newTestamentNode.ChildNodes.Where(n => n.Name == "ul");

                    foreach (HtmlNode node in booksNewTestamentNodeCollection)
                    {
                        IEnumerable<HtmlNode> booksLi = node.ChildNodes.Where(b => b.Name == "li");
                        foreach (HtmlNode li in booksLi)
                        {
                            newTestament.Add(li.InnerText);
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(bible.Name))
                {
                    HtmlNode versionNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='grid_3 box_link_dados2 margin_boxsidebar']");
                    bible.Name = versionNode.InnerText.Trim();
                    bible.Language = args[2];

                    string[] initialsName = bible.Name.Split(' ');
                    foreach (string s in initialsName)
                    {
                        if (s.Length < 3)
                            continue;

                        bible.Initials += s.Substring(0, 1);
                    }
                }

                HtmlNode postTitle = htmlDocument.DocumentNode.SelectSingleNode("//a[@class='titulo-post']");
                HtmlNode chapterNode = htmlDocument.DocumentNode.SelectSingleNode("//h2[@class='capitulo']").ChildNodes.Where(c => c.Name == "a").First();
                string chapterAttribute = chapterNode.GetAttributeValue("href", string.Empty);

                Book book = new Book();
                Chapter chapter = new Chapter() { Verses = new List<Verse>() };
                string initialsBook = string.Empty;

                pattern = @"(\w{2})\-\w*\-\d*";
                regex = new Regex(pattern);
                if (regex.IsMatch(chapterAttribute))
                {
                    match = regex.Match(chapterAttribute);
                    groups = match.Groups;

                    initialsBook = groups[1].Value.ToUpper();

                    book = bible.OldTestament.Find(b => b.Initials == initialsBook) ?? bible.NewTestament.Find(b => b.Initials == initialsBook);
                }

                pattern = @"^(.*)\s\&\#8211\;\s.*\s(\d*)$";
                regex = new Regex(pattern);
                if (regex.IsMatch(postTitle.InnerText))
                {
                    match = regex.Match(postTitle.InnerText);
                    groups = match.Groups;

                    if (book == null)
                    {
                        book = new Book()
                        {
                            Name = groups[1].Value,
                            Initials = initialsBook
                        };

                        if (newTestament.Contains(book.Name))
                        {
                            bible.NewTestament.Add(book);
                        }
                        else
                        {
                            bible.OldTestament.Add(book);
                        }
                    }
                    chapter.Number = Convert.ToInt32(groups[2].Value);
                }

                HtmlNode verseNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='versiculos']");

                HtmlNodeCollection versesNodeCollection = verseNode.ChildNodes;

                if (book.Chapters == null)
                    book.Chapters = new List<Chapter>();

                foreach (HtmlNode v in versesNodeCollection)
                {
                    if (v.Name != "div")
                        continue;

                    Verse verse = new Verse()
                    {
                        Number = Convert.ToInt32(v.ChildNodes.Where(n => n.Name == "span").First().InnerText),
                        Text = v.ChildNodes.Where(n => n.Name == "#text").First().InnerText.Trim()
                    };

                    chapter.Verses.Add(verse);
                }

                book.Chapters.Add(chapter);

                float percent =100 - ((i / (float)limitPage) * 100);

                Console.WriteLine("Created {0} {1} - Remaining {2}%", book.Name, chapter.Number, percent);
            }

            Console.WriteLine("Starting the saving :)");

            string path = string.Concat(args[3], bible.Initials, ".json");

            using (FileStream fileStream = File.Open(path, FileMode.CreateNew))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        jsonWriter.Formatting = Formatting.Indented;

                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(jsonWriter, bible);
                    }
                }
            }

            Console.WriteLine("Finishing :)");

            Console.ReadKey();
        }
    }
}