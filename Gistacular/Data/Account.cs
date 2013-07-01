using SQLite;

namespace Gistacular.Data
{
    public class Account
    {
		[PrimaryKey]
		[AutoIncrement]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
        public string Username { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
        public string Password { get; set; }

		/// <summary>
		/// Gets or sets the avatar URL.
		/// </summary>
		/// <value>The avatar URL.</value>
        public string AvatarUrl { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Account"/> class.
		/// </summary>
		public Account()
		{
		}
		
		/// <summary>
		/// Delete this instance in the database
		/// </summary>
		public void Delete()
		{
			Database.Main.Delete(this);
		}
		
		/// <summary>
		/// Update this instance in the database
		/// </summary>
		public void Update()
		{
			Database.Main.Update(this);
		}
		
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Account"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Account"/>.</returns>
		public override string ToString()
		{
			return Username;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Account"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Account"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Account"/>; otherwise, <c>false</c>.</returns>
		public override bool Equals (object obj)
		{
		    if (ReferenceEquals(null, obj)) return false;
		    if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Account)) return false;
            var act = (Account)obj;
            return this.Id.Equals(act.Id);
		}

        /// <summary>
        /// Serves as a hash function for a <see cref="Gistacular.Data.Account"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode ()
        {
            return this.Id;
        }
    }
}

