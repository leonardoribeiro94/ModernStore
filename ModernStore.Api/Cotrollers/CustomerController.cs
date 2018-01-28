using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModernStore.Domain.Commands.Handler;
using ModernStore.Domain.Commands.Inputs;
using ModernStore.Infra.Transactions;
using System.Threading.Tasks;

namespace ModernStore.Api.Cotrollers
{
    public class CustomerController : BaseController
    {
        private readonly CustomerCommandHandler _commandHandler;
        private readonly IUow _uow;

        public CustomerController(IUow uow, CustomerCommandHandler commandHandler) :
            base(uow)
        {
            _uow = uow;
            _commandHandler = commandHandler;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("v1/customers")]
        public async Task<IActionResult> Post([FromBody] RegisterCustomerCommand command)
        {
            var result = _commandHandler.Handle(command);
            return await Response(result, _commandHandler.Notifications);
        }
    }
}
