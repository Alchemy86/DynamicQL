using System.Collections.Generic;
using System.Linq;
using DynamicQL.Core;
using DynamicQL.Core.Extensions;
using DynamicQL.Interfaces;

namespace DynamicQL.Services
{
    public class DynamicQLConverter : IDynamicQLConverter
    {
        public List<QueryObject> Parse(string body)
        {
            List<QueryObject> queryObjects = new List<QueryObject>();

            var level = 0;
            var objectIdentified = false;
            var currentObject = "";
            var position = 0;
            var wordCut = 0;

            while (position < body.Length)
            {
                var code = body[position];
                switch (code)
                {
                    case '\t':
                    case '\r':
                    case ',':
                        if (!objectIdentified)
                            queryObjects[level].Variables.Add(body.Substring(wordCut, position - wordCut).ClearWhiteSpace());

                        wordCut = position;
                        objectIdentified = false;
                        break;

                    case '{':
                        objectIdentified = true;
                        var name = body.Substring(wordCut, position - wordCut).ClearWhiteSpace();
                        queryObjects.Add(new QueryObject());
                        level = queryObjects.Count() - 1;

                        queryObjects[level].Name = name;
                        queryObjects[level].ParentObject = currentObject;

                        currentObject = name;
                        break;

                    case '}':
                        objectIdentified = true;
                        level--;
                        break;
                }

                position++;
            }

            var validResults = queryObjects.Where(x => x.Variables.Count() > 0 );
            return queryObjects;
        }
    }
}
