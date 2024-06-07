using Ardalis.SharedKernel;
using Ardalis.Specification.EntityFrameworkCore;

namespace Infrastructure.Data;

public class EfRepository<T>(AppDbContext dbContext) :
    RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T>
        where T : class, IAggregateRoot
{
}
