using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR.Identity
{
    public interface IApiUserService
    {
        Task<ApiUser> Authenticate(string username, string password);
    }

    public class ApiUserService : IApiUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private static readonly List<ApiUserRecord> users = new List<ApiUserRecord>
        {
            new ApiUserRecord { Id = 1, Username = "demo", Password = "password" }
        };

        public async Task<ApiUser> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => users.SingleOrDefault(x => x.Username == username && x.Password == password));
            return (user == null) ? null : ApiUserRecord.CreateUser(user);
        }
    }
}
