using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace DoItFast.Application.Features.Queries.Gateway
{
    public sealed class GatewayFilterQuery : FilterQuery<GatewayFilterResponseDto, GatewayResponseDto,
        Domain.Models.GatewayAggregate.Gateway>
    {
        ///// <summary>
        ///// Serial number
        ///// </summary>
        //public string SerialNumber { get; set; }
        ///// <summary>
        ///// Human readable name
        ///// </summary>
        //public string ReadableName { get; set; }
        ///// <summary>
        ///// Ip address
        ///// </summary>
        //public string IpAddress { get; set; }


        public override IQueryable<Domain.Models.GatewayAggregate.Gateway> BuildFilter(IQueryRepository<Domain.Models.GatewayAggregate.Gateway> queryRepository)
        {
            var query = (IQueryable<Domain.Models.GatewayAggregate.Gateway>)queryRepository.FindAll()
                .Include(p => p.PeripheralDevices);

            //if (!string.IsNullOrEmpty(SerialNumber))
            //    query = query.Where(p => p.Id.ToUpper().Contains(SerialNumber.ToUpper()));
            //if (!string.IsNullOrEmpty(ReadableName))
            //    query = query.Where(p => p.ReadableName.ToUpper().Contains(ReadableName.ToUpper()));
            //if (!string.IsNullOrEmpty(IpAddress))
            //    query = query.Where(p => p.IpAddress.ToUpper().Contains(IpAddress.ToUpper()));

            return query;
        }

        public override IQueryable<Domain.Models.GatewayAggregate.Gateway> BuildOrder(IQueryable<Domain.Models.GatewayAggregate.Gateway> query)
        {
            var order = this.Order;
            var sortOperation = order.SortOperation == default ? SortOperation.ASC : order.SortOperation;

            if (order?.SortBy.ToUpper() == nameof(GatewayResponseDto.SerialNumber).ToUpper())
                return sortOperation == SortOperation.ASC ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id);
            else return base.BuildOrder(query);


        }
    }
}