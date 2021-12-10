using System;

namespace ConsoleApp1
{
    public class DiscomfortIndexCalculator : WeatherCalculator
    {
        public enum DiscomfortIndex
        {
            EXTREME_DISCOMFORT = 0,
            VERY_HIGH_DISCOMFORT,
            HIGH_DISCOMFORT,
            MODERATE_DISCOMFORT,
            LOW_DISCOMFORT,
            NO_DISCOMFORT
        }

        public DiscomfortIndexCalculator(WeatherData weatherData) : base(weatherData)
        {
        }

        public override void PrintTable()
        {
            Console.WriteLine("Print DiscomfortIndexCalculator");
            int[] fahrenheit = {68, 71, 74, 77, 80, 83, 86, 89, 92, 95, 98, 101, 104, 107, 110};
            int[] humidities = {25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100};
            Console.WriteLine("RH/F\t80\t82\t84\t86\t88\t90\t92\t94\t96\t98\t100\t102\t104\t106\t108\t110");
            for (int i = 0; i < humidities.Length; i++)
            {
                Console.Write(humidities[i] + "\t");
                for (int j = 0; j < fahrenheit.Length; j++)
                {
                    double value = Calculate(fahrenheit[j], humidities[i]);
                    Console.Write(value + "\t");
                }

                Console.WriteLine();
            }
        }

        public override void GetUserInput()
        {
            string input = "";

            Console.WriteLine("Calculate DiscomfortIndexCalculator");

            Console.Write("Please enter temperature (F) >>");
            WeatherData.Temperature = Double.Parse(Console.ReadLine());

            Console.Write("Please enter relative humidity (%) >>");
            WeatherData.RelativeHumidity = Double.Parse(Console.ReadLine());
        }

        public override void Calculate()
        {
            Value = Calculate(WeatherData.Temperature, WeatherData.RelativeHumidity);
        }

        public double Calculate(double temperature, double humidity)
        {
            double f_temperature = FahrenheitToCelsius(temperature);

            double result =
                CelsiusToFahrenheit((f_temperature - 0.55) * (1 - 0.01 * humidity) * (f_temperature - 14.5));

            return Math.Round(result * 10) / 10.0;
        }

        public override string ToString()
        {
            return String.Format(
                "DiscomfortIndexCalculator [Temperature={0}, RelativeHumidity={1}, Value={2}, Index={3}]",
                WeatherData.Temperature, WeatherData.RelativeHumidity, Value, GetIndex(Value));
        }

        public static DiscomfortIndex? GetIndex(double value)
        {
            if (value > 89.6)
                return DiscomfortIndex.EXTREME_DISCOMFORT;
            if (value >= 86 && value < 89.6)
                return DiscomfortIndex.VERY_HIGH_DISCOMFORT;
            if (value >= 82.4 && value < 86)
                return DiscomfortIndex.HIGH_DISCOMFORT;
            if (value >= 77 && value < 82.4)
                return DiscomfortIndex.MODERATE_DISCOMFORT;
            if (value >= 69.8 && value < 77)
                return DiscomfortIndex.LOW_DISCOMFORT;
            if (value < 69.8)
                return DiscomfortIndex.NO_DISCOMFORT;

            return null;
        }
    }
}