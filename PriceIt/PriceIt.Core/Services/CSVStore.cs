using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using PriceIt.Core.Interfaces;
using PriceIt.Data.Models;

namespace PriceIt.Core.Services
{
    public class CSVStore : ICSVStore
    {
        private const string FilePath = "wwwroot\\csvs\\Products.csv";

        public bool StoreProduct(Product product)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                };
                using var stream = File.Open(FilePath, FileMode.Append);
                using var writer = new StreamWriter(stream);
                using var csv = new CsvWriter(writer, config);

                if (new FileInfo(FilePath).Length != 0)
                {
                    csv.NextRecord();
                }

                csv.WriteRecord(product);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void StoreProducts(List<Product> products)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                };
                using var stream = File.Open(FilePath, FileMode.Append);
                using var writer = new StreamWriter(stream);
                using var csv = new CsvWriter(writer, config);

                if (new FileInfo(FilePath).Length != 0)
                {
                    csv.NextRecord();
                }

                csv.WriteRecords(products);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<Product> ReadProducts()
        {
            var products = new List<Product>();

            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                };
                using var reader = new StreamReader(FilePath);
                using var csv = new CsvReader(reader, config);

                var records = csv.GetRecords<Product>();

                if (records != null)
                {
                    products.AddRange(records);
                }
            }
            catch (Exception e)
            {
                // ignored
            }

            return products;
        }
    }
}
