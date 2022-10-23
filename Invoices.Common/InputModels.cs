namespace Invoices.Common
{
    public class QueryModel
    {
        /// <summary>
        /// structure: field comparer comparand
        /// example: creation >= 2020-10-13T15:30
        /// </summary>
        public string[]? Filters { get; set; }
        public QuerySortModel[]? Sort { get; set; }
        public QueryPagingModel Paging { get; set; }
    }

    public class QuerySortModel
    {
        public string Field { get; set; }
        public bool Ascending { get; set; }
    }

    public class QueryPagingModel
    {
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; }
    }
}