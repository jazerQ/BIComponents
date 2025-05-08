using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public interface IAppDbContext
{
    DbSet<Model> Models { get; set; }

    DbSet<Product> Products { get; set; }

    DbSet<TypeOfModel> Types { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cs);
}