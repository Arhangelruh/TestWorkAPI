using Microsoft.EntityFrameworkCore;

namespace TestWorkAPI.API.Helpers
{ 
    /// <summary>
    /// Helper for show metadata. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageList<T>
    {
        private PageList(List<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public List<T> Items { get; }

        public int Page { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public bool HasNextPage => Page * PageSize < TotalCount;

        public bool HasPreviousPage => Page > 1;

        public static async Task<PageList<T>> CreatePageAsync(IQueryable<T> qwery, int page, int pageSize) {
            var totalCount = await qwery.CountAsync();
            var items = await qwery.Skip(page-1).Take(pageSize).ToListAsync();

            return new(items, page, pageSize, totalCount);
        }

    }
}
