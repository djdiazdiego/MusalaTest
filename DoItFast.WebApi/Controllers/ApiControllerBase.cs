using AutoMapper;
using DoItFast.Application.Features.Command;
using DoItFast.Application.Exceptions;
using DoItFast.Application.Extensions;
using DoItFast.Application.Features.Queries;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Abstractions.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoItFast.WebApi.Controllers
{
    /// <summary>
    /// Controller common operations
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : Controller
    {
        protected ApiControllerBase()
        {
        }

        /// <summary>
        /// Get origin.
        /// </summary>
        protected string Origin
        {
            get
            {
                try { return Request.Headers["origin"]; } catch { return String.Empty; }
            }
        }

        /// <summary>
        /// Generate ip address.
        /// </summary>
        protected string IpAddress
        {
            get
            {
                if (Request.Headers.ContainsKey("X-Forwarded-For"))
                    return Request.Headers["X-Forwarded-For"];
                else
                {
                    var ipAddress = HttpContext.Connection.RemoteIpAddress;
                    return ipAddress != null ? ipAddress.MapToIPv4().ToString() : throw new ApiException("Ip address not found");
                }
            }
        }
    }
}
