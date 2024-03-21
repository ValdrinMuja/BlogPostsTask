using Application.Abstractions;
using Domain.BlogPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BlogPosts.Commands.Delete;
public sealed record DeleteCommand(Guid Id) : ICommand<bool>;
