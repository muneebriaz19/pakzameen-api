using Application.Dto;
using Application.Interfaces;
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
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordValidator() {

            RuleFor(resetpassword => resetpassword.Email)
                   .NotEmpty().WithMessage(AppMessages.EmailEmpty)
                   .EmailAddress().WithMessage(AppMessages.EmailInvalid);

            RuleFor(resetpassword => resetpassword.Token)
                   .NotEmpty().WithMessage(AppMessages.TokenEmpty);
                  

            RuleFor(resetpassword => resetpassword.ConfirmPassword)
                 .NotEmpty().WithMessage(AppMessages.ConfirmPasswordRequired)
                 .Equal(resetpassword => resetpassword.Password).WithMessage(AppMessages.PasswordMatch);
        }
        
           
    }
}
