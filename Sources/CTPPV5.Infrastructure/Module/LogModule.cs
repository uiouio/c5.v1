using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using log4net;

namespace CTPPV5.Infrastructure.Module
{
    public class LogModule : Autofac.Module
    {
        private static void InjectLoggerProperties(object instance)
        {
            var instanceType = instance.GetType();

            // Get all the injectable properties to set.
            // If you wanted to ensure the properties were only UNSET properties,
            // here's where you'd do it.
            var properties = instanceType
              .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
              .Where(p => p.PropertyType == typeof(CTPPV5.Infrastructure.Log.ILog) && p.CanWrite && p.GetIndexParameters().Length == 0);

            // Set the properties located.
            foreach (var propToSet in properties)
            {
                propToSet.SetValue(instance, new CTPPV5.Infrastructure.Log.Log4NetAdapter(LogManager.GetLogger(instanceType)), null);
            }
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            var t = e.Component.Activator.LimitType;
            e.Parameters = e.Parameters.Union(
              new[]
              {
                  new ResolvedParameter((p, i) =>
                      p.ParameterType == typeof(CTPPV5.Infrastructure.Log.ILog), 
                      (p, i) => new CTPPV5.Infrastructure.Log.Log4NetAdapter(LogManager.GetLogger(t))),
              });
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            // Handle constructor parameters.
            registration.Preparing += OnComponentPreparing;

            // Handle properties.
            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }
    }
}
