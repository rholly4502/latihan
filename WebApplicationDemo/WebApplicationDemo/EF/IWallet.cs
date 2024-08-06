using WebApplicationDemo.RapidModels;

namespace WebApplicationDemo.EF
{
    public interface IWallet : ICrud<Wallet>
    {
        decimal GetWalletSaldo(int walletId);
        void UpdateWalletSaldo(int walletId, decimal amount);
    }
}
