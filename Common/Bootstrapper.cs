using Microsoft.Practices.ServiceLocation;
using RuanYun.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Bootstrapper
    {
        /// <summary>
        /// 注册调用的主入口
        /// </summary>
        /// <param name="moreMap">其他注册，在正常的注册完成后，会执行该方法，做其他的注册</param>
        public static void Bootstrap(Action<IRegistration> moreMap = null)
        {
            var locator = new StructureMapServiceLocator();
            locator.UseAsDefault();
            locator.Map(() => ServiceLocator.Current);
            locator.Logger.Log("Configuring Registries");

            locator.Map<IRegistration>(() => locator);
            locator.ScanAssembly().ExtensionRegister();
            if (moreMap != null)
                moreMap(locator);
            locator.Logger.Log("Loading container...");
            locator.Load();
            locator.Logger.Log("Done");
        }
    }
}
