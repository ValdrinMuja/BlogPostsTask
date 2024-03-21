using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BlogPosts
{
    public interface IBlogPostRepository
    {
        IQueryable<BlogPost> GetAll(CancellationToken cancellationToken = default);
        Task<bool> IsExist(string name, CancellationToken cancellationToken = default);
        Task<BlogPost?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<BlogPost?> GetByFriendlyUrlAsync(string friendlyUrl, CancellationToken cancellationToken = default);
        Task CreateAsync(BlogPost blogPost, CancellationToken cancellationToken = default);
        Task UpdateAsync(BlogPost blogPost, CancellationToken cancellationToken = default);
        Task DeleteAsync(BlogPost blogPost, CancellationToken cancellationToken = default);
    }
}
