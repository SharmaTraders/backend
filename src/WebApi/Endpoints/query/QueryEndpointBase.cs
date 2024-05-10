using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.query;

public static class QueryEndpointBase {
    public static class WithRequest<TRequest> where TRequest : notnull {

        public abstract class WithResponse<TResponse> : EndpointBase where TResponse : notnull {
            public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request);
            
        }
    }

    public static class WithoutRequest {
        public abstract class WithResponse<TResponse> : EndpointBase where TResponse : notnull {
            public abstract Task<ActionResult<TResponse>> HandleAsync();
            
        }
    }
}