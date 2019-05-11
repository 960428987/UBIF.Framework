using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UBIF.Framework.Model;
namespace UBIF.Framework.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        /// <summary>
        /// 登录认证接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("LoginOn")]
        [LoginBasicAuthenticationFilter]
        public string LoginOn()
        {
            return "1111";
        }
        [Route("Login")]
        [HttpGet]
        public HttpResponseMessage Login(string username, string password)
        {
            ResultModel resultModel = new ResultModel();
            var model = GetLoginModel(username, password);
        }
    }
}
