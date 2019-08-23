using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LanguageExt;
using NetExtensions.ValueObject.Legacy;

namespace NetExtensions.ValueObjects.Legacy.Payments
{
    public class Currency : ValueObject<Currency>
    { 
        private readonly string _name;
        private readonly string _base;
        private readonly string _subUnit;
        private readonly Option<int> _code;

        private Currency(string name, string @base, string subUnit, Option<int> code)
        {
            _base = @base;
            _subUnit = subUnit;
            _code = code;
            _name = name;
        }

        public Currency Create(string name, string @base, string subUnit)
        {
            return new Currency(name, @base, subUnit, Option<int>.None);
        }
        
        public Currency Create(string name, string @base, string subUnit, int code)
        {
            return new Currency(name, @base, subUnit, code);
        }
        protected override bool EqualsCustom(Currency other)
        {
            if (other == null)
                return false;
            
            return GetHashCode() == other.GetHashCode();
        }

        protected override int GetHashCodeCustom()
        {
            return (_name + _base + _subUnit).GetHashCode();
        }
    }
}