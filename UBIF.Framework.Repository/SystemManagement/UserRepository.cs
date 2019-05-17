#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：UBIF.Framework.Repository.SystemManagement
* 项目描述 ：
* 类 名 称 ：UserRepository
* 类 描 述 ：
* 所在的域 ：DESKTOP-O4LCU7F
* 命名空间 ：UBIF.Framework.Repository.SystemManagement
* 机器名称 ：DESKTOP-O4LCU7F 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：welus
* 创建时间 ：2019/5/17 13:49:33
* 更新时间 ：2019/5/17 13:49:33
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
using UBIF.Framework.Data;
using UBIF.Framework.Domain.IRepository.SystemManagement;

namespace UBIF.Framework.Repository.SystemManagement
{
   public class UserRepository:RepositoryBase<Sys_User>, IUserRepository
    {

    }
}
