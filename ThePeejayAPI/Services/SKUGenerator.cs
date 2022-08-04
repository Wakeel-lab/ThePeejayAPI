using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePeejayAPI.Models;

namespace ThePeejayAPI.Services
{
    public static class SKUGenerator
    {
        public static string GeneratesSKU(this Product product)
        {
            if (product.Name == product.Name)
            {
                string strProductName = product.Name;
                string subStrProductName = "";
                RandomNumber randomSKU = new RandomNumber();
                subStrProductName = strProductName.Substring(0, 2) + randomSKU.GenerateRandomNumber(-21474, 21474).ToString();
                return subStrProductName;

            }
            else
                return "";

        }

    }
}
