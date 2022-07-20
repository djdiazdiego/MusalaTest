using DoItFast.Application.ApiMessages;
using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Infrastructure.Shared.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DoItFast.Application.Extensions
{
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Not null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TProperty> ApiNotNull<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder) =>
            ruleBuilder.NotNull().WithMessage(GeneralMessages.NotNull.GetDescription());

        /// <summary>
        /// Not empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TProperty> ApiNotEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder) =>
            ruleBuilder.NotEmpty().WithMessage(GeneralMessages.NotEmpty.GetDescription());

        /// <summary>
        /// The maximum allowed length
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="maximumLength"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> ApiMaximumLength<T>(this IRuleBuilder<T, string> ruleBuilder, int maximumLength) =>
            ruleBuilder.MaximumLength(maximumLength).WithMessage($"{GeneralMessages.MaximumLength.GetDescription()} {maximumLength}");

        /// <summary>
        /// Check if it already exists in database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="queryRepository"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TProperty> ApiDoesNotExist<T, TProperty, TEntity>(this IRuleBuilder<T, TProperty> ruleBuilder,
            IQueryRepository<TEntity> queryRepository) where TEntity : class, IEntity =>
            ruleBuilder.MustAsync(async (value, cancellationToken) => !(await queryRepository.FindAll().AnyAsync(p => p.Id.Equals(value), cancellationToken)))
                .WithMessage(GeneralMessages.AlreadyExists.GetDescription());

        /// <summary>
        /// Check if it already exists in database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="queryRepository"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TProperty> ApiAlreadyExists<T, TProperty, TEntity>(this IRuleBuilder<T, TProperty> ruleBuilder,
            IQueryRepository<TEntity> queryRepository) where TEntity : class, IEntity =>
            ruleBuilder.MustAsync((value, cancellationToken) => queryRepository.FindAll().AnyAsync(p => p.Id.Equals(value), cancellationToken))
                .WithMessage(GeneralMessages.NotFound.GetDescription());

        /// <summary>
        /// Validate ip address 4
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> ValidateIpAddress4<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(ipAddress =>
            {
                var splitValues = ipAddress.Split('.');
                if (splitValues.Length != 4)
                    return false;

                if (int.TryParse(splitValues[0], out int value))
                {
                    if (value < 1 || value > 223) return false;
                }
                else return false;

                for (int i = 1; i < 4; i++)
                    if (!byte.TryParse(splitValues[i], out byte tempForParsing)) 
                        return false;

                return true;
            }).WithMessage(GeneralMessages.InvalidIpAddress4.GetDescription());
        }
    }
}
