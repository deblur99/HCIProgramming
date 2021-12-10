using System;

namespace ConsoleApp1
{
    public class DewPointCalculator : WeatherCalculator
    {
        public DewPointCalculator(WeatherData weatherData) : base(weatherData)
        {
        }

        public override void PrintTable()
        {
            int[] humidities = {100, 95, 90, 85, 80, 75, 70, 65, 60, 55, 50, 45, 40, 35, 30, 25, 20, 15, 10};
            int[] fahrenheit = {110, 105, 100, 95, 90, 85, 80, 75, 70, 65, 60, 55, 50, 45, 40, 35, 32};
            Console.WriteLine("F/RH\t100\t95\t90\t85\t80\t75\t70\t65\t60\t55\t50\t45\t40\t35\t30\t25\t20\t15\t10");
            for (int i = 0; i < fahrenheit.Length; i++)
            {
                Console.Write(fahrenheit[i] + "\t");
                for (int j = 0; j < humidities.Length; j++)
                {
                    double dpt = Calculate(fahrenheit[i], humidities[j]);
                    if (dpt >= 32)
                    {
                        Console.Write(dpt + "\t");
                    }
                    else
                    {
                        Console.Write("\t\t");
                    }
                }

                Console.WriteLine();
            }
        }

        public override void GetUserInput()
        {
            string input = "";

            Console.WriteLine("Calculate DewPoint");

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
            double t_temperature = FahrenheitToCelsius(temperature);
            
            double result = CelsiusToFahrenheit(243.12 * (Math.Log(humidity / 100) + 17.62 * t_temperature / (243.12 + t_temperature)) /
                                                (17.62 - (Math.Log(humidity / 100) + 17.62 * t_temperature / (243.12 + t_temperature)))); 
            
            return Math.Round(result * 10) / 10.0;
        }

        public override string ToString()
        {
            return String.Format("DewPointCalculator [Temperature={0}, RelativeHumidity={1}, Value={2}]",
                WeatherData.Temperature, WeatherData.RelativeHumidity, Value);
        }
    }
}