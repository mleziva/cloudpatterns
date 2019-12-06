using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudPatterns.State
{
    public class SessionState : ISessionState
    {
        private readonly ISession session;
        public SessionState(IHttpContextAccessor accessor)
        {
            this.session = accessor.HttpContext.Session;
            //https://stackoverflow.com/questions/43201763/asp-net-core-mvc-loading-session-asynchronously
            session.LoadAsync();
        }
        public T Get<T>(string key)
        {
            return session.GetObject<T>(key);
        }

        public void Set(string key, object value)
        {
            session.SetObject(key, value);
        }
    }
}
