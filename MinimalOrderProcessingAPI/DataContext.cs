using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;

namespace MinimalOrderProcessingAPI
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySQL("Server=localhost;Database=myDataBase;Uid=root;Pwd=krakadak-ula2011;");
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
