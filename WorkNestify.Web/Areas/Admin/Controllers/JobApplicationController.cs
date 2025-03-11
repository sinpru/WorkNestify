using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Entities.JobApplications;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JobApplicationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobApplicationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/JobApplication
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var jobApplicationsList = _unitOfWork.JobApplications
                .GetAllAsync(includeProperties: "Job,JobSeeker,JobApplicationStatus").Result;
            return Json(new
            {
                data = jobApplicationsList.Select(ja => new
                {
                    ja.Id,
                    Job = ja.Job.Title,
                    JobSeeker = ja.JobSeeker.FullName,
                    JobApplicationStatus = ja.JobApplicationStatus.Name,
                    ja.ApplicationDate
                })
            });
        }

        // GET: Admin/JobApplication/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _unitOfWork.JobApplications
                .GetAsync(ja => ja.Id == id,
                    includeProperties: "Job,JobSeeker,JobApplicationStatus");
            
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // GET: Admin/JobApplication/Create
        public IActionResult Create()
        {
            PopulateDropdown();
            return View();
        }

        // POST: Admin/JobApplication/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CoverLetter,ApplicationDate,ModifiedDate,JobSeekerId,JobId,JobApplicationStatusId")] JobApplication jobApplication)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.JobApplications.AddAsync(jobApplication);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            
            PopulateDropdown();
            return View(jobApplication);
        }

        // GET: Admin/JobApplication/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _unitOfWork.JobApplications
                .GetAsync(ja => ja.Id == id,
                    includeProperties: "Job,JobSeeker,JobApplicationStatus");
            
            if (jobApplication == null)
            {
                return NotFound();
            }
            
            PopulateDropdown();
            return View(jobApplication);
        }

        // POST: Admin/JobApplication/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CoverLetter,ApplicationDate,ModifiedDate,JobSeekerId,JobId,JobApplicationStatusId")] JobApplication jobApplication)
        {
            if (id != jobApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.JobApplications.UpdateAsync(jobApplication);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await JobApplicationExists(jobApplication.Id))
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
            
            PopulateDropdown();
            return View(jobApplication);
        }

        // GET: Admin/JobApplication/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _unitOfWork.JobApplications
                .GetAsync(ja => ja.Id == id,
                    includeProperties: "Job,JobSeeker,JobApplicationStatus");
            
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // POST: Admin/JobApplication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _unitOfWork.JobApplications
                .GetAsync(ja => ja.Id == id,
                    includeProperties: "Job,JobSeeker,JobApplicationStatus");
            
            if (jobApplication != null)
            {
                _unitOfWork.JobApplications.Remove(jobApplication);
                await _unitOfWork.SaveAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> JobApplicationExists(int id)
        {
            return await _unitOfWork.JobApplications.GetAsync(ja => ja.Id == id) != null;
        }

        private void PopulateDropdown(JobApplication jobApplication = null)
        {
            ViewData["JobId"] = new SelectList(_unitOfWork.Jobs.GetAllAsync().Result, "Id", "Description", jobApplication.JobId);
            ViewData["JobApplicationStatusId"] = new SelectList(_unitOfWork.JobApplicationStatuses.GetAllAsync().Result, "Id", "Name", jobApplication.JobApplicationStatusId);
            ViewData["JobSeekerId"] = new SelectList(_unitOfWork.JobSeekers.GetAllAsync().Result, "Id", "Id", jobApplication.JobSeekerId);
        }
    }
}
