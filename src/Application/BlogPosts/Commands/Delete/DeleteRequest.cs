using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BlogPosts.Commands.Delete;
public sealed record DeleteRequest(Guid Id);
