
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command;

public static class CommandEndPointBase {
    public static class WithRequest<TRequest> {
        public abstract class WithResponse<TResponse> : EndpointBase {
            public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request);
        }

        public abstract class WithoutResponse : EndpointBase {
            public abstract Task<ActionResult> HandleAsync(TRequest request);

        }
    }

    public static class WithoutRequest {
        public abstract class WithResponse<TResponse> : EndpointBase {
            public abstract Task<ActionResult<TResponse>> HandleAsync();
        }

        public abstract class WithoutResponse : EndpointBase {
            public abstract Task<ActionResult> HandleAsync();
        }
    }
}