internal interface ICafeMenu
{
	public int Id { get; set; }
	public string itemName { get; set; }
	public float itemPrice { get; set; }
	public List<string> ingredients { get; set; }

	
}