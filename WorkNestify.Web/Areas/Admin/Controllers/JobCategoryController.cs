using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Jobs;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class JobCategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobCategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/JobCategory
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var jobCategoriesList = _unitOfWork.JobCategories
                .GetAllAsync().Result;
            return Json(new
            {
                data = jobCategoriesList.Select(jc => new
                {
                    jc.Id,
                    jc.Name
                })
            });
        }

        // GET: Admin/JobCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobCategory = await _unitOfWork.JobCategories.GetAsync(jc => jc.Id == id);
            
            if (jobCategory == null)
            {
                return NotFound();
            }

            return View(jobCategory);
        }

        // GET: Admin/JobCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/JobCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] JobCategory jobCategory)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.JobCategories.AddAsync(jobCategory);
                await _unitOfWork.SaveAsync();
                
                TempData["Success"] = "Job category added.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(jobCategory);
        }

        // GET: Admin/JobCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobCategory = await _unitOfWork.JobCategories.GetAsync(jc => jc.Id == id);
            
            if (jobCategory == null)
            {
                return NotFound();
            }
            
            return View(jobCategory);
        }

        // POST: Admin/JobCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] JobCategory jobCategory)
        {
            if (id != jobCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.JobCategories.UpdateAsync(jobCategory);
                    await _unitOfWork.SaveAsync();
                }
                catch (Exception ex)
                {
                    TempData["Warning"] = $"Failed to update job application: {ex.Message}";
                    return View(jobCategory);
                }
                
                TempData["Success"] = "Job category edited.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(jobCategory);
        }

        // GET: Admin/JobCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobCategory = await _unitOfWork.JobCategories.GetAsync(jc => jc.Id == id);
            
            if (jobCategory == null)
            {
                return NotFound();
            }

            return View(jobCategory);
        }

        // POST: Admin/JobCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobCategory = await _unitOfWork.JobCategories.GetAsync(jc => jc.Id == id);
            
            if (jobCategory != null)
            {
                _unitOfWork.JobCategories.Remove(jobCategory);
                await _unitOfWork.SaveAsync();
            }
            
            TempData["Success"] = "Job category deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
