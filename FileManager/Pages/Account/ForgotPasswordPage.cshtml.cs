﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileManager.Models;
using FileManager.Models.Database.UserDepartmentRoles;
using FileManager.Models.ViewModels.Account;
using FileManager.Services.ResetPasswordService;
using FileManager.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace FileManager.Pages.Account.ForgotPasswordPage
{
    public class ForgotPasswordPageModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IResetPasswordService _resetPasswordService;
        private readonly ILogger<ForgotPasswordPageModel> _logger;
        [BindProperty] public ForgotPasswordViewModel ForgotPasswordViewModel { get; set; }

        public ForgotPasswordPageModel(UserManager<User> userManager,
            IResetPasswordService resetPasswordService,
            ILogger<ForgotPasswordPageModel> logger)
        {
            _userManager = userManager;
            _resetPasswordService = resetPasswordService;
            _logger = logger;
        }


        public IActionResult OnGet()
        {
            try
            {
                return Page();
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error while getting page ForgotPasswordPage");
                return NotFound();
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await _userManager.FindByEmailAsync(ForgotPasswordViewModel.Email);
                    if (user != null && (await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        string resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                        string resetPasswordCallbackLink = Url.Page("/Account/ResetPasswordConfirmation",
                            pageHandler: "ResetPassword",
                            new { UserId = user.Id, Token = resetPasswordToken },
                            protocol: HttpContext.Request.Scheme);

                        await _resetPasswordService.SendResetPasswordConfirmationLinkToEmailAsync(user, resetPasswordCallbackLink);

                        return RedirectToPagePermanent("Info",
                            "GetInfoMessage",
                            new {message = "Ссылка на восстановление пароля выслана на вашу почту: "+user.Email });

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Неверный Email или он не подтверждён");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неверный Email");
                }
                return Page();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while interaction on ForgotPasswordPage");
                return NotFound();
            }
        }

    }
}