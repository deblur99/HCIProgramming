using System;

namespace ConsoleApp1
{
    public class DewPointCalculator: IWeatherCalculator
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
                    _RHList.Length - 7, _RHList.Length - 8, _RHList.Length - 11, _RHList.Length - 13,
                    _RHList.Length - 16,
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
}