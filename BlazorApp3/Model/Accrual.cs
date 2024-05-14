namespace BlazorApp3.Model
{
	public class Accrual
	{
		private readonly int _id;
		private readonly int _price;
		private string _from;

		private readonly DateTime _datetime;
		public int Price { get { return _price; } }
		public string From { get { return _from; } }

		public int Id { get { return _id; } }
		public DateTime DateTime { get { return _datetime; } }
		public Accrual(string sender, int price)
		{
			_price = price;
			_from = sender;

			_datetime = DateTime.Now;
		}
		public Accrual()
		{

		}
	}
}
