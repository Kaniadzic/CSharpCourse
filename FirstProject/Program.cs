using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace FirstProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string csvPath = @"C:\Users\simi4_000\Desktop\programowanie\c\CSharpCourseLINQ\FirstProject\googleplaystore1.csv";
            var googleApps = LoadGoogleAps(csvPath);

            //Display(googleApps);
            //GetData(googleApps);
            //ProjectData(googleApps);
            //DivideData(googleApps);
            //SortData(googleApps);
            DataSetOperation(googleApps);
        }

        /// <summary>
        /// Część 1 - pobieranie danych
        /// </summary>
        /// <param name="googleApps"></param>
        static void GetData(IEnumerable<GoogleApp> googleApps)
        {
            //var highRatedApps = googleApps.Where(app => app.Rating > 4.6).Where(app => app.Category == Category.BEAUTY);
            var highRatedApps = googleApps.Where(app => app.Rating > 4.6 && app.Category == Category.BEAUTY);

            //try
            //{
            //    Display(highRatedApps.First(app => app.Reviews < 10));
            //    Display(highRatedApps.Last());
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("W zbiorze danych nie ma takiego elementu!");
            //}

            Display(highRatedApps.FirstOrDefault(app => app.Reviews < 10));
            Display(highRatedApps.SingleOrDefault(app => app.Reviews < 300));
        }

        /// <summary>
        /// Część 2 - projekcja danych (select i mapowanie na obiekt)
        /// </summary>
        /// <param name="googleApps"></param>
        static void ProjectData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedApps = googleApps.Where(app => app.Rating > 4.6 && app.Category == Category.BEAUTY);
            var highRatedAppsNames = highRatedApps.Select(app => app.Name);

            //var dtos = highRatedApps.Select(app => new GoogleAppDTO()
            //{
            //    Reviews = app.Reviews,
            //    Name = app.Name
            //});

            //var genres = highRatedApps.SelectMany(app => app.Genres);
            //Console.WriteLine(string.Join(", ", dtos));

            var anonymousDtos = highRatedApps.Select(app => new
            {
                Reviews = app.Reviews,
                Name = app.Name
            });
        }

        /// <summary>
        /// Część 3 - dzielenie danych
        /// </summary>
        /// <param name="googleApps"></param>
        static void DivideData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedApps = googleApps.Where(app => app.Rating > 4.4 && app.Category == Category.BEAUTY);

            var firstFiveHighRatedApps = highRatedApps.Take(5);
            var lastFiveHighRatedApps = highRatedApps.TakeLast(5);

            // pobiera dane ze zbioru tak długo jak warunek jest spełniony
            var fiveHighRatedApps = highRatedApps.TakeWhile(app => app.Reviews > 1000);
            Display(fiveHighRatedApps);

            var skipResults0 = highRatedApps.Skip(5);
            var skipResults1 = highRatedApps.SkipWhile(app => app.Reviews > 1000);
        }

        /// <summary>
        /// Część 4 - sortowanie danych
        /// </summary>
        /// <param name="googleApps"></param>
        static void SortData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedApps = googleApps.Where(app => app.Rating > 4.4 && app.Category == Category.BEAUTY);

            // można sortować po wielu kolumnach za pomocą metod ThenBy oraz ThenByDescending
            var sortedResults = highRatedApps
                .OrderByDescending(app => app.Rating)
                .ThenBy(app => app.Name)
                .Take(10);
            Display(sortedResults);
        }

        /// <summary>
        /// Część 5 - operacje na zbiorach
        /// </summary>
        /// <param name="googleApps"></param>
        static void DataSetOperation(IEnumerable<GoogleApp> googleApps)
        {
            // Distinct - unikalne wartości
            var distinctRatings = googleApps
                .OrderByDescending(app => app.Rating)
                .Select(app => app.Rating)
                .Distinct();

            // Union - połączenie dwóch zbiorów
            var highRatedApps0 = googleApps
                .Where(app => app.Rating > 4.8);
            var lowRatedApps0 = googleApps
                .Where(app => app.Rating < 2.0);

            var unionRatings = highRatedApps0
                .Union(lowRatedApps0);

            //Display(unionRatings);

            // Intersect - część wspólna dwóch zbiorów
            var highRatedApps1 = googleApps
                .Where(app => app.Rating > 3.5);
            var lowRatedApps1 = googleApps
                .Where(app => app.Rating < 4.0);

            var intersectRatings = highRatedApps1
                .Intersect(lowRatedApps1);

            //Display(intersectRatings);

            // Except - elementy zawarte tylko w pierwszym zbiorze
            var highRatedApps2 = googleApps
                .Where(app => app.Rating > 3.5);
            var lowRatedApps2 = googleApps
                .Where(app => app.Rating < 4.0);

            var exceptRatings = highRatedApps2
                .Except(lowRatedApps2);

            Display(exceptRatings);
        }

        static void Display(IEnumerable<GoogleApp> googleApps)
        {
            foreach (var googleApp in googleApps)
            {
                Console.WriteLine(googleApp);
            }

        }
        static void Display(GoogleApp googleApp)
        {
            Console.WriteLine(googleApp);
        }

        static List<GoogleApp> LoadGoogleAps(string csvPath)
        {
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<GoogleAppMap>();
                var records = csv.GetRecords<GoogleApp>().ToList();
                return records;
            }
        }
    }
}


