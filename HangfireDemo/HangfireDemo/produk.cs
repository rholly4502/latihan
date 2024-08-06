namespace HangfireDemo
{
    public class produk
    {
        public static void SendEmail(string email)
        {
            Console.WriteLine($"Mengirim email ke {email} pada {DateTime.Now}");
            // Tambahkan logika pengiriman email di sini
        }
    }
}
