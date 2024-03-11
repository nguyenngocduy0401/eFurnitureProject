using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.ResponseViewModel;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    public class ResponseController : BaseController 
    {
        private IResponseService _responseService;
        public ResponseController(IResponseService responseService)
        {
            _responseService = responseService;
        }
        [HttpPost]
        public async Task<ApiResponse<ResponseDTO>> CreateResponse(CreateResponseDTO responseDTO, Guid feedBackId)=>await _responseService.CreateResponse(responseDTO, feedBackId);
    }
}
