using Microsoft.EntityFrameworkCore;
internal class MenuDbContext : DbContext
{
	DbSet<Drink> Drinks => Set<Drink>();
	DbSet<Meal> Meals => Set<Meal>();
    public MenuDbContext()
    {
        
    }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(@"Server=DESKTOP-GA3KCB6;Database=Menu_2;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
	}
}
