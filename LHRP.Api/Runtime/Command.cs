using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Runtime
{
    public class Command
    {
        protected IDictionary<string, object> _commandData = new Dictionary<string, object>();

        public Command(string name)
        {
            Name = name;
        }
        
        public string Name 
        { 
            get
            {
                return _commandData["Name"].ToString();
            }
            protected set
            {
                _commandData["Name"] = value;
            }
        }

        public void SetValue(string key, object value)
        {
            _commandData[key] = value;
        }
        public object GetValue(string key)
        {
            return _commandData[key];
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            foreach(var data in _commandData)
            {
                strBuilder.Append(data.Key + ":" + data.Value.ToString() + ", ");
            } 

            return strBuilder.ToString();
        }
    }
}