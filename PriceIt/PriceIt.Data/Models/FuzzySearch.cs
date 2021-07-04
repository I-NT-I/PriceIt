using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PriceIt.Data.Models
{   public class Fuzzy
    {
        private string _searchTerm;
        private string[] _searchTerms;
        private Regex _searchPattern;
        private Regex _searchWordPattern;

        public Fuzzy(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _searchTerm = "";
                _searchTerms = null;
                _searchPattern = null;
                _searchWordPattern = null;
            }
            else
            {
                searchTerm = searchTerm.ToLower();

                _searchTerm = searchTerm;
                _searchTerms = searchTerm.Split(new Char[] { ' ' });
                _searchPattern = new Regex(
                    "(?i)(?=.*" + String.Join(")(?=.*", _searchTerms) + ")");
                

                var newSearchTerms = new List<string>();

                foreach (var s in _searchTerms)
                {
                    if(s.Length > 2)
                        newSearchTerms.Add(s);
                }

                _searchWordPattern = new Regex(string.Join("|", newSearchTerms));
            }
        }

        public bool IsMatch(string value)
        {
            value = value.ToLower();

            if (_searchTerm == value) 
                return true;
            if (value.Contains(_searchTerm)) 
                return true;
            if (_searchWordPattern.IsMatch(value)) 
                return true;

            return false;
        }

        public int MatchRegexWord(string value)
        {
            value = value.ToLower();
            var words = value.Split(new char[] { ' ' });

            var result = words.Count(word => _searchWordPattern.IsMatch(word));

            return result;
        }
    }
}
