﻿using Rdp.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Rdp.Web.Framework.Core;

namespace Rdp.Web.Framework.Runtime
{
    ///// <summary>
    ///// 自定义的Razor引擎，为了在mvc中使用多级目录
    ///// </summary>
    //public class IocLifetimeScope : IIocLifetimeScope
    //{
    //    public T Resolve<T>(LifetimeScopeEnum scope) 
    //    {
    //        var container = IocContainerManager.GetInstance();
    //        return container.GetService<T>();
    //    }
    //}
}
