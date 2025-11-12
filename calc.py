import operator
import re

def calculate(expression):
    # Удаляем пробелы и проверяем корректность символов
    expression = expression.replace(" ", "")
    if not re.match(r'^[\d+\-*/().]+$', expression):
        raise ValueError("Недопустимые символы в выражении")
    
    # Словарь операторов
    ops = {
        '+': operator.add,
        '-': operator.sub,
        '*': operator.mul,
        '/': operator.truediv,
    }
    
    def apply_operator(values, operators):
        right = values.pop()
        left = values.pop()
        op = operators.pop()
        values.append(ops[op](left, right))
    
    def precedence(op):
        return 1 if op in '+-' else 2 if op in '*/' else 0
    
    values = []
    operators = []
    i = 0
    negative = False  # Флаг для обработки отрицательных чисел
    
    while i < len(expression):
        if expression[i].isdigit() or (negative and expression[i] == '-'):
            # Обрабатываем отрицательные числа и числа с плавающей точкой
            if negative and expression[i] == '-':
                i += 1
                num = '-'
            else:
                num = ''
            while i < len(expression) and (expression[i].isdigit() or expression[i] == '.'):
                num += expression[i]
                i += 1
            values.append(float(num))
            negative = False
            continue
        
        elif expression[i] in '+-*/':
            # Обработка унарного минуса
            if expression[i] == '-' and (i == 0 or expression[i-1] in '+-*/(/'):
                negative = True
                i += 1
                continue
            
            while (operators and precedence(operators[-1]) >= precedence(expression[i])):
                apply_operator(values, operators)
            operators.append(expression[i])
        
        elif expression[i] == '(':
            operators.append(expression[i])
            # Проверяем унарный минус после открывающей скобки
            if i+1 < len(expression) and expression[i+1] == '-':
                negative = True
                i += 1
        elif expression[i] == ')':
            while operators[-1] != '(':
                apply_operator(values, operators)
            operators.pop()
        
        i += 1
    
    while operators:
        apply_operator(values, operators)
    
    return values[0]

if __name__ == "__main__":
    print("Калькулятор. Введите 'exit' для выхода.")
    while True:
        try:
            expr = input("Введите выражение: ").strip()
            if expr.lower() == 'exit':
                break
            result = calculate(expr)
            print(f"Результат: {result}")
        except Exception as e:
            print(f"Ошибка: {e}")