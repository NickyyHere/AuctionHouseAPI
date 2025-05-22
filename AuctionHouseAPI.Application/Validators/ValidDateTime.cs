using System.ComponentModel.DataAnnotations;

namespace AuctionHouseAPI.Application.Validators
{
    /// <summary>
    /// <para>Validates the date based on parameters.</para>
    /// 
    /// <para>If the value is not of DateTime type, it will always return true.</para>
    /// 
    /// <para>If overlay is positive, the value date has to be at at least today + overlay,
    /// otherwise the value date has to be at least today - overlay.</para>
    /// 
    /// <para>For example if DaysOverlay = 1
    /// Today is 19.05.2025 14:00
    /// Anything below 20.05.2025 14:00 will return false</para>
    /// </summary>
    /// <returns>True if valid; otherwise false</returns>
    public class ValidDateTime : ValidationAttribute
    {
        public bool AllowPast { get; set; } = true;
        public bool AllowFuture { get; set; } = true;
        public int DaysOverlay { get; set; } = 0;
        public int HoursOverlay { get; set; } = 0;
        public int MinutesOverlay { get; set; } = 0;

        public override bool IsValid(object? value)
        {
            if (value is DateTime time)
            {
                var now = DateTime.Now;
                if ((DateTime)time >= now && !AllowFuture) return false;
                if ((DateTime)time < now && !AllowPast) return false;
                var overlayTime = now.AddDays(DaysOverlay).AddHours(HoursOverlay).AddMinutes(MinutesOverlay);
                if (overlayTime > now && AllowFuture)
                {
                    if(time < overlayTime)
                        return false;
                }
                else if (overlayTime < now && AllowPast)
                {
                    if(time > overlayTime)
                        return false;
                }
            }
            return true;
        }
    }
}
