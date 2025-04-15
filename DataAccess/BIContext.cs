using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class BIContext(DbContextOptions<BIContext> options) : DbContext(options)
{
    
}