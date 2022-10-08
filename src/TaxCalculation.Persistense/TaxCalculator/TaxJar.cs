using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.Retry;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.Enumeration;
using TaxCalculation.Core.Model;
using TaxCalculation.Core.Strategy;
using TaxCalculation.Persistent.Utilities;

namespace TaxCalculation.Persistent.Calculator
{
    public class TaxJar : ITaxCalculatorStrategy
    {
        private string _tarJarBaseURL = APIConnectionDetails.TaxJarBaseURL;
        private string _taxJarApiKey = APIConnectionDetails.TaxJarAPIKey;

        private int maxRetry = 3;
        private int retryCount = 0;
        private int retryTimeInterval = 2;
        private AsyncRetryPolicy _retryPolicy;

        public TaxJar()
        {
            _retryPolicy = Policy.Handle<HttpRequestException>().WaitAndRetryAsync(maxRetry, times =>
            {
                return TimeSpan.FromSeconds(retryTimeInterval);
            });
        }


        public bool IsApplicable(TaxCalculatorOption option)
        {
            return option == TaxCalculatorOption.Tax_Jar;
        }

        public async Task<Tax> CalculateTax(Order data)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
            var client = new RestClient(_tarJarBaseURL);
           // client.Timeout = -1;
            var request = new RestRequest("/api/v1/transactions");

            var serialize = JsonConvert.SerializeObject(data);

            //Settings for Conversion of Object Property to SnakeCase
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

                /*    var serializeToSnakeCase = JsonConvert.SerializeObject(cashPickUp, new JsonSerializerSettings
                   {
                       ContractResolver = contractResolver,
                       Formatting = Formatting.Indented
                   });


                   request.AddParameter("application/json", serializeToSnakeCase, ParameterType.RequestBody); */


                request.AddHeader("Authorization", "Bearer " + _taxJarApiKey);

                IRestResponse response = await client.ExecuteAsync(request, Method.GET);

                if (response.IsSuccessful)
                {
                    var result = JsonConvert.DeserializeObject<string>(response.Content);

                    return new Tax();
                }
                else
                {
                    if (retryCount != maxRetry)
                    {
                        retryCount += 1;
                        //  Log.Information("Request failed in {url}. Retry count: {retryCount}. Next Retry: {retryTime}", _cashPickUpUrl, retryCount, retryTime);
                        throw new HttpRequestException();

                    }
                    else
                    {
                        //throw new CashPickUpResponseException(response.ErrorMessage, response.ErrorException);
                        throw new HttpRequestException();
                    }
                }
            });
        }

        public async Task<TaxRate> GetTaxRateByLocation(Location data)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var client = new RestClient(_tarJarBaseURL);

                var request = new RestRequest("v2/rates/{zipId}");

                var serialize = JsonConvert.SerializeObject(data);

                request.AddHeader("Authorization", "Bearer " + _taxJarApiKey);

                request.AddParameter("zipId", data.Zip, ParameterType.UrlSegment);

                if (!string.IsNullOrEmpty(data.Country)) request.AddParameter("country", data.Country, ParameterType.QueryString);
                if (!string.IsNullOrEmpty(data.State)) request.AddParameter("state", data.State, ParameterType.QueryString);
                if (!string.IsNullOrEmpty(data.City)) request.AddParameter("city", data.City, ParameterType.QueryString);
                if (!string.IsNullOrEmpty(data.Street)) request.AddParameter("street", data.Street, ParameterType.QueryString);


                IRestResponse response = await client.ExecuteAsync(request, Method.GET);

                if (response.IsSuccessful)
                {
                    var result = JsonConvert.DeserializeObject<TaxRate>(response.Content);

                    return result;
                }
                else
                {
                    if (retryCount != maxRetry)
                    {
                        retryCount += 1;
                        //  Log.Information("Request failed in {url}. Retry count: {retryCount}. Next Retry: {retryTime}", _cashPickUpUrl, retryCount, retryTime);
                        throw new HttpRequestException();

                    }
                    else
                    {
                        //throw new CashPickUpResponseException(response.ErrorMessage, response.ErrorException);
                        throw new HttpRequestException();
                    }
                }
            });
        }

       
    }

}
