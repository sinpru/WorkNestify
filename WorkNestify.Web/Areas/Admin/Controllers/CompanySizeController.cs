using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Companies;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanySizeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanySizeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/CompanySize
        public async Task<IActionResult> Index()
        {
            var companySizes = await _unitOfWork.CompanySizes.GetAllAsync();
            return View(companySizes);
        }

        // GET: Admin/CompanySize/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySize = await _unitOfWork.CompanySizes.GetAsync(cs => cs.Id == id);
            
            if (companySize == null)
            {
                return NotFound();
            }

            return View(companySize);
        }

        private async Task<bool> CompanySizeExists(int id)
        {
            return await _unitOfWork.CompanySizes.GetAsync(cs => cs.Id == id) != null;
        }
    }
}
