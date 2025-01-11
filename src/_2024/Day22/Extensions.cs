namespace _2024.Day22;

static class Extensions
{
    public static long Mix(this long secret, long num) => secret ^ num;
    public static long Prune(this long num) => num % 16777216;

    public static IEnumerable<long> NextSecret(this long num)
    {
        long next = num;
        
        while (true)
        {
            next = next.Mix(next * 64).Prune();
            next = next.Mix((long)Math.Floor(next / 32d)).Prune();
            next = next.Mix(next * 2048).Prune();
            
            yield return next;
        }
    }
}