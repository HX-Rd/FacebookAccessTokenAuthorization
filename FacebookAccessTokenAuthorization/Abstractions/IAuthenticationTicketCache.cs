using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXRd.Facebook.AccessTokenAuthorization
{
    public interface IAuthenticationTicketCache
    {
        AuthenticationTicket GetTicket(object key);
        AuthenticationTicket SetTicket(object key, AuthenticationTicket ticket, DateTimeOffset expires);
    }
}
