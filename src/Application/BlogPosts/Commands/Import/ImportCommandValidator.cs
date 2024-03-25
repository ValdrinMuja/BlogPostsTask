using Application.BlogPosts.Commands.Delete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BlogPosts.Commands.Import
{
    internal class ImportCommandValidator: AbstractValidator<ImportCommand>
    {
        public ImportCommandValidator() 
        {
            RuleFor(p=>p.CsvUrl).NotEmpty();
        }
    }
}
