using BluzelleCSharp.Exceptions;

namespace BluzelleCSharp.Models
{
    public class LeaseInfo
    {
        public LeaseInfo()
        {
        }

        public LeaseInfo(long seconds)
        {
            Seconds = seconds;
        }

        /**
         * <summary>Generates lease info in blocks from user input via separated time parameters</summary>
         */
        public LeaseInfo(long days, long hours, long minutes, long seconds)
        {
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }

        public long Days { get; set; }
        public long Hours { get; set; }
        public long Minutes { get; set; }
        public long Seconds { get; set; }

        public string Value
        {
            get
            {
                if (Days == 0 && Hours == 0 && Minutes == 0 && Seconds == 0) Days = 10;
                var res = (Days * 24 * 60 * 60 + Hours * 60 * 60 + Minutes * 60 + Seconds)
                          / BluzelleApi.BlockTimeInSeconds;
                if (res < 0) throw new InvalidLeaseException();
                return res.ToString();
            }
            set { }
        }
    }
}