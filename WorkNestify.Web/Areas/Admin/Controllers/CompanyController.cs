using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Companies;
using WorkNestify.Services;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CloudinaryService _cloudinary;
        private readonly GhnService _ghnService;
        private readonly LocationManager _locationManager;

        public CompanyController(IUnitOfWork unitOfWork, CloudinaryService cloudinary, GhnService ghnService,
            LocationManager locationManager)
        {
            _unitOfWork = unitOfWork;
            _cloudinary = cloudinary;
            _ghnService = ghnService;
            _locationManager = locationManager;
        }

        // GET: Admin/Company
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Companies.GetAllAsync().Result;
            return Json(new
            {
                data = companyList.Select(c => new
                {
                    c.Name,
                    c.Email,
                    c.Phone,
                    c.StreetAddress,
                    c.Industry,
                    c.Size,
                    c.Id
                })
            });
        }

        // GET: Admin/Company/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Admin/Company/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // POST: Admin/Company/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Name,Website,Email,Phone,StreetAddress,Description,Logo,Industry,FoundedDate,Size,ProvinceId,DistrictId,WardCode")]
            Company company, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns();
                return View(company);
            }

            // Ensure the input location exists
            bool locationExists =
                await _locationManager.EnsureLocationExists(company.ProvinceId, company.DistrictId, company.WardCode);

            if (!locationExists)
            {
                TempData["Warning"] = "Invalid location data.";
                await PopulateDropdowns();
                return View(company);
            }

            // Ensure only one input is used
            if (!string.IsNullOrEmpty(company.Logo) && file != null)
            {
                TempData["Warning"] = "Please provide either a URL or upload a file, not both.";
                await PopulateDropdowns();
                return View(company);
            }

            // Handle File Upload to Cloudinary
            if (file != null)
            {
                string newLogoUrl = await _cloudinary.UploadImageAsync(file);
                if (string.IsNullOrEmpty(newLogoUrl))
                {
                    TempData["Warning"] = "Error uploading image to Cloudinary.";
                    await PopulateDropdowns();
                    return View(company);
                }

                company.Logo = newLogoUrl;
            }

            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Company/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            await PopulateDropdowns(company);
            return View(company);
        }

        // POST: Admin/Company/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "Id,Name,Website,Email,Phone,StreetAddress,Description,Logo,Industry,FoundedDate,Size,ProvinceId,DistrictId,WardCode")]
            Company company, IFormFile? file)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns();
                return View(company);
            }

            var existingCompany = await _unitOfWork.Companies.GetAsync(c => c.Id == id);
            if (existingCompany == null)
            {
                return NotFound();
            }

            // Ensure the input location exists
            bool locationExists =
                await _locationManager.EnsureLocationExists(company.ProvinceId, company.DistrictId, company.WardCode);
            if (!locationExists)
            {
                TempData["Warning"] = "Invalid location data.";
                await PopulateDropdowns();
                return View(company);
            }

            // Ensure only one input is used for logo
            if (!string.IsNullOrEmpty(company.Logo) && file != null)
            {
                TempData["Warning"] = "Please provide either a URL or upload a file, not both.";
                await PopulateDropdowns();
                return View(company);
            }

            try
            {
                string existingLogoUrl = existingCompany.Logo ?? string.Empty;

                // Handle File Upload to Cloudinary
                if (file != null && file.Length > 0)
                {
                    // Delete old logo if it exists
                    if (!string.IsNullOrEmpty(existingLogoUrl))
                    {
                        bool isDeleted = await _cloudinary.DeleteImageAsync(existingLogoUrl);
                        if (!isDeleted)
                        {
                            TempData["Warning"] = "Failed to delete the old image from Cloudinary.";
                            await PopulateDropdowns();
                            return View(company);
                        }
                    }

                    // Upload new logo
                    string newLogoUrl = await _cloudinary.UploadImageAsync(file);
                    if (string.IsNullOrEmpty(newLogoUrl))
                    {
                        TempData["Warning"] = "Error uploading image to Cloudinary.";
                        await PopulateDropdowns();
                        return View(company);
                    }

                    company.Logo = newLogoUrl;
                }
                else if (string.IsNullOrEmpty(company.Logo))
                {
                    // If no new file and logo URL is empty, keep existing logo
                    company.Logo = existingLogoUrl;
                }

                // Update only the changed properties
                existingCompany.Name = company.Name;
                existingCompany.Website = company.Website;
                existingCompany.Email = company.Email;
                existingCompany.Phone = company.Phone;
                existingCompany.StreetAddress = company.StreetAddress;
                existingCompany.Description = company.Description;
                existingCompany.Logo = company.Logo;
                existingCompany.Industry = company.Industry;
                existingCompany.FoundedDate = company.FoundedDate;
                existingCompany.Size = company.Size;
                existingCompany.ProvinceId = company.ProvinceId;
                existingCompany.DistrictId = company.DistrictId;
                existingCompany.WardCode = company.WardCode;

                await _unitOfWork.Companies.UpdateAsync(existingCompany);
                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CompanyExists(company.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Company/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Admin/Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(company.Logo))
            {
                bool isDeleted = await _cloudinary.DeleteImageAsync(company.Logo);
                if (!isDeleted)
                {
                    TempData["Warning"] = "Company deleted, but the logo could not be removed from Cloudinary.";
                }
            }

            _unitOfWork.Companies.Remove(company);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CompanyExists(int id)
        {
            return await _unitOfWork.Companies.GetAsync(c => c.Id == id) != null;
        }

        private async Task PopulateDropdowns(Company? company = null)
        {
            ViewData["Size"] = new SelectList(CompanySizes.AllSizes);
            ViewData["ProvinceId"] = new SelectList(await _ghnService.GetProvincesAsync(), "Id", "Name");

            if (company != null)
            {
                ViewData["DistrictId"] =
                    new SelectList(await _ghnService.GetDistrictsAsync(company.ProvinceId), "Id", "Name");
                ViewData["WardCode"] =
                    new SelectList(await _ghnService.GetWardsAsync(company.DistrictId), "Code", "Name");
            }
        }
    }
}