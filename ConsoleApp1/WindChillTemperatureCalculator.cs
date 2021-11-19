using System;

namespace ConsoleApp1
{
    class WindChillTemperatureCalculator : WeatherCalculator
    {
        private double WindVelocity;

        // 테이블 배열 생성
        private string[,] _windChillTemperatureTable = new string[13, 19];

        // 체감온도 Index
        public enum WindChillTemperatureIndex
        {
            EXTREME_DANGER = 0,
            DANGER,
            WARNING,
            CAUTION,
            AWARE
        }

        public WindChillTemperatureCalculator()
        {
            WindVelocity = 0.0f;
        }

        // 체감 온도 테이블 생성
        protected override void BuildTable()
        {
            // 표에 들어갈 값을 계산
            double Calculate(double F, double W)
            {
                double WCT = 35.74 + 0.6215 * F - 35.75 * Math.Pow(W, 0.16) + 0.4275 * F * Math.Pow(W, 0.16);

                return Math.Round(WCT, 1);
            }

            // row
            double[] _WList =
            {
                5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60
            };

            // column
            double[] _FList =
            {
                40, 35, 30, 25, 20, 15, 10, 5, 0, -5, -10, -15, -20, -25, -30, -35, -40, -45
            };

            // 0, 0 인덱스
            _windChillTemperatureTable[0, 0] = "W/F";

            // 0번째 row
            for (int i = 0; i < _FList.Length; i++)
            {
                _windChillTemperatureTable[0, i + 1] = _FList[i].ToString();
            }

            // 0번째 column
            for (int i = 0; i < _WList.Length; i++)
            {
                _windChillTemperatureTable[i + 1, 0] = _WList[i].ToString();
            }

            // others
            for (int i = 0; i < _WList.Length; i++)
            {
                for (int j = 0; j < _FList.Length; j++)
                {
                    // 결과값 계산 후 테이블에 입력
                    _windChillTemperatureTable[i + 1, j + 1] = Calculate(_FList[j], _WList[i]).ToString();
                }
            }
        }

        protected override void PrintTable()
        {
            BuildTable();

            int i = 0, j = 0;

            while (i < _windChillTemperatureTable.GetLength(0))
            {
                while (j < _windChillTemperatureTable.GetLength(1))
                {
                    Console.Write("{0}\t", _windChillTemperatureTable[i, j]);
                    j++;
                }

                Console.WriteLine("");
                i++;
                j = 0;
            }
        }

        protected override void GetUserInput()
        {
            string input = "";

            Console.WriteLine("Calculate WindChillTemperature");

            Console.Write("Please enter temperature (F) >>");
            input = Console.ReadLine();
            Double.TryParse(input, out Temperature);

            Console.Write("Please enter wind velocity (mph) >>");
            input = Console.ReadLine();
            Double.TryParse(input, out WindVelocity);
        }

        protected override void Calculate()
        {
            Value = 35.74 + 0.6215 * Temperature - 35.75 * Math.Pow(WindVelocity, 0.16)
                    + 0.4275 * Temperature * Math.Pow(WindVelocity, 0.16);

            Console.Write(ToString());
        }

        public static WindChillTemperatureIndex? GetIndex(double value)
        {
            WindChillTemperatureIndex? index = null;

            if (value < -75.0f)
            {
                index = WindChillTemperatureIndex.EXTREME_DANGER;
            }

            if (value >= -75.0f && value < -50.0f)
            {
                index = WindChillTemperatureIndex.DANGER;
            }

            if (value >= -50.0f && value < -15.0f)
            {
                index = WindChillTemperatureIndex.WARNING;
            }

            if (value >= -15.0f && value < 15.0f)
            {
                index = WindChillTemperatureIndex.CAUTION;
            }

            if (value >= 15.0f && value < 32.0f)
            {
                index = WindChillTemperatureIndex.AWARE;
            }

            return index;
        }

        public override string ToString()
        {
            return String.Format(
                "WindChillTemperatureCalculator [Temperature={0}, WindVelocity={1}, Value={2:0.#}, Index={3}]",
                Temperature, WindVelocity, Value, GetIndex(Value));
        }
    }
}