using Microsoft.AspNetCore.Mvc;
using ModernStore.Domain.Commands.Handler;
using ModernStore.Domain.Commands.Inputs;
using ModernStore.Infra.Transactions;
using System.Threading.Tasks;

namespace ModernStore.Api.Cotrollers
{

    public class OrderController : BaseController
    {
        private readonly IUow _uow;
        private readonly OrderCommandHandler _commandHandler;

        public OrderController(IUow uow, OrderCommandHandler commandHandler) :
            base(uow)
        {
            _uow = uow;
            _commandHandler = commandHandler;
        }


        [HttpPost]
        [Route("v1/Orders")]
        public async Task<IActionResult> Post([FromBody] RegisterOrderCommand command)
        {
            var result = _commandHandler.Handle(command);

            return await Response(result, _commandHandler.Notifications);
        }
    }
}
