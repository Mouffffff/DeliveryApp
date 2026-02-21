using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Domain.Entities;

public class Order {
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; } // Doit correspondre à TotalPrice dans DbContext
    public OrderStatus Status { get; set; } // Doit correspondre à Status dans DbContext
    
    public int CustomerId { get; set; }
    public int StoreId { get; set; }
    public int DeliveryAddressId { get; set; }
    public int? CourierId { get; set; } // Optionnel au début

    // Navigation
    public Address DeliveryAddress { get; set; } = null!;
}