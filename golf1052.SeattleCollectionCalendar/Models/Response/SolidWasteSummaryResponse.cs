using System;
using System.Collections.Generic;
using System.Linq;

namespace golf1052.SeattleCollectionCalendar.Models.Response
{
    public class SolidWasteSummaryResponse : BaseResponse
    {
        public AccountContext AccountContext { get; set; } = null!;
        public AccountInfo AccountSummaryType { get; set; } = null!;

        public List<ServiceItem>? GetServiceItem(SolidWasteType solidWasteType)
        {
            if (AccountSummaryType.SwServices == null)
            {
                return null;
            }

            return AccountSummaryType.SwServices
                .SelectMany(sws => sws.Services)
                .Where(s => solidWasteType switch
                {
                    SolidWasteType.Garbage => "Garbage".Equals(s.Description, StringComparison.CurrentCultureIgnoreCase),
                    SolidWasteType.Recycle => "Recycle".Equals(s.Description, StringComparison.CurrentCultureIgnoreCase),
                    SolidWasteType.FoodYardWaste => "Food/Yard Waste".Equals(s.Description, StringComparison.CurrentCultureIgnoreCase),
                    _ => false
                })
                .ToList();
        }
    }
}
