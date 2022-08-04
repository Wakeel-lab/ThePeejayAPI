using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThePeejayAPI.Services
{
    public class RandomNumber : Random
    {
        private readonly Random _random = new Random();
        public int GenerateRandomNumber(int minSeedRandom, int maxSeedRandom)
        {
            return _random.Next(minSeedRandom, maxSeedRandom);
        }
    }
}
