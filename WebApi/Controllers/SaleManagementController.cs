using Library.Core.Infrastructure;
using Library.Core.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DataService;
using System;
using System.Collections.Generic;
using ViewModels;

namespace WebApp.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize]
    public class SaleManagementController : ControllerBase
    {
        private readonly IRetrieveDataFromTokenService _retrieveDataFromTokenService;
        private readonly ISaleService _saleService;

        public SaleManagementController(IRetrieveDataFromTokenService retrieveDataFromTokenService, ISaleService saleService)
        {
            _retrieveDataFromTokenService = retrieveDataFromTokenService;
            _saleService = saleService;
        }


        [HttpGet("checknumberprice")]
        public IActionResult CheckNumberPrice(string lotteryNumber, int lotteryPrice)
        {
            Response response = new Response();
            try
            {
                var tokenHolder = _retrieveDataFromTokenService.GetClaims();
                _saleService.CheckNumberPrice(lotteryNumber, lotteryPrice);
                response.Status = true;
                response.StatusCode = Library.Core.ViewModels.StatusCode.Success;
                response.Message = "OK";
                response.Data = new object { };
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusCode = Library.Core.ViewModels.StatusCode.InternalServerError;
                response.Message = ex.Message;
                response.Data = new object { };
            }
            return Ok(response);
        }

        [HttpGet("getsellsetnumber")]
        public IActionResult GetSellSetNumber(string lotteryNumber, int lotteryPrice)
        {
            Response response = new Response();
            try
            {
                var tokenHolder = _retrieveDataFromTokenService.GetClaims();
                var data = _saleService.GetSellSetNumber(lotteryNumber, lotteryPrice);
                response.Status = true;
                response.StatusCode = Library.Core.ViewModels.StatusCode.Success;
                response.Message = "OK";
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusCode = Library.Core.ViewModels.StatusCode.InternalServerError;
                response.Message = ex.Message;
                response.Data = new object { };
            }
            return Ok(response);
        }

        [HttpPost("add/sale")]
        public IActionResult Add([FromBody] BillViewModel billViewModel)
        {
            Response response = new Response();
            try
            {
                var tokenHolder = _retrieveDataFromTokenService.GetClaims();
                _saleService.InsertSale(tokenHolder.DeviceCode, billViewModel.periodNumber, billViewModel.SaleList);
                response.Status = true;
                response.StatusCode = Library.Core.ViewModels.StatusCode.Success;
                response.Message = "Successfull sale add";
                response.Data = new object { };
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.StatusCode = Library.Core.ViewModels.StatusCode.InternalServerError;
                response.Message = ex.Message;
                response.Data = new object { };
            }
            return Ok(response);
        }
    }
}
