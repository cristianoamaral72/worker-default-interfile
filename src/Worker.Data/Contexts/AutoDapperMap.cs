using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dapper.FluentMap.Dommel.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Worker.Data.Contexts
{
    public static class AutoDapperMap
    {
        private static IEnumerable<Type> Mappers { get; set; }

        public static void Register()
        {
            FluentMapper.EntityMaps.Clear();

            if (Mappers == null)
            {
                Mappers = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.DeclaringType != null && t.DeclaringType.BaseType != null && t.DeclaringType.BaseType.IsGenericType)
                    .Select(t => t.DeclaringType.BaseType)
                    .Where(x => x.GetGenericTypeDefinition() == typeof(DommelEntityMap<>))
                    .SelectMany(c => c.GetGenericArguments());
            }

            if (FluentMapper.EntityMaps == null || !FluentMapper.EntityMaps.Any())
            {
                FluentMapper.Initialize(config =>
                {
                    IEnumerable<Type> mappedClasses = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(x => x.GetInterfaces()
                                        .SelectMany(y => y.GenericTypeArguments)
                                        .Any(z => Mappers.Contains(z))
                                    && !x.Name.Contains("Repository")
                        );

                    foreach (Type type in mappedClasses)
                    {
                        dynamic configurationInstance = Activator.CreateInstance(type);
                        config.AddMap(configurationInstance);
                    }

                    config.ForDommel();
                });
            }
        }
    }
}
