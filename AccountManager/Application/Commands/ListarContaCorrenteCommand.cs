using AccountManager.API.Application.Models;
using MediatR;

namespace AccountManager.API.Application.Commands
{
    public class ListarContaCorrenteCommand 
        : IRequest<ContaCorrenteViewModel[]>
    {
    }
}
