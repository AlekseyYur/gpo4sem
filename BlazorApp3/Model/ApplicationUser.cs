using BlazorApp3.Model.Operation;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp3.Model
{
    public class ApplicationUser
	{
		private List<Purchase> _purchases;
		private List<Accrual> _accruals;
		public List<Purchase> Purchases { get { return _purchases; } set { _purchases = value; } }
		public List<Accrual> Accruals { get { return _accruals; } set { _accruals = value; } }
		private float _purchase_limit;


		[Required]
		public string Login { get; set; }

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
		public float total(DateTime begin, DateTime end)
		{
			return Accrual_Amount(begin, end) - Purchase_Amount(begin, end);
		}
		public ApplicationUser()
		{
			Purchase_Limit = 1000;
			Purchases = new List<Purchase>();
			Accruals = new List<Accrual>();
			Login = "";

		}

		public ApplicationUser(string login)
		{

			Login = login;
            Purchase_Limit = 1000;
            Purchases = new List<Purchase>();
			Accruals = new List<Accrual>();
		}

	}
}
