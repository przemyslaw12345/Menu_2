

public class Viewing : IViewing
	{
	public void ViewMenuGeneralMethod(IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository)
	{
		Console.Clear();
		MenuGreeting();
		string orderMenu = OrderMenu();
		switch (orderMenu)
		{
			case "y":
				
				HowToOrderMenu(drinkRepository, foodRepository);
				break;
			case "n":
				Console.Clear();
				Console.WriteLine($"Please take a look at our menu!!  {Environment.NewLine}");
				ViewMenu(drinkRepository);
				ViewMenu(foodRepository);
				break;
			default:
				Console.WriteLine("Incorrect input, will redirect you to the General Menu");
				Console.ReadLine();
				ViewMenuGeneralMethod(drinkRepository, foodRepository);
				break;
		}
		
	}
	void MenuGreeting()
	{
		Console.WriteLine($"Thank you for dining at Soylent Green Cafe where our Customer is our specialty!!{Environment.NewLine}" +
			//$"Please take a look at our menu!! {Environment.NewLine}"
			$"Would you like to order the menu? [y/n] {Environment.NewLine}"
			);
	}
	private string OrderMenu() => Console.ReadLine().ToLower();
	private void HowToOrderMenu(IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository)
	{
		Console.Clear();
		Console.WriteLine("How would you like the menu ordered by?" + Environment.NewLine +
					"1. By item name" + Environment.NewLine +
					"2. By item price" + Environment.NewLine );
		string orderMenu = OrderMenu();
		switch (orderMenu)
		{
			case "1":
				OrderByNameMethod<Drink>(drinkRepository);
				OrderByNameMethod<Meal>(foodRepository);
				break;
			case "2":
				OrderByPriceMethod<Drink>(drinkRepository);
				OrderByPriceMethod<Meal>(foodRepository);
				break;
			default:
				Console.WriteLine("Incorrect input, will redirect you to the General Menu");
				Console.ReadLine();
				HowToOrderMenu(drinkRepository, foodRepository);
				break;
		}
	}

	private void OrderByPriceMethod<T>(IRepository<T> tempRepository)
		where T : class, ICafeMenu
	{
		Type type = typeof(T);
		Console.WriteLine($"---------------------{type.Name.ToString()} Menu---------------------");
		var items = tempRepository.GetAll().OrderBy(i => i.itemPrice).ToList();
		foreach (var item in items)
		{
			if (item.ingredients != null)
			{
				Console.WriteLine(item.Id + ". " + item.itemName + " " + item.itemPrice + Environment.NewLine + String.Join(", ", item.ingredients));
			}
			else
			{
				Console.WriteLine(item.Id + ". " + item.itemName + " " + item.itemPrice + Environment.NewLine);
			}
		}
	}

	private void OrderByNameMethod<T>(IRepository<T> tempRepository)
		where T : class, ICafeMenu
	{
		Type type = typeof(T);
		Console.WriteLine($"---------------------{type.Name.ToString()} Menu---------------------");
		var items = tempRepository.GetAll().OrderBy(i => i.itemName).ToList();
		foreach (var item in items)
		{
			if (item.ingredients != null)
			{
				Console.WriteLine(item.Id + ". " + item.itemName + " " + item.itemPrice + Environment.NewLine + String.Join(", ", item.ingredients));
			}
			else
			{
				Console.WriteLine(item.Id + ". " + item.itemName + " " + item.itemPrice + Environment.NewLine);
			}
		}
	}

	public void ViewMenu<T>(IReadRepository<T> subMenuRepository)
		where T : class, ICafeMenu
	{
		Type type = typeof(T);
        Console.WriteLine($"---------------------{type.Name.ToString()} Menu---------------------");
        var items = subMenuRepository.GetAll();
		foreach (var item in items)
		{
			if (item.ingredients != null)
			{
				Console.WriteLine(item.Id + ". " + item.itemName + " " + item.itemPrice + Environment.NewLine + String.Join(", ", item.ingredients));
			}
			else
			{
				Console.WriteLine(item.Id + ". " + item.itemName + " " + item.itemPrice + Environment.NewLine);
			}

			//item.ingredients.ForEach(Console.WriteLine);
		}
	}
}




