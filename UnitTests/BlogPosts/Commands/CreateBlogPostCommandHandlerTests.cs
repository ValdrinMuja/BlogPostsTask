using Application.Abstractions;
using Application.BlogPosts.Commands.Create;
using Domain.BlogPosts;
using Domain.Users;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Mocks;

namespace UnitTests.BlogPosts.Commands
{
    public class CreateBlogPostCommandHandlerTests
    {
        private readonly Mock<IBlogPostRepository> _blogPostRepositoryMock;
        private readonly Mock<ICurrentUserService> _currentUserService;
        private readonly Mock<IAccountRepository> _accountRepository;
        public CreateBlogPostCommandHandlerTests()
        {
            _blogPostRepositoryMock = MockBlogPostRepository.GetBlogPosts();
            _accountRepository = new Mock<IAccountRepository>();
            _currentUserService = new Mock<ICurrentUserService>();
        }

        [Fact]
        public async Task CreateBlogPost_ShouldAddBlogPost_WhenCommandIsValid()
        {
            // Arrange
            var mockedUser = ApplicationUser.Create("Test", "User", "testuser@example.com","User",DateTime.UtcNow);
            _currentUserService.Setup(s => s.UserId).Returns(mockedUser.Id);
            _accountRepository.Setup(repo => repo.FindByIdAsync(mockedUser.Id))
                     .ReturnsAsync(mockedUser);
            var handler = new CreateCommandHandler(_blogPostRepositoryMock.Object, _currentUserService.Object,_accountRepository.Object);
            var command = new CreateCommand("New Post", "Content for new post");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBe(true);
            _blogPostRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<BlogPost>(), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
