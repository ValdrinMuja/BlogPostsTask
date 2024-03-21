using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.BlogPosts
{
    public sealed class BlogPost
    {
        public BlogPost(Guid id, string title, string friendlyUrl, string content, DateTime dateCreated, ApplicationUser createdBy)
        {
            Id = id;
            Title = title;
            FriendlyUrl = friendlyUrl;
            Content = content;
            DateCreated = dateCreated;
            CreatedBy = createdBy;
        }
        public BlogPost() { }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string FriendlyUrl { get;  set; }
        public string Content { get; private set; }
        public DateTime DateCreated { get; private set; }
        public string UserId { get; private set; }

        public ApplicationUser CreatedBy { get; private set; }

        public static BlogPost Create(string title,string content,ApplicationUser createdBy)
        {
            return new BlogPost(Guid.NewGuid(), title, GenerateFriendlyURL(title), content,DateTime.UtcNow,createdBy);
        }
        public static BlogPost Update(BlogPost blogPost, string title, string content, ApplicationUser createdBy)
        {
            blogPost.Title = title??blogPost.Title;
            blogPost.FriendlyUrl = GenerateFriendlyURL(blogPost.Title) ?? blogPost.FriendlyUrl;
            blogPost.Content = content??blogPost.Content;
            blogPost.CreatedBy = createdBy;

            return blogPost;
        }

        private static string GenerateFriendlyURL(string title)
        {
            string friendlyUrl = title.ToLowerInvariant();

            friendlyUrl = Regex.Replace(friendlyUrl, @"\p{M}", "");

            friendlyUrl = friendlyUrl.Replace(' ', '-');

            friendlyUrl = Regex.Replace(friendlyUrl, @"[^a-z0-9\s-]", "");

            friendlyUrl = Regex.Replace(friendlyUrl, @"[-]+", "-");

            friendlyUrl = friendlyUrl.Trim('-');

            return friendlyUrl;
        }
    }
}
