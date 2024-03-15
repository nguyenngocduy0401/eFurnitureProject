using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.FeedBackDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IFeedBackService
    {
        Task<ApiResponse<FeedBackDTO>> CreateFeedBack(CreateFeedBackDTO feedBackDTO, string productId);
        Task<ApiResponse<Pagination<FeedBackViewDTO>>> GetFeedBackJWT(int pageIndex, int PageSize);
    }
}
