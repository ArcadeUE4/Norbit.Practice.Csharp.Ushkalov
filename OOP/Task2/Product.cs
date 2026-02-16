
    public class Product
    {
        //Поля данных.
        private string _name;
        private string _manufacturer;
        private decimal _price;
        private int _expiry;
        private DateTime _production;

        //Свойства для доступа к данным.
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Manufacturer
        {
            get => _manufacturer;
            set => _manufacturer = value;
        }

        public decimal Price
        {
            get => _price;
            set => _price = value;
        }

        public int Expiry
        {
            get => _expiry;
            set => _expiry = value;
        }

        public DateTime Production
        {
            get => _production;
            set => _production = value;
        }

        //Конструктор класса.
        public Product(string name, string manufacture, decimal price,
            int expiry, DateTime production)
        {
            _name = name;
            _manufacturer = manufacture;
            _price = price;
            _expiry = expiry;
            _production = production;
        }

        //Переопределение ToStirng().
        public override string ToString()
        {
            return $"Товар: {Name} \n" +
                $"Производитель: {Manufacturer} \n" +
                $"Цена: {Price:N2} руб. \n" +
                $"Срок годности: {Expiry} \n" +
                $"Дата производства: {Production.ToShortDateString()}";
        }
}
