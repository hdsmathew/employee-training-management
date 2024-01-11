using Core.Application.Models;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
public class CustomAuthorizeAttribute : AuthorizeAttribute
{
    private readonly IEnumerable<AccountTypeEnum> _requiredRoles;

    public CustomAuthorizeAttribute(params AccountTypeEnum[] requiredRoles)
    {
        _requiredRoles = requiredRoles.ToList();
    }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        return httpContext.Session["AuthenticatedUser"] is AuthenticatedUser authenticatedUser
            && _requiredRoles.Any(role => role.Equals(authenticatedUser.AccountType));
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectToRouteResult(
            new System.Web.Routing.RouteValueDictionary
            {
                { "controller", "Error" },
                { "action", "Forbidden" }
            });
    }
}
