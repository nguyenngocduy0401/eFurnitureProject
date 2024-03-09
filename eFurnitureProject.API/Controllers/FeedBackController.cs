using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class FeedBackController :BaseController
    {
private readonly IFeedBackService _feedBackService;

        public FeedBackController(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }
        [HttpPost]
        public async Task<ApiResponse<FeedBackDTO>> CreateFeed(CreateFeedBackDTO feedBackDTO, Guid productID) => await _feedBackService.CreateFeedBack(feedBackDTO, productID);
    }
}
