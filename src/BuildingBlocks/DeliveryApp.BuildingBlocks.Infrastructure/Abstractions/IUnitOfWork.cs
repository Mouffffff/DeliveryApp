namespace DeliveryApp.BuildingBlocks.Infrastructure.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
