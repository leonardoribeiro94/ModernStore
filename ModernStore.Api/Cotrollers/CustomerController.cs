using Microsoft.AspNetCore.Mvc;
using ModernStore.Domain.Commands.Handler;
using ModernStore.Domain.Commands.Inputs;
using ModernStore.Infra.Transactions;

namespace ModernStore.Api.Cotrollers
{
    public class CustomerController : Controller
    {
        private readonly CustomerCommandHandler _commandHandler;
        private readonly IUow _uow;

        public CustomerController(IUow uow, CustomerCommandHandler commandHandler)
        {
            _uow = uow;
            _commandHandler = commandHandler;
        }


        [HttpPost]
        [Route("v1/customers")]
        public IActionResult Post([FromBody] RegisterCustomerCommand command)
        {
            var result = _commandHandler.Handle(command);

            if (_commandHandler.IsValid())
                return Ok(result);

            else
                return BadRequest(_commandHandler.Notifications);
        }
    }
}
