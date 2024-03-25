using Domain.BlogPosts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Application.BlogPosts.Queries.GetAll;
using Shouldly;
using Application.Abstractions;
using Domain.Shared;

namespace UnitTests.BlogPosts.Query
{
    public class GetBlogPostsQueryHandlerTests
    {
        private readonly Mock<IBlogPostRepository> _blogPostRepositoryMock;
        public GetBlogPostsQueryHandlerTests()
        {
            _blogPostRepositoryMock = MockBlogPostRepository.GetBlogPosts();
        }

        [Fact]
        public async Task GetAllBlogPostTest()
        {
            var handler=new GetBlogPostsQueryHandler(_blogPostRepositoryMock.Object);
            var result=await handler.Handle(new GetBlogPostsQuery(1,10),CancellationToken.None);

            result.ShouldBeOfType<Result<PagedList<BlogPost>>>();
            result.IsSuccess.ShouldBe(true);
            result.Value.Items.Count().ShouldBe(3);
        }
    }
}
