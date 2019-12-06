using CloudPatterns.State;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudPatterns.Tests.Mocks
{
    public class MockSessionState : ISessionState
    {
        private Dictionary<string, object> internalStore = new Dictionary<string, object>();
        public T Get<T>(string key)
        {
            if (internalStore.TryGetValue(key, out var value)){
                return (T)value;
            }
            return default;
        }

        public void Set(string key, object value)
        {
            if (internalStore.ContainsKey(key))
            {
                internalStore[key] = value;
            }
            else
            {
                internalStore.Add(key, value);
            }
        }
    }
}
