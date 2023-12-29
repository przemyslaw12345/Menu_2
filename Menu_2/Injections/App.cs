using System.Text.Json;

public class App : IApp
{
	private readonly IAddingToDatabase _adding;
	private readonly IRemovingFromDatabase _removing;
	private readonly IViewingItemsInDatabase _viewing;

	public App(
		IAddingToDatabase adding,
		IRemovingFromDatabase removing,
		IViewingItemsInDatabase viewing
		) 
	{
		_adding = adding;
		_removing = removing;
		_viewing = viewing;
	}

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
			string optionSelected = OptionSelectedMethod();
			isWroking = SelectingWhatUserWishesToDoMethod(isWroking, optionSelected, drinkRepository, mealRepository);
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
		string itemAdded = $"Date Added: {DateTime.Now}, Menu Item: {e.ItemName}, Menu Price: {e.ItemPrice}, From: {sender.GetType().Name}";
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
		string itemRemoved = $"Date Removed: {DateTime.Now}, Menu Item: {e.ItemName}, Menu Price: {e.ItemPrice}, From: {sender.GetType().Name}";
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

	static string OptionSelectedMethod() => Console.ReadLine().ToLower();
	
	bool SelectingWhatUserWishesToDoMethod(bool isWroking, string optionSelected, MenuSqlRepository<Drink> drinkRepository, MenuSqlRepository<Meal> foodRepository)
	{
		switch (optionSelected)
		{
			case "view":
				_viewing.ViewMenuGeneralMethod(drinkRepository, foodRepository);
				break;
			case "add":
				_adding.AddToMenuMethod(drinkRepository, foodRepository);
				break;
			case "remove":
				_removing.RemoveFromMenuMethod(drinkRepository, foodRepository);
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

}

