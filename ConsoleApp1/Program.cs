using System;

namespace ConsoleApp1
{
    public class Program
    {
        static WeatherData[] weatherData = {
            new WeatherData(new DateTime(2019, 1, 1), 30.38, 46, 4.0265), new WeatherData(new DateTime(2019, 2, 1), 33.8, 47, 4.0265),
            new WeatherData(new DateTime(2019, 3, 1), 44.78, 51, 4.6976), new WeatherData(new DateTime(2019, 4, 1), 53.78, 51, 4.2502),
            new WeatherData(new DateTime(2019, 5, 1), 66.92, 47, 4.6976), new WeatherData(new DateTime(2019, 6, 1), 72.5, 61, 3.8028),
            new WeatherData(new DateTime(2019, 7, 1), 78.62, 69, 4.0265), new WeatherData(new DateTime(2019, 8, 1), 80.96, 69, 3.5791),
            new WeatherData(new DateTime(2019, 9, 1), 72.68, 65, 4.9213), new WeatherData(new DateTime(2019, 10, 1), 61.52, 62, 4.6976),
            new WeatherData(new DateTime(2019, 11, 1), 45.68, 56, 4.9213), new WeatherData(new DateTime(2019, 12, 1), 34.52, 58, 4.6976),
            new WeatherData(new DateTime(2020, 1, 1), 34.88, 56, 4.6976), new WeatherData(new DateTime(2020, 2, 1), 36.5, 58, 5.1450),
            new WeatherData(new DateTime(2020, 3, 1), 45.86, 46, 5.5924), new WeatherData(new DateTime(2020, 4, 1), 51.98, 50, 6.7108),
            new WeatherData(new DateTime(2020, 5, 1), 64.4, 67, 5.3687), new WeatherData(new DateTime(2020, 6, 1), 75.02, 68, 5.1450),
            new WeatherData(new DateTime(2020, 7, 1), 75.38, 77, 5.3687), new WeatherData(new DateTime(2020, 8, 1), 79.7, 85, 5.1450),
            new WeatherData(new DateTime(2020, 9, 1), 70.52, 71, 5.5924), new WeatherData(new DateTime(2020, 10, 1), 57.74, 61, 4.6976),
            new WeatherData(new DateTime(2020, 11, 1), 46.4, 64, 4.9213), new WeatherData(new DateTime(2020, 12, 1), 31.46, 58, 4.9213)
        };
        
        public static void Main(string[] args)
        {
            WeatherCalculator[] calc = new WeatherCalculator[4];

            calc[0] = new DewPointCalculator(weatherData[0]);
            calc[1] = new WindChillTemperatureCalculator(weatherData[5]);
            calc[2] = new HeatIndexCalculator(weatherData[9]);
            calc[3] = new DiscomfortIndexCalculator(weatherData[12]);
        }
    }
}