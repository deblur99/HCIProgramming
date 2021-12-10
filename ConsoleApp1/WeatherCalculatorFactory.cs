using System;

namespace ConsoleApp1
{
    public class WeatherCalculatorFactory
    {
        public static WeatherCalculator GetInstance(int mode)
        {
            switch (mode)
            {
                case 0:
                    return new DewPointCalculator(new WeatherData());
                case 1:
                    return new WindChillTemperatureCalculator(new WeatherData());
                case 2:
                    return new HeatIndexCalculator(new WeatherData());
                case 3:
                    return new DiscomfortIndexCalculator(new WeatherData());
                default:
                    return null;
            }
        }

        public static WeatherCalculator GetInstance(int mode, WeatherData data)
        {
            switch (mode)
            {
                case 0:
                    return new DewPointCalculator(data);
                case 1:
                    return new WindChillTemperatureCalculator(data);
                case 2:
                    return new HeatIndexCalculator(data);
                case 3:
                    return new DiscomfortIndexCalculator(data);
                default:
                    return null;
            }
        }
    }
}