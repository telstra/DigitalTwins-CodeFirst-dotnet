using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.DigitalTwins.Core;
using Telstra.Twins.Responses;

namespace Telstra.Twins
{
    public interface ITwinsClient
    {
        TwinsResponse<List<TwinBase>> Query(string twinId, CancellationToken cancellationToken = default);
        Task<TwinsResponse<List<TwinBase>>> QueryAsync(string twinId, CancellationToken cancellationToken = default);

        #region Twins
        Task<TwinsResponse<TwinBase>> CreateTwinAsync(TwinBase twin, ETag? etag = null, CancellationToken cancellationToken = default);
        TwinsResponse<TwinBase> CreateTwin(TwinBase twin, ETag? etag = null, CancellationToken cancellationToken = default);
        Task<TwinsResponse<TwinBase>> GetTwinAsync(string twinId, CancellationToken cancellationToken = default);
        TwinsResponse<TwinBase> GetTwin(string twinId, CancellationToken cancellationToken = default);
        Task<TwinsResponse> UpdateTwinAsync(string twinId, JsonPatchDocument patch, ETag? etag = null, CancellationToken cancellationToken = default);
        TwinsResponse UpdateTwin(string twinId, JsonPatchDocument patch, ETag? etag = null, CancellationToken cancellationToken = default);
        Task<TwinsResponse> DeleteTwinAsync(string twinId, ETag? etag = null, CancellationToken cancellationToken = default);
        TwinsResponse DeleteTwin(string twinId, ETag? etag = null, CancellationToken cancellationToken = default);
        #endregion // Twins

        Task<TwinsResponse<BasicRelationship>> CreateRelationshipAsync(string sourceId, string targetId, string relationshipName, CancellationToken cancellationToken = default);
        TwinsResponse DeleteRelationship(string twinId, string relationshipId, CancellationToken cancellationToken = default);
        Task<TwinsResponse> DeleteRelationshipAsync(string twinId, string relationshipId, CancellationToken cancellationToken = default);
        TwinsResponse<List<BasicRelationship>> GetRelationships(string twinId, string relationshipName, CancellationToken cancellationToken = default);
        Task<TwinsResponse<List<BasicRelationship>>> GetRelationshipsAsync(string twinId, string relationshipName, CancellationToken cancellationToken = default);
        //Pageable<string> GetIncomingRelationships(string twinId, CancellationToken cancellationToken = default);
        //Task<TwinsResponse<List<string>>> GetIncomingRelationshipsAsync(string twinId, CancellationToken cancellationToken = default);

        Task<TwinsResponse<DigitalTwinsModelData[]>> CreateModelsAsync(List<string> modelDTDL, CancellationToken cancellationToken = default);
        TwinsResponse<DigitalTwinsModelData[]> CreateModels(List<string> modelDTDL, CancellationToken cancellationToken = default);
        Task<TwinsResponse<List<DigitalTwinsModelData>>> GetModelsAsync(List<string> modelID, bool includeModelDefinitions, CancellationToken cancellationToken = default);
        TwinsResponse<List<DigitalTwinsModelData>> GetModels(List<string> modelID, bool includeModelDefinitions, CancellationToken cancellationToken = default);
        Task<TwinsResponse<DigitalTwinsModelData>> GetModelAsync(string modelID, CancellationToken cancellationToken = default);
        TwinsResponse<DigitalTwinsModelData> GetModel(string modelID, CancellationToken cancellationToken = default);
        Task<TwinsResponse> DecommissionModelAsync(string modelID, CancellationToken cancellationToken = default);
        TwinsResponse DecommissionModel(string modelID, CancellationToken cancellationToken = default);
        Task<TwinsResponse> DeleteModelAsync(string modelID, CancellationToken cancellationToken = default);
        TwinsResponse DeleteModel(string modelID, CancellationToken cancellationToken = default);

        Task<TwinsResponse<TwinBase>> GetComponentAsync(string twinId, string componentPath, CancellationToken cancellationToken = default);
        TwinsResponse<TwinBase> GetComponent(string twinId, string componentPath, CancellationToken cancellationToken = default);
        Task<TwinsResponse> UpdateComponentAsync(string twinId, string componentPath, JsonPatchDocument patch, ETag? etag = null, CancellationToken cancellationToken = default);
    
        Task<TwinsResponse> CreateEventRoute(EventRouteInfo eventRouteInfo);
    
    }
}