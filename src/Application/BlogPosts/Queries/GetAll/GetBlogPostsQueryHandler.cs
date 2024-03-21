using Application.Abstractions;
using Domain.BlogPosts;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BlogPosts.Queries.GetAll
{
    internal sealed class GetBlogPostsQueryHandler : IQueryHandler<GetBlogPostsQuery, PagedList<BlogPost>>
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public GetBlogPostsQueryHandler(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository ?? throw new ArgumentNullException(nameof(blogPostRepository));
        }

        public async Task<Result<PagedList<BlogPost>>> Handle(GetBlogPostsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<BlogPost> blogPosts = _blogPostRepository.GetAll();
            blogPosts = blogPosts.OrderBy(c => c.Title).ThenBy(i => i.Id);

            return await PagedList<BlogPost>.ApplyAsync(blogPosts, request.Page, request.PageSize);
        }
    }
}
