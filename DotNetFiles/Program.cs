﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileModule
{
    class Program
    {
        public static void Main( string[] args )
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var storesDirectory = Path.Combine(currentDirectory, "stores");

            var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
            Directory.CreateDirectory(salesTotalDir);

            var salesFiles = FindFiles(storesDirectory);

            var salesTotal = CalculateSalesTotal(salesFiles);

            File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"Total Sales for all Stores - {salesTotal:F2}{Environment.NewLine}");
        }

        private static IEnumerable<string> FindFiles( string folderName )
        {
            var salesFiles = new List<string>();

            var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

            foreach (var file in foundFiles)
            {
                var extension = Path.GetExtension(file);
                if (extension == ".json")
                    salesFiles.Add(file);
            }

            return salesFiles;
        }


        static double CalculateSalesTotal( IEnumerable<string> salesFiles )
        {
            double salesTotal = 0;

            foreach (var file in salesFiles)
            {
                string salesJson = File.ReadAllText(file);

                SalesData data = JsonConvert.DeserializeObject<SalesData>(salesJson);

                salesTotal += data.Total;
            }

            return salesTotal;
        }
        class SalesData
        {
            public double Total { get; set; }
        }
    }
}
