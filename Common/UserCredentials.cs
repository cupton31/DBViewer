namespace DBViewer.Common
{
	/// <summary>
	/// UserCredentials - represents the user's credentials info.
	/// </summary>
	public struct UserCredentials
	{
		#region Public Members.
		/// <summary>
		/// User.
		/// </summary>
		public string User;
		/// <summary>
		/// Password.
		/// </summary>
		public string Password;
		#endregion  Public Members.

		#region Public Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="user">user</param>
		/// <param name="password">password</param>
		public UserCredentials(string user, string password)
		{
			User = user;
			Password = password;
		}
		#endregion Public Constructor.

		#region Overloaded Operators.
		/// <summary>
		/// Implements an equality operator - reduces performance. 
		/// </summary>
		/// <param name="user1"></param>
		/// <param name="user2"></param>
		/// <returns>true if user1 equals user2</returns>
		public static bool operator==(UserCredentials user1, UserCredentials user2)
		{
			if((user1.User == user2.User) && (user1.Password == user2.Password))
				return true;
			return false;
		}
		/// <summary>
		/// Implements a non equality operator - as equlity operator was overloaded. 
		/// </summary>
		/// <param name="user1"></param>
		/// <param name="user2"></param>
		/// <returns>true if user1 not equals user2</returns>
		public static bool operator!=(UserCredentials user1, UserCredentials user2)
		{
			if(user1 == user2)
				return false;
			return true;
		}

		#endregion Overloaded Operators.

		#region Public Overrided Methods.
		/// <summary>
		/// Implements an equality operation on obj - reduces performance.
		/// </summary>
		/// <param name="obj">obj</param>
		/// <returns>true if obj is of type UserCredentials and equals to current UserCredentials</returns>
		public override bool Equals(object obj)
		{
			try
			{
				UserCredentials user = (UserCredentials)obj;
				if(this == user)
					return true;
				return false;
			}
			catch
			{
				return false;
			}
		}
		/// <summary>
		/// Implements hash code - as equlity operation was overriden.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}
		#endregion  Public Overrided Methods.
	}
}
