using Application.Abstractions;
using Domain.BlogPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.BlogPosts.Commands.Import;
public sealed record ImportCommand(string CsvUrl,string UserId) : Abstractions.ICommand;
