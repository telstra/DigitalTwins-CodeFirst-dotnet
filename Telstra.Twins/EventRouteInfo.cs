using System;

namespace Telstra.Twins
{
    public sealed class EventRouteInfo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string RouteName { get; set; }

        public string EndPointName { get; set; }

        public string Filter { get; set; }

    }

}
