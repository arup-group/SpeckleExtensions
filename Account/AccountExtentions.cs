using Speckle.Core.Credentials;

namespace SpeckleExtensions
{
    public static class AccountExtentions
    {
        /// <summary>
        /// Simply checks important properties are not NullOrEmpty
        /// </summary>
        /// <returns></returns>
        public static bool IsValid(this Account account)
        {
            return !string.IsNullOrEmpty(account.token) &&
                  !string.IsNullOrEmpty(account.userInfo.id) &&
                  !string.IsNullOrEmpty(account.userInfo.email) &&
                  !string.IsNullOrEmpty(account.userInfo.name) &&
                  !string.IsNullOrEmpty(account.serverInfo.url) &&
                  !string.IsNullOrEmpty(account.serverInfo.name);
        }
    }

}
