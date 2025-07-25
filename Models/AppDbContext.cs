using Microsoft.EntityFrameworkCore;

namespace ContactAgendaApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Contact> Contacts => Set<Contact>();
    }
}
