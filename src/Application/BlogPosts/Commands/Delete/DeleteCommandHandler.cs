using Application.Abstractions;
using Application.BlogPosts.Commands.Create;
using Domain.BlogPosts;
using Domain.Shared;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BlogPosts.Commands.Delete
{
    internal sealed class DeleteCommandHandler : ICommandHandler<DeleteCommand, bool>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteCommandHandler(IBlogPostRepository blogPostRepository, ICurrentUserService currentUserService, IAccountRepository accountRepository)
        {
            _blogPostRepository = blogPostRepository ?? throw new ArgumentNullException(nameof(blogPostRepository));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<Result<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            if (await _blogPostRepository.GetByIdAsync(request.Id) is not BlogPost blogPost)
            {
                return Result.Failure<bool>(new Error("INVALID_POST", "This post does not exist, please try again!"));
            }

            await _blogPostRepository.DeleteAsync(blogPost, cancellationToken);

            return Result.Success(true);
        }
    }
}
