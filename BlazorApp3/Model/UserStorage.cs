using Microsoft.Extensions.Diagnostics.HealthChecks;
using BlazorApp3.Model.Categories;

namespace BlazorApp3.Model
{
	static public  class UserStorage
	{
		private static  List<ApplicationUser> _users = new();
		public static List<ApplicationUser>  Users { get { return _users; }  set { _users = value; } }

		/// <summary>
		/// Проверяет траты пользователя по данной категории и сообщает, если траты выходят за рамки
		/// </summary>
		/// <param name="user">Проверяемый пользователь</param>
		/// <param name="category">Категория трат</param>
		/// <returns>Предупреждающее сообщение</returns>
		public static string Deviation_Message(ApplicationUser user, Category category, bool relatively)
		{
			// Нормированное отклонение
			double T = 0;

			// Коофицент допускаемых нормированных отклонений
			const double T_gr = 2.5;

			DateTime today = DateTime.Today;
			DateTime MounthAgo = today.AddMonths(-1);

			if (!relatively)
			{
				T = (user.Purchase_Amount_Category(MounthAgo, today, category) - Average(category)) / Standard_Deviation(category);
			}
			else
			{
				T = (user.Purchase_Amount_Category(MounthAgo, today, category) / user.Purchase_Amount(MounthAgo, today) - Average_Relatively(category)) / Standard_Deviation_Relatively(category);
			}
			

			switch (T)
			{
				case (< -T_gr) when Importance_Category(category) > 0:
					return "You are spending too little on this category";
				case (> T_gr) when Importance_Category(category) < 0:
					return "You are spending too much on this category";
				default:
					return "";
			}
		}

		/// <summary>
		/// Возвращает средние траты всех пользователей по категориям за последний месяц
		/// </summary>
		/// <param name="category">категория трат</param>
		/// <returns>Среднее значение</returns>
		public static float Average(Category category)
		{
			float average = 0;
			float i = 0;

			DateTime today = DateTime.Today;
			DateTime MounthAgo = today.AddMonths(-1);

			foreach (var user in Users)
			{
				if (user.Purchase_Amount_Category(MounthAgo, today, category) != 0)
				{
					average += user.Purchase_Amount_Category(MounthAgo, today, category);
					i++;
				}
			}

			return average / i;
		}

		/// <summary>
		/// Возвращает средние траты всех пользователей по категориям за последний месяц относительно всех трат
		/// </summary>
		/// <param name="category">категория трат</param>
		/// <returns>Среднее значение</returns>
		public static float Average_Relatively(Category category)
		{
			float average = 0;
			float i = 0;

			DateTime today = DateTime.Today;
			DateTime MounthAgo = today.AddMonths(-1);

			foreach (var user in Users)
			{
				if (user.Purchase_Amount_Category(MounthAgo, today, category) != 0)
				{
					average += user.Purchase_Amount_Category(MounthAgo, today, category) / user.Purchase_Amount(MounthAgo, today);
					i++;
				}
			}

			return average / i;
		}

		/// <summary>
		/// Возвращает среднеквадратичное отклонение трат пользователей по категории
		/// </summary>
		/// <param name="category">Категория трат</param>
		/// <returns>Среднеквадратичное отклонение</returns>
		public static float Standard_Deviation(Category category)
		{
			int i = 0;
			float S = 0;
			float average = Average(category);

			DateTime today = DateTime.Today;
			DateTime MounthAgo = today.AddMonths(-1);

			i--;

			foreach (var user in Users)
			{
				if (user.Purchase_Amount_Category(MounthAgo, today, category) != 0)
				{
					S += (float)Math.Pow(user.Purchase_Amount_Category(MounthAgo, today, category) - average, 2);
					i++;
				}
			}

			return (float)Math.Sqrt(S) / i;
		}

		/// <summary>
		/// Возвращает среднеквадратичное отклонение трат пользователей по категории относительно всех трат
		/// </summary>
		/// <param name="category">Категория трат</param>
		/// <returns>Среднеквадратичное отклонение</returns>
		public static float Standard_Deviation_Relatively(Category category)
		{
			int i = 0;
			float S = 0;
			float average = Average_Relatively(category);

			DateTime today = DateTime.Today;
			DateTime MounthAgo = today.AddMonths(-1);

			i--;

			foreach (var user in Users)
			{
				if (user.Purchase_Amount_Category(MounthAgo, today, category) != 0)
				{
					S += (float)Math.Pow(user.Purchase_Amount_Category(MounthAgo, today, category) / user.Purchase_Amount(MounthAgo, today) - average, 2);
					i++;
				}
			}

			return (float)Math.Sqrt(S) / i;
		}

		/// <summary>
		/// Возвращает важность категории 
		/// </summary>
		/// <param name="category">Категория</param>
		/// <returns>Важность: больше - важнее</returns>
		public static int Importance_Category(Category category)
		{
			switch (category) 
			{
				case Category.sport:
					return 0;
				case Category.medicine:
					return 1;
				case Category.clothing:
					return -1;
				case Category.supermarket:
					return 0;
				default:
					return 0;
			}
		}




	}
}
