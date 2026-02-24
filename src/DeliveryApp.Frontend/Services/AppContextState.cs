namespace DeliveryApp.Frontend.Services;

public sealed class AppContextState
{
    public event Action? Changed;

    public int? SelectedCustomerId { get; private set; }
    public int? SelectedAddressId { get; private set; }
    public int? SelectedCourierId { get; private set; }
    public int? CurrentOrderId { get; private set; }

    public void SetCustomer(int? customerId)
    {
        SelectedCustomerId = customerId;
        Changed?.Invoke();
    }

    public void SetAddress(int? addressId)
    {
        SelectedAddressId = addressId;
        Changed?.Invoke();
    }

    public void SetCourier(int? courierId)
    {
        SelectedCourierId = courierId;
        Changed?.Invoke();
    }

    public void SetOrder(int? orderId)
    {
        CurrentOrderId = orderId;
        Changed?.Invoke();
    }

    public void SetContext(int? customerId, int? addressId, int? courierId, int? orderId)
    {
        SelectedCustomerId = customerId;
        SelectedAddressId = addressId;
        SelectedCourierId = courierId;
        CurrentOrderId = orderId;
        Changed?.Invoke();
    }

    public void Reset()
    {
        SelectedCustomerId = null;
        SelectedAddressId = null;
        SelectedCourierId = null;
        CurrentOrderId = null;
        Changed?.Invoke();
    }
}
