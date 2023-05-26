using Microsoft.AspNetCore.Authorization;
using Plantt.API.Constants;
using Plantt.Domain.Interfaces;
using System.Security.Claims;

namespace Plantt.API.Middleware
{
    public class AccountFromTokenMiddleware : IMiddleware
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountFromTokenMiddleware> _logger;

        private readonly string[] _rolesThatRequireAccountList =
        {
            AuthorizePolicyConstant.Registered,
            AuthorizePolicyConstant.Premium
        };

        public AccountFromTokenMiddleware(
            IUnitOfWork unitOfWork,
            ILogger<AccountFromTokenMiddleware> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (context.GetEndpoint() is Endpoint endpoint)
                {
                    var authorizeData = endpoint.Metadata.GetMetadata<IAuthorizeData>();

                    if (authorizeData is not null && _rolesThatRequireAccountList.Contains(authorizeData.Policy))
                    {
                        var accountSubject = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                        if (Guid.TryParse(accountSubject, out Guid accountGuid))
                        {
                            context.Items["account"] = await _unitOfWork.AccountRepository.GetByGuidAsync(accountGuid);
                        }

                        if (context.Items["account"] is null)
                        {
                            _logger.LogCritical("Failed to find account for the account {accountGuid}", accountSubject);
                        }
                    }
                }

                await next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
        }
    }
}
