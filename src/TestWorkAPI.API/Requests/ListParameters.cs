namespace TestWorkAPI.API.Requests
{
    public class ListParameters
    {
            const int maxPageSize = 50;

        /// <summary>
        /// Current page number.
        /// </summary>
            public int PageNumber { get; set; } = 1;

            private int _pageSize = 10;

        /// <summary>
        /// Count items on page.
        /// </summary>
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

        /// <summary>
        /// Search filter.
        /// </summary>
           public string? searchTeam { get; set; }

        /// <summary>
        /// Sort by column.
        /// </summary>
           public string? sortColumn { get; set; }  

        /// <summary>
        /// Sort by desc or asc.
        /// </summary>
           public string? sortOrder { get; set; }


    }
}
