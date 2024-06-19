namespace FiorelloAPI.Helpers
{
    public class Paginate<T>
    {
        public IEnumerable<T> Datas { get; private set; }
        public int TotalPages { get; private set; }
        public int CurrentPage { get; private set; }

        public bool HasNext => CurrentPage < TotalPages;
        public bool HasPrevious => CurrentPage > 1;

        public Paginate(IEnumerable<T> datas, int totalPages, int currentPage)
        {
            Datas = datas;
            TotalPages = totalPages;
            CurrentPage = currentPage;
        }
    }
}
