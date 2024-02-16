
namespace WebScraping.Infra.Models
{
    public class Result<T>
    {
        public int Page {  get; set; }
        public int Quantity { get; set; }
        public long Total { get; set; }
        public long TotalPages { get; set; }
        public ICollection<T>? Data { get; set; }
    }
}
