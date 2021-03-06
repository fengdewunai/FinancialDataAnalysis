﻿using AutoMapper;
using Model.data;
using RuanYun.IoC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 依赖类库的自动注入逻辑类
    /// </summary>
    public static class LibraryAutoRegister
    {
        /// <summary>
        /// 针对一些自动注入无法进行扫码的接口注入
        /// 以下情况需要额外进行注入
        /// 1. 接口的名字和实现类的名字出现差异（包括大小写）的情况
        /// 2. 一个实现类实现了多个接口
        /// </summary>
        /// <param name="registration">IOC注册对象</param>
        /// <returns>IOC注册后的对象</returns>
        public static IRegistration ExtensionRegister(this IRegistration registration)
        {
            return registration;
        }

        /// <summary>
        /// 扫码引用的程序集
        /// </summary>
        /// <param name="registration">IOC注册对象</param>
        /// <returns>IOC注册后的对象</returns>
        public static IRegistration ScanAssembly(this IRegistration registration)
        {
            var registry = registration.CreateRegistry();
            var assemblyNames = GetAllAssemblyFiles();
            for (int i = 0; i < assemblyNames.Count; i++)
            {
                var referencedAssemblyName = assemblyNames[i];
                if (referencedAssemblyName.Contains("Model"))
                {
                    continue;
                }
                var referencedAssembly = Assembly.Load(referencedAssemblyName);

                registry.Scan(
                    with =>
                    {
                        with.Assembly(referencedAssembly.FullName);
                        with.WithDefaultConventions();
                        with.SingleImplementationsOfInterface();
                    });
            }
            registration.Register(registry);
            return registration;
        }

        /// <summary>
        /// 扫码bin目录获取所有依赖的dll
        /// </summary>
        /// <returns>所有依赖的类库信息</returns>
        private static List<string> GetAllAssemblyFiles()
        {
            string path = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
            if (string.IsNullOrEmpty(path))
            {
                path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
            return
                Directory.GetFiles(path, "*.dll")
                    .Where(x=>x.ToLower().Contains("business") || x.ToLower().Contains("ibusiness") || x.ToLower().Contains("dataacess") || x.ToLower().Contains("idataacess"))
                    .Select(Path.GetFileNameWithoutExtension)
                    .ToList();
        }

        /// <summary>
        /// Creates the automatic mapper.
        /// </summary>
        /// <param name="autoMapperConfig">外部注册</param>
        public static void InitializeAutoMapper(Action<MapperTypeConfig> autoMapperConfig = null)
        {
            MapperProvider.Initialize((MapperTypeConfig cfg) =>
            {
                autoMapperConfig?.Invoke(cfg);
            },
            (IMapperConfigurationExpression cfg) =>
            {
            });
        }
    }
}
