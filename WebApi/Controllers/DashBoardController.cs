using Library.Core.Infrastructure;
using Library.Core.Models.Securities;
using Library.Core.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Services;
using System;
using System.Net.Http.Headers;

namespace WebApp.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize]
    public class DashBoardController : ControllerBase
    {
        private readonly IOnlineService _onlineService;
        private readonly IRetrieveDataFromTokenService _retrieveDataFromTokenService;

        public DashBoardController(IOnlineService onlineService, IRetrieveDataFromTokenService retrieveDataFromTokenService)
        {
            _onlineService = onlineService;
            _retrieveDataFromTokenService = retrieveDataFromTokenService;
        }

        [HttpGet("get")]
        public ActionResult Get()
        {
            //Response response = new Response();

            //try
            //{
            //    var tokenHolder = _retrieveDataFromTokenService.GetClaims();
            //    var data = _onlineService.Get(tokenHolder.DeviceCode);

            //    response.Status = data is null;
            //    response.StatusCode = data is null ? Library.Core.ViewModels.StatusCode.NoDataAvailable : Library.Core.ViewModels.StatusCode.Success;
            //    response.Message = "No data found";
            //    response.Data = data;

            //}
            //catch (Exception ex)
            //{
            //    response.Status = false;
            //    response.StatusCode = Library.Core.ViewModels.StatusCode.InternalServerError;
            //    response.Message = ex.Message;
            //    response.Data = new object { };
            //}
            var tokenHolder = _retrieveDataFromTokenService.GetClaims();
            return Ok(_onlineService.Get(tokenHolder.DeviceCode));

            //return Ok(response);
        }
    }
}
