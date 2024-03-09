using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.ViewModels.ImportViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Interfaces
{
    public interface IImportService 
    {
        Task<ApiResponse<ImportViewDTO>> CreateImportWithDetailAsync(CreateImportDTO createImport);

        Task<ApiResponse<ImportViewDTO>> UpdateImportAysnc(Guid importId, UpdateImportDTO updateImport);
    }
}
