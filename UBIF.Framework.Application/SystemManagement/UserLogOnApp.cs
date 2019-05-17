#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：UBIF.Framework.Application.SystemManagement
* 项目描述 ：
* 类 名 称 ：UserLogOnApp
* 类 描 述 ：
* 所在的域 ：DESKTOP-O4LCU7F
* 命名空间 ：UBIF.Framework.Application.SystemManagement
* 机器名称 ：DESKTOP-O4LCU7F 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：welus
* 创建时间 ：2019/5/17 13:35:25
* 更新时间 ：2019/5/17 13:35:25
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
using UBIF.Framework.Domain.IRepository;
using UBIF.Framework.Repository.SystemManagement;
using UBIF.Framework.Data;
namespace UBIF.Framework.Application.SystemManagement
{
    public class UserLogOnApp
    {
        private IUserLogOnRepository service = new UserLogOnRepository();
        public Sys_UserLogOn GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public int UpdateForm(Sys_UserLogOn sys_UserLogOn)
        {
          return service.Update(sys_UserLogOn);
        }
    }
}
