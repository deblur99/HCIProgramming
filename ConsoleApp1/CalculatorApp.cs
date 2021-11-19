using System;

namespace ConsoleApp1
{
    class CalculatorApp
    {
        public static void Main(string[] args)
        {
            WeatherCalculator[] calculators =
            {
                new DewPointCalculator(),
                new WindChillTemperatureCalculator(),
                new HeatIndexCalculator()
            };

            WeatherCalculator myCal;
            string s = "";

            int decision;

            for (;;)
            {
                // get mode
                Console.Write("Please enter mode [1: DP, 2: WCT, 3: HI] >>");
                s = Console.ReadLine();

                if (Int32.TryParse(s, out decision))
                {
                    if (decision == 1 || decision == 2 || decision == 3)
                        break;
                }
            }

            // calculate DP/WCT/HI
            switch (decision)
            {
                case 1:
                    myCal = new DewPointCalculator();
                    break;
                case 2:
                    myCal = new WindChillTemperatureCalculator();
                    break;
                case 3:
                    myCal = new HeatIndexCalculator();
                    break;
                default:
                    myCal = null;
                    break;
            }

            myCal.Process();
        }
    }
}