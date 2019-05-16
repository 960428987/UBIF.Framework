using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using UBIF.Framework.Data;

namespace UBIF.Framework
{
    public class LoginBasicAuthenticationFilter : BasicAuthenticationFilter
    {
        public override ResultStatusEnum OnAuthorize(string userName, string userPassword, HttpActionContext actionContext)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return ResultStatusEnum.userNameNullError;
            }
            if (string.IsNullOrWhiteSpace(userPassword))
            {
                return ResultStatusEnum.passwordNullError;
            }
            if ("xpy0928" != userName)
            {
                return ResultStatusEnum.userNameError;
            }
            if ("cnblogs" != userPassword)
            {
                return ResultStatusEnum.passwordError;
            }
            return ResultStatusEnum.ok;
        }
    }
}