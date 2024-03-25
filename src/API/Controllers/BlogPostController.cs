using Application.Abstractions;
using Application.BlogPosts;
using Application.BlogPosts.Commands.Create;
using Application.BlogPosts.Commands.Delete;
using Application.BlogPosts.Commands.Import;
using Application.BlogPosts.Commands.Update;
using Application.BlogPosts.Queries.GetAll;
using Infrastructure.Hangfire;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BlogPostController : ApiController
    {
        private readonly ICurrentUserService _currentUserService;
        public BlogPostController(ISender sender, ICurrentUserService currentUserService)
         : base(sender)
        {
            _currentUserService = currentUserService;
        }

        [Authorize(Policy = "ViewAll")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] GetBlogPostsRequest request, CancellationToken cancellationToken)
        {
            GetBlogPostsQuery query = request.Adapt<GetBlogPostsQuery>();

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result.Value.Adapt<PagedList<BlogPostResponse>>());
        }

        [Authorize(Policy = "CreatePost")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateRequest request, CancellationToken cancellationToken)
        {
            CreateCommand command = request.Adapt<CreateCommand>();

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value.Adapt<BlogPostResponse>());
        }

        [Authorize(Policy = "UpdatePost")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateRequest request, CancellationToken cancellationToken)
        {
            UpdateCommand command = request.Adapt<UpdateCommand>();

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value.Adapt<BlogPostResponse>());
        }

        [Authorize(Policy = "DeletePost")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest request, CancellationToken cancellationToken)
        {
            DeleteCommand command = request.Adapt<DeleteCommand>();

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }

        [Authorize(Policy = "ImportPost")]
        [HttpPost("import")]
        public async Task<IActionResult> ImportBlogPosts([FromBody] ImportRequest request, CancellationToken cancellationToken)
        {
            //TODO: 
            string userId = _currentUserService.UserId;
            ImportCommand command= request.Adapt<ImportCommand>();
            var updatedCommand = command with { UserId = userId };
            _sender.Enqueue(updatedCommand, cancellationToken);

            return Ok();
        }
    }
}
