using System;
using System.Globalization;
using LanguageExt;
using NetExtensions.ValueObject.Legacy;

namespace NetExtensions.ValueObjects.Legacy
{
    public class DateValue : ValueObject<DateValue>
    {
       private readonly Option<DateTime> _value;

       private DateValue()
        {
            _value = Option<DateTime>.None;
        }

        private DateValue(DateTime dateTime)
        {
            _value = dateTime;
        }

        public Option<DateTime> Value => _value.Match(dt=> dt.Date, () => Option<DateTime>.None);

        public static DateValue Create(DateTime dateTime)
        {
            return new DateValue(dateTime);
        }
        
        public static DateValue Create(DateTime? dateTime)
        {
            return dateTime.Match(dt => new DateValue(dt), () => new DateValue());
        }

        public static DateValue Create(string dateTimeString, bool lastCentury = false)
        {
            const string yyyyMMddhhssmm = "yyyyMMddhhssmm";
            const string yyyyMMdd = "yyyyMMdd";
            const string yyMMdd = "yyMMdd";
            
            if (string.IsNullOrEmpty(dateTimeString))
                return new DateValue();

            var stringDate = dateTimeString.Trim();
            if (DateTime.TryParseExact(stringDate, new[] {yyyyMMddhhssmm, yyyyMMdd}, null, DateTimeStyles.None, out var date))
                return new DateValue(date);

            if (!DateTime.TryParseExact(stringDate, yyMMdd, null, DateTimeStyles.None, out var lastCenturyDate))
                throw new InvalidCastException($"given string is invalid: {dateTimeString}");
            
            // do not use TwoDigitYearMax!
            if (Math.Floor((decimal)lastCenturyDate.Year/100) >= Math.Floor((decimal)DateTime.Now.Year/100))
            {
                return lastCentury ? new DateValue(lastCenturyDate.AddYears(-100)) : new DateValue(lastCenturyDate);
            }
            return lastCentury ? new DateValue(lastCenturyDate) : new DateValue(lastCenturyDate.AddYears(100));
        }

        protected override bool EqualsCustom(DateValue other)
        {
            if (other == null)
                return false;

            return other.Value == Value;
        }

        protected override int GetHashCodeCustom()
        {
            return Value.GetHashCode();
        }
    }
}