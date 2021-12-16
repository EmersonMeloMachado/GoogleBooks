using System;
using System.Collections.Generic;

namespace GoogleBooks.Model.Navigation
{
    public class NavigationParameters : Dictionary<string, object>
    {
        public NavigationParameters() { }

        public NavigationParameters(string key, object value)
        {
            Add(key, value);
        }

        public T GetValue<T>(string parameterName)
        {
            if (!ContainsKey(parameterName))
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }

            return (T)Convert.ChangeType(this[parameterName], typeof(T));
        }
    }
}
