using System;

namespace ConsoleApp1
{
    public class WeatherData: IEquatable<WeatherData>
    {
        DateTime DateTime { get; set; }
        double Temperature { get; set; } // fahrenheit
        double RelativeHumidity { get; set; } // %
        double WindVelocity { get; set; }
        double Value { get; set; } // 계산값

        protected WeatherData(DateTime datetime, double temperature, double relativeHumidity, double windVelocity)
        {
            DateTime = datetime;
            Temperature = temperature;
            RelativeHumidity = relativeHumidity;
            WindVelocity = windVelocity;
        }

        public override string ToString()
        {
            return String.Format("WeatherData [DataTime = {0}, Temperature = {1}, RelativeHumidity = {2}, WindVelocity = {3}]",
                DateTime, Temperature, RelativeHumidity, WindVelocity);
        }
        
        public bool Equals(WeatherData other)
        {
            if (other is not null)
            {
                return Temperature - other.Temperature <= Double.Epsilon &&
                       RelativeHumidity - other.RelativeHumidity <= Double.Epsilon &&
                       WindVelocity - other.WindVelocity <= Double.Epsilon;
            }

            return false;
        }

        public static bool operator ==(WeatherData x, WeatherData y)
        {
            return x?.Value - y?.Value <= Double.Epsilon;
        }

        public static bool operator !=(WeatherData x, WeatherData y)
        {
            return x?.Value - y?.Value > Double.Epsilon;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Temperature, RelativeHumidity, WindVelocity);
        }
    }
}