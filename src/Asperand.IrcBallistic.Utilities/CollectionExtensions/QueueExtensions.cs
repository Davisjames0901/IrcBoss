using System.Collections.Generic;

namespace Asperand.IrcBallistic.Utilities.CollectionExtensions
{
    public static class QueueExtensions
    {
        public static void AddButDontExceed<T>(this Queue<T> queue, T item, int maxSize)
        {
            queue.Enqueue(item);
            if (queue.Count > maxSize)
                queue.Dequeue();
        }
    }
}