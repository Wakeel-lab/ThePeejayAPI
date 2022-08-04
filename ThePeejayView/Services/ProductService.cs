using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ThePeejayAPI.Models;

namespace ThePeejayView.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<Product> CreateProduct(Product product)
        {
            try
            {
                var str = JsonConvert.SerializeObject(product);
                StringContent content = new StringContent(str, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(new Uri("https://localhost:44339/api/administrator/createproduct/"), content);

                var resp = await response.Content.ReadAsStringAsync();

                if(response.IsSuccessStatusCode)
                {
                    var productReturned = JsonConvert.DeserializeObject<Product>(resp);
                    return productReturned;
                }
                return null;

            }
            catch(Exception)
            {
                throw;
            }
            
        }
    }
}
