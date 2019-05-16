using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using System.Web.Http;
using UBIF.Framework.Code;
using UBIF.Framework.Data;
namespace UBIF.Framework.Controllers
{
    /// <summary>
    /// 登录接口
    /// </summary>
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAuthCode")]
        public HttpResponseMessage GetAuthCode()
        {
           // ResultModel resultModel = new ResultModel();
            var data = new VerifyCode().GetVerifyCode();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            //resultModel.status = ResultType.success.ToString();
           // resultModel.message = "Api调用成功";
           // resultModel.content = new ByteArrayContent(data);  //data为二进制图片数据
            response.Content = new ByteArrayContent(data);  //data为二进制图片数据
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/Gif");
            return response;
        }
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
        //[Route("Login")]
        //[HttpGet]
        //public HttpResponseMessage Login(string username, string password)
        //{
        //    ResultModel resultModel = new ResultModel();
        //    var model = GetLoginModel(username, password);
        //}

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckLogin")]
        public HttpResponseMessage CheckLogin(string userAccount, string password, string code)
        {
            password = "4a7d1ed414474e4033ac29ccb8653d9b";
            ResultModel result = new ResultModel();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            try
            {
                if(string.IsNullOrWhiteSpace(userAccount))
                {
                    throw new Exception("用户名错误，请重新输入");
                }
                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new Exception("密码错误，请重新输入");
                }
                if (HttpContext.Current.Session["ubif_session_verifycode"].IsEmpty() || Md5.md5(code.ToLower(), 16) != HttpContext.Current.Session["ubif_session_verifycode"].ToString())
                {
                    throw new Exception("验证码错误，请重新输入");
                }
                using (UbifBaseEntities context = new UbifBaseEntities())
                {
                    Sys_User userModel = context.Sys_User.Where(t=>t.F_Account == userAccount).FirstOrDefault();//这个表存的用户名
                    if(userModel != null)
                    {
                        if(userModel.F_EnabledMark==true)
                        {
                            Sys_UserLogOn UserLogOnModel = context.Sys_UserLogOn.Where(t => t.F_UserId == userModel.F_Id).FirstOrDefault();//这个表存的登录密码
                           //string pw = Md5.md5(password.ToLower(), 16);//这个要前端加密，传过来，这句是前端写的
                            //UserLogOnModel.F_UserSecretkey  这个是什么意思？
                            string dbPassword = Md5.md5(DESEncrypt.Encrypt(password.ToLower(), UserLogOnModel.F_UserSecretkey).ToLower(), 32).ToLower();
                            if (dbPassword == UserLogOnModel.F_UserPassword)
                            {
                                DateTime lastVisitTime = DateTime.Now;
                                int LogOnCount = (UserLogOnModel.F_LogOnCount).ToInt() + 1;
                                if (UserLogOnModel.F_LastVisitTime != null)
                                {
                                    UserLogOnModel.F_PreviousVisitTime = UserLogOnModel.F_LastVisitTime.ToDate();
                                }
                                UserLogOnModel.F_LastVisitTime = lastVisitTime;
                                UserLogOnModel.F_LogOnCount = LogOnCount;
                                //通过反射，遍历出值是空的字段，空的值不修改
                                PropertyInfo[] props = UserLogOnModel.GetType().GetProperties();
                                foreach (PropertyInfo prop in props)
                                {
                                    if (prop.GetValue(UserLogOnModel, null) != null)
                                    {
                                        if (prop.GetValue(UserLogOnModel, null).ToString() == "&nbsp;")
                                        {
                                            context.Entry(UserLogOnModel).Property(prop.Name).CurrentValue = null;
                                        }
                                        context.Entry(UserLogOnModel).Property(prop.Name).IsModified = true;
                                    }
                                }
                                context.SaveChanges();
                               // result.status = ResultType.success.ToString();

                            }
                            else
                            {
                                throw new Exception("密码不正确，请重新输入");
                            }
                        }
                        else
                        {
                            throw new Exception("账户被系统锁定,请联系管理员");
                        }
                    }
                    else
                    {
                        throw new Exception("账户不存在，请重新输入");
                    }

                    Sys_Log sys_Log = new Sys_Log();
                    sys_Log.F_ModuleName = "系统登录";
                    sys_Log.F_Type = DbLogType.Login.ToString();
                }
            }
            catch (Exception e)
            {

              
            }
            result.status = "1";
            result.message = "Api调用成功";
            httpResponseMessage.StatusCode = HttpStatusCode.OK;
            httpResponseMessage.Content = new StringContent(result.ToJson(), System.Text.Encoding.UTF8, "application/json"); ;
            return httpResponseMessage;
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void CheckLogin(string username,string password)
        {

        }
    }
}
