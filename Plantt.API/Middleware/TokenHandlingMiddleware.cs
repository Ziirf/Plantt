using Microsoft.AspNetCore.Authorization;
using Plantt.API.Constants;
using Plantt.Domain.Exceptions;
using Plantt.Domain.Interfaces;
using System.Security.Claims;

namespace Plantt.API.Middleware
{
    /*
     * This middleware is used to get the entity stored in the JWT into the HTTPContext, so that we can
     * use this entity for validating and verifying. This will bring the repeated code to a minimum.
     */
    public class TokenHandlingMiddleware : IMiddleware
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TokenHandlingMiddleware> _logger;

        public TokenHandlingMiddleware(
            IUnitOfWork unitOfWork,
            ILogger<TokenHandlingMiddleware> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Check if there is any policy on the endpoint we are trying to reach.
                if (GetPolicyFromHttpContext(context) is string policy)
                {
                    // Get the identifier from the JWT, as a string, if non were found, throw and exception.
                    string? identifier = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (identifier is null)
                    {
                        throw new JWTIdentifierNotFoundException("Token didn't have an Identifier.");
                    }

                    // Depending on which context were found, we need to handle it differently.
                    switch (policy)
                    {
                        case AuthorizePolicyConstant.Registered:
                            await AddAccountToContext(context, identifier);
                            break;

                        case AuthorizePolicyConstant.Premium:
                            await AddAccountToContext(context, identifier);
                            break;

                        case AuthorizePolicyConstant.Hub:
                            await AddHubToContext(context, identifier);
                            break;
                    }
                }

                await next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
        }

        private string? GetPolicyFromHttpContext(HttpContext context)
        {
            if (context.GetEndpoint() is Endpoint endpoint)
            {
                var authorizeData = endpoint.Metadata.GetMetadata<IAuthorizeData>();

                return authorizeData?.Policy;
            }

            return null;
        }

        private async Task AddAccountToContext(HttpContext context, string identifier)
        {
            // Make sure that the public id is in fact a guid, and get it from the database.
            if (Guid.TryParse(identifier, out Guid accountGuid))
            {
                context.Items["account"] = await _unitOfWork.AccountRepository.GetByGuidAsync(accountGuid);
            }

            // Log it if it fails, this shouldn't happen.
            if (context.Items["account"] is null)
            {
                _logger.LogCritical("Failed to find account for the account {accountGuid}", identifier);
            }
        }

        private async Task AddHubToContext(HttpContext context, string identifier)
        {
            // Get the hub from the database.
            context.Items["hub"] = await _unitOfWork.HubRepository.GetHubByIdentityAsync(identifier);

            // Log it if it fails, this shouldn't happen.
            if (context.Items["hub"] is null)
            {
                _logger.LogCritical("Failed to find a hub with identifiere: {identifier}", identifier);
            }
        }
    }
}
