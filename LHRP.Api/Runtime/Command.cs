using System.Collections.Generic;

namespace LHRP.Api.Runtime
{
    public class Command
    {
        protected IDictionary<string, object> _commandData = new Dictionary<string, object>();

        public Command(string name)
        {
            Name = name;
        }
        
        public string Name { get; protected set; }

        public void SetValue(string key, object value)
        {
            _commandData[key] = value;
        }
        public object GetValue(string key)
        {
            return _commandData[key];
        }
    }
}