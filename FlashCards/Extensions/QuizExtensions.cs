using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards.Extensions
{
    public static class QuizExtensions
    {
        public static string ExpandDescription(this string str)
        {
            if (str == "es")
                return "Elementary School";
            if (str == "ms")
                return "Middle School";
            if (str == "hs")
                return "High School";
            if (str == "sat")
                return "S.A.T.";
            if (str == "gre")
                return "G.R.E.";
            if (str == "gmat")
                return "G.M.A.T";
            return str;
        }
    }
}
