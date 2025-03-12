using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Jobs;
using WorkNestify.Utilities.Constants;
using SelectList = Microsoft.AspNetCore.Mvc.Rendering.SelectList;

namespace WorkNestify.Web.Areas.Employer.Controllers
{
    [Area("Employer")]
    public class JobController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Employer/Job
        public async Task<IActionResult> Index()
        {
            var jobs = _unitOfWork.Jobs.GetAllAsync();
            return View(await jobs);
        }

        // GET: Employer/Job/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _unitOfWork.Jobs.GetAsync(j => j.Id == id);
            
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Employer/Job/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            PopulateDateFields();
            return View();
        }

        // POST: Employer/Job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Location,Salary,StartDate,EndDate,JobTypeId,JobLevelId,JobCategoryId")] Job job)
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

        // GET: Employer/Job/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _unitOfWork.Jobs.GetAsync(j => j.Id == id);
            
            if (job == null)
            {
                return NotFound();
            }
            
            PopulateDropdowns();
            PopulateDateFields();
            return View(job);
        }

        // POST: Employer/Job/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,Salary,StartDate,EndDate,JobStatusId,JobLevelId,JobCategoryId")] Job job)
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

        // GET: Employer/Job/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _unitOfWork.Jobs.GetAsync(j => j.Id == id);
            
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Employer/Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _unitOfWork.Jobs.GetAsync(j => j.Id == id);
            
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
            ViewData["JobCategoryId"] = new SelectList(_unitOfWork.JobCategories.GetAllAsync().Result, "Id", "Name");
            ViewData["Level"] = new SelectList(JobLevels.AllLevels);
            ViewData["Type"] = new SelectList(JobTypes.AllTypes);
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
