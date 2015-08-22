using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CTPPV5.Infrastructure.Util
{
    public sealed class AppConfig
    {
        /// <summary>
        /// 从AppSetting获取值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回值</returns>
        public static T GetValueFromAppSetting<T>(string key, T defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            else
            {
                if (typeof(T) == typeof(int))
                {
                    int result;
                    if (int.TryParse(value, out result))
                        return (T)(object)result;
                    else
                        throw new ArgumentException(
                            string.Format("AppSetting中参数设置不符合规范，无法转换为{2}类型。参数名:{0} 值:{1}",
                                key, value, typeof(T).ToString()),
                            key);
                }
                else if (typeof(T) == typeof(bool))
                {
                    bool result;
                    if (bool.TryParse(value, out result))
                        return (T)(object)result;
                    else
                        throw new ArgumentException(
                            string.Format("AppSetting中参数设置不符合规范，无法转换为{2}类型。参数名:{0} 值:{1}",
                                key, value, typeof(T).ToString()),
                            key);
                }
                else if (typeof(T) == typeof(string))
                {
                    return (T)(object)value;
                }
                else if (typeof(T) == typeof(float))
                {
                    float result;
                    if (float.TryParse(value, out result))
                        return (T)(object)result;
                    else
                        throw new ArgumentException(
                            string.Format("AppSetting中参数设置不符合规范，无法转换为{2}类型。参数名:{0} 值:{1}",
                                key, value, typeof(T).ToString()),
                            key);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
