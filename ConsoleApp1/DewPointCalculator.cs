using System;

namespace ConsoleApp1
{
    class DewPointCalculator
    {
        // double 매개변수 2개를 갖는 메서드 선언 및 정의
        public static double CalculateDewPoint(double RH, double T)
        {
            double dewPoint;

            // C#에서의 로그 값을 구하는 함수는 Math 라이브러리에 내장되어 있다.
            dewPoint = 243.12 * (Math.Log(RH / 100) + 17.62 * T / (243.12 + T)) /
                       (17.62 - (Math.Log(RH / 100) + 17.62 * T / (243.12 + T)));

            // 소수 둘째 자리에서 반올림하여 실수 값을 소수점 하나만 남기는 Math 라이브러리의 Round 함수 호출
            return Math.Round(dewPoint, 1);
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
                // 문자열의 0번째 요소는 RH 값으로, 1번째 요소는 T 값으로 변환하기 위해 double로 형 변환
                else
                {
                    RH = double.Parse(input[0]);
                    T = double.Parse(input[1]);
                }

                // 이슬점 계산하는 메서드 출력하여 계산 후 계산 결과를 출력
                Console.WriteLine("Dew Point : {0}\n", CalculateDewPoint(RH, T));
            }
        }
    }
}