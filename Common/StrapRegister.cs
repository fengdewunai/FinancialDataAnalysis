using System;

namespace Common
{
    using System.IO;
    using log4net.Config;
    using Microsoft.Practices.ServiceLocation;
    using RuanYun.IoC;

    public static class StrapRegister
    {
        /// <summary>
        /// 注册调用的主入口
        /// </summary>
        /// <param name="cfg">其他注册，在正常的注册完成后，会执行该方法，做其他的注册</param>
        public static void Bootstrap(Action<BootstrapConfig> cfg = null)
        {
            var config = new BootstrapConfig();
            cfg?.Invoke(config);
            LibraryAutoRegister.InitializeAutoMapper(autoMapperCfg =>
            {
                config.AutoMapperConfigurator?.Invoke(autoMapperCfg);
            });
        }

        /// <summary>
        /// 日志的注册方法
        /// </summary>
        /// <param name="logConfig">日志的配置文件信息</param>
        public static void UseLog4Net(FileInfo logConfig)
        {
            XmlConfigurator.ConfigureAndWatch(logConfig);
        }
    }

    /// <summary>
    /// 启动引导配置
    /// </summary>
    public class BootstrapConfig
    {
        internal BootstrapConfig()
        {
        }

        /// <summary>
        /// Ioc注册
        /// </summary>
        public IRegistration IocRegistration { get; internal set; }

        /// <summary>
        /// AutoMapper类型注册
        /// </summary>
        public Action<MapperTypeConfig> AutoMapperConfigurator;
    }
}
