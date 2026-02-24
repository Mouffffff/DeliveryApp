namespace DeliveryApp.Application.DTOs;

public record CreateDeliveryItemDto(
    int ProductId,
    int Quantity
);

public class CreateDeliveryDto
{
    public int CustomerId { get; set; }
    public int StoreId { get; set; }
    public List<int> ProductIds { get; set; } = new();
    public List<CreateDeliveryItemDto> Items { get; set; } = new();
    public int DeliveryAddressId { get; set; }
}
