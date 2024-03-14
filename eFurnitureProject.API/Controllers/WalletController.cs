using eFurnitureProject.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class WalletController : BaseController
    {
        private readonly IOrderService _service;

        public WalletController(IOrderService service)
        {
            _service = service;
        }

    }
}
