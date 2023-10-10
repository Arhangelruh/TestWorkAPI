namespace TestWorkAPI.API.Requests
{
    public class ListParameters
    {
            const int maxPageSize = 50;
            public int PageNumber { get; set; } = 1;

            private int _pageSize = 10;
            public int PageSize
            {
                get
                {
                    return _pageSize;
                }
                set
                {
                    _pageSize = (value > maxPageSize) ? maxPageSize : value;
                }
            }

           public string? searchTeam { get; set; }
           public string? sortColumn { get; set; }  
           public string? sortOrder { get; set; }


    }
}
