using DoItFast.Application.ApiMessages;
using DoItFast.Application.Extensions;
using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Infrastructure.Shared.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Text.RegularExpressions;

namespace DoItFast.Application.Features.ValidatorExtensions
{
    public static class GatewayValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> SerialNumberCorrectComposition<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var regex = new Regex("^[A-Z0-9]+");
            return ruleBuilder.Must(value => regex.IsMatch(value))
                .WithMessage(GatewayMessages.SerialNumberIncorrectComposition.GetDescription());
        }

        public static IRuleBuilderOptions<T, string> ValidateSerialNumber<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.ApiNotNull()
                .ApiNotEmpty()
                .ApiMaximumLength(32)
                .SerialNumberCorrectComposition();

        public static IRuleBuilderOptions<T, string> ValidateReadableName<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.ApiNotNull()
                .ApiNotEmpty()
                .ApiMaximumLength(64);

        public static IRuleBuilderOptions<T, string> ValidateIpAddress<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.ApiNotNull()
                .ApiNotEmpty()
                .ValidateIpAddress4();

        public static IRuleBuilderOptions<T, IList> ValidatePeripheralDevices<T>(this IRuleBuilder<T, IList> ruleBuilder) =>
            ruleBuilder.Must(p => !(p != null && p.Count > 10))
                .WithMessage(GatewayMessages.ExceededPeripheralDeviceNumber.GetDescription());

        public static IRuleBuilderOptions<T, PeripheralDeviceModel> ValidatePeripheralDevicesElements<T>(
            this IRuleBuilder<T, PeripheralDeviceModel> ruleBuilder,
            IQueryRepository<PeripheralDeviceStatus> deviceStatusQueryRepository) =>
            ruleBuilder.ChildRules(c =>
            {
                c.RuleLevelCascadeMode = CascadeMode.Stop;

                c.RuleFor(p => p.PeripheralDeviceStatusId)
                    .ValidatePeripheralDeviceStatusId(deviceStatusQueryRepository);

                c.RuleFor(p => p.Vendor)
                    .ValidateVendor();
            });

        public static IRuleBuilderOptions<T, string> ValidateVendor<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.ApiNotNull()
                .ApiNotEmpty()
                .ApiMaximumLength(64);

        public static IRuleBuilderOptions<T, PeripheralDeviceStatusValues> ValidatePeripheralDeviceStatusId<T>(
            this IRuleBuilder<T, PeripheralDeviceStatusValues> ruleBuilder,
            IQueryRepository<PeripheralDeviceStatus> deviceStatusQueryRepository) =>
            ruleBuilder.MustAsync((status, cancellationToken) => deviceStatusQueryRepository.FindAll().AnyAsync(s => s.Id.Equals(status), cancellationToken))
                .WithMessage(GatewayMessages.PeripheralDeviceStatusNonexistent.GetDescription());

        public static IRuleBuilderOptions<T, PeripheralDeviceToUpdateDeleteModel> ValidatePeripheralDeviceToUpdateDelete<T>(
            this IRuleBuilder<T, PeripheralDeviceToUpdateDeleteModel> ruleBuilder,
            IQueryRepository<Gateway> gatewayQueryRepository) =>
            ruleBuilder.MustAsync((command, cancellationToken) =>
                gatewayQueryRepository.FindAll().AnyAsync(s => s.Id.Equals(command.SerialNumber) && s.PeripheralDevices.Any(p => p.Id.Equals(command.Id)), cancellationToken))
                    .WithMessage(GeneralMessages.NotFound.GetDescription());
    }
}
