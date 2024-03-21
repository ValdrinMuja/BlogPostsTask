using Application.Abstractions;
using Domain.BlogPosts;

namespace Application.BlogPosts.Queries.GetAll;
public sealed record GetBlogPostsQuery(int Page, int PageSize) : IQuery<PagedList<BlogPost>>;
