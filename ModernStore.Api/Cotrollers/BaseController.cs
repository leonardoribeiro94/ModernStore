using FluentValidator;
using Microsoft.AspNetCore.Mvc;
using ModernStore.Infra.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernStore.Api.Cotrollers
{
    public class BaseController : Controller
    {
        private readonly IUow _uow;

        public BaseController(IUow uow)
        {
            _uow = uow;
        }

        public async Task<IActionResult> Response(object result, IEnumerable<Notification> notifications)
        {
            if (!notifications.Any())
            {
                try
                {
                    _uow.Commit();
                    return Ok(new
                    {
                        success = true,
                        data = result
                    });
                }
                catch (Exception ex)
                {
                    //logar error
                    return BadRequest(new
                    {
                        succssesResult = false,
                        error = new[] { "Houve um erro interno" }
                    });
                }
            }
            else
                return BadRequest(new
                {
                    success = false,
                    errors = notifications
                });
        }
    }
}
