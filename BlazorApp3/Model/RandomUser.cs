
using BlazorApp3.Model.Operation;
using BlazorApp3.Model.Categories;

namespace BlazorApp3.Model
{
    public static class RandomUser
    {
        private static Random rand = new Random();

        private static GaussianRandom gRand = new GaussianRandom();
        public static ApplicationUser NewUser(string login)
        {
            ApplicationUser user = new ApplicationUser(login);

            int purchaseAmount = (int)rand.NextInt64(50, 500);
            int accrualAmount = (int)rand.NextInt64(50, 500);

            for (int i = 0; i < purchaseAmount; i++) 
            {
                DateTime date = DateTime.UtcNow.AddDays(-rand.Next(0, 500));
                Purchase newPurchase = new Purchase(i.ToString(), (float)Math.Abs(gRand.NextGaussian(1000, 100)), (Category)rand.Next(0,4), date);
                user.Purchases.Add(newPurchase);
            }

            for (int i = 0; i < accrualAmount; i++)
            {
                DateTime date = DateTime.UtcNow.AddDays(-rand.Next(0, 500));
                Accrual newAccrual = new Accrual(i.ToString(), (float)Math.Abs(gRand.NextGaussian(1000, 100)), date);
                user.Accruals.Add(newAccrual);
            }

            return user;

        }


    }
}

public class GaussianRandom
{
    private readonly Random _random;
    private bool _hasSpare;
    private double _spare;

    public GaussianRandom()
    {
        _random = new Random();
        _hasSpare = false;
    }

    public double NextGaussian(double mean = 0, double stdDev = 1)
    {
        if (_hasSpare)
        {
            _hasSpare = false;
            return _spare * stdDev + mean;
        }

        double u, v, s;
        do
        {
            u = _random.NextDouble() * 2.0 - 1.0;
            v = _random.NextDouble() * 2.0 - 1.0;
            s = u * u + v * v;
        } while (s >= 1.0 || s == 0);

        s = Math.Sqrt(-2.0 * Math.Log(s) / s);
        _spare = v * s;
        _hasSpare = true;

        return mean + stdDev * u * s;
    }
}
