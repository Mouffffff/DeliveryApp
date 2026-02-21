namespace DeliveryApp.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    // Un client peut avoir plusieurs adresses enregistrées (Maison, Travail, etc.)
    public List<Address> SavedAddresses { get; set; } = new();
}