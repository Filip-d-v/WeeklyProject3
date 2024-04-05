using System;
using System.Collections.Generic;

namespace AssetTracking
{
    // Enum to represent different offices
    public enum Office
    {
        NewYork,
        London,
        Tokyo
    }

    // Base class representing an item(Asset)
    public abstract class Item
    {
        public string Name { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PriceUSD { get; set; } // Price in USD
        public Office OfficeLocation { get; set; }

        protected Item(string name, DateTime purchaseDate, decimal priceUSD, Office officeLocation)
        {
            Name = name;
            PurchaseDate = purchaseDate;
            PriceUSD = priceUSD;
            OfficeLocation = officeLocation;
        }

        // Method to calculate remaining time
        public TimeSpan RemainingLifespan()
        {
            return PurchaseDate.AddYears(3) - DateTime.Today;
        }

        // Method to convert price to appropriate currency based on office location
        public decimal ConvertToOfficeCurrency()
        {
            switch (OfficeLocation)
            {
                case Office.NewYork:
                    return PriceUSD; 
                case Office.London:
                    return PriceUSD * 0.8m; // Convert to GBP
                case Office.Tokyo:
                    return PriceUSD * 152m; // Convert to JPY
                default:
                    return PriceUSD; // Default to USD
            }
        }

        // Method to get currency symbol based on office location
        public string GetCurrencySymbol()
        {
            switch (OfficeLocation)
            {
                case Office.NewYork:
                    return "$";
                case Office.London:
                    return "£";
                case Office.Tokyo:
                    return "¥";
                default:
                    return "";
            }
        }
    }
    public class Computer : Item
    {
        public Computer(string name, DateTime purchaseDate, decimal priceUSD, Office officeLocation)
            : base(name, purchaseDate, priceUSD, officeLocation)
        {
        }
    }
    public class Phone : Item
    {
        public Phone(string name, DateTime purchaseDate, decimal priceUSD, Office officeLocation)
            : base(name, purchaseDate, priceUSD, officeLocation)
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Item> items = new List<Item>();
            // Presets to fill out the list a bit
            items.Add(new Phone("S10", new DateTime(2022, 07, 16), 200, Office.London));
            items.Add(new Computer("Unix", new DateTime(2021, 06, 18), 600, Office.Tokyo));
            items.Add(new Phone("Iphone", new DateTime(2023, 07, 03), 150, Office.NewYork)); 

            while (true)
            {
                Console.Clear(); 
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Display List");
                Console.WriteLine("2. Add new item");
                Console.WriteLine("3. Exit");

                Console.WriteLine("Enter your choice:");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear(); 
                        DisplayList(items);
                        break;
                    case "2":
                        Console.Clear(); 
                        AddNewItem(items);
                        break;
                    case "3":
                        Console.WriteLine("Exiting the application...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option (1, 2, or 3).");
                        break;
                }
            }
        }
        static void DisplayList(List<Item> items)
        {           
            var sortedItems = items.OrderBy(item =>
            {
                if (item is Computer)
                    return 0; // Computers first
                else
                    return 1; // Phones second
            })
                .ThenBy(item => item.OfficeLocation) // Sort by office
                .ThenBy(item => item.PurchaseDate); // Sort by purchase date

            Console.WriteLine("======================================================================================");
            Console.WriteLine("|    Type    |            Name            |       Price       |   Office   |   Date   |");
            Console.WriteLine("======================================================================================");

            foreach (var item in sortedItems)
            {
                string type = item is Computer ? "Computer" : "Phone";
                string name = item.Name;
                decimal price = item.ConvertToOfficeCurrency();
                string currencySymbol = item.GetCurrencySymbol();
                string office = item.OfficeLocation.ToString();
                string date = item.PurchaseDate.ToShortDateString();

                string status = "";
                if (item.RemainingLifespan() < TimeSpan.FromDays(90))
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (item.RemainingLifespan() < TimeSpan.FromDays(180))
                    Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine($"| {type,-10} | {name,-25} | {currencySymbol}{price,-17} | {office,-10} | {date,-9} |");

                Console.ResetColor();
            }

            Console.WriteLine("======================================================================================");
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }

        static void AddNewItem(List<Item> items)
        {
            Console.WriteLine("Is it a Computer or Phone you would like to add? :");
            Console.WriteLine("1. Computer");
            Console.WriteLine("2. Phone");
            Console.WriteLine("Enter your choice:");
            string choice = Console.ReadLine();

            Console.Clear();

            Console.WriteLine("Enter name:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter purchase date (yyyy-MM-dd):");
            DateTime purchaseDate;
            while (!DateTime.TryParse(Console.ReadLine(), out purchaseDate))
            {
                Console.WriteLine("Invalid date format. Please enter in yyyy-MM-dd format:");
            }

            Console.WriteLine("Enter price in USD:");
            decimal priceUSD;
            while (!decimal.TryParse(Console.ReadLine(), out priceUSD))
            {
                Console.WriteLine("Invalid price. Please enter a valid number:");
            }

            Console.WriteLine("Enter office location (NewYork, London, Tokyo):");
            Office officeLocation;
            while (!Enum.TryParse(Console.ReadLine(), true, out officeLocation))
            {
                Console.WriteLine("Invalid office location. Please enter NewYork, London, or Tokyo:");
            }

            switch (choice)
            {
                case "1":
                    items.Add(new Computer(name, purchaseDate, priceUSD, officeLocation));
                    Console.WriteLine("Computer added successfully.");
                    break;
                case "2":
                    items.Add(new Phone(name, purchaseDate, priceUSD, officeLocation));
                    Console.WriteLine("Phone added successfully.");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Item not added.");
                    break;
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey(); 
        }
    }
}
