using System;

namespace ConsoleApp1
{
    class HeatIndexCalculator : WeatherCalculator
    {
        private double RelativeHumidity;

        // 테이블 배열 생성
        private string[,] _heatIndexTable = new string[14, 17];

        // 열지수 Index
        public enum HeatIndex
        {
            EXTREME_DANGER = 0,
            DANGER,
            EXTREME_CAUTION,
            CAUTION
        }

        public HeatIndexCalculator()
        {
            RelativeHumidity = 0.0f;
        }

        protected override void BuildTable()
        {
            // 표에 들어갈 값을 계산
            double Calculate(double F, double RH)
            {
                double HI = -42.379 + (2.04901523 * F) + (10.14333127 * RH) - (0.22475541 * F * RH) -
                    (0.00683770 * F * F) - (0.05481717 * RH * RH) + (0.00122874 * F * F * RH) +
                    (0.00085282 * F * RH * RH) - (0.00000199 * F * F * RH * RH);

                return Math.Round(HI, 1);
            }

            // row
            double[] _RHList =
            {
                40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100
            };

            // column
            double[] _FList =
            {
                80, 82, 84, 86, 88, 90, 92, 94, 96, 98, 100, 102, 104, 106, 108, 110
            };

            // 몇 번째 column 인덱스부터 NaN이 존재하는지 지정하는 배열
            int[] filter =
            {
                _FList.Length, _FList.Length - 1, _FList.Length - 2, _FList.Length - 3, _FList.Length - 4,
                _FList.Length - 5, _FList.Length - 6, _FList.Length - 7, _FList.Length - 8, _FList.Length - 8,
                _FList.Length - 9, _FList.Length - 10, _FList.Length - 10
            };

            // 0, 0 인덱스
            _heatIndexTable[0, 0] = "RH/F";

            // 0번째 row
            for (int i = 0; i < _FList.Length; i++)
            {
                _heatIndexTable[0, i + 1] = _FList[i].ToString();
            }

            // 0번째 column
            for (int i = 0; i < _RHList.Length; i++)
            {
                _heatIndexTable[i + 1, 0] = _RHList[i].ToString();
            }

            // others
            for (int i = 0; i < _RHList.Length; i++)
            {
                for (int j = 0; j < _FList.Length; j++)
                {
                    // column 인덱스가 filter 배열의 값에 도달하면 그 뒤 값은 -1로 채움
                    if (j >= filter[i])
                    {
                        _heatIndexTable[i + 1, j + 1] = "-1";
                        continue;
                    }

                    // 그 외 인덱스에는 계산하는 함수의 반환값을 입력한다.
                    _heatIndexTable[i + 1, j + 1] = Calculate(_FList[j], _RHList[i]).ToString();
                }
            }
        }

        protected override void PrintTable()
        {
            BuildTable();

            int i = 0, j = 0;

            while (i < _heatIndexTable.GetLength(0))
            {
                while (j < _heatIndexTable.GetLength(1))
                {
                    // 테이블의 요소가 -1인 지점을 발견하기 전까지 출력하며, 각 요소는 tab 문자로 구분된다.
                    if (!_heatIndexTable[i, j].Equals("-1"))
                        Console.Write("{0}\t", _heatIndexTable[i, j]);

                    // 요소가 -1인 지점을 발견하면 다음 row로 넘긴다.
                    else
                        j = _heatIndexTable.GetLength(1);

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

            Console.WriteLine("Calculate HeatIndex");

            Console.Write("Please enter temperature (F) >>");
            input = Console.ReadLine();
            Double.TryParse(input, out Temperature);

            Console.Write("Please enter relative humidity (mph) >>");
            input = Console.ReadLine();
            Double.TryParse(input, out RelativeHumidity);
        }

        protected override void Calculate()
        {
            Value = -42.379 + (2.04901523 * Temperature) + (10.14333127 * RelativeHumidity) -
                    (0.22475541 * Temperature * RelativeHumidity) -
                    (0.00683770 * Temperature * Temperature) - (0.05481717 * RelativeHumidity * RelativeHumidity) +
                    (0.00122874 * Temperature * Temperature * RelativeHumidity) +
                    (0.00085282 * Temperature * RelativeHumidity * RelativeHumidity) -
                    (0.00000199 * Temperature * Temperature * RelativeHumidity * RelativeHumidity);

            Console.WriteLine(ToString());
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

        public override string ToString()
        {
            return String.Format(
                "HeatIndexCalculator [Temperature={0}, WindVelocity={1}, Value={2:0.#}, Index={3}]",
                Temperature, RelativeHumidity, Value, GetIndex(Value));
        }
    }
}