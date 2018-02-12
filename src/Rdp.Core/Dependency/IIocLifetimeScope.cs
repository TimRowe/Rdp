namespace Rdp.Core.Dependency
{
    public interface IIocLifetimeScope
    {
        /// <summary>
        /// 处理基于IDependency接口的注入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>(LifetimeScopeEnum scope);
    }

}

