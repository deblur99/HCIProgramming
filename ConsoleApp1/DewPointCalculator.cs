using System;

namespace ConsoleApp1
{
    class Calculator
    {
        protected double temperature; // fahrenheit

        protected double value; // 환산 결과값

        public double Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }

        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        protected virtual void Init()
        {
            temperature = 0.0f;
            value = 0.0f;
        }

        public virtual void BuildTable()
        {
            return;
        }

        public virtual void Calculate()
        {
            return;
        }

        // 사용자로부터 화씨 온도와 상대습도 입력받기
        public virtual void GetUserInput()
        {
            return;
        }

        public virtual void ShowCalcResult()
        {
            return;
        }

        // 이슬점 테이블 출력
        public virtual void PrintTable()
        {
            return;
        }
    }

    class DewPointCalculator : Calculator
    {
        // 테이블 배열 생성
        private string[,] _dewPointTable = new string[18, 20];

        private double relativeHumidity; // %

        public double RelativeHumidity
        {
            get { return relativeHumidity; }
            set { relativeHumidity = value; }
        }

        protected override void Init()
        {
            base.Init();
            relativeHumidity = 0.0f;
        }

        public override void BuildTable()
        {
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

        // 테이블의 요소를 출력하는 메서드
        public override void PrintTable()
        {
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

        public override void Calculate()
        {
            value = 243.12 * (Math.Log(relativeHumidity / 100) + 17.62 * temperature / (243.12 + temperature)) /
                    (17.62 - (Math.Log(relativeHumidity / 100) + 17.62 * temperature / (243.12 + temperature)));
        }

        // double 매개변수 2개를 갖는 메서드 선언 및 정의
        public static double Calculate(double F, double RH)
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

        public override void ShowCalcResult()
        {
            Console.WriteLine(ToString());
        }

        // 객체의 요소를 출력
        public override string ToString()
        {
            return String.Format("DewPointCalculator [Temperature={0}, RelativeHumidity={1}, Value={2:0.#}]",
                temperature, relativeHumidity, value);
        }

        public override void GetUserInput()
        {
            string input = "";

            Init();

            Console.WriteLine("Calculate DewPoint");

            Console.Write("Please enter temperature (F) >>");
            input = Console.ReadLine();
            Double.TryParse(input, out temperature);

            Console.Write("Please enter relative humidity (%) >>");
            input = Console.ReadLine();
            Double.TryParse(input, out relativeHumidity);
        }
    }

    class WindChillTemperatureCalculator : Calculator
    {
        private double windVelocity; // mph

        public double WindVelocity
        {
            get { return windVelocity; }
            set { windVelocity = value; }
        }

        private string[,] _windChillTemperatureTable = new string[13, 19];

        protected override void Init()
        {
            base.Init();
            windVelocity = 0.0f;
        }

        // 체감 온도 테이블 생성
        public override void BuildTable()
        {
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
        
        public override void ShowCalcResult()
        {
            Console.WriteLine(ToString());
        }

        // 객체의 요소 출력
        public override string ToString()
        {
            return String.Format("WindChillTemperatureCalculator [Temperature={0}, WindVelocity={1}, Value={2:0.#}]",
                temperature, windVelocity, value);
        }

        // 체감 온도 계산
        public override void Calculate()
        {
            value = 35.74 + 0.6215 * temperature - 35.75 * Math.Pow(windVelocity, 0.16)
                    + 0.4275 * temperature * Math.Pow(windVelocity, 0.16);
        }

        public static double Calculate(double F, double W)
        {
            double WCT = 35.74 + 0.6215 * F - 35.75 * Math.Pow(W, 0.16) + 0.4275 * F * Math.Pow(W, 0.16);

            return Math.Round(WCT, 1);
        }

        // 체감 온도 테이블 출력하기
        public override void PrintTable()
        {
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

        // 사용자로부터 입력 받기
        public override void GetUserInput()
        {
            string input;

            Init();

            Console.WriteLine("Calculate WindChillTemperature");

            Console.Write("Please enter temperature (F) >>");
            input = Console.ReadLine();
            Double.TryParse(input, out temperature);

            Console.Write("Please enter wind velocity (mph) >>");
            input = Console.ReadLine();
            Double.TryParse(input, out windVelocity);
        }
    }

    class CalculatorApp
    {
        static void Main(string[] args)
        {
            do
            {
                string decision = "";
                Calculator calc = new Calculator();

                // 작업 선택
                do
                {
                    Console.Write("Please enter mode [1: DP, 2: WCT] >>");
                    decision = Console.ReadLine();
                } while (!decision.Equals("1") && !decision.Equals("2"));

                // 주요 함수 호출
                switch (decision)
                {
                    // 입력 값에 대응하는 자식 클래스 객체로 형 변환
                    case "1":
                        calc = new DewPointCalculator();
                        break;
                    case "2":
                        calc = new WindChillTemperatureCalculator();
                        break;
                }

                // 주요 함수 호출
                calc.BuildTable();
                calc.PrintTable(); // 테이블 출력
                calc.GetUserInput(); // 사용자로부터 값 입력
                calc.Calculate(); // 계산
                calc.ShowCalcResult(); // 계산 결과 출력

                // 종료 여부
                Console.Write("Press q-key to exit or enter-key to continue >>");
                decision = Console.ReadLine();

                // 입력 값이 q나 Q일 경우 프로그램 종료
                if (decision.Equals("q") || decision.Equals("Q"))
                {
                    Console.WriteLine("done..");
                    return;
                }
            } while (true); // q 또는 Q를 입력할 때까지 반복
        }
    }
}