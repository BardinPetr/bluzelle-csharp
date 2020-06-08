using BluzelleCSharp.Utils;

namespace BluzelleCSharp.Models
{
    public class LeaseInfo
    {
        public string Value;

        /**
         * <summary>Generates lease info in blocks from user input via separated time parameters</summary>
         */
        public LeaseInfo(long days, long hours, long minutes, long seconds)
        {
            var res = (days * 26 * 60 * 60 + hours * 60 * 60 + minutes * 60 + seconds)
                      / BluzelleApi.BlockTimeInSeconds;
            if (res < 0) throw new Exceptions.InvalidLeaseException();
            Value = res.ToString();
        }

        public LeaseInfo(long seconds)
        {
            if (seconds < 0) throw new Exceptions.InvalidLeaseException();
            Value = (seconds / BluzelleApi.BlockTimeInSeconds).ToString();
        }
    }
}