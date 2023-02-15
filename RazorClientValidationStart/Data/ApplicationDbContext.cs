using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SkysFormsDemo.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //jhsdfkfsdjklfjsdklsdfjklsdfjkl
    }


    public DbSet<Person> Person { get; set; }
    public DbSet<Account> Accounts { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<Product> Products { get; set; }
}