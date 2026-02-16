using System.Collections;

/// <summary>
/// Предсатвляет собой стек с динамическим изменением размера
/// и доступом к индексу.
/// </summary>
/// <typeparam name="T">Тип элементов в стеке.</typeparam>
public class SmartStack<T> : IEnumerable<T>
{
        //Поля.
        private T[] _items;
        private int _count;

        /// <summary>
        /// Инциализирует новый экзмеляр 
        /// <see cref="SmartStack{T}"/>. с начальной емкостью.
        /// </summary>
        public SmartStack()
        { 
            _items = new T[4];
            _count = 0;
        }

        /// <summary>
        /// Инциализирует новый экзмеляр 
        /// <see cref="SmartStack{T}"/> с заданной емкостью.
        /// </summary>
        /// <param name="capacity">
        /// Начальное количество элементов стека.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Отображается, если <paramref name="capacity"/> 
        /// меньше 0. </exception>
        public SmartStack(int capacity)
        { 
            if (capacity < 0) throw new 
                    ArgumentOutOfRangeException(nameof(capacity));
            _items = new T[capacity];
            _count = 0;
        }

        /// <summary>
        /// Инициализирует новый экземпляр 
        /// <see cref="SmartStack{T}"/>, 
        /// содержащий элементы из указанной коллекции.
        /// </summary>
        /// <param name="collection">
        /// Коллекция, элементы которой будут скопированы в стек.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="collection"/> 
        /// имеет значение null.</exception>
        public SmartStack(IEnumerable<T> collection)
        { 
            if (collection == null) throw new
                ArgumentNullException(nameof(collection));

            T[] tempArray = collection.ToArray();
            _items = new T[tempArray.Length];

            for (int i = 0; i < tempArray.Length; i++)
            { 
                _items[i] = tempArray[i];
            }
            _count = tempArray.Length;
        }

        //Свойства.
        public int Count => _count;
        public int Capacity => _items.Length;

        /// <summary>
        /// Вставляет объект на вершину стека.
        /// </summary>
        /// <param name="item"> 
        /// Объект, который необходимо вставить в стек.</param>
        public void Push(T item)
        {
            if (_count == _items.Length)
            {
                EnsureCapacity(_items.Length == 0 ? 4 : _items.Length * 2);
            }
            _items[_count++] = item;
        }

        /// <summary>
        /// Добавляет последовательность элементов 
        /// на вершину стека.
        /// </summary>
        /// <param name="collection">Коллекция элементов для добавления.</param>
        /// <exception cref="ArgumentNullException">
        /// Отображается, если <paramref name="collection"/> 
        /// имеет значение null.</exception>
        public void PushRange(IEnumerable<T> collection)
        { 
            if (collection == null) throw new
                    ArgumentNullException(nameof(collection));

            foreach (var item in collection)
            { 
                Push(item);
            }
        }

        /// <summary>
        /// Удаляет и возвращает объект на вершину стека.
        /// </summary>
        /// <returns>Объект удаленный из начала стека.</returns>
        /// <exception cref="InvalidOperationException">
        /// Отображается, если стек пуст.</exception>
        public T Pop()
        {
            if (_count == 0) throw new 
                    InvalidOperationException("Стек пуст.");

            T item = _items[--_count];
            _items[_count] = default!;
            return item;
        }
        
        /// <summary>
        /// Возращает объект на вершину стека без удаления.
        /// </summary>
        /// <returns>Объект на вершине стека.</returns>
        /// <exception cref="InvalidOperationException">
        /// Отображается, если стек пуст</exception>
        public T Peek()
        {
            if (_count == 0) throw new 
                    InvalidOperationException("Стек пуст.");

            return _items[_count - 1];
        }

        /// <summary>
        /// Определяет, входят ли элемент в стек.
        /// </summary>
        /// <param name="item">Объект для поиска стека.</param>
        /// <returns>true, если объект найден.
        /// false, если объект не найден.</returns>
        public bool Contains(T item)
        {
            var comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < _count; i++)
            {
                if (comparer.Equals(_items[i], item))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _count - 1; i >= 0; i--)
            { 
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() 
        => GetEnumerator();

        /// <summary>
        /// Получает элемент по указанному индексу. 
        /// Индекс 0 соответствует вершине стека.
        /// </summary>
        /// <param name="index">
        /// Отсчитываемый от нуля индекс элемента (0 — верхний элемент).
        /// </param>
        /// <returns>Элемент на указанной позиции.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Отображается, если индекс вне диапазона.</exception>
        public T this[int index]
        {
            get
            { 
                if (index < 0 || index >= _count) throw new
                    ArgumentOutOfRangeException(nameof(index));

                return _items[_count - 1 - index];
            }
        }

        //Вспомогательный метод для рассширения массива.
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                T[] newArray = new T[min];
                Array.Copy(_items, 0, newArray, 0, _count);
                _items = newArray;
            }
        }


}
