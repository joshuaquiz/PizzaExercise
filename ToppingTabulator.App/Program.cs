using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ToppingTabulator.App
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            using var client = new WebClient();
            using var sr = new StreamReader(client.OpenRead(configuration["ToppingsData"]) ?? throw new InvalidOperationException());
            using JsonReader reader = new JsonTextReader(sr);
            var serializer = new JsonSerializer();
            var pizzas = serializer.Deserialize<List<Pizza>>(reader);

            if (pizzas == null)
            {
                Console.WriteLine("No pizza data :(");
                Console.ReadKey();
                return;
            }

            var top20ToppingCombinations = pizzas
                .Select(x =>
                    new
                    {
                        Key = string.Join("-", x.Toppings.OrderBy(t => t)),
                        x
                    })
                .GroupBy(x => x.Key)
                .OrderByDescending(x => x.Count())
                .Take(20)
                .Select((x, i) =>
                    new
                    {
                        Position = i + 1,
                        ItemCount = x.Count(),
                        Toppings = string.Join(", ", x.Key)
                    });

            Console.WriteLine("Whats the 20 most popular topping combinations?");
            foreach (var combination in top20ToppingCombinations)
            {
                Console.WriteLine($"#{combination.Position:D2}: {combination.Toppings} ({combination.ItemCount})");
            }
            Console.ReadKey();
        }
    }
}