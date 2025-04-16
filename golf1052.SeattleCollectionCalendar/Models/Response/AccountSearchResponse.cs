using System;
using System.Collections.Generic;
using System.Text;

namespace golf1052.SeattleCollectionCalendar.Models.Response
{
    public class AccountSearchResponse : BaseResponse
    {
        public AccountInfo Account { get; set; } = null!;
    }
}
