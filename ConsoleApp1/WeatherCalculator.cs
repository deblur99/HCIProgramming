using System;

namespace ConsoleApp1
{
    public abstract class WeatherCalculator
    {
        protected double Temperature; // fahrenheit

        public WeatherCalculator()
        {
            Temperature = 0.0f;
        }

        protected double Value // 자식클래스에서 공통으로 사용
        {
            get;
            set;
        }

        public void Process() // 내부적으로 PrintTable, GetUserInput, Calculate 호출
        {
            PrintTable();
            GetUserInput();
            Calculate();
        }

        protected abstract void BuildTable();

        protected abstract void PrintTable();

        protected abstract void GetUserInput();

        protected abstract void Calculate();
    }

    class DewPointCalculator : WeatherCalculator
    {
        private double RelativeHumidity;

        // 테이블 배열 생성
        private string[,] _dewPointTable = new string[18, 20];

        public DewPointCalculator()
        {
            RelativeHumidity = 0.0f;
        }

        protected override void BuildTable()
        {
            // 표에 들어갈 값을 계산
            double Calculate(double F, double RH)
            {
                // 화씨 온도와 섭씨 온도 간 변환
                double CelsiusToFahrenheit(double c)
                {
                    return 1.8f * c + 32;
                }

                double FahrenheitToCelsius(double f)
                {
                    return (f - 32) * 5 / 9;
                }

                double C = FahrenheitToCelsius(F);
                double dewPoint;

                // C#에서의 로그 값을 구하는 함수는 Math 라이브러리에 내장되어 있다.
                dewPoint = 243.12 * (Math.Log(RH / 100) + 17.62 * C / (243.12 + C)) /
                           (17.62 - (Math.Log(RH / 100) + 17.62 * C / (243.12 + C)));

                // 소수 둘째 자리에서 반올림하여 실수 값을 소수점 하나만 남기는 Math 라이브러리의 Round 함수 호출
                return Math.Round(CelsiusToFahrenheit(dewPoint), 1);
            }

            // row
            double[] _FList =
            {
                110, 105, 100, 95, 90, 85, 80, 75, 70, 65, 60, 55, 50, 45, 40, 35, 32
            };

            // column
            double[] _RHList =
            {
                100, 95, 90, 85, 80, 75, 70, 65, 60, 55, 50, 45, 40, 35, 30, 25, 20, 15, 10
            };

            // 몇 번째 column 인덱스부터 NaN이 존재하는지 지정하는 배열
            int[] filter =
            {
                _RHList.Length, _RHList.Length, _RHList.Length, _RHList.Length - 1, _RHList.Length - 1,
                _RHList.Length - 1,
                _RHList.Length - 2, _RHList.Length - 3, _RHList.Length - 3, _RHList.Length - 4, _RHList.Length - 5,
                _RHList.Length - 7, _RHList.Length - 8, _RHList.Length - 11, _RHList.Length - 13, _RHList.Length - 16,
                _RHList.Length - 18
            };

            _dewPointTable[0, 0] = "F/RH";

            // 0번째 row에 RH 기준값 채우기
            for (int i = 0; i < _RHList.Length; i++)
            {
                _dewPointTable[0, i + 1] = _RHList[i].ToString();
            }

            // 0번째 col에 F 기준값 채우기
            for (int i = 0; i < _FList.Length; i++)
            {
                _dewPointTable[i + 1, 0] = _FList[i].ToString();
            }

            // 1번째 row, 1번째 col부터 DewPoint 값 채우기
            for (int i = 0; i < _FList.Length; i++)
            {
                for (int j = 0; j < _RHList.Length; j++)
                {
                    // column 인덱스가 filter 배열의 값에 도달하면 그 뒤 값은 -1로 채움
                    if (j >= filter[i])
                    {
                        _dewPointTable[i + 1, j + 1] = "-1";
                        continue;
                    }

                    // 그 외 인덱스에는 계산하는 함수의 반환값을 입력한다.
                    _dewPointTable[i + 1, j + 1] = Calculate(_FList[i], _RHList[j]).ToString();
                }
            }
        }

        protected override void PrintTable()
        {
            BuildTable();

            int i = 0, j = 0;

            while (i < _dewPointTable.GetLength(0))
            {
                while (j < _dewPointTable.GetLength(1))
                {
                    // 테이블의 요소가 -1인 지점을 발견하기 전까지 출력하며, 각 요소는 tab 문자로 구분된다.
                    if (!_dewPointTable[i, j].Equals("-1"))
                        Console.Write("{0}\t", _dewPointTable[i, j]);

                    // 요소가 -1인 지점을 발견하면 다음 row로 넘긴다.
                    else
                        j = _dewPointTable.GetLength(1);

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

            Console.WriteLine("Calculate DewPoint");

            Console.Write("Please enter temperature (F) >>");
            input = Console.ReadLine();
            Double.TryParse(input, out Temperature);

            Console.Write("Please enter relative humidity (%) >>");
            input = Console.ReadLine();
            Double.TryParse(input, out RelativeHumidity);
        }

        protected override void Calculate()
        {
            Value = 243.12 * (Math.Log(RelativeHumidity / 100) + 17.62 * Temperature / (243.12 + Temperature)) /
                    (17.62 - (Math.Log(RelativeHumidity / 100) + 17.62 * Temperature / (243.12 + Temperature)));

            Console.WriteLine(ToString());
        }

        public override string ToString()
        {
            return String.Format("DewPointCalculator [Temperature={0}, WindVelocity={1}, Value={2:0.#}]",
                Temperature, RelativeHumidity, Value);
        }
    }

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