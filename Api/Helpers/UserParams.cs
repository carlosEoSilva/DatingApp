namespace Api.Helpers
{
    public class UserParams
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

        public string  CurrentUsername { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; }= 18;
        public int MaxAge { get; set; }= 100;
        public string OrderBy { get; set; }= "lastActive";
    }
}