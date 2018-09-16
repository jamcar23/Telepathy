// incoming message queue of <connectionId, message>
// (not a HashSet because one connection can have multiple new messages)
using System;

namespace Telepathy
{
    public struct Message : IDisposable
    {
        public int ConnectionId;
        public EventType EventType;
        public byte[] Buffer;
        public ArraySegment<byte> Segment;
        [Obsolete("Use segment instead, and Dispose messages.")]
        public byte[] Data {
            get {
                var array = new byte[Segment.Count - Segment.Offset];
                Array.Copy(Segment.Array, Segment.Offset, array, 0, Segment.Count);
                return array;
            }
        }

        public Message(int connectionId, EventType eventType, byte[] buffer, int size=0)
        {
            this.ConnectionId = connectionId;
            this.EventType = eventType;
            this.Buffer = buffer;
            this.Segment = new ArraySegment<byte>(this.Buffer, 0, size);
        }

        public void Dispose() 
        {
            if(Buffer != null)
                ByteArrayPool.Return(Buffer);
        }
    }
}