
public class Adding : IAdding
	{
	public void AddToMenuMethod(IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository)
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
	public string optionToAddSelectedMethod() => Console.ReadLine().ToLower();
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
		string yesOrNo = DetermineYesOrNeMethod();
		List<string> ingredientsTempList = new List<string>();
		switch (yesOrNo)
		{
			case "y":
				ingredientsTempList = IngredientsOfItems();
				break;
			case "n":
				ingredientsTempList = null;
				break;
		}

		tempRepository.Add(new T { itemName = nameOfItem, itemPrice = priceOfItem, ingredients = ingredientsTempList });
		tempRepository.Save();
	}
	private string DetermineYesOrNeMethod()
	{
		bool subLoop = true;
		string? yesOrNo = "n";
		
		while (subLoop)
		{
			yesOrNo = Console.ReadLine().ToLower();
			if (yesOrNo == "y")
			{
				subLoop = false;
			}
			else if (yesOrNo == "n")
			{
				subLoop = false;
			}
			else
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
			string yesOrNo = DetermineYesOrNeMethod();
			switch (yesOrNo)
			{
				case "y":
					subLoop = true;
					break;
				case "n":
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
}
