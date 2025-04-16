# Seattle Collection Calendar

An API version of [Seattle's View Collection Calendar website](https://myutilities.seattle.gov/eportal/#/accountlookup/calendar).

## Usage

```csharp
using golf1052.SeattleCollectionCalendar;
using System.Linq;

// Create collection client. HttpClient is an optional argument to the constructor.
CollectionClient collectionClient = new CollectionClient();

// Find an address
AddressSearchResponse addressSearchResponse = await collectionClient.FindAddress("660 4th Ave");

// Find an account based upon the found address
AccountSearchResponse accountSearchResponse = await collectionClient.FindAccount(addressSearchResponse.Address.First());

// Get guest authorization, this is required for the next set of APIs
GuestAuthResponse guestAuthResponse = await collectionClient.GetGuestAuth();

// Get solid waste summary
SolidWasteSummaryResponse solidWasteSummaryResponse = await collectionClient.SolidWasteSummary(accountSearchResponse.Account, guestAuthResponse);

// There's helper methods to get service items from the SolidWasteSummaryResponse
ServiceItem garbageService = solidWasteSummaryResponse.GetServiceItem(SolidWasteType.Garbage).First();

// Get solid waste calendar
SolidWasteCalendarResponse solidWasteCalendarResponse = await collectionClient.SolidWasteCalendar(solidWasteSummaryResponse.AccountSummaryType, guestAuthResponse);

// Now use the service item to get the calendar information for it
// Next pickup
DateTime nextPickup = solidWasteCalendarResponse.GetNextPickup(garbageService.ServicePointId);

// Last pickup
DateTime lastPickup = solidWasteCalendarResponse.GetLastPickup(garbageService.ServicePointId);

// All pickups (should return all pickups for the current year and year after)
List<DateTime> allPickups = solidWasteCalendarResponse.GetAllPickups(garbageService.ServicePointId);
```
