    
    Console.WriteLine("Базовые операции");
    var stack = new SmartStack<string>();

    stack.Push("Первый (Дно)");
    stack.Push("Второй");
    stack.Push("Третий (Вершина)");

    Console.WriteLine($"Текущая вершина (Peek): " +
        $"{stack.Peek()}");
    Console.WriteLine($"Извлекаем элемент (Pop): " +
        $"{stack.Pop()}");
    Console.WriteLine($"Новая вершина (Peek): " +
        $"{stack.Peek()}");
    Console.WriteLine($"Элементов в стеке: " +
        $"{stack.Count}\n");

    
    Console.WriteLine("Динамическое расширение");
    Console.WriteLine($"Вместимость до добавления: " +
        $"{stack.Capacity}");

    stack.Push("Четвертый");
    stack.Push("Пятый");
    stack.Push("Шестой"); 

    Console.WriteLine($"Вместимость после добавления:" +
        $" {stack.Capacity}");
    Console.WriteLine($"Элементов в стеке: " +
        $"{stack.Count}\n");

    
    Console.WriteLine("Доступ по индексу");
    Console.WriteLine($"Элемент [0] (Вершина): {stack[0]}");
    Console.WriteLine($"Элемент [2]: {stack[2]}");
    Console.WriteLine($"Элемент [4] (Дно): {stack[4]}\n");

    
    Console.WriteLine("Перебор через foreach");
    Console.WriteLine("Стек перебирается от вершины " +
        "к основанию:");
    
    foreach (var item in stack)
    {
        Console.WriteLine($"- {item}");
    }

    Console.WriteLine();

    
    Console.WriteLine("Работа с коллекциями и Contains");

    var numbers = new List<int> { 10, 20, 30 };
    var numberStack = new SmartStack<int>(numbers);

    Console.WriteLine($"Создан стек из коллекции. " +
        $"Элементов: {numberStack.Count}, " +
        $"Вершина: {numberStack.Peek()}");

    Console.WriteLine("Добавляем коллекцию " +
        "через PushRange (40, 50)...");
    numberStack.PushRange(new[] { 40, 50 });

    Console.WriteLine($"Новая вершина: {numberStack.Peek()}");

    Console.WriteLine($"Содержит ли стек число 30? " +
        $"{numberStack.Contains(30)}");

    Console.WriteLine($"Содержит ли стек число 99? " +
        $"{numberStack.Contains(99)}\n");

    Console.WriteLine("Проверка исключений");
    var emptyStack = new SmartStack<double>();
       
    try
    {
        emptyStack.Pop();
    }

    catch (InvalidOperationException ex)
    {   
       Console.WriteLine($"Ожидаемая ошибка при Pop из пустого " +
           $"стека: \"{ex.Message}\"");
    }

    try
    {
        var item = stack[100];
    }

    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Ожидаемая ошибка при неверном " +
            $"индексе поймана успешно.");
    }