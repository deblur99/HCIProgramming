using System;

namespace ConsoleApp1
{
    class Calculator
    {
        protected double temperature; // fahrenheit

        protected double relativeHumidity; // %

        protected double value; // DewPoint 값

        public virtual void BuildTable()
        {
            return;
        }

        public double Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }

        public double RelativeHumidity
        {
            get { return relativeHumidity; }
            set { relativeHumidity = value; }
        }

        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public virtual void Calculate()
        {
            return;
        }

        public static double CelsiusToFahrenheit(double c)
        {
            return 1.8f * c + 32;
        }

        public static double FahrenheitToCelsius(double f)
        {
            return (f - 32) * 5 / 9;
        }

        // 사용자로부터 화씨 온도와 상대습도 입력받기
        public void GetUserInput()
        {
            String input;

            Console.WriteLine("Please enter temperature (F) >>");
            input = Console.Read().ToString();
            Double.TryParse(input, out temperature);

            Console.WriteLine("Please enter relative humidity (%) >>");
            input = Console.Read().ToString();
            Double.TryParse(input, out relativeHumidity);

            Calculate();
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
        private string[,] _dewPointTable = new string[18,20];
        
        // row
        private double[] _FList =
        {
            110, 105, 100, 95, 90, 85, 80, 75, 70, 65, 60, 55, 50, 45, 40, 35, 32
        };
        
        // column
        private double[] _RHList =
        {
            100, 95, 90, 85, 80, 75, 70, 65, 60, 55, 50, 45, 40, 35, 30, 25, 20, 15, 10
        };

        public override void BuildTable()
        {
            // debug
            Console.WriteLine("row: {0}, col: {1}", _FList.Length, _RHList);
            _dewPointTable[0, 0] = "F/RH";

            for (int i = 0; i < _FList.Length; i++)
            {
                for (int j = 0; j < _RHList.Length; j++)
                {
                    // F/RH 문자열이 들어간 인덱스 생략
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    
                    // 0번째 row는 RH 기준값을 의미한다.
                    // 해당 row의 모든 column에 _RHList 배열의 요소를 저장한다.
                    if (i == 0)
                    {
                        _dewPointTable[i, j] = _RHList[j].ToString();
                        continue;
                    }
                    
                    // 0번째 col은 F 기준값을 의미한다.
                    // 해당 column의 모든 row에 _FList 배열의 요소를 저장한다.
                    if (j == 0)
                    {
                        _dewPointTable[i, j] = _FList[i].ToString();
                        continue;
                    }

                    // 그 외 인덱스에는 계산하는 함수의 반환값을 입력한다.
                    _dewPointTable[i + 1, j + 1] = CalculateDewPoint(_FList[i], _RHList[j]).ToString();
                }
            }
        }
        
        public override void PrintTable()
        {
            Console.Write(_dewPointTable[0,2]);
            // for (int i = 0; i < _dewPointTable.GetLength(0); i++)
            // {
            //     for (int j = 0; j < _dewPointTable.GetLength(1); j++)
            //     {
            //         Console.Write("{0}\t", _dewPointTable[i, j]);
            //     }
            //     Console.WriteLine("");
            // }
        }
        
        /*
        {
            {
                "F/RH", "100", "95", "90", "85", "80", "75", "70", "65", "60", "55", "50", "45", "40", "35", "30", "25",
                "20", "15", "10"
            },
            {
                "110", "110", "108", "106", "104", "102", "100", "98", "95", "93", "90", "87", "84", "80", "76", "72",
                "65", "60", "51", "41"
            },
            {
                "105", "105", "103", "101", "99", "97", "95", "93", "91", "88", "85", "83", "80", "76", "72", "67",
                "62", "55", "47", "37"
            },
            {
                "100", "100", "99", "97", "95", "93", "91", "89", "86", "84", "81", "78", "72", "71", "67", "63", "58",
                "52", "44", "32"
            },
            {
                "95", "95", "93", "92", "90", "88", "86", "84", "81", "79", "76", "73", "70", "67", "63", "59", "54",
                "48", "40", "32"
            },
            {
                "90", "90", "88", "87", "85", "83", "81", "79", "76", "74", "71", "68", "65", "62", "59", "54", "49",
                "43", "36", "32"
            },
            {
                "85", "85", "83", "81", "80", "78", "76", "74", "72", "69", "67", "64", "61", "58", "54", "50", "45",
                "38", "32", "-1"
            },
            {
                "80", "80", "78", "77", "75", "73", "71", "69", "67", "65", "62", "59", "56", "53", "50", "45", "40",
                "35", "32", "-1"
            },
            {
                "75", "75", "73", "72", "70", "68", "66", "64", "62", "60", "58", "55", "52", "49", "45", "41", "36",
                "32", "-1", "-1"
            },
            {
                "70", "70", "68", "67", "65", "63", "61", "59", "57", "55", "53", "50", "47", "44", "40", "37", "32",
                "-1", "-1", "-1"
            },
            {
                "65", "65", "63", "62", "60", "59", "57", "55", "53", "50", "48", "45", "42", "40", "36", "32", "-1",
                "-1", "-1", "-1"
            },
            {
                "60", "60", "58", "57", "55", "53", "52", "50", "48", "45", "43", "41", "38", "35", "32", "-1", "-1",
                "-1", "-1", "-1"
            },
            {
                "55", "55", "53", "52", "50", "49", "47", "45", "43", "40", "38", "36", "33", "32", "-1", "-1", "-1",
                "-1", "-1", "-1"
            },
            {
                "50", "50", "48", "46", "45", "44", "42", "40", "38", "36", "34", "32", "-1", "-1", "-1", "-1", "-1",
                "-1", "-1", "-1"
            },
            {
                "45", "45", "43", "42", "40", "39", "37", "35", "33", "32", "-1", "-1", "-1", "-1", "-1", "-1", "-1",
                "-1", "-1", "-1"
            },
            {
                "40", "40", "39", "37", "35", "34", "32", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1",
                "-1", "-1", "-1"
            },
            {
                "35", "35", "34", "32", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1",
                "-1", "-1", "-1"
            },
            {
                "32", "32", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1", "-1",
                "-1", "-1", "-1"
            }
        };*/

        public DewPointCalculator()
        {
            Console.WriteLine("Calculate DewPoint");
            //GetUserInput();
        }

        public override void Calculate()
        {
            value = 243.12 * (Math.Log(relativeHumidity / 100) + 17.62 * temperature / (243.12 + temperature)) /
                    (17.62 - (Math.Log(relativeHumidity / 100) + 17.62 * temperature / (243.12 + temperature)));
        }

        // double 매개변수 2개를 갖는 메서드 선언 및 정의
        public static double CalculateDewPoint(double F, double RH)
        {
            double C = FahrenheitToCelsius(F);
            double dewPoint;

            // C#에서의 로그 값을 구하는 함수는 Math 라이브러리에 내장되어 있다.
            dewPoint = 243.12 * (Math.Log(RH / 100) + 17.62 * C / (243.12 + C)) /
                       (17.62 - (Math.Log(RH / 100) + 17.62 * C / (243.12 + C)));

            // 소수 둘째 자리에서 반올림하여 실수 값을 소수점 하나만 남기는 Math 라이브러리의 Round 함수 호출
            return Math.Round(CelsiusToFahrenheit(dewPoint), 1);
        }

        // 객체의 요소를 출력
        public override String ToString()
        {
            return String.Format("DewPointCalculator [Temperature={0}, RelativeHumidity={1}, Value={2}]",
                temperature, relativeHumidity, value);
        }
    }

    class WindChillTemperatureCalculator : Calculator
    {
        private readonly string[,] _windChillTemperatureTable = new string[,]
        {
            {
                "W/F", "40", "35", "30", "25", "20", "15", "10", "5", "0", "-5", "-10", "-15", "-20", "-25", "-30",
                "-35", "-40", "-45"
            },
            {
                "5", "36", "31", "25", "19", "13", "7", "1", "-5", "-11", "-16", "-22", "-28", "-34", "-40", "-46",
                "-52",
                "-57", "-63"
            },
            {
                "10", "34", "27", "21", "15", "9", "3", "-4", "-10", "-16", "-22", "-28", "-35", "-41", "-47", "-53",
                "-59", "-66", "-72"
            },
            {
                "15", "32", "25", "19", "13", "6", "0", "-7", "-13", "19", "-26", "-32", "-39", "-45", "-51", "-58",
                "-64",
                "-71", "-77"
            },
            {
                "20", "30", "24", "17", "11", "4", "-2", "-9", "-15", "-22", "-29", "-35", "-42", "-48", "-55", "-61",
                "-68", "-74", "-81"
            },
            {
                "25", "29", "23", "16", "9", "3", "-4", "-11", "-17", "-24", "-31", "-37", "-44", "-51", "-58", "-64",
                "-71", "-78", "-84"
            },
            {
                "30", "28", "22", "15", "8", "1", "-5", "-12", "-19", "-26", "-33", "-39", "-46", "-53", "-60", "-67",
                "-73", "-80", "-87"
            },
            {
                "35", "28", "21", "14", "7", "0", "-7", "-14", "-21", "-27", "-34", "-41", "-48", "-55", "-62", "-69",
                "-76", "-82", "-89"
            },
            {
                "40", "27", "20", "13", "6", "-1", "-8", "-15", "-22", "-29", "-36", "-43", "-50", "-57", "-64", "-71",
                "-78", "-84", "-91"
            },
            {
                "45", "26", "19", "12", "5", "-2", "-9", "-16", "-23", "-30", "-37", "-44", "-51", "-58", "-65", "-72",
                "-79", "-86", "-93"
            },
            {
                "50", "26", "19", "12", "4", "-3", "-10", "-17", "-24", "-31", "-38", "-45", "-52", "-60", "-67", "-74",
                "-81", "-88", "-95"
            },
            {
                "55", "25", "18", "11", "4", "-3", "-11", "-18", "-25", "-32", "-39", "-46", "-54", "-61", "-68", "-75",
                "-82", "-89", "-97"
            },
            {
                "60", "25", "17", "10", "3", "-4", "-11", "-19", "-26", "-33", "-40", "-48", "-55", "-62", "-69", "-76",
                "-84", "-91", "-98"
            }
        };

        public WindChillTemperatureCalculator()
        {
            Console.WriteLine("Calculate WindChillTemperatureCalculator");
        }
        
        public override void PrintTable()
        {
            for (int i = 0; i < _windChillTemperatureTable.GetLength(0); i++)
            {
                for (int j = 0; j < _windChillTemperatureTable.GetLength(1); j++)
                {
                    Console.Write("{0}\t", _windChillTemperatureTable[i, j]);
                }
                Console.WriteLine("");
            }
        }
    }

    class CalculatorApp
    {
        static void Main(string[] args)
        {
            DewPointCalculator d = new DewPointCalculator();
            d.BuildTable();
            d.PrintTable();
            // WindChillTemperatureCalculator w = new WindChillTemperatureCalculator();
            // w.GetUserInput();
            // w.PrintTable();
        }
    }
}