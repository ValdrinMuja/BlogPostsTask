using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BlogPosts.Commands.Create
{
    internal class CreateCommandValidator:AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator() 
        {
            RuleFor(p=>p.Title).NotEmpty();
            RuleFor(p => p.Content).NotEmpty().MaximumLength(1000);
        }
    }
}
