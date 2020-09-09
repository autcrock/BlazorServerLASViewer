using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerLASViewer.Data
{
    public class WellService
    {
        //private static readonly string[] Summaries = new[]
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        public static Task<WellModel[]> GetWellModelAsync(/*DateTime startDate*/)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WellModel
            {
                //Date = startDate.AddDays(index),
                //TemperatureC = rng.Next(-20, 55),
                //Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }
    }
}
