using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace golf1052.SeattleCollectionCalendar.Models.Response
{
    public class SolidWasteCalendarResponse : BaseResponse
    {
        public CalendarInfo Calendar { get; set; } = null!;

        public DateTime? GetLastPickup(string servicePointId)
        {
            return GetSpecificPickup(servicePointId, "LP");
        }

        public DateTime? GetNextPickup(string servicePointId)
        {
            return GetSpecificPickup(servicePointId, "NP");
        }

        public List<DateTime>? GetAllPickups(string servicePointId)
        {
            if (Calendar.CalendarItems == null)
            {
                return null;
            }

            if (Calendar.CalendarItems.ContainsKey(servicePointId))
            {
                List<string>? dates = Calendar.CalendarItems[servicePointId].Deserialize<List<string>>();
                if (dates == null || dates.Count == 0)
                {
                    return null;
                }

                return dates.Select(date => DateTime.Parse(date)).ToList();
            }

            return null;
        }

        private DateTime? GetSpecificPickup(string servicePointId, string pickupType)
        {
            if (Calendar.CalendarItems == null)
            {
                return null;
            }

            foreach (var calendarItem in Calendar.CalendarItems)
            {
                if (calendarItem.Key.StartsWith(servicePointId) && calendarItem.Key.EndsWith(pickupType))
                {
                    List<string>? dates = calendarItem.Value.Deserialize<List<string>>();
                    if (dates == null || dates.Count == 0)
                    {
                        return null;
                    }

                    return DateTime.Parse(dates.First());
                }
            }

            return null;
        }
    }

    public class CalendarInfo
    {
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? CalendarItems { get; set; }
    }
}
