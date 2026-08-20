using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TeamPortal.NET.Models.ViewModel
{
    public class PaginatedListVM<T>: List<T>
    {
        public int PageIndex { get;private set; }
        public int TotalPage { get; private set; }
        public PaginatedListVM(List<T> items, int pageIndex, int pagesize ,int count)
        {
            PageIndex = pageIndex;
            TotalPage = (int)Math.Ceiling(count / (double)pagesize);
            this.AddRange(items);
        }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPage;

        public static async Task<PaginatedListVM<T>> CreateAsync(IQueryable<T> source,int pageIndex,int pagesize)
        {
            var count = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(source);
            var items = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
        source.Skip((pageIndex - 1) * pagesize).Take(pagesize));
            return new PaginatedListVM<T>(items, pageIndex, pagesize, count);
        }
    }
}
