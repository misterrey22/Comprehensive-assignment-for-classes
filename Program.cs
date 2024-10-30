using System;
using System.Collections.Generic;

// Інтерфейс для товарів
interface IProduct
{
    string Name { get; set; }
    double Price { get; set; }
    double CalculateDiscount();
}

// Абстрактний клас для товарів
abstract class Product : IProduct
{
    public string Name { get; set; }
    public double Price { get; set; }

    public abstract double CalculateCost();

    public virtual double CalculateDiscount()
    {
        // Реалізація знижки за замовчуванням
        return 0.05 * Price;
    }
}

// Класи-нащадки для різних видів товарів
class Book : Product
{
    public int PageCount { get; set; }

    public override double CalculateCost()
    {
        return Price + CalculateDiscount();
    }
}

class Electronics : Product
{
    public int MemorySize { get; set; }

    public override double CalculateCost()
    {
        return Price + CalculateDiscount();
    }
}

class Clothing : Product
{
    public string Size { get; set; }

    public override double CalculateCost()
    {
        return Price + CalculateDiscount();
    }
}

// Клас для замовлення
class Order
{
    public int OrderNumber { get; set; }
    public List<IProduct> Products { get; set; }
    public double TotalCost => CalculateTotalCost();
    public event Action<string> OrderStatusChanged;

    private double CalculateTotalCost()
    {
        double total = 0;
        foreach (var product in Products)
        {
            total += product.CalculateCost();
        }
        return total;
    }

    public void ChangeOrderStatus(string status)
    {
        OrderStatusChanged?.Invoke(status);
    }
}

// Клас для обробки замовлення
class OrderProcessor
{
    public void ProcessOrder(Order order)
    {
        // Логіка обробки замовлення, наприклад, розрахунок вартості, підготовка до відправлення тощо
        Console.WriteLine($"Order {order.OrderNumber} processed. Total cost: {order.TotalCost}");

        // Змінюємо статус та відправляємо сповіщення
        order.ChangeOrderStatus("Processed");
    }
}

// Клас для сервісу сповіщень
class NotificationService
{
    public void SendNotification(string message)
    {
        Console.WriteLine($"Notification: {message}");
    }
}

class MainClass
{
    public static void Main(string[] args)
    {
        // Створюємо товари
        var book = new Book { Name = "Book1", Price = 20.0, PageCount = 100 };
        var electronics = new Electronics { Name = "Phone", Price = 500.0, MemorySize = 64 };
        var clothing = new Clothing { Name = "Shirt", Price = 30.0, Size = "M" };

        // Створюємо замовлення
        var order = new Order { OrderNumber = 1, Products = new List<IProduct> { book, electronics, clothing } };

        // Створюємо об'єкти для обробки та сповіщення
        var orderProcessor = new OrderProcessor();
        var notificationService = new NotificationService();

        // Підписуємо метод SendNotification на подію OrderStatusChanged
        order.OrderStatusChanged += notificationService.SendNotification;

        // Створюємо об'єкт для обробки та обробляємо замовлення
        orderProcessor.ProcessOrder(order);
    }
}