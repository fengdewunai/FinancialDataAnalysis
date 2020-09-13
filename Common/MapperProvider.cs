using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    using global::AutoMapper;
    using System.Collections;
    using System.Collections.Concurrent;


    /// <summary>
    /// 实体映射
    /// 把A对象转换成B对象的帮助类
    /// </summary>
    public static class MapperProvider
    {
        /// <summary>
        /// 全局锁对象，添加AutoMapper映射关系时不运行并发，通过静态方式添加
        /// </summary>
        private readonly static object _lock = new object();

        /// <summary>
        /// mapper
        /// </summary>
        private static IMapper _mapper = null;

        /// <summary>
        /// 已初始化的类型映射
        /// </summary>
        private static readonly ConcurrentDictionary<MapperTypePair, object> _initializedMapper =
            new ConcurrentDictionary<MapperTypePair, object>();

        /// <summary>
        /// Initializes the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <exception cref="ArgumentNullException">config</exception>
        public static void Initialize(Action<MapperTypeConfig> config = null, Action<IMapperConfigurationExpression> expressionConfig = null)
        {
            var c = new MapperTypeConfig();
            if (config != null)
                config(c);
            var configuration = new MapperConfiguration(cfg =>
            {
                if (expressionConfig != null)
                    expressionConfig(cfg);
                foreach (var mapperType in c.MapperTypes)
                {
                    CreateMappers(cfg, mapperType.SourceType, mapperType.DestinationType);
                    if (mapperType.Reversal)
                    {
                        CreateMappers(cfg, mapperType.DestinationType, mapperType.SourceType);
                    }
                }
            });
            _mapper = configuration.CreateMapper();
        }

        /// <summary>
        ///  类型映射
        /// </summary>
        /// <remarks>
        /// <![CDATA[
        ///     备注:把A对象转换成User类型的对象
        ///     用法：A.MapTo<User>();
        /// ]]>
        /// </remarks>
        public static T MapTo<T>(this object obj)
        {
            if (obj == null)
                return default(T);
            if(_mapper == null)
            {
                throw new Exception("使用AutoMapper前请先执行MapperProvider.Initialize");
            }
            return _mapper.Map<T>(obj);
        }

        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <remarks>
        /// <![CDATA[
        ///     备注:把A集合转换成User类型的集合
        ///     用法：A.MapToList<User>();
        /// ]]>
        /// </remarks>
        public static List<TDestination> MapToList<TDestination>(this IEnumerable source)
        {
            if (source == null)
            {
                return new List<TDestination>();
            }

            var result = new List<TDestination>();
            foreach (var item in source)
            {
                result.Add(MapTo<TDestination>(item));
            }

            return result;
        }

        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        /// <remarks>
        /// <![CDATA[
        ///     备注:把A集合转换成TDestination类型的集合
        ///     用法：A.MapToList<TSource,TDestination>(IEnumerable<TSource>);
        /// ]]>
        /// </remarks>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            return MapToList<TDestination>(source);
        }

        /// <summary>
        /// 创建Mapper
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CreateMappers(IMapperConfigurationExpression cfg, Type source, Type destination)
        {
            lock (_lock)
            {
                var mapperPair = new MapperTypePair(source, destination);
                if (_initializedMapper.ContainsKey(mapperPair))
                {
                    return;
                }
                cfg.CreateMap(source, destination, MemberList.Source);
                _initializedMapper.TryAdd(mapperPair, null);
                var sourceInfo = source.GetProperties();
                var destinationInfo = destination.GetProperties();
                var members = sourceInfo.Join(destinationInfo, m => m.Name.ToLower(), n => n.Name.ToLower(),
                    (m, n) => new { From = m, To = n }).ToList();
                foreach (var member in members)
                {
                    var sourceType = member.From.PropertyType;
                    var destinationType = member.To.PropertyType;
                    if (destinationType.IsGenericType)
                    {
                        var sourceGenerityTypes = sourceType.GetGenericArguments().ToList();
                        var destinationGenerityTypes = destinationType.GetGenericArguments().ToList();
                        for (var i = 0; i < Math.Min(sourceGenerityTypes.Count, destinationGenerityTypes.Count); i++)
                        {
                            var sourceGenerityType = sourceGenerityTypes[i];
                            var destinationGenerityType = destinationGenerityTypes[i];
                            if (destinationGenerityType != null && destinationGenerityType.FullName != null &&
                                destinationGenerityType.FullName.StartsWith("System."))
                            {
                                continue;
                            }

                            CreateMappers(cfg, sourceGenerityType, destinationGenerityType);
                        }
                    }
                    else
                    {
                        if (destinationType.FullName != null && destinationType.FullName.StartsWith("System."))
                        {
                            continue;
                        }

                        CreateMappers(cfg, sourceType, destinationType);
                    }
                }
            }
        }
    }


    /// <summary>
    /// Class MapperTypeConfig.
    /// </summary>
    public class MapperTypeConfig
    {
        /// <summary>
        /// mappers
        /// </summary>
        private IList<MapperTypePair> _mappers = null;

        /// <summary>
        /// Gets the mapper types.
        /// </summary>
        /// <value>The mapper types.</value>
        internal IEnumerable<MapperTypePair> MapperTypes
        {
            get { return _mappers; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapperTypeConfig" /> class.
        /// </summary>
        internal MapperTypeConfig()
        {
            _mappers = new List<MapperTypePair>();
        }

        /// <summary>
        /// Creates the map.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <param name="reversal"></param>
        public void CreateMap(Type sourceType, Type destinationType, bool reversal = false)
        {
            _mappers.Add(new MapperTypePair(sourceType, destinationType, reversal));
        }
    }

    internal class MapperTypePair
    {
        /// <summary>
        /// 构建MapperTypePair
        /// </summary>
        /// <param name="sourceType">源类型</param>
        /// <param name="destinationType">目标类型</param>
        /// <param name="reversal">是否进行翻转</param>
        internal MapperTypePair(Type sourceType, Type destinationType, bool reversal = false)
        {
            if (sourceType == null)
                new ArgumentNullException("sourceType");
            if (destinationType == null)
                new ArgumentNullException("destinationType");
            SourceType = sourceType;
            DestinationType = destinationType;
            Reversal = reversal;
        }

        /// <summary>
        /// 源类型
        /// </summary>
        public Type SourceType { get; set; }

        /// <summary>
        /// 目标类型
        /// </summary>
        public Type DestinationType { get; set; }

        /// <summary>
        /// 是否进行翻转
        /// </summary>
        public bool Reversal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return SourceType.GetHashCode() ^ DestinationType.GetHashCode();
        }

        /// <summary>
        /// 比较两个实例是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var pair = obj as MapperTypePair;
            if (pair == null)
                return false;

            return pair.SourceType.Equals(this.SourceType) && pair.DestinationType.Equals(this.DestinationType);
        }
    }
}
