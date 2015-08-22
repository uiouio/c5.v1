using CTPPV5.Infrastructure.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CTPPV5.Rpc.Serial
{
    public class SerialPortConfig
    {
        private string portName;
        private int? baudRate;

        public string PortName
        {
            get
            {
                if (string.IsNullOrEmpty(portName))
                    portName = AppConfig.GetValueFromAppSetting<string>("PortName", "COM1");
                return portName;
            }
            set { portName = value; }
        }

        public int BaudRate
        {
            get
            {
                if (baudRate == null)
                    baudRate = AppConfig.GetValueFromAppSetting<int>("BaudRate", 9600);
                return baudRate.Value;
            }
            set { baudRate = value; }
        }

        public void Save(string portName, int baudRate)
        {
            var conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.AppSettings.Settings["PortName"].Value = portName;
            conf.AppSettings.Settings["baudRate"].Value = baudRate.ToString();
            conf.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
