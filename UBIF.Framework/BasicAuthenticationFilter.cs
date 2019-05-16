using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using UBIF.Framework.Data;
using Newtonsoft.Json;

namespace UBIF.Framework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BasicAuthenticationFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var userIdentity = ParseHeader(actionContext);
            if (userIdentity == null)
            {
                Challenge(actionContext, ResultStatusEnum.BasicError);
                return;
            }
            ResultStatusEnum resultStatusEnum = OnAuthorize(userIdentity.Name, userIdentity.Password, actionContext);
            if (resultStatusEnum != ResultStatusEnum.ok)
            {
                Challenge(actionContext, resultStatusEnum);
                return;
            }

            var principal = new GenericPrincipal(userIdentity, null);

            Thread.CurrentPrincipal = principal;

            base.OnAuthorization(actionContext);
        }
        public virtual BasicAuthenticationIdentity ParseHeader(HttpActionContext actionContext)
        {
            /*
             Basic认证 前端请求的时候需要再header中加入Authorization，这的值为：Basic + " " + 用户名和密码的组合(账号密码可用:隔开)的ase64格式
             */
            string authParameter = null;
            var authValue = actionContext.Request.Headers.Authorization;
            if (authValue != null && authValue.Scheme == "Basic")
            {
                authParameter = authValue.Parameter;
            }
            if (string.IsNullOrEmpty(authParameter))
            {
                return null;
            }
            authParameter = Encoding.Default.GetString(Convert.FromBase64String(authParameter));
            var authToken = authParameter.Split(':');
            if (authToken.Length < 2)
            {
                return null;
            }
            return new BasicAuthenticationIdentity(authToken[0], authToken[1]);
        }
        void Challenge(HttpActionContext actionContext, ResultStatusEnum resultStatusEnum)
        {
            ResultModel resultModel = new ResultModel();
            switch (resultStatusEnum)
            {
                case ResultStatusEnum.ok:
                    resultModel.status = "1";
                    resultModel.message = "成功";
                    break;
                case ResultStatusEnum.BasicError:
                    resultModel.status = "0";
                    resultModel.message = "Basic错误";
                    break;
                case ResultStatusEnum.passwordError:
                    resultModel.status = "0";
                    resultModel.message = "密码错误";
                    break;
                case ResultStatusEnum.passwordNullError:
                    resultModel.status = "0";
                    resultModel.message = "密码为空";
                    break;
                case ResultStatusEnum.userNameError:
                    resultModel.status = "0";
                    resultModel.message = "用户名为空";
                    break;
                case ResultStatusEnum.userNameNullError:
                    resultModel.status = "0";
                    resultModel.message = "用户名不存在";
                    break;
                case ResultStatusEnum.verificationCodeError:
                    resultModel.status = "0";
                    resultModel.message = "验证码错误";
                    break;
                case ResultStatusEnum.verificationCodeNullError:
                    resultModel.status = "0";
                    resultModel.message = "验证码为空";
                    break;
            }
            var host = actionContext.Request.RequestUri.DnsSafeHost;
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
            actionContext.Response.Content = new StringContent(JsonConvert.SerializeObject(resultModel));
        }
        public virtual ResultStatusEnum OnAuthorize(string userName, string userPassword, HttpActionContext actionContext)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return ResultStatusEnum.userNameNullError;
            }
            if (string.IsNullOrWhiteSpace(userPassword))
            {
                return ResultStatusEnum.passwordNullError;
            }
            return ResultStatusEnum.ok;
        }
    }
}