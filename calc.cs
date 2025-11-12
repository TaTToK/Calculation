using System;

namespace ConsoleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Простой консольный калькулятор на C#");
            Console.WriteLine("Поддерживаемые операции: +  -  *  /  ^ (степень)");
            Console.WriteLine("Введите 'exit' для выхода.");

            while (true)
            {
                Console.Write("\nВведите выражение (например: 2 + 2 или 3.5 ^ 2): ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;
                input = input.Trim();
                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

                try
                {
                    double result = EvaluateSimpleExpression(input);
                    Console.WriteLine($"= {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }

            Console.WriteLine("Выход. До свидания!");
        }

        // Очень простой парсер: ожидает формат: <число> <оператор> <число>
        // Оператор может быть +, -, *, /, ^
        static double EvaluateSimpleExpression(string expr)
        {
            // Разделим по пробелам: "2 + 2" -> ["2","+","2"]
            var parts = expr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
            {
                throw new ArgumentException("Ожидается формат: число пробел оператор пробел число. Например: 12.5 * 3");
            }

            if (!double.TryParse(parts[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double a))
                throw new ArgumentException("Невозможно распознать первое число.");
            string op = parts[1];
            if (!double.TryParse(parts[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double b))
                throw new ArgumentException("Невозможно распознать второе число.");

            return op switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => b == 0 ? throw new DivideByZeroException("Деление на ноль") : a / b,
                "^" => Math.Pow(a, b),
                _ => throw new ArgumentException("Неизвестный оператор. Допустимые: + - * / ^"),
            };
        }
    }
}
