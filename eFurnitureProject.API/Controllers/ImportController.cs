using eFurnitureProject.Application.Commons;
using eFurnitureProject.Application.Interfaces;
using eFurnitureProject.Application.ViewModels.ImportViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFurnitureProject.API.Controllers
{
    [Authorize(Roles = AppRole.Admin + "," + AppRole.Staff)]
    public class ImportController : BaseController
    {
        private readonly IImportService _importService;

        public ImportController(IImportService importService)
        {
            _importService = importService;
        }

        [HttpPost]
        public async Task<ApiResponse<ImportViewDTO>> CreateImportWithDetail(CreateImportDTO createImport) => await _importService.CreateImportWithDetailAsync(createImport);

        [HttpPut]
        public async Task<ApiResponse<ImportViewDTO>> UpdateImport(Guid importId,UpdateImportDTO updateImport) => await _importService.UpdateImportAysnc(importId, updateImport);
    }
}
