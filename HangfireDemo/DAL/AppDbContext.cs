using Microsoft.EntityFrameworkCore;

namespace HangfireDemo.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
    }
}