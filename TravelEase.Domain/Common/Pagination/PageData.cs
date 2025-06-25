namespace TravelEase.Domain.Common.Pagination
{
    public class PageData
    {
        public int TotalItems { get; }
        public int PageSize { get; }
        public int CurrentPage { get; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public PageData(int totalItems, int pageSize, int currentPage)
        {
            TotalItems = totalItems;
            PageSize = pageSize;
            CurrentPage = currentPage;
        }
    }
}