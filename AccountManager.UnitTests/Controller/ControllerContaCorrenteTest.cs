using AccountManager.API.Application.Commands;
using AccountManager.API.Controllers;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AccountManager.UnitTests.Controller
{
    public class ControllerContaCorrenteTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<ContaCorrenteController>> _loggerMock;

        public ControllerContaCorrenteTest()
        {
            _mediator = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<ContaCorrenteController>>();
        }
        
        [Fact]
        public async Task Mediator_Envia_Command_Handler()
        {
            //Arrange
            var contaCorrenteId = Guid.NewGuid();
            var correntistaId = Guid.NewGuid();
            var saldoInicial = 110m;
            var fakeCommand = new CriarContaCorrenteCommand(
                contaCorrenteId,
                correntistaId,
                saldoInicial);
            var commadResultFake = new CommandResult(true);

            _mediator.Setup(x => x.Send(It.IsAny<IRequest<CommandResult>>(),
                default(System.Threading.CancellationToken)))
                .Returns(Task.FromResult(commadResultFake));
            
            //Act
            var controller = new ContaCorrenteController(
                _loggerMock.Object,
                _mediator.Object);

            var result = await controller.CriarContaCorrente(fakeCommand);

            //Assert
            Assert.NotNull(result);
            _mediator.Verify(x => x.Send(It.IsAny<IRequest<CommandResult>>(),
                default(System.Threading.CancellationToken)),
                Times.Once());
        }
    }
}
