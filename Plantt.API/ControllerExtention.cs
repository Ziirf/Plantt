using Microsoft.AspNetCore.Mvc;
using Plantt.Domain.Entities;
using Plantt.Domain.Exceptions;
using System.Security.Authentication;

namespace Plantt.API
{
    public class ControllerExtention : ControllerBase
    {
        protected AccountEntity GetAccountFromHttpContext()
        {
            if (!HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                throw new AuthenticationException("No Accesstoken was found.");
            }

            AccountEntity? account = HttpContext.Items["account"] as AccountEntity;

            if (account is null)
            {
                throw new AccountNotFoundException("The Account associated with the JWT was not found");
            }

            return account;
        }
    }
}
