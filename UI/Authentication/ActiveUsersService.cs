using System.Collections.Concurrent;

namespace UI.Authentication
{
    public class ActiveUsersService
    {
        private static readonly ConcurrentDictionary<string, DateTime> ActiveUsers = new();

        public void AddUser(string username)
        {
            ActiveUsers[username] = DateTime.UtcNow;
        }

        public void RemoveUser(string username)
        {
            ActiveUsers.TryRemove(username, out _);
        }

        public int GetActiveUserCount()
        {
            return ActiveUsers.Count;
        }
    }
}