using ProxyCheckerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HTTP_ProxyChecker
{
    class Program
    {
        static IAPI proxyCheckerAPI;
        static List<string> proxiesList = new List<string>();
        static List<string> WorkingProxies = new List<string>();
        static List<string> NonWorkingProxies = new List<string>();

        static void Main(string[] args)
        {
            string filePath = "";
            Console.WriteLine("=====================Design by Danviet02======================");
            Console.WriteLine("Enter the file path HTTP proxy");
            filePath = Console.ReadLine();
            Console.WriteLine("Ok wait me!");
            proxiesList = File.ReadAllLines(filePath).ToList();

            proxyCheckerAPI = API.CreateBuilder().SetProxyList(proxiesList)
                .SetMaxParallelTask(50)
                .Build();

            proxyCheckerAPI.ProxyListWorked.CollectionChanged += ProxyListWorked_CollectionChanged;
            proxyCheckerAPI.TestProxy();

            Console.ReadKey();
        }

        private static void ProxyListWorked_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var lastElement = proxyCheckerAPI.ProxyListWorked.Last();
            if (lastElement != null)
            {
                if (lastElement.proxyStatus != ProxyCheckerLib.Enums.ProxyStatus.Dead)
                {
                    WorkingProxies.Add($"{lastElement.Address}:{lastElement.Port}");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{lastElement.Address}:{lastElement.Port}, Level:{lastElement.proxyAnonymous}, Country: {lastElement.Country}, Time: {lastElement.Time}ms");
                    Console.Title = $"HTTP Proxy Checker | Loaded: {proxiesList.Count} proxies | Working proxies: {WorkingProxies.Count} | Dead proxies: {NonWorkingProxies.Count}";
                }
                else
                {
                    NonWorkingProxies.Add($"{lastElement.Address}:{lastElement.Port}");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{lastElement.Address}:{lastElement.Port}, Level:{lastElement.proxyAnonymous}, Country: {lastElement.Country}, Time: {lastElement.Time}ms");
                    Console.Title = $"HTTP Proxy Checker | Loaded: {proxiesList.Count} proxies | Working proxies: {WorkingProxies.Count} | Dead proxies: {NonWorkingProxies.Count}";
                }                
            }
        }
    }
}
