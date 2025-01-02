using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProiectDAW.Data;
using ProiectDAW.Models;

namespace ProiectDAW.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _db;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment env,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
            _db = db;
        }

        public string Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Description { get; set; }

        public string? ProfilePic { get; set; }

        public bool Privacy { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string? PhoneNumber { get; set; }

            [Display(Name = "First Name")]
            public string? FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string? LastName { get; set; }

            [Display(Name = "Description")]
            public string? Description { get; set; }

            [Display(Name = "Profile Picture")]
            public IFormFile? ProfilePic { get; set; }

            [Display(Name = "Privacy")]
            public bool Privacy { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var description = user.Description;
            var profilePic = user.ProfilePic;
            var privacy = user.Privacy;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                Description = description,
                ProfilePic = null,
                Privacy = privacy
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            TempData["ProfilePic"] = user.ProfilePic;

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }

            if (Input.Description != user.Description)
            {
                user.Description = Input.Description;
            }

            if (Input.Privacy != user.Privacy)
            {
                user.Privacy = Input.Privacy;
            }

            if (Input.ProfilePic != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(Input.ProfilePic.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Input.ProfilePic", "Invalid image format. Allowed formats are .jpg, .jpeg, .png, .gif.");
                    await LoadAsync(user);
                    return Page();
                }

                var storagePath = Path.Combine(_env.WebRootPath, "images", Input.ProfilePic.FileName);
                var databaseFileName = "/images/" + Input.ProfilePic.FileName;

                using (var fileStream = new FileStream(storagePath, FileMode.Create))
                {
                    await Input.ProfilePic.CopyToAsync(fileStream);
                }
                ModelState.Remove(nameof(user.ProfilePic));
                user.ProfilePic = databaseFileName;

            }

            _db.Update(user);
            await _db.SaveChangesAsync();

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
