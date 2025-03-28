using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Companies;
using WorkNestify.Services;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CloudinaryService _cloudinary;
        private readonly GhnService _ghnService;
        private readonly LocationManager _locationManager;

        public CompanyController(
            IUnitOfWork unitOfWork, 
            CloudinaryService cloudinary, 
            GhnService ghnService,
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

            try
            {
                // Ensure the input location exists
                bool locationExists =
                    await _locationManager.EnsureLocationExists(company.ProvinceId, company.DistrictId,
                        company.WardCode);

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

                TempData["Success"] = "Company added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Error creating company: {ex.Message}";
                await PopulateDropdowns();
                return View(company);
            }
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

            try
            {
                // Ensure the input location exists
                bool locationExists =
                    await _locationManager.EnsureLocationExists(company.ProvinceId, company.DistrictId,
                        company.WardCode);
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

                // Get the existing company and update it instead of tracking a new instance
                var existingCompany = await _unitOfWork.Companies.GetAsync(c => c.Id == id, tracked: false);
                if (existingCompany == null)
                {
                    return NotFound();
                }

                string existingLogoUrl = existingCompany.Logo ?? string.Empty;

                // Handle File Upload to Cloudinary
                if (file != null && file.Length > 0)
                {
                    // Delete old logo if it exists and is from Cloudinary
                    if (!string.IsNullOrEmpty(existingLogoUrl))
                    {
                        bool isCloudinaryUrl = existingLogoUrl.Contains("cloudinary.com");
                        if (isCloudinaryUrl)
                        {
                            bool isDeleted = await _cloudinary.DeleteImageAsync(existingLogoUrl);
                            if (!isDeleted)
                            {
                                TempData["Warning"] = "Failed to delete the old image from Cloudinary.";
                                await PopulateDropdowns();
                                return View(company);
                            }
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

                    existingCompany.Logo = newLogoUrl;
                }
                else if (string.IsNullOrEmpty(company.Logo))
                {
                    existingCompany.Logo = existingLogoUrl;
                }

                // Update other properties
                existingCompany.Name = company.Name;
                existingCompany.Website = company.Website;
                existingCompany.Email = company.Email;
                existingCompany.Phone = company.Phone;
                existingCompany.StreetAddress = company.StreetAddress;
                existingCompany.Description = company.Description;
                existingCompany.Industry = company.Industry;
                existingCompany.FoundedDate = company.FoundedDate;
                existingCompany.Size = company.Size;
                existingCompany.ProvinceId = company.ProvinceId;
                existingCompany.DistrictId = company.DistrictId;
                existingCompany.WardCode = company.WardCode;
                existingCompany.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.Companies.UpdateAsync(existingCompany);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Company updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Error editing company: {ex.Message}";
                await PopulateDropdowns();
                return View(company);
            }
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

            TempData["Success"] = "Company deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdowns(Company? company = null)
        {
            ViewData["Size"] = new SelectList(CompanySizes.AllSizes);
            ViewData["ProvinceId"] =
                new SelectList(await _ghnService.GetProvincesAsync(), "Id", "Name", company?.ProvinceId);

            if (company != null)
            {
                ViewData["DistrictId"] =
                    new SelectList(await _ghnService.GetDistrictsAsync(company.ProvinceId), "Id", "Name",
                        company?.DistrictId);
                ViewData["WardCode"] =
                    new SelectList(await _ghnService.GetWardsAsync(company.DistrictId), "Code", "Name",
                        company?.WardCode);
            }
        }
    }
}