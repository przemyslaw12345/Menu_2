

public class Viewing : IViewing
	{
	public void ViewMenuGeneralMethod(IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository)
	{
		Console.Clear();
		MenuGreeting();
		ViewMenu(drinkRepository);
		ViewMenu(foodRepository);
	}
	void MenuGreeting()
	{
		Console.WriteLine($"Thank you for dining at Soylent Green Cafe where our Customer is our specialty!!{Environment.NewLine}" +
			$"Please take a look at our menu!! {Environment.NewLine}"
			);
	}
	public void ViewMenu(IReadRepository<ICafeMenu> subMenuRepository)
	{
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

