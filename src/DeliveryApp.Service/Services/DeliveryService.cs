using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.Interfaces;
using DeliveryApp.Application.Validation;
using DeliveryApp.Domain.Entities;
using DeliveryApp.Domain.Enums;

namespace DeliveryApp.Service;

public class DeliveryService : IDeliveryService
{
    private readonly IDeliveryRepository _repo;

    public DeliveryService(IDeliveryRepository repo) => _repo = repo;

    public async Task<List<DeliveryDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(ToDto).ToList();
    }

    public async Task<DeliveryDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item == null ? null : ToDto(item);
    }

    public async Task<List<DeliveryDto>> GetByStatusAsync(int status)
    {
        var items = await _repo.GetByStatusAsync(status);
        return items.Select(ToDto).ToList();
    }

    public async Task<(bool ok, string error, DeliveryDto? created)> CreateAsync(CreateDeliveryDto dto)
    {
        var (ok, error) = DeliveryValidators.Validate(dto);
        if (!ok) return (false, error, null);

        var entity = new Order
        {
            OrderDate = DateTime.UtcNow, // Remplace CreatedAt
            Status = OrderStatus.Pending,
            TotalPrice = 0, // À initialiser selon ta logique
            CustomerId = 1, // À lier à l'utilisateur connecté plus tard
            StoreId = 1,
            DeliveryAddressId = 1
        };

        var created = await _repo.AddAsync(entity);
        return (true, "", ToDto(created));
    }

    public async Task<(bool ok, string error, DeliveryDto? updated)> UpdateAsync(int id, UpdateDeliveryDto dto)
    {
        var (ok, error) = DeliveryValidators.Validate(dto);
        if (!ok) return (false, error, null);

        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return (false, "Order not found.", null);

        // On ne met à jour que ce qui existe dans Order.cs
        existing.Status = (OrderStatus)dto.Status;

        var updated = await _repo.UpdateAsync(existing);
        return updated == null ? (false, "Update failed.", null) : (true, "", ToDto(updated));
    }

    public async Task<bool> DeleteAsync(int id) 
        => await _repo.DeleteAsync(id);

    // ── Private helper ──────────────────────────────
    private static DeliveryDto ToDto(Order e)
        => new DeliveryDto(
            e.Id,
            $"Order #{e.Id}", // On génère un titre fictif pour le DTO
            $"Total: {e.TotalPrice}€", // On utilise TotalPrice pour la description du DTO
            (int)e.Status,
            e.OrderDate,
            null 
        );
}