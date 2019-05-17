#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：UBIF.Framework.Application.SystemManagement
* 项目描述 ：
* 类 名 称 ：LogApp
* 类 描 述 ：
* 所在的域 ：DESKTOP-O4LCU7F
* 命名空间 ：UBIF.Framework.Application.SystemManagement
* 机器名称 ：DESKTOP-O4LCU7F 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：welus
* 创建时间 ：2019/5/17 14:37:27
* 更新时间 ：2019/5/17 14:37:27
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ welus 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBIF.Framework.Code;
using UBIF.Framework.Data;
using UBIF.Framework.Domain.IRepository.SystemManagement;
using UBIF.Framework.Repository.SystemManagement;

namespace UBIF.Framework.Application.SystemManagement
{
   public class LogApp
    {
        private ILogRepository service = new LogRepository();

        public List<Sys_Log> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<Sys_Log>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_Account.Contains(keyword));
            }
            if (!queryParam["timeType"].IsEmpty())
            {
                string timeType = queryParam["timeType"].ToString();
                DateTime startTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate();
                DateTime endTime = DateTime.Now.ToString("yyyy-MM-dd").ToDate().AddDays(1);
                switch (timeType)
                {
                    case "1":
                        break;
                    case "2":
                        startTime = DateTime.Now.AddDays(-7);
                        break;
                    case "3":
                        startTime = DateTime.Now.AddMonths(-1);
                        break;
                    case "4":
                        startTime = DateTime.Now.AddMonths(-3);
                        break;
                    default:
                        break;
                }
                expression = expression.And(t => t.F_Date >= startTime && t.F_Date <= endTime);
            }
            return service.FindList(expression, pagination);
        }
        public void RemoveLog(string keepTime)
        {
            DateTime operateTime = DateTime.Now;
            if (keepTime == "7")            //保留近一周
            {
                operateTime = DateTime.Now.AddDays(-7);
            }
            else if (keepTime == "1")       //保留近一个月
            {
                operateTime = DateTime.Now.AddMonths(-1);
            }
            else if (keepTime == "3")       //保留近三个月
            {
                operateTime = DateTime.Now.AddMonths(-3);
            }
            var expression = ExtLinq.True<Sys_Log>();
            expression = expression.And(t => t.F_Date <= operateTime);
            service.Delete(expression);
        }
        public void WriteDbLog(bool result, string resultLog)
        {
            Sys_Log logEntity = new Sys_Log();
            logEntity.F_Id = Common.GuId();
            logEntity.F_Date = DateTime.Now;
            logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            logEntity.F_IPAddress = Net.Ip;
            logEntity.F_IPAddressName = Net.GetLocation(logEntity.F_IPAddress);
            logEntity.F_Result = result;
            logEntity.F_Description = resultLog;
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            if (LoginInfo != null)
            {
                logEntity.F_CreatorUserId = LoginInfo.UserId;
            }
            logEntity.F_CreatorTime = DateTime.Now;
            service.Insert(logEntity);
        }
        public void WriteDbLog(Sys_Log logEntity)
        {
            logEntity.F_Id = Common.GuId();
            logEntity.F_Date = DateTime.Now;
            logEntity.F_IPAddress = Net.Ip;
            logEntity.F_IPAddressName = Net.GetLocation(logEntity.F_IPAddress);
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            if (LoginInfo != null)
            {
                logEntity.F_CreatorUserId = LoginInfo.UserId;
            }
            logEntity.F_CreatorTime = DateTime.Now;
            service.Insert(logEntity);
        }
    }
}
