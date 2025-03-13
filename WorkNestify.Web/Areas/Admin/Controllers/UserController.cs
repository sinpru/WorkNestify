using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Users;
using WorkNestify.Models.ViewModels;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public UserController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: Admin/User
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usersList = await _userManager.Users
                .Include(u => u.Company)
                .Include(u => u.JobApplications)
                .ToListAsync();
            
            return Json(new
            {
                data = usersList.Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.PhoneNumber,
                    u.Role,
                    Company = u.Company?.Name ?? "WorkNestify",
                    JobApplication = u.JobApplications?.Count ?? 0,
                })
            });
        }

        // GET: Admin/User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // GET: Admin/User/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Admin/User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("FullName,Role,CompanyId,Email,PasswordHash,PhoneNumber")] ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(applicationUser);
            }

            try
            {
                var user = new ApplicationUser
                {
                    UserName = applicationUser.Email,
                    Role = applicationUser.Role,
                    Email = applicationUser.Email,
                    PhoneNumber = applicationUser.PhoneNumber,
                    FullName = applicationUser.FullName,
                    CompanyId = applicationUser.CompanyId
                };

                var result = await _userManager.CreateAsync(user, applicationUser.PasswordHash!);

                if (result.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(applicationUser.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                }

                // If creation failed, add errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                PopulateDropdowns();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = ex.Message;
                PopulateDropdowns();
                return View(applicationUser);
            }
        }

        // GET: Admin/User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            PopulateDropdowns();
            return View(applicationUser);
        }

        // POST: Admin/User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind("Id,FullName,Role,CompanyId,Email,PhoneNumber")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(applicationUser);
            }

            try
            {
                // Retrieve the existing user from the database
                var existingUser = await _userManager.FindByIdAsync(applicationUser.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                // Update basic properties
                existingUser.FullName = applicationUser.FullName;
                existingUser.Role = applicationUser.Role;
                existingUser.CompanyId = applicationUser.CompanyId;
                existingUser.PhoneNumber = applicationUser.PhoneNumber;
                existingUser.Email = applicationUser.Email;
                existingUser.UserName = applicationUser.Email;
                existingUser.ModifiedDate = DateTime.UtcNow;

                // Update the user using UserManager
                var result = await _userManager.UpdateAsync(existingUser);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                // If update failed, add errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                PopulateDropdowns();
                return View(applicationUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ApplicationUserExists(applicationUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    TempData["Warning"] = "Concurrency error: The user was modified by another process.";
                    PopulateDropdowns();
                    return View(applicationUser);
                }
            }
            catch (Exception ex)
            {
                TempData["Warning"] = ex.Message;
                PopulateDropdowns();
                return View(applicationUser);
            }
        }
        
        // GET: Admin/User/ChangePassword/5
        public async Task<IActionResult> ChangePassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            var model = new ChangePasswordViewModel { Id = id };
            return View(model);
        }

        // POST: Admin/User/ChangePassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded)
                {
                    foreach (var error in removeResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!addResult.Succeeded)
                {
                    foreach (var error in addResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
                
                user.ModifiedDate = DateTime.UtcNow;

                TempData["Success"] = "Password changed successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Error changing password: {ex.Message}";
                return View(model);
            }
        }

        // GET: Admin/User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            try
            {
                var result = await _userManager.DeleteAsync(applicationUser);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                // If deletion failed, add errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                // Return to the Delete view if there are errors
                return View(applicationUser);
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Error deleting user: {ex.Message}";
                return View(applicationUser);
            }
        }

        private async Task<bool> ApplicationUserExists(string id)
        {
            return await _userManager.FindByIdAsync(id) != null;
        }

        private void PopulateDropdowns(ApplicationUser? applicationUser = null)
        {
            ViewData["Role"] = new SelectList(Roles.AllRoles);
            ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Name");
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                                                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}