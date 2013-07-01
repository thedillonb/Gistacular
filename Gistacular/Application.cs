using Gistacular.Data;

namespace Gistacular
{
    /// <summary>
    /// Application.
    /// </summary>
    public static class Application
    {
		public static GitHubSharp.Client Client { get; private set; }
        public static Account Account { get; private set; }
        public static Accounts Accounts { get; private set; }
        public static WebCacheProvider Cache { get; private set; }

        static Application()
        {
            Accounts = new Accounts();
            Cache = new WebCacheProvider();
        }

        public static void SetUser(Account account)
        {
            if (account == null)
            {
                Account = null;
                Client = null;
                Accounts.SetDefault(null);
                return;
            }

            Account = account;
            Accounts.SetDefault(Account);

            Client = new GitHubSharp.Client(Account.Username, Account.Password) {
                Timeout = 1000 * 30, //30 seconds
                CacheProvider = Cache,
            }; 
        }
    }
}

