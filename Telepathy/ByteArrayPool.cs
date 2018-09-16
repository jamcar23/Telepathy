// common code used by server and client
using System;
using System.Collections.Generic;

namespace Telepathy
{
    /// <summary>
    /// Byte array pool allows recycling of arrays in order to minimise memory allocation.
    /// </summary>
    public static class ByteArrayPool
    {
        [ThreadStatic] private static readonly Queue<byte[]> ArrayPool;

        static ByteArrayPool()
        {
            ArrayPool = new Queue<byte[]>();
        }

        /// <summary>
        /// Take an array from the pool.
        /// </summary>
        /// <returns>The array.</returns>
        public static byte[] Take()
        {
            return ArrayPool.Count == 0 ? new byte[ushort.MaxValue] : ArrayPool.Dequeue();
        }

        /// <summary>
        /// Return an array to the pool.
        /// </summary>
        /// <param name="bytes">The array.</param>
        public static void Return(byte[] bytes)
        {
            if (bytes.Length != ushort.MaxValue)
            {
                throw new System.ArgumentException("incorrect length", nameof(bytes));

            }
            ArrayPool.Enqueue(bytes);
        }

    }
}
