using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            //IWebDriver driver = new ChromeDriver();
            //driver.Url = "http://www.google.com";
            string[] fileEntries;

            Console.WriteLine("Please give the directory to search");
            string DirectoryPath = Console.ReadLine();
            Console.WriteLine("Please give the keyword to search");
            string stringToSearch = Console.ReadLine();
            Console.WriteLine("Please give the path to store the csv");
            string filesavepath = Console.ReadLine();
            Dictionary<string, string> groups = new Dictionary<string, string>();
            //Dictionary<key,List<string>> groups = new List<List<string>>();
            string current = null;
            ProcessDirectories(DirectoryPath, stringToSearch, groups, current);

            //Console.WriteLine(groups.ToString());

            DicToExcel(groups, @filesavepath);

        }

        private static void ProcessDirectories(string DirectoryPath, string stringToSearch, Dictionary<string, string> groups, string current)
        {
            string[] fileEntries;
            if (Directory.Exists(DirectoryPath))
            {

                string[] directoryEntries = Directory.GetDirectories(DirectoryPath);

                if (directoryEntries.Count() != 0)
                {
                    foreach(string dirEntry in directoryEntries)
                    {
                        ProcessDirectories(dirEntry, stringToSearch, groups, current);
                    }
                    
                }

                fileEntries = Directory.GetFiles(DirectoryPath);
                if (fileEntries.Count() != 0)
                {
                    foreach (string fileName in fileEntries)
                    {
                        // This path is a directory
                        ProcessDirectory(groups, current, fileName, stringToSearch);
                    }
                }
               
            }

            
        }

        private static Dictionary<string, string> ProcessDirectory(Dictionary<string, string> groups, string current,string pathToFile,string stringToSearch)
        {
            var i = 1;
            foreach (var line in File.ReadAllLines(pathToFile))
            {
                
                if (line.Contains(stringToSearch) && current == null)
                {
                    //current = new List<string>();
                    //current.Add(line);
                    groups.Add(pathToFile+"_"+i.ToString(), line.ToString());
                    current = null;
                }
                i++;
                                          
            }

            return groups;
        }



        public static void DicToExcel(Dictionary<string, string> dict, string path)
        {
            //We will put all results here in StringBuilder and just append everything
            StringBuilder sb = new StringBuilder();
            List<string> header = new List<string>();
            List<String> keyvalue = null;
            header.Add("Key");
            header.Add("value");
            //The key will be our header
            String csv = String.Join(";", header);
            sb.Append(csv + Environment.NewLine);

            foreach (var i in dict)
            {
               string csvrow= CreateRows(keyvalue, csv, i);
                sb.Append(csvrow + Environment.NewLine);
            }




            //sb.Append(csv + Environment.NewLine);

            //We will take every string by element position


            //Write the file
            System.IO.File.WriteAllText(path, sb.ToString());

        }

        private static string CreateRows(List<string> keyvalue, string csv, KeyValuePair<string, string> i)
        {
            
            keyvalue = new List<string>();
            keyvalue.Add(i.Key);
            keyvalue.Add(i.Value);
            csv = String.Join(";", keyvalue);

            return csv;
        }

    }
}
