using Application.Abstractions;
using Domain.BlogPosts;
using Hangfire;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Hangfire
{
    public static class MediatorExtensions
    {
        public static void Enqueue(this ISender sender, ICommand command, CancellationToken cancellationToken)
        {
            var backgroundJobClient = new BackgroundJobClient();
            backgroundJobClient.Enqueue<MediatorHangfireBridge>(x => x.Send(command));
        }
    }
}
