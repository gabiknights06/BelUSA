using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.Retry;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Core.Enumeration;
using TaxCalculation.Core.Model;
using TaxCalculation.Core.Strategy;
using TaxCalculation.Persistent.Exceptions;
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
                var request = new RestRequest("/v2/taxes");

                var serialize = JsonConvert.SerializeObject(data);

                //Settings for Conversion of Object Property to SnakeCase
                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };

                var serializeToSnakeCase = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented
                });              

                request.AddHeader("Authorization", "Bearer " + _taxJarApiKey);

                request.AddParameter("application/json", serializeToSnakeCase, ParameterType.RequestBody);

                IRestResponse response = await client.ExecuteAsync(request, Method.POST);

                if (response.IsSuccessful)
                {
                    var result = JsonConvert.DeserializeObject<Tax>(response.Content);

                    return result;
                }
                else
                {
                     var result = JsonConvert.DeserializeObject<TaxJarErrorResponseDTO>(response.Content);

                     if (retryCount != maxRetry)
                    {
                        retryCount += 1;
                         Log.Error("Request failed in {url}. Retry count: {retryCount}. Next Retry: {retryTimeInterval} second/s. {status} - {error} {detail}", _tarJarBaseURL, retryCount, retryTimeInterval, result.Status, result.Error, result.Detail);
                         throw new HttpRequestException();

                     }
                    else
                    {
                        throw new BadGateWayException($@"{result.Status} {result.Error} - {result.Detail}", response.ErrorException);
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
                    var result = JsonConvert.DeserializeObject<TaxJarErrorResponseDTO>(response.Content);

                    if (retryCount != maxRetry)
                    {
                        retryCount += 1;
                        Log.Error("Request failed in {url}. Retry count: {retryCount}. Next Retry: {retryTimeInterval} second/s. {status} - {error} {detail}", _tarJarBaseURL, retryCount, retryTimeInterval, result.Status, result.Error, result.Detail);
                        throw new HttpRequestException();
                    }
                    else
                    {
                        throw new BadGateWayException($@"{result.Status} {result.Error} - {result.Detail}", response.ErrorException);
                    }
                }
            });
        }

       
    }

}
