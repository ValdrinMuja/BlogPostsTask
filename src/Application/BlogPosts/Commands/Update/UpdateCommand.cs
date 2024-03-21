using Application.Abstractions;
using Domain.BlogPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BlogPosts.Commands.Update;
public sealed record UpdateCommand(Guid Id,string Title, string Content) : ICommand<BlogPost>;
