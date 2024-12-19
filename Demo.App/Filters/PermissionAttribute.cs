using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Net;

namespace Demo.App.Filters
{
    public class PermissionAttribute: Attribute, IAuthorizationFilter
    {
        private readonly string[] _permission;
        /// <param name="permissions"></param>

        public PermissionAttribute(params string[] permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentUser= HelperUtility
        }


    }
}
