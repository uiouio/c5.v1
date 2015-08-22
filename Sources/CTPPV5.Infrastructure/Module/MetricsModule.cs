using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Metrics;
using CTPPV5.Infrastructure.Util;

namespace CTPPV5.Infrastructure.Module
{
    public class MetricsModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var config = Metric.Config.WithHttpEndpoint(
                AppConfig.GetValueFromAppSetting<string>("DashboardUrl", "http://localhost:1234/"));
        }
    }
}
