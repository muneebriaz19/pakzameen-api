using Application.Models;
using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class ChangePasswordValidator:AbstractValidator<ChangePasswordDto>
    {
    public ChangePasswordValidator() {
            RuleFor(resetpassword => resetpassword.ConfirmPassword)
                     .NotEmpty().WithMessage(AppMessages.ConfirmPasswordRequired)
                     .Equal(resetpassword => resetpassword.NewPassword).WithMessage(AppMessages.PasswordMatch);
        }
    }
}
