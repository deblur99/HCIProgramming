using System;

namespace ConsoleApp1
{
    public abstract class WeatherCalculator : IWeatherCalculator, IEquatable<WeatherCalculator>
    {
        protected WeatherData WeatherData { get; set; }

        protected double Value // 자식클래스에서 공통으로 사용
        {
            get;
            set;
        }

        protected WeatherCalculator(WeatherData weatherData)
        {
            WeatherData = weatherData;
            Process();
            Console.WriteLine(this);
        }

        // 화씨 온도와 섭씨 온도 간 변환
        protected double CelsiusToFahrenheit(double c)
        {
            return 1.8f * c + 32;
        }

        protected double FahrenheitToCelsius(double f)
        {
            return (f - 32) * 5 / 9;
        }

        public abstract void PrintTable();

        public abstract void GetUserInput();

        public abstract void Calculate();

        public void Process() // 내부적으로 PrintTable, GetUserInput, Calculate 호출
        {
            PrintTable();
            GetUserInput();
            Calculate();
        }
        
        public static bool operator ==(WeatherCalculator x, WeatherCalculator y)
        {
            return x?.Value - y?.Value <= Double.Epsilon;
        }

        public static bool operator !=(WeatherCalculator x, WeatherCalculator y)
        {
            return x?.Value - y?.Value > Double.Epsilon;
        }
        
        public bool Equals(WeatherCalculator other)
        {
            if (other is not null)
            {
                return Value - other.Value <= Double.Epsilon;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(WeatherData);
        }
    }
}