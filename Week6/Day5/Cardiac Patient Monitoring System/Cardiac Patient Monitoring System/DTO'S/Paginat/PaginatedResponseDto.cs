namespace Cardiac_Patient_Monitoring_System.DTO_S.Paginat
{
    public class PaginatedResponseDto<T>
    {
        public IEnumerable<T> Data { get; set; } = [];
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
