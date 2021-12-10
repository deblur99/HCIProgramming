using System;

namespace ConsoleApp1
{
    public class HeatIndexCalculator : WeatherCalculator
    {
        public enum HeatIndex
        {
            EXTREME_DANGER = 0,
            DANGER,
            EXTREME_CAUTION,
            CAUTION
        }

        public HeatIndexCalculator(WeatherData weatherData) : base(weatherData)
        {
        }

        public override void PrintTable()
        {
            Console.WriteLine("Print WindChillTemperature");
            int[] fahrenheit = {80, 82, 84, 86, 88, 90, 92, 94, 96, 98, 100, 102, 104, 106, 108, 110};
            int[] humidities = {40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100};
            Console.WriteLine("RH/F\t80\t82\t84\t86\t88\t90\t92\t94\t96\t98\t100\t102\t104\t106\t108\t110");
            for (int i = 0; i < humidities.Length; i++)
            {
                Console.Write(humidities[i] + "\t");
                for (int j = 0; j < fahrenheit.Length; j++)
                {
                    double value = Calculate(fahrenheit[j], humidities[i]);
                    if (value < 137)
                        Console.Write(value + "\t");
                    else
                        Console.Write("\t\t");
                }

                Console.WriteLine();
            }
        }

        public override void GetUserInput()
        {
            string input = "";

            Console.WriteLine("Calculate HeatIndex");

            Console.Write("Please enter temperature (F) >>");
            WeatherData.Temperature = Double.Parse(Console.ReadLine());

            Console.Write("Please enter relative humidity (mph) >>");
            WeatherData.RelativeHumidity = Double.Parse(Console.ReadLine());
        }

        public override void Calculate()
        {
            Value = Calculate(WeatherData.Temperature, WeatherData.RelativeHumidity);
        }

        public double Calculate(double temperature, double humidity)
        {
            double t_temperature = FahrenheitToCelsius(temperature);

            double result = CelsiusToFahrenheit(-42.379 + (2.04901523 * t_temperature) + (10.14333127 * humidity) -
                                                (0.22475541 * t_temperature * humidity) -
                                                (0.00683770 * t_temperature * t_temperature) -
                                                (0.05481717 * humidity * humidity) +
                                                (0.00122874 * t_temperature * t_temperature * humidity) +
                                                (0.00085282 * t_temperature * humidity * humidity) -
                                                (0.00000199 * t_temperature * t_temperature * humidity * humidity));

            return Math.Round(result * 10) / 10.0;
        }

        public override string ToString()
        {
            return String.Format(
                "HeatIndexCalculator [Temperature={0}, WindVelocity={1}, Value={2:0.#}, Index={3}]",
                WeatherData.Temperature, WeatherData.RelativeHumidity, WeatherData.Value, GetIndex(Value));
        }

        public static HeatIndex? GetIndex(double value)
        {
            HeatIndex? index = null;

            if (value > 130.0f)
            {
                index = HeatIndex.EXTREME_DANGER;
            }

            if (value >= 105.0f && value < 130.0f)
            {
                index = HeatIndex.DANGER;
            }

            if (value >= 90.0f && value < 105.0f)
            {
                index = HeatIndex.EXTREME_CAUTION;
            }

            if (value >= 80.0f && value < 90.0f)
            {
                index = HeatIndex.CAUTION;
            }

            return index;
        }
    }
}