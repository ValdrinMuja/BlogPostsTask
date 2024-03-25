using Application.Abstractions;
using Domain.BlogPosts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Hangfire
{
    public class MediatorHangfireBridge
    {
        readonly ISender _sender;

        public MediatorHangfireBridge(ISender sender)
        {
            _sender = sender;
        }

        public async Task Send(ICommand command)
        {
            await _sender.Send(command);
        }
    }
}
