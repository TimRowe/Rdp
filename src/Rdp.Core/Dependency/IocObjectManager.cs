using Rdp.Core.Data;

namespace Rdp.Core.Dependency
{
    public class IocObjectManager 
    {
        private IIocLifetimeScope _iocLifetimeScope;
        private static IocObjectManager _manage;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rdpLifetimeScope"></param>
        public IocObjectManager(IIocLifetimeScope rdpLifetimeScope)
        {
            _iocLifetimeScope = rdpLifetimeScope;
        }

        /// <summary>
        /// 获取单例对象
        /// </summary>
        /// <returns></returns>
        public static IocObjectManager GetInstance()
        {
            return _manage;
        }

        /// <summary>
        /// 设置实体
        /// </summary>
        /// <param name="o"></param>
        public static void SetInstance(IocObjectManager o)
        {
            _manage = o;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return _iocLifetimeScope.Resolve<T>();
        }

        
    }
}


