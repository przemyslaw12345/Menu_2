
public interface IViewing
	{
	public void ViewMenuGeneralMethod(IRepository<Drink> drinkRepository, IRepository<Meal> foodRepository);
	public void ViewMenu(IReadRepository<ICafeMenu> subMenuRepository);
	}

