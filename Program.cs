using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Console;
using System.Linq.Expressions;

namespace Market
{
    public delegate void MenuDelegate(List<Product> p, List<Manufactory> m, List<Category> c);
    public class Product
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public int ManufactoryId { get; set; }
        public DateTime Data { get; set; }

        public Product(string name, int price, int cat, int manuf, int year, int month, int day)
        {
            Name = name;
            Price = price;
            CategoryId = cat;
            ManufactoryId = manuf;
            Data = new DateTime(year, month, day); 
        }
        public override string ToString()
        {
            return ($"Название= {Name},\tЦена= {Price},\tСрок хранения = {Data}\n");
        }
    }
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public override string ToString()
        {
            return ($"Id= {Id.ToString()}, Name= {Name}\n");
        }
    }

    public class Manufactory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Manufactory(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public override string ToString()
        {
            return ($"Id= {Id.ToString()}, Name= {Name}\n");
        }

    }

    class User
    {
        public string Login { get; set;}
        public string Password { get; set;}
        public event MenuDelegate menuEvent;
        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }
        public void Action(List<Product> prodList, List<Manufactory> mList, List<Category> catList)
        {
            if (menuEvent != null)
            {
                menuEvent(prodList, mList, catList);
            }            
        }
    }
    class Market
    {
        public static List<string> ReadDate(string args)
        {
            string filePath = args;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs, Encoding.Unicode);
                List<string> _list = new List<string>();
                while (!sr.EndOfStream)
                {
                    _list.Add(sr.ReadLine());
                }
                fs.Close();
                return _list;
            }
        }

        public static void WriteDate(string args, List<Product> prodList)
        {
            string filePath = args;
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                {
                    foreach(Product pr in prodList)
                    {
                        sw.WriteLine($"{pr.Name} {pr.Price} {pr.CategoryId} {pr.ManufactoryId} {pr.Data.Year} {pr.Data.Month} {pr.Data.Day}");
                    }   
                }
            }
        }

        static void ShowAllMenu()
        {
            WriteLine("\n1. Получить список всех товаров;\n" +
                "2. Получить список всех товаров заданной категории;\n" +
                "3. Получить список всех товаров, срок реализации которых истекает сегодня;\n" +
                "4. Получить список всех товаров заданного производителя и заданной категории;\n" +
                "5. Получить список всех производителей, товары которых в заданной категории имеют цену, лежащую в заданном диапазоне;\n" +
                "6. Получить количество товаров по категориям.\n");
        }
        public static void Menu(List<Product> prodList, List<Manufactory> mList, List<Category> catList, User _user)
        {
            if (_user.Login == "admin")
            {
                WriteLine("\nМЕНЮ пользователя Админ!");
                WriteLine("\n\tПРОСМОТР:");
                ShowAllMenu();
                WriteLine("7. РЕДАКТИРОВАТЬ");
            }
            if (_user.Login == "kassir")
            {
                WriteLine("\nМЕНЮ пользователя Кассир!");
                WriteLine("\n\tПРОСМОТР:");
                ShowAllMenu();
                WriteLine("7. ПОКУПКА");
            }           
        }
        public static void ShowAllProduct(List<Product> prodList, List<Manufactory> mList, List<Category> cList)
        {
            Write("\n\tПолучить список всех товаров\n");
            IEnumerable<Product> query = from p in prodList
                                         select p;
            foreach (Product item in query)
            {
                Write($"Название - {item.Name}\t\t Стоимость - {item.Price}\n");
            }      
        }

        public static void ShowAllProductCat(List<Product> prodList, List<Manufactory> mList, List<Category> cList)
        {
            Write("\n\tПолучить список всех товаров заданной категории\n");
            WriteLine("\nУкажите категорию, товар которой нужно вывести:\n1-Выпечка, 2-Фрукты, 3-Конфеты, 4-Напитки:");
            int chose = Convert.ToInt32(ReadLine());

            IEnumerable<Product> query1 = from p in prodList
                                          where p.CategoryId == chose
                                          select p;
            foreach (Product item in query1)
            {
                Write($"Название - {item.Name}\t\t Стоимость - {item.Price}  Срок годности {item.Data.ToShortDateString()}\n");
            }
        }
        public static void ShowDataTodayOff(List<Product> prodList, List<Manufactory> mList, List<Category> cList)
        {
            Write("\n\tПолучить список всех товаров, срок реализации которых истекает сегодня\n");

            IEnumerable<Product> query2 = from p in prodList
                                          where p.Data.ToShortDateString() == DateTime.Now.ToShortDateString()
                                          select p;
            foreach (Product item in query2)
            {
                Write($"Название - {item.Name}\t\t Стоимость - {item.Price} Срок годности {item.Data.ToShortDateString()} \n");
            }
        }
        public static void ShowProductCategoryManufactoru(List<Product> prodList, List<Manufactory> mList, List<Category> cList)
        {
            Write("\n\tПолучить список всех товаров заданного производителя и заданной категории\n");

            WriteLine("\nУкажите категорию, товар которой нужно вывести:\n1-Выпечка, 2-Фрукты, 3-Конфеты, 4-Напитки:");
            int chosecat = Convert.ToInt32(ReadLine());

            WriteLine("\nУкажите производителя:\n 1-Tripoly, 2-Lukas, 3-Roshen, 4-Ilona, 5-Ahmad, 6-Becida: ");
            int chosemanuf = Convert.ToInt32(ReadLine());

            IEnumerable<Product> query3 = from p in prodList
                                          where p.CategoryId == chosecat && p.ManufactoryId == chosemanuf
                                          select p;

            foreach (Product item in query3)
            {
                Write($"Название - {item.Name}\t\t Стоимость - {item.Price} Срок годности {item.Data.ToShortDateString()} \n");
            }
        }
        public static void ShowProductPriceRange(List<Product> prodList, List<Manufactory> mList, List<Category> cList)
        {
            Write("\n\tПолучить список всех производителей, товары которых в заданной категории имеют цену, лежащую в заданном диапазоне\n");

            WriteLine("\nУкажите категорию, товар которой нужно вывести:\n1-Выпечка, 2-Фрукты, 3-Конфеты, 4-Напитки:");
            int chosecat1 = Convert.ToInt32(ReadLine());
            WriteLine("\nУкажите минимальную цену:");
            int minprice = Convert.ToInt32(ReadLine());
            WriteLine("\nУкажите максимальную цену:");
            int maxprice = Convert.ToInt32(ReadLine());

            IEnumerable<Manufactory> query4 = from p in prodList
                                              join man in mList on p.ManufactoryId equals man.Id
                                              where p.CategoryId == chosecat1 && p.Price > minprice && p.Price < maxprice
                                              select man;

            foreach (Manufactory item in query4)
            {
                Write($"Производитель: {item.Name}\n");
            }
        }
        public static void ShowProductCountCategory(List<Product> prodList, List<Manufactory> mList, List<Category> cList)
        {
            Write("\n\tПолучить количество товаров по категориям.\n");

            IEnumerable<IGrouping<int, Product>> query5 = from p in prodList
                                                          group p by p.CategoryId;

            foreach (IGrouping<int, Product> key in query5)
            {
                Write($"Категория: {cList.First(p => p.Id == key.Key).Name}\nКоличество:");
                int a = 0;
                foreach (Product item in key)
                {
                    a++;
                }
                WriteLine(a);
            }
        }
        public static void Edit(List<Product> prodList, List<Manufactory> mList, List<Category> cList)
        {
            ShowAllProduct(prodList, mList, cList);

            Write($"\n\tРедактирование: укажите название товара, который нужно отредактировать - ");
            string _nameProduct = ReadLine();
            Product pr= prodList.Find(x => x.Name == _nameProduct);
            Write($"\n\tУкажите новые данные:\n");
            Write($"\n\tНазвание - ");
            pr.Name = ReadLine();
            Write($"\n\tЦена - ");
            pr.Price = Convert.ToInt32(ReadLine());
            WriteDate("product.txt", prodList);
            WriteLine("\t\tТовар отредактирован!!!\n");
        }
        public static void BuyProduct(List<Product> prodList, List<Manufactory> mList, List<Category> cList)
        {
            ShowAllProduct(prodList, mList, cList);

            Write($"\n\tПокупка: укажите название товара - ");
            string _nameProduct = ReadLine();
            prodList.Remove(prodList.Find(x => x.Name == _nameProduct));
            WriteDate("product.txt", prodList);
            WriteLine("\t\tТовар продан!!!\n");
        }
    }
    class Program
    {       
        static void Main(string[] args)
        {             
            List<string> mas = new List<string>();

            List<Product> ProductList = new List<Product>();
            List<Manufactory> ManufactoryList = new List<Manufactory>();
            List<Category> CategoryList = new List<Category>();
            List<User> UserList = new List<User>();

            mas = Market.ReadDate("login.txt");
            foreach (string item in mas)
            {
                string[] word = item.Split(' ');
                UserList.Add(new User(word[0], word[1]));
            }

            mas = Market.ReadDate("product.txt");
            foreach (string item in mas)
            {
                string[] word = item.Split(' ');
                ProductList.Add(new Product(word[0], Convert.ToInt32(word[1]), Convert.ToInt32(word[2]), Convert.ToInt32(word[3]), Convert.ToInt32(word[4]), Convert.ToInt32(word[5]), Convert.ToInt32(word[6])));
            }

            mas = Market.ReadDate("manufactory.txt");
            foreach (string item in mas)
            {
                string[] word = item.Split(' ');
                ManufactoryList.Add(new Manufactory(Convert.ToInt32(word[0]), word[1]));
            }

            mas = Market.ReadDate("category.txt");
            foreach (string item in mas)
            {
                string[] word = item.Split(' ');
                CategoryList.Add(new Category(Convert.ToInt32(word[0]), word[1]));
            }
            bool fl = false;
            do
            {
                try
                {
                    Write("\n\tВведите логин:\n");
                    string log = ReadLine();
                    Write("\n\tВведите пароль:\n");
                    string pas = ReadLine();
                   
                    foreach (User _user in UserList)
                    {
                        if (_user.Login == log && _user.Password == pas)

                        {
                            WriteLine("Авторизация прошла успешно!");
                            fl = true;                            
                                                      
                            for (; ; )
                            {
                                Clear();
                                MenuDelegate deleg = null;
                                Market.Menu(ProductList, ManufactoryList, CategoryList, _user);
                                WriteLine("\nУкажите пункт МЕНЮ!");
                                int chose = Convert.ToInt32(ReadLine());                               
                                switch (chose)
                                {
                                    case 1:
                                        deleg = Market.ShowAllProduct;
                                        break;
                                    case 2:
                                        deleg = Market.ShowAllProductCat;
                                        break;
                                    case 3:
                                        deleg = Market.ShowDataTodayOff;
                                        break;
                                    case 4:
                                        deleg = Market.ShowProductCategoryManufactoru;
                                        break;
                                    case 5:
                                        deleg = Market.ShowProductPriceRange;
                                        break;
                                    case 6:
                                        deleg = Market.ShowProductCountCategory;
                                        break;
                                    case 7:
                                        if (log == "admin") deleg = Market.Edit;
                                        if (log == "kassir") deleg = Market.BuyProduct;
                                        break;
                                    default:
                                        throw new InvalidOperationException();
                                }
                                _user.menuEvent += deleg;
                                _user.Action(ProductList, ManufactoryList, CategoryList);
                                _user.menuEvent -= deleg;
                                WriteLine("\nВыбрать другой пункт меню - 1. Закончить работу с программой - 2.");                                
                                int chose3 = Convert.ToInt32(ReadLine());
                                if (chose3 == 1) continue;
                                else
                                    if (chose == 2) break;
                                else break;
                            }
                            break;
                        }
                    }
                    if(fl==false) throw new Exception("Вы ввели неверный логин или пароль");
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                }
            }
            while (fl==false);          
        }
    }
}
