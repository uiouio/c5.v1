using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Models;
using CTPPV5.Models.CommandModel;
using CTPPV5.Rpc;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.Repository;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.CommandServer.Command
{
    public class Register : AbstractCommandExecutor
    {
        private IMetaRepository metaRepository;
        public Register(IMetaRepository metaRepository)
        {
            this.metaRepository = metaRepository;
        }

        protected override DuplexMessage DoExecute(DuplexMessage commandMessage)
        {
            DuplexMessage resultMessage = null;
            var school = metaRepository
                .GetAllSchools()
                .ByIdentifier(commandMessage.Header.Identifier);

            if (school != null)
            {
                if (UpdateRegisterInfo(school, commandMessage))
                    resultMessage = DuplexMessage.CreateAckMessage(commandMessage);
                else
                   resultMessage = DuplexMessage.CreateCallbackMessage(commandMessage, ErrorCode.RegisterFailed);
            }
            else
                resultMessage = DuplexMessage.CreateCallbackMessage(commandMessage, ErrorCode.RegisterFailed);

            return resultMessage;
        }

        bool UpdateRegisterInfo(School school, DuplexMessage commandMessage)
        {
            var registerOk = false;
            var registerInfo = commandMessage.GetContent<RegisterInfo>();
            if (registerInfo != null)
            {
                school.ClientPubKey = registerInfo.ClientPubKey;
                school.UniqueToken = registerInfo.ClientMacAddr;
                metaRepository.AddOrUpdateSchool(school, (updateOk) =>
                {
                    DuplexMessage resultMessage;
                    if (updateOk)
                        resultMessage = DuplexMessage.CreateCallbackMessage(commandMessage);
                    else
                        resultMessage = DuplexMessage.CreateCallbackMessage(commandMessage, ErrorCode.RegisterFailed);

                    Return(resultMessage);
                });
                registerOk = true;
            }
            return registerOk;
        }
    }
}
