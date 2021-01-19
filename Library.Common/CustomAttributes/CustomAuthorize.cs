using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.Core.CustomAttributes
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(params string[] claim) : base(typeof(AuthorizeFilter))
        {
            Arguments = new object[] { claim };
        }

        //public AuthorizeAttribute(string claimType, string claimValue) : base(typeof(AuthorizeFilter))
        //{
        //    Arguments = new object[] { new Claim(claimType, claimValue) };
        //}
    }

    public class AuthorizeFilter : IAuthorizationFilter
    {
        readonly string[] _claim;

        public AuthorizeFilter(params string[] claim)
        {
            _claim = claim;
        }


        //readonly Claim _claim;

        //public AuthorizeFilter(Claim claim)
        //{
        //    _claim = claim;
        //}

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var principal = ApiTokenHelper.GetPrincipalFromToken(token);
            //var expClaim = principal.Claims.First(x => x.Type == "exp").Value;
            //var tokenExpiryTime = Convert.ToDouble(expClaim).UnixTimeStampToDateTime();
            //if (tokenExpiryTime < DateTime.UtcNow)
            //{
            //    //return token expried
            //}


            //var hasClaim = context.HttpContext.User.HasClaim(c => c.Type == _claim.Type && c.Value == _claim.Value);
            ////var currentUser = HttpContext.User;
            ////var hasClaim = currentUser.HasClaim(c => c.Type == _claim.Type && c.Value == _claim.Value);
            //if (!hasClaim)
            //{
            //    context.Result = new ForbidResult();
            //}
            return;
        }
    }
}
