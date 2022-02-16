using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection;
using Stages;

namespace Stages
{
    class Program
    {
        static async Task Main()
        {
            string path = Path.GetFullPath(@"..\..\..\" + @$"\test.json");
            StagesList stages = await JsonFileMove.ReadAsync<StagesList>(path);

            MoveDocument moveDocument = new MoveDocument();
            moveDocument.Run(stages);

            Report report = new Report();
            report.Run();


            Console.ReadKey();
        }
    }
    internal static class JsonFileMove
    {
        public static async Task<T> ReadAsync<T>(string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
    }
}
