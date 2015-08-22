using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Infrastructure.Module;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Rpc.Net.Message.Filter;
using CTPPV5.Rpc.Net.Message.Serializer;

namespace CTPPV5.Rpc.Net
{
    public class RpcNetModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new LogModule());
            builder.RegisterModule(new SecurityModule());
            builder.Register(c => new GZipFilter()).Keyed<IMessageFilter>(ZipMode.GZip).SingleInstance();
            builder.Register(c => new RSACryptoFilter()).Keyed<IMessageFilter>(CryptoMode.RSA).SingleInstance();
            builder.Register(c => new Crc32Filter()).Keyed<IMessageFilter>(ChecksumMode.Crc32).SingleInstance();
            builder.Register(c => new MessageFilterFactory()).AsSelf().SingleInstance();
            builder.Register(c => new Json()).Keyed<ISerializer>(SerializeMode.Json).SingleInstance();
            builder.Register(c => new Protobuf()).Keyed<ISerializer>(SerializeMode.Protobuf).SingleInstance();
            builder.Register(c => new ExceptionCounterFilter()).AsSelf().SingleInstance();
            builder.Register(c => new CommandHandler(c.Resolve<TimeoutNotifyProducerConsumer<AbstractAsyncCommand>>())).AsSelf().SingleInstance();
            builder.Register(c => new DuplexMessageWriterImplV1()).Keyed<IMessageWriter<DuplexMessage>>(MessageVersion.V1).SingleInstance();
            builder.Register(c => new DuplexMessageReaderImplV1()).Keyed<IMessageReader<DuplexMessage>>(MessageVersion.V1).SingleInstance();
            builder.RegisterType<TimeoutNotifyProducerConsumer<AbstractAsyncCommand>>()
                .WithParameter(new NamedParameter("capacity", AbstractAsyncCommand.BLOCK_UNTIL_TIMEOUT_QUEUE_CAPACITY))
                .AsSelf()
                .SingleInstance()
                .OnActivated(o =>
                {
                    o.Instance.OnConsume += (sender, e) =>
                    {
                        var consumingItem = e.ConsumingItem as ChunkedConsumingItem<AbstractAsyncCommand>;
                        if (consumingItem == null) return;
                        foreach (var item in consumingItem.Chunk)
                        {
                            try
                            {
                                item.RunAsync();
                            }
                            catch (Exception) { }
                        }
                    };
                });
        }
    }
}
