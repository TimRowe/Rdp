namespace Rdp.Core.Dependency
{
    /// <summary>
    /// dependency injection
    /// </summary>
    public interface IDIProvider
    {
        /// <summary>
        /// 处理基于IDependency接口的注入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();
    }

}

