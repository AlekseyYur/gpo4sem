﻿using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BlazorApp3.Model
{
	static public  class UserStorage
	{
		private static  List<ApplicationUser> _users = new List<ApplicationUser> ();
		public static List<ApplicationUser>  Users { get { return _users; }  set { _users = value; } }
		
		 
	}
}
