using Mina.Core.Filterchain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Core.Session;
using Metrics;

namespace CTPPV5.Rpc.Net.Server.Filter
{
    public class SessionMetricsFilter : IoFilterAdapter
    {
        public override void SessionCreated(INextFilter nextFilter, IoSession session)
        {
            Metric.Gauge("01.message.read.count", () => session.Service.Statistics.ReadMessages, Unit.Requests);
            Metric.Gauge("02.message.read.bytes", () => session.Service.Statistics.ReadBytes / 1024 / 1024, Unit.MegaBytes);
            Metric.Gauge("03.message.read.throughput", () => session.Service.Statistics.ReadMessagesThroughput, Unit.Requests);
            Metric.Gauge("04.message.read.throughput.max", () => session.Service.Statistics.LargestReadMessagesThroughput, Unit.Requests);
            Metric.Gauge("05.message.read.bytes.throughput", () => session.Service.Statistics.ReadBytesThroughput / 1024, Unit.KiloBytes);
            Metric.Gauge("06.message.read.bytes.throughput.max", () => session.Service.Statistics.LargestReadBytesThroughput / 1024, Unit.KiloBytes);
            Metric.Gauge("07.message.write.count", () => session.Service.Statistics.WrittenMessages, Unit.Results);
            Metric.Gauge("08.message.write.bytes", () => session.Service.Statistics.WrittenBytes / 1024 / 1024, Unit.MegaBytes);
            Metric.Gauge("09.message.write.throughput", () => session.Service.Statistics.WrittenMessagesThroughput, Unit.Results);
            Metric.Gauge("10.message.write.throughput.max", () => session.Service.Statistics.LargestWrittenMessagesThroughput, Unit.Requests);
            Metric.Gauge("11.message.write.bytes.throughput", () => session.Service.Statistics.WrittenBytesThroughput / 1024, Unit.KiloBytes);
            Metric.Gauge("12.message.write.bytes.throughput.max", () => session.Service.Statistics.LargestWrittenBytesThroughput / 1024, Unit.KiloBytes);
            Metric.Gauge("13.message.write.queue.count", () => session.Service.Statistics.ScheduledWriteMessages, Unit.Results);
            Metric.Gauge("14.message.write.queue.bytes", () => session.Service.Statistics.ScheduledWriteBytes / 1024, Unit.KiloBytes);
            base.SessionCreated(nextFilter, session);
        }
    }
}
