using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WorkNestify.DataAccess.Entities.Companies;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Utilities.Services;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CloudinaryService _cloudinary;

        public CompanyController(IUnitOfWork unitOfWork, CloudinaryService cloudinary)
        {
            _unitOfWork = unitOfWork;
            _cloudinary = cloudinary;
        }

        // GET: Admin/Company
        public async Task<IActionResult> Index()
        {
            var companies = await _unitOfWork.Companies.GetAllAsync(includeProperties: "CompanySize");
            return View(companies);
        }

        // GET: Admin/Company/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id, includeProperties: "CompanySize");

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Admin/Company/Create
        public IActionResult Create()
        {
            ViewData["CompanySizeId"] = new SelectList(_unitOfWork.CompanySizes.GetAllAsync().Result, "Id", "Name");
            return View();
        }

        // POST: Admin/Company/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Website,Email,Phone,Address,Description,Logo,Industry,FoundedDate,CompanySizeId")]
            Company company, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CompanySizeId"] = new SelectList(await _unitOfWork.CompanySizes.GetAllAsync(), "Id", "Name");
                return View(company);
            }

            // Ensure only one input is used
            if (!string.IsNullOrEmpty(company.Logo) && file != null)
            {
                ModelState.AddModelError("Logo", "Please provide either a URL or upload a file, not both.");
                ViewData["CompanySizeId"] = new SelectList(await _unitOfWork.CompanySizes.GetAllAsync(), "Id", "Name");
                return View(company);
            }

            // Handle File Upload to Cloudinary
            if (file != null)
            {
                string newLogoUrl = await _cloudinary.UploadImageAsync(file);
                if (string.IsNullOrEmpty(newLogoUrl))
                {
                    ModelState.AddModelError("Logo", "Error uploading image to Cloudinary.");
                    ViewData["CompanySizeId"] = new SelectList(await _unitOfWork.CompanySizes.GetAllAsync(), "Id", "Name");
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

            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id, includeProperties: "CompanySize");

            if (company == null)
            {
                return NotFound();
            }

            ViewData["CompanySizeId"] = new SelectList(_unitOfWork.CompanySizes.GetAllAsync().Result, "Id", "Name");
            return View(company);
        }

        // POST: Admin/Company/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "Id,Name,Website,Email,Phone,Address,Description,Logo,Industry,FoundedDate,CreatedDate,ModifiedDate,CompanySizeId")]
            Company company, IFormFile? file)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCompany = await _unitOfWork.Companies.GetAsync(c => c.Id == id);
                    if (existingCompany == null)
                    {
                        return NotFound();
                    }
                    
                    string existingLogoUrl = existingCompany.Logo ?? string.Empty;
                    
                    if (file != null && file.Length > 0)
                    {
                        // Delete old logo if it exists
                        if (!string.IsNullOrEmpty(existingLogoUrl))
                        {
                            bool isDeleted = await _cloudinary.DeleteImageAsync(existingLogoUrl);
                            if (!isDeleted)
                            {
                                ModelState.AddModelError("Logo", "Failed to delete the old image from Cloudinary.");
                                ViewData["CompanySizeId"] = new SelectList(_unitOfWork.CompanySizes.GetAllAsync().Result, "Id", "Name");
                                return View(company);
                            }
                        }

                        // Upload new logo
                        string newLogoUrl = await _cloudinary.UploadImageAsync(file);
                        if (string.IsNullOrEmpty(newLogoUrl))
                        {
                            ModelState.AddModelError("Logo", "Error uploading image to Cloudinary.");
                            ViewData["CompanySizeId"] = new SelectList(_unitOfWork.CompanySizes.GetAllAsync().Result, "Id", "Name");
                            return View(company);
                        }

                        company.Logo = newLogoUrl;
                    }

                    // Ensure the user does not provide both file and URL
                    if (!string.IsNullOrEmpty(company.Logo) && file != null)
                    {
                        ModelState.AddModelError("Logo", "Please provide either a file or a URL, not both.");
                        ViewData["CompanySizeId"] =
                            new SelectList(_unitOfWork.CompanySizes.GetAllAsync().Result, "Id", "Name");
                        return View(company);
                    }

                    existingCompany = company;

                    await _unitOfWork.Companies.UpdateAsync(existingCompany);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CompanyExists(company.Id))
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

            ViewData["CompanySizeId"] = new SelectList(_unitOfWork.CompanySizes.GetAllAsync().Result, "Id", "Name");
            return View(company);
        }

        // GET: Admin/Company/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id, includeProperties: "CompanySize");

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
            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id, includeProperties: "CompanySize");

            if (company != null)
            {
                bool isDeleted = await _cloudinary.DeleteImageAsync(company.Logo ?? string.Empty);
                if (!isDeleted)
                {
                    ModelState.AddModelError("Logo", "Failed to delete the old image from Cloudinary.");
                    ViewData["CompanySizeId"] = new SelectList(_unitOfWork.CompanySizes.GetAllAsync().Result, "Id", "Name");
                    return View(company);
                }
                
                _unitOfWork.Companies.Remove(company);
                await _unitOfWork.SaveAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CompanyExists(int id)
        {
            return await _unitOfWork.Companies.GetAsync(c => c.Id == id) != null;
        }
    }
}