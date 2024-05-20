using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BlazorApp3.Model
{
	static public  class UserStorage
	{
		private static  List<ApplicationUser> _users = new();
		private static ApplicationUser _currentUser;

		public static ApplicationUser CurrentUser {  get { return _currentUser; }  set { _currentUser = value; } }
		public static List<ApplicationUser>  Users { get { return _users; }  set { _users = value; } }
		
	}
}
