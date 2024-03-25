using Application.Abstractions;
using Domain.BlogPosts;
using Domain.Shared;
using Domain.Users;

namespace Application.BlogPosts.Commands.Import
{
    public class ImportCommandHandler : ICommandHandler<ImportCommand>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccountRepository _accountRepository;

        public ImportCommandHandler(IBlogPostRepository blogPostRepository,ICurrentUserService currentUserService,IAccountRepository accountRepository)
        {
            _blogPostRepository = blogPostRepository;
            _currentUserService = currentUserService;
            _accountRepository = accountRepository;
        }

        public async Task<Result> Handle(ImportCommand request, CancellationToken cancellationToken)
        {
            if (await _accountRepository.FindByIdAsync(request.UserId) is not ApplicationUser user)
            {
                return Result.Failure<List<BlogPost>>(new Error("INVALID_USER", "User invalid, please try again!"));
            }

            using var httpClient = new HttpClient();
            var csvStream = await httpClient.GetStreamAsync(request.CsvUrl);

            var blogPosts = BlogPost.ParseCsv(csvStream, user);

            await _blogPostRepository.AddRangeAsync(blogPosts);

            return Result.Success();
        }
    }
}
