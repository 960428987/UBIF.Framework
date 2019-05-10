#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：UBIF.Framework.Model
* 项目描述 ：
* 类 名 称 ：ResultStatusEnum
* 类 描 述 ：
* 所在的域 ：DESKTOP-O4LCU7F
* 命名空间 ：UBIF.Framework.Model
* 机器名称 ：DESKTOP-O4LCU7F 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：welus
* 创建时间 ：2019/5/10 17:55:44
* 更新时间 ：2019/5/10 17:55:44
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

namespace UBIF.Framework.Model
{
    public enum ResultStatusEnum
    {
        ok = 1,
        verificationCodeError = 2,//验证码错误
        userNameError = 3,//用户名错误
        passwordError = 4,//密码错误
        verificationCodeNullError = 5,//验证码为空
        userNameNullError = 6,//用户名为空
        passwordNullError = 7,//密码为空
        BasicError = 8,//密码为空
    }
}
