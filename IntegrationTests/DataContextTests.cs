using Domain.BlogPosts;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class DataContextTests
    {
        private DataContext _context;
        public DataContextTests()
        {
            var dbOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _context = new DataContext(dbOptions);
        }

        [Fact]
        public async void Save_SetTitleValue()
        {
            // Arrange
            var user = ApplicationUser.Create("test", "test", "test@test.com", "test", DateTime.UtcNow);
            var blogPost = BlogPost.Create("Title test 1", "Content test 1", user);

            // Act
            await _context.Users.AddAsync(user);
            await _context.BlogPosts.AddAsync(blogPost);
            await _context.SaveChangesAsync();

            // Assert
            blogPost.Title.ShouldNotBeNullOrEmpty();
        }


        [Fact]
        public async void Save_SetFriendlyUrlValue()
        {
            // Arrange
            var user = ApplicationUser.Create("test", "test", "test@test.com", "test", DateTime.UtcNow);
            var blogPost = BlogPost.Create("Title test 1", "Content test 1", user);

            // Act
            await _context.Users.AddAsync(user);
            await _context.BlogPosts.AddAsync(blogPost);
            await _context.SaveChangesAsync();

            // Assert
            blogPost.FriendlyUrl.ShouldNotBeNullOrEmpty();
        }
    }
}
