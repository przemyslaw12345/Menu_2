using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddTransient<IApp, App>();
services.AddTransient<IAddingToDatabase, AddingToDatabase>();
services.AddTransient<IRemovingFromDatabase, RemovingFromDatabase>();
services.AddTransient<IViewingItemsInDatabase, ViewingItemsInDatabase>();
services.AddTransient<IRepository<Meal>, MenuSqlRepository<Meal>>();
services.AddTransient<IRepository<Drink>, MenuSqlRepository<Drink>>();
services.AddDbContext<MenuDbContext>();

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetRequiredService<IApp>();
app.Run();



