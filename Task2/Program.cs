using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAppExample
{
    public class WeatherApp
    {
        public async Task<double> GetAverageTemperatureAsync(List<Func<Task<double>>> apiCalls)
        {
            var tasks = apiCalls.Select(apiCall => HandleApiCallAsync(apiCall)).ToList();
            
            var results = await Task.WhenAll(tasks);
            var successfulResults = results.Where(result => result.HasValue).Select(result => result.Value);
            
            if (!successfulResults.Any())
            {
                throw new InvalidOperationException("No successful API calls.");
            }
            
            return successfulResults.Average();
        }

        private async Task<double?> HandleApiCallAsync(Func<Task<double>> apiCall)
        {
            try
            {
                return await apiCall();
            }
            catch
            {
                return null;
            }
        }
        public async Task<double> API1()
        {
            await Task.Delay(1000); 
            return 25.0;
        }

        public async Task<double> API2()
        {
            await Task.Delay(1000);
            return 27.0; 
        }

        public async Task<double> API3()
        {
            await Task.Delay(1000); 
            throw new Exception("API call failed");
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var weatherApp = new WeatherApp();

            List<Func<Task<double>>> apiCalls = new List<Func<Task<double>>>
            {
                async () => await weatherApp.API1(),
                async () => await weatherApp.API2(),
                async () => await weatherApp.API3()
            };

            try
            {
                double averageTemperature = await weatherApp.GetAverageTemperatureAsync(apiCalls);
                Console.WriteLine($"Average Temperature: {averageTemperature}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
