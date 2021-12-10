using System;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WeatherCalculator[] calc = new WeatherCalculator[4];

            calc[0] = new DewPointCalculator(new WeatherData());
            calc[1] = new WindChillTemperatureCalculator(new WeatherData());
            calc[2] = new HeatIndexCalculator(new WeatherData());
            calc[3] = new DiscomfortIndexCalculator(new WeatherData());
        }
    }
}