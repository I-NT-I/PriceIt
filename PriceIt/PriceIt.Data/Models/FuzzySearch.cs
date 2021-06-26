﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PriceIt.Data.Models
{   public class FuzzySearch
    {
        private string _searchTerm;
        private string[] _searchTerms;
        private Regex _searchPattern;

        public FuzzySearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _searchTerm = "";
                _searchTerms = null;
                _searchPattern = null;
            }
            else
            {
                _searchTerm = searchTerm;
                _searchTerms = searchTerm.Split(new Char[] { ' ' });
                _searchPattern = new Regex(
                    "(?i)(?=.*" + String.Join(")(?=.*", _searchTerms) + ")");
            }
        }

        public bool IsMatch(string value)
        {
            // do the cheap stuff first
            if (_searchTerm == value) return true;
            if (value.Contains(_searchTerm)) return true;
            // if the above don't return true, then do the more expensive stuff
            if (_searchPattern.IsMatch(value)) return true;
            // etc.

            return false;
        }
    }
}
