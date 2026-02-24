using DeliveryApp.Frontend.Models;

namespace DeliveryApp.Frontend.Services;

public sealed class CartState
{
    private readonly List<CartLine> _lines = [];

    public event Action? Changed;

    public IReadOnlyList<CartLine> Lines => _lines;
    public int? StoreId { get; private set; }
    public int TotalItems => _lines.Sum(l => l.Quantity);
    public decimal TotalAmount => _lines.Sum(l => l.UnitPrice * l.Quantity);

    public void AddProduct(ProductDto product, int quantity)
    {
        if (quantity <= 0)
        {
            return;
        }

        if (StoreId.HasValue && StoreId.Value != product.StoreId)
        {
            _lines.Clear();
        }

        StoreId = product.StoreId;

        var existing = _lines.FirstOrDefault(l => l.ProductId == product.Id);
        if (existing is null)
        {
            _lines.Add(new CartLine(product.Id, product.Name, product.Price, quantity));
        }
        else
        {
            existing.Quantity += quantity;
        }

        Changed?.Invoke();
    }

    public void SetQuantity(int productId, int quantity)
    {
        var existing = _lines.FirstOrDefault(l => l.ProductId == productId);
        if (existing is null)
        {
            return;
        }

        if (quantity <= 0)
        {
            _lines.Remove(existing);
        }
        else
        {
            existing.Quantity = quantity;
        }

        if (_lines.Count == 0)
        {
            StoreId = null;
        }

        Changed?.Invoke();
    }

    public void Remove(int productId)
    {
        _lines.RemoveAll(l => l.ProductId == productId);
        if (_lines.Count == 0)
        {
            StoreId = null;
        }

        Changed?.Invoke();
    }

    public void Clear()
    {
        _lines.Clear();
        StoreId = null;
        Changed?.Invoke();
    }

    public List<CreateOrderItemRequest> BuildOrderItems() =>
        _lines.Select(l => new CreateOrderItemRequest(l.ProductId, l.Quantity)).ToList();
}

public sealed class CartLine
{
    public int ProductId { get; }
    public string ProductName { get; }
    public decimal UnitPrice { get; }
    public int Quantity { get; set; }

    public CartLine(int productId, string productName, decimal unitPrice, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}
