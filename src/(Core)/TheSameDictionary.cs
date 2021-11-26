using System;
using System.Collections.Generic;

namespace WebRequest.Elegant.Core
{
    public class TheSameDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _expected;
        private readonly Dictionary<TKey, TValue> _actual;

        public TheSameDictionary(Dictionary<TKey, TValue> expected, Dictionary<TKey, TValue> actual)
        {
            _expected = expected;
            _actual = actual;
        }

        public bool ToBool()
        {
            if (_expected.Keys.Count == _actual.Keys.Count)
            {
                foreach (var key in _expected.Keys)
                {
                    if (!_actual.ContainsKey(key) || !TheSame(_actual[key], _expected[key]))
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }

        private bool TheSame(object expected, object actual)
        {
            if (expected != null)
            {
                return expected.Equals(actual);
            }

            return actual == null;
        }
    }
}

