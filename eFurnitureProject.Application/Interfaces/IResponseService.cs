using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.ResponseViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public  interface IResponseService
    {
        Task<ApiResponse<ResponseDTO>> CreateResponse(CreateResponseDTO responseDTO, Guid feedBackId);
    }
}
