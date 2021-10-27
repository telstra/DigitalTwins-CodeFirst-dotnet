using System.Runtime.Serialization;
using Telstra.Twins.Enums;

namespace Telstra.Twins.Helpers
{
    public static class TwinStreamingContextHelpers
    {
        public static TwinStreamingContext GetTwinContext(this StreamingContext context) =>
            context.Context is TwinStreamingContext ? (TwinStreamingContext)context.Context : TwinStreamingContext.ADTTwin;
    }
}
