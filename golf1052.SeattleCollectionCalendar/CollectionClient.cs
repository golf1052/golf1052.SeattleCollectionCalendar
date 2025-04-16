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

        /// <summary>
        /// Find an address.
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>An AddressSearchResponse</returns>
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

        /// <summary>
        /// Find an address.
        /// </summary>
        /// <param name="addressRequest">The address request</param>
        /// <returns>An AddressSearchResponse</returns>
        public async Task<AddressSearchResponse> FindAddress(AddressRequest addressRequest)
        {
            Url url = new Url(BaseUrl).AppendPathSegments("serviceorder", "findaddress");
            HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(url, addressRequest, jsonSerializerOptions);
            return await ProcessResponse<AddressSearchResponse>(responseMessage);
        }

        /// <summary>
        /// Find an account for the given AddressInfo from an AddressSearchResponse.
        /// </summary>
        /// <param name="addressInfo">An AddressInfo from an AddressSearchResponse.</param>
        /// <returns>An AccountSearchResponse</returns>
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

        /// <summary>
        /// Find an account.
        /// </summary>
        /// <param name="addressRequest">An AddressRequest</param>
        /// <returns>An AccountSearchResponse</returns>
        public async Task<AccountSearchResponse> FindAccount(AddressRequest addressRequest)
        {
            Url url = new Url(BaseUrl).AppendPathSegments("serviceorder", "findAccount"); // findAccount is case sensitive
            HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(url, addressRequest, jsonSerializerOptions);
            return await ProcessResponse<AccountSearchResponse>(responseMessage);
        }

        /// <summary>
        /// Get authorization for a guest user.
        /// </summary>
        /// <returns>A GuestAuthResponse</returns>
        public async Task<GuestAuthResponse> GetGuestAuth()
        {
            Url url = new Url(BaseUrl).AppendPathSegments("auth", "guest");
            HttpResponseMessage responseMessage = await httpClient.PostAsync(url, null);
            return await ProcessResponse<GuestAuthResponse>(responseMessage);
        }

        /// <summary>
        /// Get the solid waste summary for the given account info.
        /// </summary>
        /// <param name="accountInfo">An AccountInfo from an AccountSearchResponse</param>
        /// <param name="authInfo">A GuestAuthResponse</param>
        /// <returns>A SolidWasteSummaryResponse</returns>
        /// <exception cref="Exception">Thrown if AccountNumber is null in AccountInfo</exception>
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

        /// <summary>
        /// Get the solid waste summary for a given account.
        /// </summary>
        /// <param name="accountRequest">An AccountRequest</param>
        /// <param name="authInfo">A GuestAuthResponse</param>
        /// <returns>A SolidWasteSummaryResponse</returns>
        public async Task<SolidWasteSummaryResponse> SolidWasteSummary(AccountRequest accountRequest, GuestAuthResponse authInfo)
        {
            Url url = new Url(BaseUrl).AppendPathSegments("guest", "swsummary");
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authInfo.TokenType, authInfo.AccessToken);
            requestMessage.Content = JsonContent.Create(accountRequest, options: jsonSerializerOptions);
            HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);
            return await ProcessResponse<SolidWasteSummaryResponse>(responseMessage);
        }

        /// <summary>
        /// Get the solid waste calendar for the given account info.
        /// </summary>
        /// <param name="accountInfo">An AccountInfo from a SolidWasteSummaryResponse</param>
        /// <param name="authInfo">A GuestAuthResponse</param>
        /// <returns>A SolidWasteCalendarResponse</returns>
        /// <exception cref="Exception">Thrown if AccountNumber is null in AccountInfo</exception>
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

        /// <summary>
        /// Get the solid waste calendar for a given account.
        /// </summary>
        /// <param name="accountRequest">An AccountRequest</param>
        /// <param name="authInfo">A GuestAuthResponse</param>
        /// <returns>A SolidWasteCalendarResponse</returns>
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
