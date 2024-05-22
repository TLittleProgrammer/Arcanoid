using System;

namespace App.Scripts.General.DateTime
{
    public sealed class DateTimeService : IDateTimeService
    {
        public long GetCurrentTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}