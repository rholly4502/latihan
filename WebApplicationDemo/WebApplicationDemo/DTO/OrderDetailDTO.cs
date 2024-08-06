namespace WebApplicationDemo.DTO
{
    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
}
