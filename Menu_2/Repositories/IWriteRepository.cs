
public interface IWriteRepository<in T>
	where T : class, ICafeMenu
{
	void Add(T item);
	void RemoveItem(T item);
	void Edit();

	void Save();
}

