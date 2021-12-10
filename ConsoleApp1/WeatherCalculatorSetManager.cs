using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class WeatherCalculatorSetManager
    {
        private ISet<WeatherCalculator> calculators = null;

        public WeatherCalculatorSetManager() : this(new HashSet<WeatherCalculator>())
        {
        }

        public WeatherCalculatorSetManager(ISet<WeatherCalculator> calculators)
        {
            this.calculators = calculators;
        }
        
        // 셋에서 해당 predicate 조건에 맞는 모든 calculator 셋 반환
        public ISet<WeatherCalculator> Select(Predicate<WeatherCalculator>
            predicate)
        {
            ISet<WeatherCalculator> result = new HashSet<WeatherCalculator>();
            foreach (WeatherCalculator c in this.calculators)
            {
                if (predicate(c))
                {
                    result.Add(c);
                }
            }
            return result;
        }

        // 인자로 넘겨준 셋에서 랜덤하게 하나 선택해서 반환
        public WeatherCalculator GetRandom(ISet<WeatherCalculator> set)
        {
            Random randomGenerator = new Random();
            int idx = randomGenerator.Next(set.Count);
            
            WeatherCalculator result = null;
            int index = 0;
            
            IEnumerator<WeatherCalculator> iter = set.GetEnumerator();
            
            while (iter.MoveNext())
            {
                result = iter.Current;
                if (index == idx)
                {
                    return result;
                }
                index++;
            }
            return result;
        }
    }
}