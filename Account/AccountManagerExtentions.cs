using Speckle.Core.Credentials;
using Speckle.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeckleExtensions
{
    public static class AccountManagerExtentions
    {
        public static async Task<Account> GetAccount(string token, string url)
        {
            var userInfo = await AccountManager.GetUserInfo(token, url);
            var serverInfo = await AccountManager.GetServerInfo(url);
            var account = new Account()
            {
                token = token,
                serverInfo = serverInfo,
                userInfo = userInfo,
            };
            return account;
        }

        /// <summary>
        /// Add a local account if it doesn't exist. Return true if it wrote the file. 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool AddLocalAccount(Account account)
        {
            var exists = GetLocalAccounts().Any(x => x.Equals(account));
            if (!exists)
            {
                var localAccountPath = LocalAccountPath;
                if (!Directory.Exists(localAccountPath))
                    Directory.CreateDirectory(localAccountPath);

                var data = JsonConvert.SerializeObject(account);
                File.WriteAllText(Path.Combine(localAccountPath, $"{account.id}.json"), data);
                return true;
            }
            return false;
        }

        private static string LocalAccountPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Speckle", "Accounts");
            }
        }

        /// <summary>
        /// Gets the local accounts
        /// These are accounts not handled by Manager and are stored in json format in a local directory
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Account> GetLocalAccounts()
        {
            var accounts = new List<Account>();
            var accountsDir = LocalAccountPath;
            if (!Directory.Exists(accountsDir))
            {
                return accounts;
            }
            var files = Directory.GetFiles(accountsDir, "*.json", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                try
                {
                    var json = File.ReadAllText(file);
                    var account = JsonConvert.DeserializeObject<Account>(json);

                    if (account.IsValid())
                        accounts.Add(account);
                }
                catch
                { //ignore it
                }
            }
            return accounts;
        }
    }
}
