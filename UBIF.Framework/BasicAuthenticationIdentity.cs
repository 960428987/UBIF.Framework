using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace UBIF.Framework
{
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public string Password { get; set; }
        public BasicAuthenticationIdentity(string name, string password) : base(name, "Basic")
        {
            this.Password = password;
        }
    }
}