using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JobController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/Job
        public async Task<IActionResult> Index()
        {
            var jobs = await _unitOfWork.Jobs
                .GetAllAsync(includeProperties: "Company," +
                                                "JobCategory," +
                                                "JobLevel," +
                                                "JobStatus," +
                                                "JobType");
            return View(jobs);
        }

        // GET: Admin/Job/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _unitOfWork.Jobs
                .GetAsync(j => j.Id == id, 
                    includeProperties: "Company,JobCategory,JobLevel,JobStatus,JobType");

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Admin/Job/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            PopulateDateFields();
            return View();
        }

        // POST: Admin/Job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Id,Title,Description,Location,Salary,StartDate,EndDate,CreatedDate,ModifiedDate,JobTypeId,JobStatusId,JobLevelId,JobCategoryId,CompanyId")]
            Job job)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Jobs.AddAsync(job);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns();
            PopulateDateFields();
            return View(job);
        }

        // GET: Admin/Job/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _unitOfWork.Jobs
                .GetAsync(j => j.Id == id,
                    includeProperties: "Company,JobCategory,JobLevel,JobStatus,JobType");
            
            if (job == null)
            {
                return NotFound();
            }

            PopulateDropdowns();
            PopulateDateFields();
            return View(job);
        }

        // POST: Admin/Job/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "Id,Title,Description,Location,Salary,StartDate,EndDate,CreatedDate,ModifiedDate,JobTypeId,JobStatusId,JobLevelId,JobCategoryId,CompanyId")]
            Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.Jobs.UpdateAsync(job);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await JobExists(job.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateDropdowns();
            PopulateDateFields();
            return View(job);
        }

        // GET: Admin/Job/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _unitOfWork.Jobs
                .GetAsync(j => j.Id == id,
                    includeProperties: "Company,JobCategory,JobLevel,JobStatus,JobType");
            
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Admin/Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _unitOfWork.Jobs
                .GetAsync(j => j.Id == id,
                    includeProperties: "Company,JobCategory,JobLevel,JobStatus,JobType");
            
            if (job != null)
            {
                _unitOfWork.Jobs.Remove(job);
                await _unitOfWork.SaveAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> JobExists(int id)
        {
            return await _unitOfWork.Jobs.GetAsync(j => j.Id == id) != null;
        }
        
        private void PopulateDropdowns(Job job = null)
        {
            ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Address", job?.CompanyId);
            ViewData["JobCategoryId"] = new SelectList(_unitOfWork.JobCategories.GetAllAsync().Result, "Id", "Name", job?.JobCategoryId);
            ViewData["JobLevelId"] = new SelectList(_unitOfWork.JobLevels.GetAllAsync().Result, "Id", "Name", job?.JobLevelId);
            ViewData["JobStatusId"] = new SelectList(_unitOfWork.JobStatuses.GetAllAsync().Result, "Id", "Name", job?.JobStatusId);
            ViewData["JobTypeId"] = new SelectList(_unitOfWork.JobTypes.GetAllAsync().Result, "Id", "Name", job?.JobTypeId);
        }

        private void PopulateDateFields(Job job = null)
        {
            var currentDate = DateTime.Now;
            var futureDate = currentDate.AddDays(14);
            
            ViewData["EndDate"] = job?.EndDate?.ToString("yyyy-MM-ddTHH:mm") ?? futureDate.ToString("yyyy-MM-ddTHH:mm");
            ViewData["StartDate"] = job?.StartDate?.ToString("yyyy-MM-ddTHH:mm") ?? currentDate.ToString("yyyy-MM-ddTHH:mm");
        }
    }
}