using Domain.BlogPosts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public sealed class BlogPostRepository : IBlogPostRepository
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<BlogPostRepository> _logger;

        public BlogPostRepository(DataContext dataContext, ILogger<BlogPostRepository> logger)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext)); ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task CreateAsync(BlogPost blogPost, CancellationToken cancellationToken = default)
        {
            await _dataContext.BlogPosts
                .AddAsync(blogPost, cancellationToken);

            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(BlogPost blogPost, CancellationToken cancellationToken = default)
        {
            await Task.FromResult(_dataContext.BlogPosts.Remove(blogPost));

            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<BlogPost> GetAll(CancellationToken cancellationToken = default)
        {
            return _dataContext.BlogPosts
                .AsNoTracking()
                .Include(u=>u.CreatedBy)
                .AsQueryable();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _dataContext.BlogPosts
            .Include(u => u.CreatedBy)
            .SingleOrDefaultAsync(bp => bp.Id == Id, cancellationToken);
        }
        public async Task<BlogPost?> GetByFriendlyUrlAsync(string friendlyUrl, CancellationToken cancellationToken = default)
        {
            return await _dataContext.BlogPosts
            .Include(u => u.CreatedBy)
            .SingleOrDefaultAsync(bp => bp.FriendlyUrl == friendlyUrl, cancellationToken);
        }

        public async Task<bool> IsExist(string title, CancellationToken cancellationToken = default)
        {
            bool isExist = await _dataContext.BlogPosts.AnyAsync(bp => bp.Title == title, cancellationToken);

            if (isExist)
            {
                _logger.LogInformation("Post with title: {PostTitle} exists!", title);
            }
            else
            {
                _logger.LogWarning("Post with title: {PostTitle} does not exist!", title);
            }

            return isExist;
        }

        public async Task UpdateAsync(BlogPost blogPost, CancellationToken cancellationToken = default)
        {
            await Task.FromResult(_dataContext.BlogPosts.Update(blogPost));
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
