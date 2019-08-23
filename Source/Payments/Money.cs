using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LanguageExt;
using NetExtensions.ValueObject.Legacy;


namespace NetExtensions.ValueObjects.Legacy.Payments
{
    public class Money : ValueObject<Money>
    {
        private readonly decimal value;
        private readonly Currency currency;

        private Money(decimal value)
        {
            this.value = value;
        }

        protected override bool EqualsCustom(Money other)
        {
            if (other == null)
                return false;

            return value == other.value;
        }

        protected override int GetHashCodeCustom()
        {
            return value.GetHashCode();
        }
    }
}