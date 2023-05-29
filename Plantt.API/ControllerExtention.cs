using Microsoft.AspNetCore.Mvc;
using Plantt.Domain.Entities;
using Plantt.Domain.Exceptions;
using System.Security.Authentication;

namespace Plantt.API
{
    public class ControllerExtention : ControllerBase
    {
        /// <summary>
        /// Retrieves the account associated with the JWT in the current HTTP context.
        /// </summary>
        /// <returns>The <see cref="AccountEntity"/> representing the account associated with the JWT in the request.</returns>
        /// <exception cref="AuthenticationException">Thrown if no access token is found in the request headers.</exception>
        /// <exception cref="JWTIdentifierNotFoundException">Thrown if the account associated with the JWT is not found.</exception>
        protected AccountEntity GetAccountFromHttpContext()
        {
            if (!HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                throw new AuthenticationException("No Accesstoken was found.");
            }

            AccountEntity? account = HttpContext.Items["account"] as AccountEntity;

            if (account is null)
            {
                throw new JWTIdentifierNotFoundException("The Account associated with the JWT was not found");
            }

            return account;
        }

        /// <summary>
        /// Retrieves the hub associated with the JWT in the current HTTP context.
        /// </summary>
        /// <returns>The <see cref="HubEntity"/> representing the hub associated with the JWT in the request.</returns>
        /// <exception cref="AuthenticationException">Thrown if no access token is found in the request headers.</exception>
        /// <exception cref="JWTIdentifierNotFoundException">Thrown if the hub associated with the JWT is not found.</exception>
        protected HubEntity GetHubFromHttpContext()
        {
            if (!HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                throw new AuthenticationException("No Accesstoken was found.");
            }

            HubEntity? hub = HttpContext.Items["hub"] as HubEntity;

            if (hub is null)
            {
                throw new JWTIdentifierNotFoundException("The Hub associated with the JWT was not found");
            }

            return hub;
        }
    }
}
