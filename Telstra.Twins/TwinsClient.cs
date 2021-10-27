using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.DigitalTwins.Core;
using Telstra.Twins.Responses;

namespace Telstra.Twins
{
    public abstract class TwinsClient : ITwinsClient
    {
        #region Twins
        public abstract Task<TwinsResponse<TwinBase>> CreateTwinAsync(TwinBase twin, ETag? etag = null, CancellationToken cancellationToken = default);
        public abstract TwinsResponse<TwinBase> CreateTwin(TwinBase twin, ETag? etag = null, CancellationToken cancellationToken = default);
        public abstract TwinsResponse<List<TwinBase>> Query(string query, CancellationToken cancellationToken = default);
        public abstract Task<TwinsResponse<List<TwinBase>>> QueryAsync(string query, CancellationToken cancellationToken = default);
        public abstract Task<TwinsResponse<TwinBase>> GetTwinAsync(string twinId, CancellationToken cancellationToken = default);
        public abstract TwinsResponse<TwinBase> GetTwin(string twinId, CancellationToken cancellationToken = default);
        public abstract Task<TwinsResponse> UpdateTwinAsync(string twinId, JsonPatchDocument patch, ETag? etag = null, CancellationToken cancellationToken = default);
        public abstract TwinsResponse UpdateTwin(string twinId, JsonPatchDocument patch, ETag? etag = null, CancellationToken cancellationToken = default);
        public abstract Task<TwinsResponse> DeleteTwinAsync(string twinId, ETag? etag = null,
            CancellationToken cancellationToken = default);
        public abstract TwinsResponse DeleteTwin(string twinId, ETag? etag = null,
            CancellationToken cancellationToken = default);
        #endregion // Twins
        public abstract Task<TwinsResponse<BasicRelationship>> CreateRelationshipAsync(string sourceId, string targetId, string relationshipName, CancellationToken cancellationToken = default);
        public abstract TwinsResponse DeleteRelationship(string twinId, string relationshipId, CancellationToken cancellationToken = default);
        public abstract Task<TwinsResponse> DeleteRelationshipAsync(string twinId, string relationshipId, CancellationToken cancellationToken = default);

        public abstract Task<TwinsResponse<List<BasicRelationship>>> GetRelationshipsAsync(string twinId, string relationshipName, CancellationToken cancellationToken = default);
        public abstract TwinsResponse<List<BasicRelationship>> GetRelationships(string twinId, string relationshipName, CancellationToken cancellationToken = default);
        //public abstract TwinsResponse<List<string>> GetIncomingRelationships(string twinId, CancellationToken cancellationToken = default);
        //public abstract AsyncTwinsResponse<List<string>> GetIncomingRelationshipsAsync(string twinId, CancellationToken cancellationToken = default);

        public abstract Task<TwinsResponse<DigitalTwinsModelData[]>> CreateModelsAsync(List<string> modelDTDL, CancellationToken cancellationToken = default);
        public abstract TwinsResponse<DigitalTwinsModelData[]> CreateModels(List<string> modelDTDL, CancellationToken cancellationToken = default);
        public abstract Task<TwinsResponse<List<DigitalTwinsModelData>>> GetModelsAsync(List<string> modelIDs, bool includeModelDefinitions, CancellationToken cancellationToken = default);
        public abstract TwinsResponse<List<DigitalTwinsModelData>> GetModels(List<string> modelIDs, bool includeModelDefinitions, CancellationToken cancellationToken = default);
        public abstract Task<TwinsResponse> DecommissionModelAsync(string modelID, CancellationToken cancellationToken = default);
        public abstract TwinsResponse DecommissionModel(string modelID, CancellationToken cancellationToken = default);
        public abstract Task<TwinsResponse> DeleteModelAsync(string modelID, CancellationToken cancellationToken = default);
        public abstract TwinsResponse DeleteModel(string modelID, CancellationToken cancellationToken = default);
        public abstract Task<TwinsResponse<DigitalTwinsModelData>> GetModelAsync(string modelID, CancellationToken cancellationToken = default);
        public abstract TwinsResponse<DigitalTwinsModelData> GetModel(string modelID, CancellationToken cancellationToken = default);

        public abstract Task<TwinsResponse<TwinBase>> GetComponentAsync(string twinId, string componentPath, CancellationToken cancellationToken = default);
        public abstract TwinsResponse<TwinBase> GetComponent(string twinId, string componentPath, CancellationToken cancellationToken = default);

        public abstract Task<TwinsResponse> UpdateComponentAsync(string twinId, string componentPath, JsonPatchDocument patch, ETag? etag = null,
            CancellationToken cancellationToken = default);

        public abstract Task<TwinsResponse> CreateEventRoute(EventRouteInfo eventRouteInfo);
    }
}