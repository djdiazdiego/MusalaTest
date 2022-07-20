using AutoMapper;
using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Application.Features.Dtos;
using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Application.Features.Queries.PeripheralDeviceStatus;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoItFast.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class PeripheralDeviceController : ApiControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;

        public PeripheralDeviceController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Update Peripheral Device
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch, Route("update-device")]
        [ProducesResponseType(typeof(Response<PeripheralDeviceResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<PeripheralDeviceResponseDto>>> UpdatePeripheralDevice([FromBody] GatewayUpdatePeripheralDeviceRequestDto dto, CancellationToken cancellationToken)
        {
            return await this.BuildGenericAsync<GatewayUpdatePeripheralDeviceRequestDto, PeripheralDeviceResponseDto>(dto, _mapper, _mediator, typeof(GatewayUpdatePeripheralDeviceCommand), cancellationToken);
        }

        /// <summary>
        /// Delete Peripheral Device
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete-device")]
        [ProducesResponseType(typeof(Response<PeripheralDeviceResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<PeripheralDeviceResponseDto>>> DeletePeripheralDevice([FromBody] GatewayDeletePeripheralDeviceRequestDto dto, CancellationToken cancellationToken)
        {
            return await this.BuildGenericAsync<GatewayDeletePeripheralDeviceRequestDto, PeripheralDeviceResponseDto>(dto, _mapper, _mediator, typeof(GatewayDeletePeripheralDeviceCommand), cancellationToken);
        }

        /// <summary>
        /// Search Peripheral Devices
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet, Route("page")]
        [ProducesResponseType(typeof(Response<PeripheralDeviceFilterResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<PeripheralDeviceFilterResponseDto>>> Page([FromQuery] PeripheralDeviceFilterRequestDto filter, CancellationToken cancellationToken) =>
            await this.BuildFilterAsync<PeripheralDeviceFilterRequestDto, PeripheralDeviceWithGatewayResponseDto, PeripheralDeviceFilterResponseDto>(filter, _mapper, _mediator, cancellationToken);

        /// <summary>
        /// Get Peripheral Device status.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(Response<EnumerationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Response<EnumerationDto>>> Get(PeripheralDeviceStatusValues id, CancellationToken cancellationToken) =>
            await this.BuildGetDeleteAsync<PeripheralDeviceStatusValues, EnumerationDto>(id, _mediator, typeof(PeripheralDeviceStatusGetQuery), cancellationToken);

        /// <summary>
        /// Get Peripheral Devices status.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet, Route("all")]
        [ProducesResponseType(typeof(Response<EnumerationDto[]>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<EnumerationDto[]>>> GetAll(CancellationToken cancellationToken) =>
            await this.BuildGetAllAsync<PeripheralDeviceStatusValues, EnumerationDto>(_mediator, cancellationToken);
    }
}
