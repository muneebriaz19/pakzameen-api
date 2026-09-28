using Application.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;


namespace Application.Validators
{
   public  class UserValidator : AbstractValidator<RegisterUserDto>
    {
        public UserValidator() {

     
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage(AppMessages.NameRequired);

            RuleFor(user => user.UserName)
               .NotEmpty().WithMessage(AppMessages.UsernameEmpty);

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage(AppMessages.EmailEmpty)
                .EmailAddress().WithMessage(AppMessages.EmailInvalid);

            RuleFor(user => user.ConfirmPassword)
                .NotEmpty().WithMessage(AppMessages.ConfirmPasswordRequired)
                .Equal(user => user.Password).WithMessage(AppMessages.PasswordMatch);

        
        }
    }
}
