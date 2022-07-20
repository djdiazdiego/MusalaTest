using AutoMapper;
using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Application.Wrappers;
using DoItFast.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoItFast.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class GatewayController : ApiControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        public GatewayController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Search Gateways
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet, Route("page")]
        [ProducesResponseType(typeof(Response<GatewayFilterResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<GatewayFilterResponseDto>>> Page([FromQuery] GatewayFilterRequestDto filter, CancellationToken cancellationToken) =>
            await this.BuildFilterAsync<GatewayFilterRequestDto, GatewayResponseDto, GatewayFilterResponseDto>(filter, _mapper, _mediator, cancellationToken);

        /// <summary>
        /// Create Gateway
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Response<GatewayResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<GatewayResponseDto>>> Post([FromBody] GatewayCreateRequestDto dto, CancellationToken cancellationToken) =>
           await this.BuildGenericAsync<GatewayCreateRequestDto, GatewayResponseDto>(dto, _mapper, _mediator, typeof(GatewayCreateCommand), cancellationToken);

        /// <summary>
        /// Update Gateway
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Response<GatewayResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<GatewayResponseDto>>> Put([FromBody] GatewayUpdateRequestDto dto, CancellationToken cancellationToken) =>
            await this.BuildGenericAsync<GatewayUpdateRequestDto, GatewayResponseDto>(dto, _mapper, _mediator, typeof(GatewayUpdateCommand), cancellationToken);

        /// <summary>
        /// Delete Gateway
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Response<GatewayResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<GatewayResponseDto>>> Delete(string id, CancellationToken cancellationToken) =>
            await this.BuildGetDeleteAsync<string, GatewayResponseDto>(id, _mediator, typeof(GatewayDeleteCommand), cancellationToken);
    }
}
