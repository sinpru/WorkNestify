#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkNestify.Models.Models.Users;
using WorkNestify.Services;

namespace WorkNestify.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly CloudinaryService _cloudinary;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            CloudinaryService cloudinary)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cloudinary = cloudinary;
        }

        public string Username { get; set; }
        
        [TempData]
        public string StatusMessage { get; set; }
        
        [BindProperty]
        public InputModel Input { get; set; }
        
        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }
            
            [Display(Name = "Resume URL")]
            public string ResumeUrl { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FullName = user.FullName,
                ResumeUrl = user.ResumeUrl
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? resumeFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            bool hasChanges = false;

            // Update PhoneNumber if changed
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
                hasChanges = true;
            }

            // Update FullName if changed
            if (Input.FullName != user.FullName)
            {
                user.FullName = Input.FullName;
                hasChanges = true;
            }

            // Handle Resume file upload if provided
            if (resumeFile != null && resumeFile.Length > 0)
            {
                try
                {
                    string existingResumeUrl = user.ResumeUrl ?? string.Empty;

                    if (!string.IsNullOrEmpty(existingResumeUrl))
                    {
                        bool resumeIsDeleted = await _cloudinary.DeleteDocumentAsync(existingResumeUrl);
                        if (!resumeIsDeleted)
                        {
                            StatusMessage = "Failed to delete the old resume file.";
                            await LoadAsync(user);
                            return Page();
                        }
                    }
                    
                    string resumeUrl = await _cloudinary.UploadDocumentAsync(resumeFile);
                    if (string.IsNullOrEmpty(resumeUrl))
                    {
                        StatusMessage = "Failed to upload resume file.";
                        await LoadAsync(user);
                        return Page();
                    }

                    user.ResumeUrl = resumeUrl;
                    hasChanges = true;
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Error uploading resume: {ex.Message}";
                    await LoadAsync(user);
                    return Page();
                }
            }

            // Update ModifiedDate and save if there are changes
            if (hasChanges)
            {
                user.ModifiedDate = DateTime.UtcNow;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to update profile.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
