using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models
{
    public class BookedSlot
    {
        public DateTime Start { get; set; } // 預約開始時間
        public DateTime End { get; set; }   // 預約結束時間
        public TimeSpan Duration { get; set; }
    }
}