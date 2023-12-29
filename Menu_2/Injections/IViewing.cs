
public interface IViewing
	{
	public void ViewMenuGeneralMethod(IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository);
	public void ViewMenu<T>(IReadRepository<T> subMenuRepository) where T : class, ICafeMenu;
	}

