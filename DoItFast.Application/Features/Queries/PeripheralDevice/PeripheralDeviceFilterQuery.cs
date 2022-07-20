using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Core.Abstractions.Wrappers;
using DoItFast.Domain.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace DoItFast.Application.Features.Queries.PeripheralDevice
{
    public class PeripheralDeviceFilterQuery : FilterQuery<PeripheralDeviceFilterResponseDto,
        PeripheralDeviceWithGatewayResponseDto,
        Domain.Models.GatewayAggregate.PeripheralDevice>
    {
        public override IQueryable<Domain.Models.GatewayAggregate.PeripheralDevice> BuildFilter(IQueryRepository<Domain.Models.GatewayAggregate.PeripheralDevice> queryRepository)
        {
            var query = queryRepository.FindAll()
                .Include(p => p.Gateway);

            return query;
        }

        public override IQueryable<Domain.Models.GatewayAggregate.PeripheralDevice> BuildOrder(IQueryable<Domain.Models.GatewayAggregate.PeripheralDevice> query)
        {
            var order = this.Order;
            var sortOperation = order.SortOperation == default ? SortOperation.ASC : order.SortOperation;

            if (order?.SortBy.ToUpper() == nameof(PeripheralDeviceWithGatewayResponseDto.SerialNumber).ToUpper())
                return sortOperation == SortOperation.ASC ? query.OrderBy(p => p.Gateway.Id) : query.OrderByDescending(p => p.Gateway.Id);
            else if (order?.SortBy.ToUpper() == nameof(PeripheralDeviceWithGatewayResponseDto.IpAddress).ToUpper())
                return sortOperation == SortOperation.ASC ? query.OrderBy(p => p.Gateway.IpAddress) : query.OrderByDescending(p => p.Gateway.IpAddress);
            else if (order?.SortBy.ToUpper() == nameof(PeripheralDeviceWithGatewayResponseDto.ReadableName).ToUpper())
                return sortOperation == SortOperation.ASC ? query.OrderBy(p => p.Gateway.ReadableName) : query.OrderByDescending(p => p.Gateway.ReadableName);
            else
                return base.BuildOrder(query);
        }
    }


}
