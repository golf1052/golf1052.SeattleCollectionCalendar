using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Flurl;
using golf1052.SeattleCollectionCalendar.Models.Request;
using golf1052.SeattleCollectionCalendar.Models.Response;

namespace golf1052.SeattleCollectionCalendar
{
    public enum SolidWasteType
    {
        Garbage,
        Recycle,
        FoodYardWaste
    }
    
    public class CollectionClient
    {
        private const string BaseUrl = "https://myutilities.seattle.gov/rest/";

        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public CollectionClient() : this(new HttpClient())
        {
        }

        public CollectionClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
        }

        public async Task<AddressSearchResponse> FindAddress(string address)
        {
            AddressRequest addressRequest = new AddressRequest()
            {
                Address = new AddressSearchObject()
                {
                    AddressLine1 = address
                }
            };
            return await FindAddress(addressRequest);
        }

        public async Task<AddressSearchResponse> FindAddress(AddressRequest addressRequest)
        {
            Url url = new Url(BaseUrl).AppendPathSegments("serviceorder", "findaddress");
            HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(url, addressRequest, jsonSerializerOptions);
            return await ProcessResponse<AddressSearchResponse>(responseMessage);
        }

        public async Task<AccountSearchResponse> FindAccount(AddressInfo addressInfo)
        {
            AddressRequest addressRequest = new AddressRequest()
            {
                Address = new AddressSearchObject()
                {
                    PremCode = addressInfo.PremCode
                }
            };
            return await FindAccount(addressRequest);
        }

        public async Task<AccountSearchResponse> FindAccount(AddressRequest addressRequest)
        {
            Url url = new Url(BaseUrl).AppendPathSegments("serviceorder", "findAccount"); // findAccount is case sensitive
            HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(url, addressRequest, jsonSerializerOptions);
            return await ProcessResponse<AccountSearchResponse>(responseMessage);
        }

        public async Task<GuestAuthResponse> GetGuestAuth()
        {
            Url url = new Url(BaseUrl).AppendPathSegments("auth", "guest");
            HttpResponseMessage responseMessage = await httpClient.PostAsync(url, null);
            return await ProcessResponse<GuestAuthResponse>(responseMessage);
        }

        public async Task<SolidWasteSummaryResponse> SolidWasteSummary(AccountInfo accountInfo, GuestAuthResponse authInfo)
        {
            if (accountInfo.AccountNumber == null)
            {
                throw new Exception("No account exists for this address");
            }

            AccountRequest accountRequest = new AccountRequest()
            {
                AccountContext = new Models.Request.AccountContext()
                {
                    AccountNumber = accountInfo.AccountNumber
                },
                CustomerId = authInfo.User.CustomerId
            };
            return await SolidWasteSummary(accountRequest, authInfo);
        }

        public async Task<SolidWasteSummaryResponse> SolidWasteSummary(AccountRequest accountRequest, GuestAuthResponse authInfo)
        {
            Url url = new Url(BaseUrl).AppendPathSegments("guest", "swsummary");
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authInfo.TokenType, authInfo.AccessToken);
            requestMessage.Content = JsonContent.Create(accountRequest, options: jsonSerializerOptions);
            HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);
            return await ProcessResponse<SolidWasteSummaryResponse>(responseMessage);
        }

        public async Task<SolidWasteCalendarResponse> SolidWasteCalendar(AccountInfo accountInfo, GuestAuthResponse authInfo)
        {
            if (accountInfo.AccountNumber == null)
            {
                throw new Exception("No account exists for this address");
            }

            AccountRequest accountRequest = new AccountRequest()
            {
                AccountContext = new Models.Request.AccountContext()
                {
                    AccountNumber = accountInfo.AccountNumber,
                    CompanyCd = accountInfo.CompanyCd,
                    PersonId = accountInfo.PersonId
                },
                CustomerId = authInfo.User.CustomerId,
                ServicePoints = accountInfo.SwServices!.SelectMany(s => s.Services!).Select(s => s.ServicePointId!).ToList()
            };
            return await SolidWasteCalendar(accountRequest, authInfo);
        }

        public async Task<SolidWasteCalendarResponse> SolidWasteCalendar(AccountRequest accountRequest, GuestAuthResponse authInfo)
        {
            Url url = new Url(BaseUrl).AppendPathSegments("solidwastecalendar");
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authInfo.TokenType, authInfo.AccessToken);
            requestMessage.Content = JsonContent.Create(accountRequest, options: jsonSerializerOptions);
            HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);
            return await ProcessResponse<SolidWasteCalendarResponse>(responseMessage);
        }

        private async Task<T> ProcessResponse<T>(HttpResponseMessage responseMessage) where T : class
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception(responseMessage.StatusCode.ToString());
            }

            string s = await responseMessage.Content.ReadAsStringAsync();
            T? response = await responseMessage.Content.ReadFromJsonAsync<T>(jsonSerializerOptions);
            if (response == null)
            {
                throw new Exception("Could not deserialize response");
            }
            return response;
        }
    }
}
