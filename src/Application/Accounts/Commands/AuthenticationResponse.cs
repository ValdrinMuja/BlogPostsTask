using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Accounts.Commands
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string AccessToken { get; set; }
    }
}
