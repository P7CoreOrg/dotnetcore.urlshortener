using System;

namespace dotnetcore.urlshortener.contracts
{
    public class EventSource<T> where T : class
    {
        private event EventHandler<T> Handler;
        public void FireEvent(T evt)
        {
            Handler?.Invoke(this, evt);
        }
        public void AddListenter(EventHandler<T> handler)
        {
            Handler += handler;
        }

        public void RemoveListenter(EventHandler<T> handler)
        {
            Handler -= handler;
        }
    }
}