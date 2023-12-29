
using System.Text.Json;

internal class App : IApp
{
	public void Run()
	{
		bool isWroking = true;
		var drinkRepository = new MenuSqlRepository<Drink>(new MenuDbContext());
		drinkRepository.AddedItem += AddEventItemToList;
		drinkRepository.RemovedItem += RemoveEventItemToList;

		var mealRepository = new MenuSqlRepository<Meal>(new MenuDbContext());
		mealRepository.AddedItem += AddEventItemToList;
		mealRepository.RemovedItem += RemoveEventItemToList;

		while (isWroking)
		{
			Console.Clear();
			Greeting();
			string optionSelected = optionSelectedMethod();
			isWroking = selectingWhatUserWishesToDoMethod(isWroking, optionSelected, drinkRepository, mealRepository);
			Console.ReadKey();
		}
	}
	//--------------------------------------------------------------------------------------------------------------------------------------------------------------
	void AddEventItemToList(object? sender, CafeMenu e)
	{
		const string addedItemEvent = "AddedItemEvent.json";
		List<string> addedItemEventList = new List<string>();
		string jsonAddedItemEventList;
		if (File.Exists(addedItemEvent))
		{
			jsonAddedItemEventList = File.ReadAllText(addedItemEvent);
			addedItemEventList = JsonSerializer.Deserialize<List<string>>(jsonAddedItemEventList);
		}
		string itemAdded = $"Date Added: {DateTime.Now}, Menu Item: {e.itemName}, Menu Price: {e.itemPrice}, From: {sender.GetType().Name}";
		addedItemEventList.Add(itemAdded);
		jsonAddedItemEventList = JsonSerializer.Serialize(addedItemEventList);
		File.WriteAllText(addedItemEvent, jsonAddedItemEventList);
	}
	void RemoveEventItemToList(object? sender, CafeMenu e)
	{
		const string removedItemEvent = "RemovedItemEvent.json";
		List<string> removedItemEventList = new List<string>();
		string jsonRemovedItemEventList;
		if (File.Exists(removedItemEvent))
		{
			jsonRemovedItemEventList = File.ReadAllText(removedItemEvent);
			removedItemEventList = JsonSerializer.Deserialize<List<string>>(jsonRemovedItemEventList);
		}
		string itemRemoved = $"Date Removed: {DateTime.Now}, Menu Item: {e.itemName}, Menu Price: {e.itemPrice}, From: {sender.GetType().Name}";
		removedItemEventList.Add(itemRemoved);
		jsonRemovedItemEventList = JsonSerializer.Serialize(removedItemEventList);
		File.WriteAllText(removedItemEvent, jsonRemovedItemEventList);
	}
	//--------------------------------------------------------------------------------------------------------------------------------------------------------------
	void Greeting()
	{
		Console.WriteLine(
			$"Welcome to Soylent Green Cafe where our Customer is our specialty!! {Environment.NewLine}" +
			$"I will be your host Jenna {Environment.NewLine}" +
			$"{Environment.NewLine}" +
			$"How may I assist you from the following options? {Environment.NewLine}" +
			$"{Environment.NewLine}" +
			$"[View] the Menu {Environment.NewLine}" +
			$"[Add] new item to the menu {Environment.NewLine}" +
			$"[Remove] an item from the menu {Environment.NewLine}" +
			$"[Exit] Soylent Green Cafe {Environment.NewLine}" +
			$"{Environment.NewLine}" +
			$"Input preferred choice within []?"
			);
	}
	static string optionSelectedMethod() => Console.ReadLine().ToLower();
	bool selectingWhatUserWishesToDoMethod(bool isWroking, string optionSelected, MenuSqlRepository<Drink> drinkRepository, MenuSqlRepository<Meal> foodRepository)
	{
		switch (optionSelected)
		{
			case "view":
				ViewMenuGeneralMethod(drinkRepository, foodRepository);
				break;
			case "add":
				AddToMenuMethod(drinkRepository, foodRepository);
				break;
			case "remove":
				RemoveFromMenuMethod(drinkRepository, foodRepository);
				break;
			case "exit":
				Console.WriteLine("Thank you for trying Soylent Green Cafe where our Customers are our specialty!");
				isWroking = false;
				break;
			default:
				Console.WriteLine("Incorrect input, please try again.");
				break;

		}
		return isWroking;
	}
	//--------------------------------------------------------------------------------------------------------------------------------------------------------------
	void ViewMenuGeneralMethod(IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository)
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
	void ViewMenu(IReadRepository<ICafeMenu> subMenuRepository)
		
	{
		var items = subMenuRepository.GetAll();
		foreach (var item in items)
		{
			
			Console.WriteLine(item.Id+ ". "+ item.itemName + " " + item.itemPrice + Environment.NewLine + String.Join(", ", item.ingredients));
			//item.ingredients.ForEach(Console.WriteLine);
		}
	}
	//--------------------------------------------------------------------------------------------------------------------------------------------------------------
	void AddToMenuMethod(IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository)
	{
		bool isWorking = true;
		while (isWorking)
		{
			Console.Clear();
			WhatToAddText();
			string optionToAddSelected = optionToAddSelectedMethod();
			selectingWhatToAddToTheMenuMethod(optionToAddSelected, drinkRepository, foodRepository);
			isWorking = ContinueAddingMethod(isWorking);
		}
	}
	void WhatToAddText()
	{
		Console.WriteLine(
			$"What would you like to add to the menu? {Environment.NewLine}" +
			$"A [Drink]? {Environment.NewLine}" +
			$"A [Meal] {Environment.NewLine}"
			);
	}
	string optionToAddSelectedMethod() => Console.ReadLine().ToLower();
	void selectingWhatToAddToTheMenuMethod(string optionToAddSelected, IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository)
	{
		bool isWorkingSubLoop = true;
		while (isWorkingSubLoop)
		{
			switch (optionToAddSelected)
			{
				case "drink":
					//AddDrinkMethod(drinkRepository);
					AddMethod<Drink>(drinkRepository);
					isWorkingSubLoop = false;
					break;
				case "meal":
					//AddMealMethod(foodRepository);
					AddMethod<Meal>(foodRepository);
					isWorkingSubLoop = false;
					break;
				default:
					Console.WriteLine("You entered an invalid option, please try again");
					optionToAddSelected = optionToAddSelectedMethod();
					isWorkingSubLoop = true;
					Console.ReadKey();
					break;
			}
		}
	}
	void AddMethod<T>(IWriteRepository<T> tempRepository)
		where T : class, ICafeMenu, new()
	{
		Type type = typeof(T);

		Console.WriteLine($"What is the {type.ToString().ToLower()}s name? {Environment.NewLine}");
		string nameOfItem = NamingItemMethod();

		Console.WriteLine($"What is the {type.ToString().ToLower()}s price? {Environment.NewLine}");
		float priceOfItem = PriceItemMethod();

		Console.WriteLine($"Are there any ingredients you would like to display? [y/n] {Environment.NewLine}");
		char yesOrNo = DetermineYesOrNeMethod();
		List<string> ingredientsTempList = new List<string>();
		switch (yesOrNo)
		{
			case 'y':
				ingredientsTempList = IngredientsOfItems();
				break;
			case 'n':
				ingredientsTempList = null;
				break;
		}

		tempRepository.Add(new T { itemName = nameOfItem, itemPrice = priceOfItem, ingredients = ingredientsTempList });
		tempRepository.Save();
	}
	private char DetermineYesOrNeMethod()
	{
		bool subLoop = true;
		char yesOrNo = 'n';
		while (subLoop)
		{
			yesOrNo = Convert.ToChar(Console.ReadLine().ToLower());
			if (yesOrNo == 'y')
			{
				subLoop = false;
			}else if (yesOrNo == 'n')
			{
				subLoop = false;
			}else
			{
				Console.WriteLine("Improper input, would you like to add any ingrediants for display? [y/n]");
				subLoop = false;
			}
		}
		return yesOrNo;
	}
	List<string> IngredientsOfItems()
	{
		List<string> ingrediantsTempList = new List<string>();
		string ingrediant;
		bool subLoop = true;
		int counter = 0;
		while (subLoop)
		{
			switch (counter)
			{
				case 0:
                    Console.WriteLine("Please add first ingrediant.");
					counter++;
					break;
				case 1:
                    Console.WriteLine("Please add next ingrediant.");
					break;
            }
			ingrediant = NamingItemMethod().ToLower();
			ingrediantsTempList.Add(ingrediant);
            Console.WriteLine("Add another ingrediant? [y/n]");
			char yesOrNo = DetermineYesOrNeMethod();
			switch (yesOrNo)
			{
				case 'y':
					subLoop = true;
					break;
				case 'n':
					subLoop = false;
                    Console.WriteLine("Thank you for adding all the ingrediants, have a nice day!");
                    break;
			}
		}
		return ingrediantsTempList;
	}
	string NamingItemMethod() => Console.ReadLine();
	float PriceItemMethod() => float.Parse(Console.ReadLine());
	bool ContinueAddingMethod(bool isWorking)
	{
		bool isWorkingSubLoop = true;
		Console.WriteLine("Would you like to request another item to the menu? Y/N");
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
	//--------------------------------------------------------------------------------------------------------------------------------------------------------------
	void RemoveFromMenuMethod(IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository)
	{
		bool isWorking = true;
		while (isWorking)
		{
			Console.Clear();
			WhatToRemoveText();
			string optionToRemoveSelected = optionToRemoveSelectedMethod();
			selectingWhatToRemoveFromTheMenuMethod(optionToRemoveSelected, drinkRepository, foodRepository);
			isWorking = ContinueRemovingMethod(isWorking);
		}
	}
	void WhatToRemoveText()
	{
		Console.WriteLine("Would you like to remove a [drink] or [meal] from the menu");
	}
	string optionToRemoveSelectedMethod() => (Console.ReadLine());
	void selectingWhatToRemoveFromTheMenuMethod(string optionToAddSelected, IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository)
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
					optionToAddSelected = optionToAddSelectedMethod();
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
		ViewMenu(tempRepository);
		int removeOptionNumber = RemoveOptionNumberMethod();
		var itemToRemove = tempRepository.GetSpecific(removeOptionNumber);
		tempRepository.RemoveItem(itemToRemove);
		tempRepository.Save();
	}
	//void RemoveDrinkMethod(IRepository<Drink> drinkRepository)
	//{
	//	Console.WriteLine("Which drink do you want to remove?");
	//	ViewMenu(drinkRepository);
	//	int removeOptionNumber = RemoveOptionNumberMethod();
	//	var itemToRemove = drinkRepository.GetSpecific(removeOptionNumber);
	//	drinkRepository.RemoveItem(itemToRemove);
	//	drinkRepository.Save();
	//}
	//void RemoveMealMethod(IRepository<Meal> foodRepository)
	//{
	//	Console.WriteLine("Which meal do you want to remove?");
	//	ViewMenu(foodRepository);
	//	int removeOptionNumber = RemoveOptionNumberMethod();
	//	var itemToRemove = foodRepository.GetSpecific(removeOptionNumber);
	//	foodRepository.RemoveItem(itemToRemove);
	//	foodRepository.Save();
	//}
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

