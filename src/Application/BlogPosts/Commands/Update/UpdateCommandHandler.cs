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

namespace Application.BlogPosts.Commands.Update
{
    internal sealed class UpdateCommandHandler : ICommandHandler<UpdateCommand, BlogPost>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccountRepository _accountRepository;

        public UpdateCommandHandler(IBlogPostRepository blogPostRepository, ICurrentUserService currentUserService, IAccountRepository accountRepository)
        {
            _blogPostRepository = blogPostRepository ?? throw new ArgumentNullException(nameof(blogPostRepository));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _accountRepository = accountRepository;
        }
        public async Task<Result<BlogPost>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            string? userId = _currentUserService.UserId;

            if (await _accountRepository.FindByIdAsync(userId) is not ApplicationUser user)
            {
                return Result.Failure<BlogPost>(new Error("INVALID_USER", "User invalid, please try again!"));
            }

            if (await _blogPostRepository.GetByIdAsync(request.Id) is not BlogPost blogPost)
            {
                return Result.Failure<BlogPost>(new Error("INVALID_POST", "This post does not exist, please try again!"));
            }

            BlogPost data = BlogPost.Update(blogPost, request.Title, request.Content, user);

            await _blogPostRepository.UpdateAsync(data, cancellationToken);
            
            return Result.Success(data);
        }
    }
}
