using System;
using System.Linq;
using DynamicQL.Interfaces;

namespace DynamicQL.Core.Services
{
    public class DynamicQLConverter : IDynamicQLConverter
    {
        public void Convert(string data)
        {
            var more = data.Split(Environment.NewLine);
            var layers = spintaxParse(data);
        }

        public static String spintaxParse(String s)
        {
            if (s.Contains("{"))
            {
                int closingBracePosition = s.LastIndexOf('}');
                int openingBracePosition = s.IndexOf('{');

                String spintaxBlock = s.Substring(openingBracePosition + 1, closingBracePosition - 1 - openingBracePosition).Trim();

                var items = spintaxBlock.Substring(0, spintaxBlock.Length).Split("\n\t").Select(x => x.Trim()).Where(y => !string.IsNullOrEmpty(y));

                var ss = spintaxBlock.Substring(0, spintaxBlock.Length).Trim();

                return spintaxParse(ss);
            }
            else
            {
                return s;
            }
        }

    }
}
