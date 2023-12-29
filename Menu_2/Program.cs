using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<IApp, App>();
services.AddSingleton<IRepository<Meal>, MenuSqlRepository<Meal>>();
services.AddSingleton<IRepository<Drink>, MenuSqlRepository<Drink>>();
services.AddDbContext<MenuDbContext>();

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetRequiredService<IApp>();
app.Run();



