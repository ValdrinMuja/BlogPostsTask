using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BlogPosts.Commands.Create;
public sealed record CreateRequest(string Title, string Content);

