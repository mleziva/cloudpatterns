using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudPatterns.State
{
    public interface ISessionState
    {
        T Get<T>(string key);
        void Set(string key, object value);
    }
}
