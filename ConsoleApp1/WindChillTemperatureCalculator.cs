using System;

namespace ConsoleApp1
{
    public class WindChillTemperatureCalculator : WeatherCalculator
    {
        // 체감온도 Index
        public enum WindChillTemperatureIndex
        {
            EXTREME_DANGER = 0,
            DANGER,
            WARNING,
            CAUTION,
            AWARE
        }

        public WindChillTemperatureCalculator(WeatherData weatherData) : base(weatherData)
        {
        }

        public override void PrintTable()
        {
            // 체감 온도 테이블 생성
            void BuildTable()
            {
                Console.WriteLine("Print WindChillTemperature");
                Console.WriteLine("W/F\t40\t35\t30\t25\t20\t15\t10\t5\t0\t-5\t-10\t-15\t-20\t-25\t-30\t-35\t-40\t-45");
                for (int wind = 5; wind < 65; wind += 5)
                {
                    Console.Write(wind + "\t");
                    for (int fahrenheit = 40; fahrenheit > -50; fahrenheit -= 5)
                    {
                        double value = Calculate(fahrenheit, wind);
                        Console.Write(value + "\t");
                    }

                    Console.WriteLine();
                }
            }
        }

        public override void GetUserInput()
        {
            string input = "";

            Console.WriteLine("Calculate WindChillTemperaturePoint");

            Console.Write("Please enter temperature (F) >>");
            WeatherData.Temperature = Double.Parse(Console.ReadLine());

            Console.Write("Please enter wind velocity (%) >>");
            WeatherData.WindVelocity = Double.Parse(Console.ReadLine());
        }

        public override void Calculate()
        {
            Value = Calculate(WeatherData.Temperature, WeatherData.WindVelocity);
        }

        public double Calculate(double temperature, double velocity)
        {
            double t_temperature = FahrenheitToCelsius(temperature);

            double result = CelsiusToFahrenheit(35.74 + 0.6215 * t_temperature - 35.75 * Math.Pow(velocity, 0.16)
                                                + 0.4275 * t_temperature * Math.Pow(velocity, 0.16));
            
            return Math.Round(result * 10) / 10.0;
        }

        public override string ToString()
        {
            return String.Format(
                "WindChillTemperatureCalculator [Temperature={0}, WindVelocity={1}, Value={2:0.#}, Index={3}]",
                WeatherData.Temperature, WeatherData.WindVelocity, WeatherData.Value, GetIndex(Value));
        }

        public static WindChillTemperatureIndex? GetIndex(double value)
        {
            if (value < -75.0f)
            {
                return WindChillTemperatureIndex.EXTREME_DANGER;
            }

            if (value >= -75.0f && value < -50.0f)
            {
                return WindChillTemperatureIndex.DANGER;
            }

            if (value >= -50.0f && value < -15.0f)
            {
                return WindChillTemperatureIndex.WARNING;
            }

            if (value >= -15.0f && value < 15.0f)
            {
                return WindChillTemperatureIndex.CAUTION;
            }

            if (value >= 15.0f && value < 32.0f)
            {
                return WindChillTemperatureIndex.AWARE;
            }

            return null;
        }
    }
}