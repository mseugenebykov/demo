using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR.Identity
{
    public class ApiUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

    public class ApiUserRecord : ApiUser
    {
        public string Password { get; set; }

        public static ApiUser CreateUser(ApiUserRecord record)
        {
            return new ApiUser()
            {
                Id = record.Id,
                Username = record.Username
            };
        }
    }
}
