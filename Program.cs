using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plants;

namespace lab11
{
    public class TestCollections<TBaseKey, TBaseValue, TKey, TValue>
    where TBaseKey : Plant
    where TBaseValue : Plant
    {
        // Поле для коллекции1<TValue>
        public HashSet<TBaseValue> Collection1 { get; set; }

        // Поле для коллекции1<string>
        public HashSet<string> Collection1String { get; set; }

        // Поле для коллекции2<TKey, TValue>
        public Dictionary<TKey, TBaseValue> Collection2 { get; set; }

        // Поле для коллекции2<string, TValue>
        public Dictionary<string, TBaseValue> Collection2String { get; set; }

        // Конструктор класса TestCollections
        public TestCollections()
        {
            // Инициализация коллекций
            Collection1 = new HashSet<TBaseValue>();
            Collection1String = new HashSet<string>();
            Collection2 = new Dictionary<TKey, TBaseValue>();
            Collection2String = new Dictionary<string, TBaseValue>();

            // Генерация элементов коллекций
            GenerateElements(1000);
        }

        // Метод для генерации элементов коллекций
        private void GenerateElements(int count)
        {
            for (int i = 0; i < count; i++)
            {
                // Создание нового элемента производного класса
                TBaseValue value = (TBaseValue)Activator.CreateInstance(typeof(TBaseValue));

                // Добавление элемента в коллекцию1<TValue>
                Collection1.Add(value);

                // Добавление элемента в коллекцию1<string>
                Collection1String.Add(value.ToString());

                // Создание нового элемента производного класса для ключа и значения коллекции2<TKey, TValue>
                TKey key = (TKey)Activator.CreateInstance(typeof(TKey));
                Collection2.Add(key, value);

                // Добавление элемента в коллекцию2<string, TValue>
                Collection2String.Add(key.ToString(), value);
            }
        }

        // Метод для измерения времени поиска элемента в коллекциях
        public void MeasureSearchTime()
        {
            // Выбираем четыре разных элемента для поиска
            TBaseValue firstElement = Collection1.FirstOrDefault();
            TBaseValue centralElement = Collection1.ElementAtOrDefault(Collection1.Count / 2);
            TBaseValue lastElement = Collection1.LastOrDefault();
            TBaseValue notInCollectionElement = (TBaseValue)Activator.CreateInstance(typeof(TBaseValue));

            // Измеряем время поиска в коллекциях
            MeasureTime(() => Collection1.Contains(firstElement), "Collection1 (TValue) - First Element");
            MeasureTime(() => Collection1.Contains(centralElement), "Collection1 (TValue) - Central Element");
            MeasureTime(() => Collection1.Contains(lastElement), "Collection1 (TValue) - Last Element");
            MeasureTime(() => Collection1.Contains(notInCollectionElement), "Collection1 (TValue) - Not in Collection");

            MeasureTime(() => Collection1String.Contains(firstElement.ToString()), "Collection1 (string) - First Element");
            MeasureTime(() => Collection1String.Contains(centralElement.ToString()), "Collection1 (string) - Central Element");
            MeasureTime(() => Collection1String.Contains(lastElement.ToString()), "Collection1 (string) - Last Element");
            MeasureTime(() => Collection1String.Contains(notInCollectionElement.ToString()), "Collection1 (string) - Not in Collection");

            MeasureTime(() => Collection2.ContainsKey((TKey)Activator.CreateInstance(typeof(TKey))), "Collection2 (TKey, TValue) - First Element");
            MeasureTime(() => Collection2.ContainsKey((TKey)Activator.CreateInstance(typeof(TKey))), "Collection2 (TKey, TValue) - Central Element");
            MeasureTime(() => Collection2.ContainsKey((TKey)Activator.CreateInstance(typeof(TKey))), "Collection2 (TKey, TValue) - Last Element");
            MeasureTime(() => Collection2.ContainsKey((TKey)Activator.CreateInstance(typeof(TKey))), "Collection2 (TKey, TValue) - Not in Collection");

            MeasureTime(() => Collection2String.ContainsKey(firstElement.ToString()), "Collection2 (string, TValue) - First Element");
            MeasureTime(() => Collection2String.ContainsKey(centralElement.ToString()), "Collection2 (string, TValue) - Central Element");
            MeasureTime(() => Collection2String.ContainsKey(lastElement.ToString()), "Collection2 (string, TValue) - Last Element");
            MeasureTime(() => Collection2String.ContainsKey(notInCollectionElement.ToString()), "Collection2 (string, TValue) - Not in Collection");
        }

        // Метод для измерения времени выполнения действия
        private void MeasureTime(Action action, string description)
        {
            const int iterations = 10;
            long totalTicks = 0;

            for (int i = 0; i < iterations; i++)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                action();
                stopwatch.Stop();
                totalTicks += stopwatch.ElapsedTicks;
            }

            Console.WriteLine($"{description}: Average time - {(double)totalTicks / iterations / Stopwatch.Frequency * 1000:F2} ms");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //1-------------------------------------------------------
            Stack<Plant> plantStack = new Stack<Plant>();

            // Добавление объектов в коллекцию
            plantStack.Push(new Flower("Rose", "Red", "Fragrant", 1));
            plantStack.Push(new Flower("Daisy", "White", "Fresh", 2));
            plantStack.Push(new Tree("Oak", "Green", 10.5, 3));
            plantStack.Push(new Tree("Pine", "Brown", 8.2, 4));

            // Вывод содержимого коллекции
            Console.WriteLine("Collection contents:");
            foreach (var plant in plantStack)
            {
                Console.WriteLine(plant);
            }
            Console.WriteLine();

            // Создаем временный массив из элементов стека
            Plant[] tempArray = new Plant[plantStack.Count];
            plantStack.CopyTo(tempArray, 0);

            // Клонирование коллекции
            Stack<Plant> clonedStack = new Stack<Plant>(plantStack);

            // Удаление объектов из коллекции
            Console.WriteLine("Removing objects from the collection:");
            while (plantStack.Count > 0)
            {
                Plant removedPlant = plantStack.Pop();
                Console.WriteLine($"Removed: {removedPlant}");
            }
            Console.WriteLine();

           
            //запросы
            Query1(tempArray);
            Query2(tempArray);
            Query3(tempArray, "Fragrant");

            // Перебор элементов коллекции с помощью foreach
            Console.WriteLine("Iterating over collection with foreach:");
            foreach (var plant in tempArray)
            {
                Console.WriteLine(plant);
            }
            Console.WriteLine();

            // сортировка временного массива
            Array.Sort(tempArray);
            Console.WriteLine("Collection sorted.");
            foreach (var plant in tempArray)
            {
                Console.WriteLine(plant);
            }
            Console.WriteLine();

            // Поиск элемента в массиве
            Flower flowerToFind = new Flower("Daisy", "White", "Fresh", 2);
            int foundIndex = Array.IndexOf(tempArray, flowerToFind);
            if (foundIndex != -1)
            {
                Console.WriteLine($"Found flower at index {foundIndex}: {tempArray[foundIndex]}");
            }
            else
            {
                Console.WriteLine("Flower not found in the collection.");
            }

            // Возвращаем элементы обратно в стек в обратном порядке
            plantStack.Clear();
            foreach (var plant in tempArray)
            {
                plantStack.Push(plant);
            }
            //1-------------------------------------------------------

            //2-------------------------------------------------------
            Console.WriteLine("\n");
            // Создаем и заполняем коллекцию SortedDictionary
            SortedDictionary<int, Plant> plantDictionary = new SortedDictionary<int, Plant>();
            plantDictionary.Add(1, new Flower("Rose", "Red", "Fragrant", 1));
            plantDictionary.Add(2, new Flower("Daisy", "White", "Fresh", 2));
            plantDictionary.Add(3, new Tree("Oak", "Green", 10.5, 3));
            plantDictionary.Add(4, new Tree("Pine", "Brown", 8.2, 4));

            // Вывод содержимого коллекции
            Console.WriteLine("Collection contents:");
            foreach (var kvp in plantDictionary)
            {
                Console.WriteLine(kvp.Value);
            }
            Console.WriteLine();

            //временный массив для хранения занчений
            Plant[] tempArray1 = new Plant[plantDictionary.Count];
            int index = 0;
            foreach (var kvp in plantDictionary)
            {
                tempArray1[index++] = kvp.Value;
            }

            // Перебор элементов коллекции с помощью foreach
            Console.WriteLine("Iterating over collection with foreach:");
            foreach (var kvp in plantDictionary)
            {
                Console.WriteLine(kvp.Value);
            }
            Console.WriteLine();

            // Клонирование коллекции
            SortedDictionary<int, Plant> clonedDictionary = new SortedDictionary<int, Plant>(plantDictionary);

            // Удаление объектов из коллекции
            Console.WriteLine("Removing objects from the collection:");
            plantDictionary.Clear();
            Console.WriteLine("All objects removed from the collection.");
            Console.WriteLine();

            //запросы
            Query1(tempArray);
            Query2(tempArray);
            Query3(tempArray, "Fragrant");

            // Заданный элемент для поиска
            Plant plantToFind = new Flower("Daisy", "White", "Fresh", 2);

            // Проверка наличия элемента в коллекции
            if (clonedDictionary.ContainsValue(plantToFind))
            {
                Console.WriteLine($"The collection contains the plant: {plantToFind}");
            }
            else
            {
                Console.WriteLine("The collection does not contain the specified plant.");
            }

            //2-------------------------------------------------------

        }

        // Запросы
        public static void Query1(Plant[] plants)
        {
            Console.WriteLine("1. Roses without thorns:");
            foreach (var plant in plants)
            {
                if (plant is Flower flower && flower.Smell == "Fragrant")
                {
                    Console.WriteLine(flower);
                }
            }
            Console.WriteLine();
        }

        public static void Query2(Plant[] plants)
        {
            Console.WriteLine("2. The smallest tree:");
            Tree smallestTree = null;
            double minHeight = double.MaxValue;

            foreach (var plant in plants)
            {
                if (plant is Tree tree && tree.Height < minHeight)
                {
                    smallestTree = tree;
                    minHeight = tree.Height;
                }
            }

            if (smallestTree != null)
            {
                Console.WriteLine(smallestTree);
            }
            Console.WriteLine();
        }

        public static void Query3(Plant[] plants, string targetSmell)
        {
            Console.WriteLine($"3. Flowers with smell '{targetSmell}':");
            foreach (var plant in plants)
            {
                if (plant is Flower flower && flower.Smell == targetSmell)
                {
                    Console.WriteLine(flower);
                }
            }
            Console.WriteLine();
        }
    }
}
