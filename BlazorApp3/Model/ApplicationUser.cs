using BlazorApp3.Model.Operation;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using BlazorApp3.Model.Categories;
using BlazorApp3.Components.Pages;

namespace BlazorApp3.Model
{
    public class ApplicationUser
	{
		private List<Purchase> _purchases;
		private List<Accrual> _accruals;
		
		public List<Purchase> Purchases { get { return _purchases; } set { _purchases = value; } }
		public List<Accrual> Accruals { get { return _accruals; } set { _accruals = value; } }

		private float _purchase_limit;

		/// <summary>
		/// Список настроек категорий по отображению разброса от нормы
		/// </summary>
		private List<bool> _isAbsolute = new();
		public List<bool> IsAbsolute
		{
			get	{ return _isAbsolute; }
			set { _isAbsolute = value; }
		}


		[Required]
		public string Login { get; set; }

		public Purchase CurrentPurchase;
		public DateTime PeriodBegin;
        public DateTime PeriodEnd;
        public float Purchase_Limit { get { return _purchase_limit; } set { _purchase_limit = value; } }
		public string SPurchase_Limit { get { return _purchase_limit.ToString(); } set { _purchase_limit = float.Parse(value); } }
		public float Purchase_Amount(DateTime begin, DateTime end)
		{
			float total = 0;
			foreach (var purchase in Purchases)
			{
				if (purchase.DateTime < end & purchase.DateTime > begin)
				{
					total += purchase.Price;
				}
			}
			return total;
		}

		/// <summary>
		/// Траты за переод по категории
		/// </summary>
		/// <param name="begin">Начало переода</param>
		/// <param name="end">Конец переода</param>
		/// <param name="category">Категория</param>
		/// <returns>Сумма трат</returns>
		public float Purchase_Amount_Category(DateTime begin, DateTime end, Category category)
		{
			float total = 0;
			foreach (var purchase in Purchases)
			{
				if (purchase.DateTime < end & purchase.DateTime > begin & purchase.Category == category)
				{
					total += purchase.Price;
				}
			}
			return total;
		}


		/// <summary>
		/// Траты за месяц по категории
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public float Purchase_Amount_Category_ByMouth(Category category)
		{
			DateTime today = DateTime.UtcNow;
			DateTime MounthAgo = today.AddMonths(-1);

			return Purchase_Amount_Category(MounthAgo, today, category);
		}

		/// <summary>
		/// Траты за месяц
		/// </summary>
		/// <returns></returns>
		public float Purchase_Amount_ByMouth()
		{
			DateTime today = DateTime.UtcNow;
			DateTime MounthAgo = today.AddMonths(-1);

			return Purchase_Amount(MounthAgo, today);
		}


		public float Accrual_Amount(DateTime begin, DateTime end)
		{
			float total = 0;
			foreach (var purchase in Accruals)
			{
				if (purchase.DateTime < end & purchase.DateTime > begin)
				{
					total += purchase.Price;
				}
			}
			return total;
		}
		public float Total(DateTime begin, DateTime end)
		{
			return Accrual_Amount(begin, end) - Purchase_Amount(begin, end);
		}
		public ApplicationUser()
		{
			Purchase_Limit = 1000;
			Purchases = new List<Purchase>();
			Accruals = new List<Accrual>();
			Login = "";
			foreach (Category cat in Enum.GetValues(typeof(Category)))
			{
				IsAbsolute.Add(false);
			}

		}

		public ApplicationUser(string login)
		{

			Login = login;
            Purchase_Limit = 1000;
            Purchases = new List<Purchase>();
			Accruals = new List<Accrual>();
			foreach (Category cat in Enum.GetValues(typeof(Category)))
			{
				IsAbsolute.Add(false);
			}
		}

	}
}
