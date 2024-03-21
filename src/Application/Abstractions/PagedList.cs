using System;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions;

public class PagedList<T>
{
    public PagedList()
    {
    }

    public PagedList(IEnumerable<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public IEnumerable<T> Items { get; init; }

	public int Page { get; init; }

	public int PageSize { get; init; }

	public int TotalCount { get; init; }

	public bool HasNextPage => Page * PageSize < TotalCount;

	public bool HasPreviousPage => Page > 1 && TotalCount >= 1;

	public static async Task<PagedList<T>> ApplyAsync(IQueryable<T> query, int page, int pageSize)
    {
        var totalCount = await query.CountAsync();

        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new(items, page, pageSize, totalCount);
    }
}

