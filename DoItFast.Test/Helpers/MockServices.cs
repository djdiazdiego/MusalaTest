using AutoMapper;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Commands;
using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Infrastructure.Shared.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Threading;

namespace DoItFast.Test.Helpers
{
    public class MockServices
    {
        public SetupServices SetupServices { get; }
        public Mock<IUnitOfWork> UnitOfWork { get; }
        public Mock<ISqlGuidGenerator> SqlGuidGenerator { get; }
        public Mock<IMapper> Mapper { get; }
        public Mock<IMediator> Mediator { get; }

        public MockServices()
        {
            SetupServices = new SetupServices();
            var scope = SetupServices.Provider.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            UnitOfWork.Setup(p => p.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(unitOfWork.SaveChangesAsync(default));

            var sqlGuidGenerator = scope.ServiceProvider.GetService<ISqlGuidGenerator>();
            SqlGuidGenerator.Setup(p => p.NewGuid())
                .Returns(sqlGuidGenerator.NewGuid());
        }
        
        public void MockMediator(InvocationFunc invocationFunc)
        {
            var scope = SetupServices.Provider.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            Mediator.Setup(p => p.Send(It.IsAny<ICommand<Response<IDto>>>(), It.IsAny<CancellationToken>()))
                .Returns(invocationFunc);
        }

        public void MockMapper(InvocationFunc invocationFunc)
        {
            var scope = SetupServices.Provider.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMapper>();
            Mapper.Setup(p => p.Map(It.IsAny<IEntity>(), It.IsAny<IDto>()))
                .Returns(invocationFunc);
        }
    }
}
