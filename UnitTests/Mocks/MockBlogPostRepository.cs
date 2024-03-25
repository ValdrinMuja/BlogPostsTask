using Domain.BlogPosts;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Mocks
{
    public class MockBlogPostRepository
    {
        public static Mock<IBlogPostRepository> GetBlogPosts()
        {
            var blogPosts = new List<BlogPost>
            {
                new BlogPost(Guid.NewGuid(),"Title test 1","Title-test-1","Content test 1",DateTime.UtcNow,null),
                new BlogPost(Guid.NewGuid(),"Title test 2","Title-test-2","Content test 2",DateTime.UtcNow,null),
                new BlogPost(Guid.NewGuid(),"Title test 3","Title-test-3","Content test 3",DateTime.UtcNow,null),
            };

            var mockRepo=new Mock<IBlogPostRepository>();


            mockRepo.Setup(r => r.GetAll(It.IsAny<CancellationToken>())).Returns(blogPosts.AsAsyncQueryable());

            mockRepo.Setup(r => r.CreateAsync(It.IsAny<BlogPost>(), It.IsAny<CancellationToken>()))
                .Returns((BlogPost blogPost, CancellationToken cancellationToken) =>
                {
                    blogPosts.Add(blogPost);
                    return Task.CompletedTask;
                });

            return mockRepo;
        }
    }

    public static class AsyncQueryableExtensions
    {
        public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> enumerable)
        {
            return new TestAsyncEnumerable<T>(enumerable);
        }
    }

    public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestAsyncQueryProvider<T>(this); }
        }
    }

    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            var expectedResultType = typeof(TResult).GetGenericArguments()[0];
            var executionResult = typeof(IQueryProvider)
                                  .GetMethod(
                                      name: nameof(IQueryProvider.Execute),
                                      genericParameterCount: 1,
                                      types: new[] { typeof(Expression) })
                                  .MakeGenericMethod(expectedResultType)
                                  .Invoke(this, new[] { expression });

            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
                                       ?.MakeGenericMethod(expectedResultType)
                                       .Invoke(null, new[] { executionResult });
        }
    }

    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return new ValueTask();
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_inner.MoveNext());
        }
    }
}
