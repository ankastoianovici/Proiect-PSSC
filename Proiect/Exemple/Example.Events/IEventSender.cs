using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Events
{
    public interface IEventSender
    {
        public static void Main(string[] args)
        {
        }
        TryAsync<Unit> SendAsync<T>(string topicName, T @event);
    }
}
