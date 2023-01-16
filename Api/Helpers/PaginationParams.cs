namespace Api.Helpers
{
    public class PaginationParams
    {
       //-quantidade de itens por página.
        private const int MaxPageSize= 50;
        //-números da página com valor inicial 1.
        public int PageNumber { get; set; }= 1;
        //-quantidade de itens por página definida pelo usuário.
        public int _pageSize= 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize= (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}