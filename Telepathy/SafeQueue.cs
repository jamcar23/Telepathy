// replaces ConcurrentQueue which is not available in .NET 3.5 yet.
using System.Collections.Generic;

namespace Telepathy
{
    public class SafeQueue<T>
    {
        Queue<T> _queue = new Queue<T>();

        // for statistics. don't call Count and assume that it's the same after the
        // call.
        public int Count
        {
            get
            {
                lock(_queue)
                {
                    return _queue.Count;
                }
            }
        }

        public void Enqueue(T item)
        {
            lock(_queue)
            {
                _queue.Enqueue(item);
            }
        }

        // can't check .Count before doing Dequeue because it might change inbetween,
        // so we need a TryDequeue
        public bool TryDequeue(out T result)
        {
            lock(_queue)
            {
                result = default(T);
                if (_queue.Count > 0)
                {
                    result = _queue.Dequeue();
                    return true;
                }
                return false;
            }
        }

        public void Clear()
        {
            lock(_queue)
            {
                _queue.Clear();
            }
        }
    }
}