using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Client.Winform.Events
{
    public class ModelEventArgs : EventArgs
    {
        private object model;
        static ModelEventArgs args = new ModelEventArgs(null);
        public ModelEventArgs(object model)
        {
            this.model = model;
        }

        public TModel GetModel<TModel>()
        {
            return (TModel)model;
        }

        public static ModelEventArgs Instance { get { return args; } }
    }
}
