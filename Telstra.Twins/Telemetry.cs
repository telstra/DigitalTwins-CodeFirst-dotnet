using System;

namespace Telstra.Twins
{
    public class Telemetry<T>
    {
        public T Value { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public void Update(T value)
        {
            this.Value = value;
            this.Timestamp = DateTimeOffset.UtcNow;
        }
    }
}