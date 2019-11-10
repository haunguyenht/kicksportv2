using KickSport.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KickSport.Data.Models
{
    public class Log : BaseModel<int>
    {
        public int EventId { get; set; }

        public string EventName { get; set; }

        public string LogLevel { get; set; }

        public string StackTrace { get; set; }

        public string Message { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
