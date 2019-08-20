using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LanguageExt;
using NetExtensions.ValueObject.Legacy;

namespace NetExtensions.ValueObjects.Legacy
{
    public class DateValue : ValueObject<DateValue>
    {
        private readonly Option<DateTime> _value;
        private const string YyyyMMddhhssmm = "yyyyMMddhhssmm";
        private const string YyyyMMdd = "yyyyMMdd";
        private const string YyMMdd = "yyMMdd";
        private DateValue()
        {
            _value = Option<DateTime>.None;
        }

        private DateValue(DateTime dateTime)
        {
            _value = dateTime;
        }

        private DateValue(Option<DateTime> dateTime)
        {
            _value = dateTime;
        }

        public Option<DateTime> Value => _value.Match(dt => dt.Date, () => Option<DateTime>.None);

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
            if (string.IsNullOrEmpty(dateTimeString))
                return new DateValue();

            var stringDate = dateTimeString.Trim();
            if (TryParse(stringDate, new[] { YyyyMMddhhssmm, YyyyMMdd }, out var date))
                return new DateValue(date);

            if (!TryParse(stringDate, YyMMdd, out var lastCenturyDate))
                throw new InvalidCastException($"given string is invalid: {dateTimeString}");

            // do not use TwoDigitYearMax!
            if (Math.Floor((decimal)lastCenturyDate.Year / 100) >= Math.Floor((decimal)DateTime.Now.Year / 100))
            {
                return lastCentury ? new DateValue(lastCenturyDate.AddYears(-100)) : new DateValue(lastCenturyDate);
            }
            return lastCentury ? new DateValue(lastCenturyDate) : new DateValue(lastCenturyDate.AddYears(100));
        }
        public static DateValue Create(string dateTimeString, bool lastCentury = false, params string[] formats)
        {
            if (string.IsNullOrEmpty(dateTimeString) || formats == null || formats.Length == 0)
                return new DateValue();

            var stringDate = dateTimeString.Trim();
            var parsed = Parse(stringDate, formats.Where(x => x.Length >= 8)).Match(dt => dt,
                () => ParseWithoutCenturies(stringDate, formats.Where(x => x.Length == 6), lastCentury)
            );
            return new DateValue(parsed);
        }

        public static DateValue Create(string dateTimeString, int fromYearBelongsToPreviousCentury)
        {
           return Create(dateTimeString, fromYearBelongsToPreviousCentury,new[] { YyyyMMddhhssmm, YyyyMMdd, YyMMdd });
        }
        public static DateValue Create(string dateTimeString, int fromYearBelongsToPreviousCentury, string[] formats)
        {
            if (string.IsNullOrEmpty(dateTimeString) || formats == null || formats.Length == 0)
                return new DateValue();

            var stringDate = dateTimeString.Trim();
            var parsed = Parse(stringDate, formats.Where(x => x.Length >= 8)).Match(dt => dt,
                () => ParseWithoutCenturies(stringDate, formats.Where(x => x.Length == 6), fromYearBelongsToPreviousCentury)
            );
            return new DateValue(parsed);
        }
        private static Option<DateTime> Parse(string stringDate, IEnumerable<string> formats)
        {
            var enumerable = formats.ToArray();
            if (!enumerable.Any())
                return Option<DateTime>.None;

            return TryParse(stringDate, enumerable, out var date) ? date : Option<DateTime>.None;
        }
        private static Option<DateTime> ParseWithoutCenturies(string stringDate, IEnumerable<string> formats, bool lastCentury)
        {
            var enumerable = formats.ToArray();
            if (!enumerable.Any())
                return Option<DateTime>.None;

            if (!TryParse(stringDate, enumerable, out var lastCenturyDate))
                throw new InvalidCastException($"given string is invalid: {stringDate}");

            if (Math.Floor((decimal)lastCenturyDate.Year / 100) >= Math.Floor((decimal)DateTime.Now.Year / 100))
            {
                return lastCentury ? lastCenturyDate.AddYears(-100) : lastCenturyDate;
            }
            return lastCentury ? lastCenturyDate : lastCenturyDate.AddYears(100);
        }

        private static Option<DateTime> ParseWithoutCenturies(string stringDate, IEnumerable<string> formats, int fromYearBelongsToPreviousCentury)
        {
            var enumerable = formats.ToArray();
            if (!enumerable.Any())
            {
                return Option<DateTime>.None;
            }

            var culture = new CultureInfo("en-US");
            culture.Calendar.TwoDigitYearMax = fromYearBelongsToPreviousCentury + 99;
            return TryParse(stringDate, enumerable, out var parsedFromShortDate, culture) ? parsedFromShortDate : throw new InvalidCastException($"given string is invalid: {stringDate}");
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
        
        private static bool TryParse(string stringDate, string format, out DateTime result, CultureInfo cultureInfo = null) => TryParse(stringDate, new[] { format }, out result, cultureInfo);
        private static bool TryParse(string stringDate, string[] formats, out DateTime result, CultureInfo cultureInfo = null) => DateTime.TryParseExact(stringDate, formats, cultureInfo ?? CultureInfo.InvariantCulture, DateTimeStyles.None, out result);


    }
}