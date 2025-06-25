namespace TravelEase.Domain.Common.Models.PaginationModels
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public PageData PageData { get; }

        public PaginatedList(List<T> items, PageData pageData)
        {
            Items = items;
            PageData = pageData;
        }
    }
}