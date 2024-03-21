using Application.Abstractions;
using Domain.BlogPosts;
using Domain.Shared;
using Domain.Users;

namespace Application.BlogPosts.Commands.Create
{
    internal sealed class CreateCommandHandler : ICommandHandler<CreateCommand, BlogPost>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccountRepository _accountRepository;

        public CreateCommandHandler(IBlogPostRepository blogPostRepository, ICurrentUserService currentUserService, IAccountRepository accountRepository)
        {
            _blogPostRepository = blogPostRepository ?? throw new ArgumentNullException(nameof(blogPostRepository));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _accountRepository = accountRepository;
        }

        public async Task<Result<BlogPost>> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            string? userId = _currentUserService.UserId;

            if (await _accountRepository.FindByIdAsync(userId) is not ApplicationUser user)
            {
                return Result.Failure<BlogPost>(new Error("INVALID_USER", "User invalid, please try again!"));
            }


            BlogPost data = BlogPost.Create(request.Title, request.Content, user);


            data.FriendlyUrl = await GenerateUniqueFriendlyUrl(data.FriendlyUrl);

            await _blogPostRepository.CreateAsync(data, cancellationToken);

            return Result.Success(data);
        }

        private async Task<string> GenerateUniqueFriendlyUrl(string friendlyUrl)
        {
            if (await _blogPostRepository.GetByFriendlyUrlAsync(friendlyUrl) is BlogPost postCheck)
            {
                Random rnd = new Random();
                friendlyUrl = $"{postCheck.FriendlyUrl}-{rnd.Next(0, 100)}";
            }

            return friendlyUrl;
        }
    }
}
