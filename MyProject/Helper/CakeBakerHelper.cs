using Newtonsoft.Json;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Project.Helper
{
    public class CakeBakerHelper
    {
        public List<ProductModel> GetProducts()
        {
            var response = GetProductsAPI();
            
            var mappedResult = JsonConvert.DeserializeObject<List<ProductModel>>(response.RawJson);
            return mappedResult;
        }

        public ProductModel AddProduct(ProductModel product)
        {
            var jsonRequest = product.Jsonify();
            var response = AddProductAPI(jsonRequest);
            var mappedResult = JsonConvert.DeserializeObject<ProductModel>(response.RawJson);
            return mappedResult;
        }

        private ServiceResponse GetProductsAPI()
        {
            ServiceResponse serviceResponse = null;
            using (var client = new HttpClient())
            {
                string url = "https://localhost:44334/api/CakeDetails";
                client.BaseAddress = new Uri(url);
                //HTTP GET
                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                using (HttpResponseMessage message = client.GetAsync(url).Result)
                {
                    serviceResponse = new ServiceResponse
                    {
                        RawJson = message.Content.ReadAsStringAsync().Result,
                        IsSuccessful = message.IsSuccessStatusCode
                    };
                }
            }
            return serviceResponse;
        }
        private ServiceResponse AddProductAPI(string jsonProduct)
        {
            ServiceResponse serviceResponse = null;
            using (var client = new HttpClient())
            {
                string url = "http://localhost:60464/api/PostProduct";
                client.BaseAddress = new Uri(url);
                //HTTP GET
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                using (HttpContent content = new StringContent(jsonProduct, UTF8Encoding.UTF8, "application/json"))
                {
                    /*below line is to bypass security certificate issue , neede only for domain*/
                    //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;                    
                    using (HttpResponseMessage message = client.PostAsync(url, content).Result)
                    {
                        serviceResponse = new ServiceResponse
                        {
                            RawJson = message.Content.ReadAsStringAsync().Result,
                            IsSuccessful = message.IsSuccessStatusCode
                        };
                    }
                }
            }
            return serviceResponse;
        }

    }
}
