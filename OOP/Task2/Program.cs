
    Console.WriteLine("Введите данные о товаре.");

    Console.WriteLine("Наименование:");
    string name = Console.ReadLine() ?? "Не указано.";

    Console.WriteLine("Производитель:");
    string manufacture = Console.ReadLine() ?? "Не указано.";

    Console.WriteLine("Цена:");

    if (!decimal.TryParse(Console.ReadLine(),
        out decimal price))
    {
        price = 0;
    }

    Console.WriteLine("Срок годности (в днях):");

    if (!int.TryParse(Console.ReadLine(), out int expiry))
    {
        expiry = 0;
    }

    Console.WriteLine("Дата производства (ДД-ММ-ГГГГ):");

    if (!DateTime.TryParse(Console.ReadLine(),
        out DateTime production))
    {
        production = DateTime.Now;
    }

    //Создание объекта класса
    Product product = new Product(name, manufacture, price,
        expiry, production);

    Console.WriteLine("\nИнформация о продукте:");
    Console.WriteLine(product.ToString());
