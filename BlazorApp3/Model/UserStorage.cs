using Microsoft.Extensions.Diagnostics.HealthChecks;
using BlazorApp3.Model.Categories;
using BlazorApp3.Model.Operation;

namespace BlazorApp3.Model
{
	static public  class UserStorage
	{
		private static  List<ApplicationUser> _users = new();
		private static ApplicationUser _currentUser;

		public static ApplicationUser CurrentUser {  get { return _currentUser; }  set { _currentUser = value; } }
		public static List<ApplicationUser>  Users { get { return _users; }  set { _users = value; } }

		public static Purchase currentPurchase { get; set; }

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

			if (!relatively)
			{
				T = (user.Purchase_Amount_Category_ByMouth(category) - Average(category)) / Standard_Deviation(category);
			}
			else
			{
				T = (user.Purchase_Amount_Category_ByMouth(category) / user.Purchase_Amount_ByMouth() - Average_Relatively(category)) / Standard_Deviation_Relatively(category);
			}
			

			switch (T)
			{
				case (< -T_gr) when Importance_Category(category) > 0:
					return "Вы тратите слишком мало для данной категории";
				case (> T_gr) when Importance_Category(category) < 0:
					return "Вы тратите слишком много для данной категории";
				default:
					return "Траты в пределах нормы";
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

			foreach (var user in Users)
			{
				if (user.Purchase_Amount_Category_ByMouth(category) != 0)
				{
					average += user.Purchase_Amount_Category_ByMouth(category);
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

			foreach (var user in Users)
			{
				if (user.Purchase_Amount_Category_ByMouth(category) != 0)
				{
					average += user.Purchase_Amount_Category_ByMouth(category) / user.Purchase_Amount_ByMouth();
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

			i--;

			foreach (var user in Users)
			{
				if (user.Purchase_Amount_Category_ByMouth(category) != 0)
				{
					S += (float)Math.Pow(user.Purchase_Amount_Category_ByMouth(category) - average, 2);
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

			i--;

			foreach (var user in Users)
			{
				if (user.Purchase_Amount_Category_ByMouth(category) != 0)
				{
					S += (float)Math.Pow(user.Purchase_Amount_Category_ByMouth(category) / user.Purchase_Amount_ByMouth() - average, 2);
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
