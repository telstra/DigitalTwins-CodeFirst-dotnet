using Telstra.Twins.Responses;

namespace Telstra.Twins.API.Models
{
    public class ModelsResponse : TwinsResponse
    {
        public string ModelId { get; }
        public string ModelDesc { get; }
        public string Result { get; }
    }
}
