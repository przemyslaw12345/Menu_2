
public class RemovingFromDatabase : IRemovingFromDatabase
	{
	
	private readonly IAddingToDatabase _adding;
	private readonly IViewingItemsInDatabase _viewing;

	public RemovingFromDatabase(
		IAddingToDatabase adding,
		IViewingItemsInDatabase viewing)
	{
		
		_adding = adding;
		_viewing = viewing;
	}

	public void RemoveFromMenuMethod(IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository)
	{
		bool isWorking = true;
		while (isWorking)
		{
			Console.Clear();
			WhatToRemoveText();
			string optionToRemoveSelected = OptionToRemoveSelectedMethod();
			SelectingWhatToRemoveFromTheMenuMethod(optionToRemoveSelected, drinkRepository, foodRepository);
			isWorking = ContinueRemovingMethod(isWorking);
		}
	}

	void WhatToRemoveText()
	{
		Console.WriteLine("Would you like to remove a [drink] or [meal] from the menu");
	}

	string OptionToRemoveSelectedMethod() => (Console.ReadLine());

	void SelectingWhatToRemoveFromTheMenuMethod(string optionToAddSelected, IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository)
	{
		bool isWorkingSubLoop = true;
		while (isWorkingSubLoop)
		{
			switch (optionToAddSelected)
			{
				case "drink":
					//RemoveDrinkMethod(drinkRepository);
					RemoveMethod<Drink>(drinkRepository);
					isWorkingSubLoop = false;
					break;
				case "meal":
					//RemoveMealMethod(foodRepository);
					RemoveMethod<Meal>(foodRepository);
					isWorkingSubLoop = false;
					break;
				default:
					Console.WriteLine("You entered an invalid option, please try again");
					optionToAddSelected = _adding.OptionToAddSelectedMethod();
					isWorkingSubLoop = true;
					Console.ReadKey();
					break;
			}
		}
	}

	void RemoveMethod<T>(IRepository<T> tempRepository)
		where T : class, ICafeMenu
	{
		Type type = typeof(T);
		Console.WriteLine($"Which {type.Name.ToLower()} do you want to remove?");
		_viewing.ViewMenu(tempRepository);
		int removeOptionNumber = RemoveOptionNumberMethod();
		var itemToRemove = tempRepository.GetSpecific(removeOptionNumber);
		tempRepository.RemoveItem(itemToRemove);
		tempRepository.Save();
	}

	int RemoveOptionNumberMethod() => int.Parse(Console.ReadLine());

	bool ContinueRemovingMethod(bool isWorking)
	{
		bool isWorkingSubLoop = true;
		Console.WriteLine("Would you like to continue removing items? y/n");
		while (isWorkingSubLoop)
		{
			string willContinue = Console.ReadLine().ToLower();
			if (willContinue == "y")
			{
				isWorking = true;
				isWorkingSubLoop = false;
			}
			else if (willContinue == "n")
			{
				isWorking = false;
				isWorkingSubLoop = false;
			}
			else
			{
				isWorkingSubLoop = true;
				Console.WriteLine("Please write y for yes and n for no");
			}
		}
		return isWorking;
	}

}

