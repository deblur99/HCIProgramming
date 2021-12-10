using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class WeatherCalculatorListManager
    {
        private List<WeatherCalculator> calculators = null;

        public WeatherCalculatorListManager() : this(new List<WeatherCalculator>())
        {
        }

        public WeatherCalculatorListManager(List<WeatherCalculator> calculators)
        {
            this.calculators = calculators;
        }

        // 리스트에서 해당 predicate 조건에 맞는 모든 calculator 리스트 반환
        public IList<WeatherCalculator> Select(Predicate<WeatherCalculator>
            predicate)
        {
            List<WeatherCalculator> result = calculators.FindAll(calc => calc.Equals(calc));
            return result;
        }

        // 인자로 넘겨준 리스트에서 랜덤하게 하나 선택해서 반환
        public WeatherCalculator GetRandom(IList<WeatherCalculator> list)
        {
            Random randomGenerator = new Random();
            int idx = randomGenerator.Next(list.Count);
            return calculators[idx];
        }
    }
}