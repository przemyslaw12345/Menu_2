public interface IAddingToDatabase
	{
	public void AddToMenuMethod(IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository);
	public string OptionToAddSelectedMethod();
	}

