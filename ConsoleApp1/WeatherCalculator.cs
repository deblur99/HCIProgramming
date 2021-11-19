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
}