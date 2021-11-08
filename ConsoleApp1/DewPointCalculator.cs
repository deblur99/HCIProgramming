﻿using System;

namespace ConsoleApp1
{
    class DewPointCalculator
    {
        private double temperature; // fahrenheit

        private double relativeHumidity; // %

        private double value; // DewPoint 값

        public double Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                temperature = value;
            }
        }
        
        public double RelativeHumidity
        {
            get
            {
                return relativeHumidity;
            }
            set
            {
                relativeHumidity = value;
            }
        }
        
        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        DewPointCalculator()
        {
            Console.WriteLine("Calculate DewPoint");    
        }
        
        public void Calculate()
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
        
        // 단위 환산을 위한 메서드
        public static double CelsiusToFahrenheit(double c)
        {
            return 1.8f * c + 32;
        }
        
        public static double FahrenheitToCelsius(double f)
        {
            return (f - 32) * 5 / 9;
        }
        
        // 사용자가 입력한 
        public override String ToString()
        {
            return String.Format("DewPointCalculator [Temperature={0}, RelativeHumidity={1}, Value={2}]",
                temperature, relativeHumidity, value);
        }

        public void GetUserInput()
        {
            double temperature, relativeHumidity;
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
        public static void PrintTable()
        {
            
        }
        
        static void Main(string[] args)
        {
            // 변수 선언
            string[] input;
            double RH, T;

            for (;;)
            {
                // 한 줄 단위로 읽은 후 공백 단위로 각 문자열을 나누어 배열로 만듦
                Console.Write("Enter two values: RH, T >> ");
                input = Console.ReadLine().Split();

                // 문자열 배열의 요소 개수가 2가 아니면 입력을 잘못 받은 것으로 간주하여 다시 입력받음
                if (input.Length != 2)
                {
                    if (input.Length == 1)
                    {
                        if (input[0].Equals("q") || input[0].Equals("Q"))
                        {
                            return; // q 또는 Q를 입력하면 프로그램 종료
                        }
                    }

                    Console.WriteLine("Invalid Input");
                    continue;
                }
                
                else
                {
                    // 문자열의 0번째 요소는 RH 값으로, 1번째 요소는 T 값으로 변환하기 위해 double로 형 변환
                    // 입력 문자열이 double 형으로 변환할 수 있는 문자열인지 검증하는 함수 TryParse 사용
                    // 변환 성공 시 true를 반환하며 각 변수에 변환된 값이 저장된다.
                    // 변환 실패 시 오류 출력
                    if (!double.TryParse(input[0], out RH)) {
                        Console.WriteLine("Invalid Input");
                        continue;
                    }

                    if (!double.TryParse(input[1], out T)) {
                        Console.WriteLine("Invalid Input");
                        continue;
                    }
                    
                    Console.WriteLine("Dew Point : {0}", CalculateDewPoint(RH, T));
                }
            }
        }
    }
}