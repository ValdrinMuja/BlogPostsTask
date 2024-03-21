using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BlogPosts
{
    public class BlogPostResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string FriendlyUrl { get; init; }
        public string Content { get; init; }
        public DateTime DateCreated { get; init; }
        public UserResponse CreatedBy { get; init; }
    }

    public class UserResponse
    {
        public string Id { get; init; }
        public string FirstName { get; init; }

        public string LastName { get; init; }
        public string Email { get; init; }
    }
}
