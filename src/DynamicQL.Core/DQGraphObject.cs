using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DynamicQL.Core
{
    public class DQObject : DQElement
    {
        public List<DQElement> Properties { get; } = new List<DQElement>();

        public DQObject(string name) : base(DQElementType.Object, name) { }


        public static DQObject Read(string graphString)
        {
            return Read(new StringReader(graphString));
        }

        public static DQObject Read(TextReader reader)
        {
            return Read(ROOT_NAME, reader);
        }

        // Read an object with the given name from the given reader.
        // The reader must be positioned at the opening brace of the object.
        protected static DQObject Read(string name, TextReader reader)
        {
            // Consume opening '{'
            if (reader.Read() != '{') throw new ApplicationException("Invalid object start");

            var queryObject = new DQObject(name);
            while (true)
            {
                var next = ConsumeWhitespaceAndPeek(reader);
                if (next == -1) throw new ApplicationException("Unexpected end of input, unclosed object");

                if (next == '}')
                {
                    reader.Read(); // consume closing '}'
                    return queryObject;
                }

                var str = ConsumeUntilWhitespaceOrBrace(reader);
                next = ConsumeWhitespaceAndPeek(reader);

                if (str.Length > 0 && next == '{')
                {
                    queryObject.Properties.Add(Read(str, reader));
                }
                else if (str.Length > 0)
                {
                    queryObject.Properties.Add(new DQValue(str));
                }
            }
        }
 
        // Helper method: Collect all non-whitespace, non-brace characters into a string
        private static string ConsumeUntilWhitespaceOrBrace(TextReader reader)
        {
            var stringBuilder = new StringBuilder();

            var next = reader.Peek();
            while (next != -1 && !Char.IsWhiteSpace((char)next) && next != '}' && next != '{')
            {
                stringBuilder.Append((char)next);
                reader.Read();
                next = reader.Peek();
            }

            return stringBuilder.ToString();
        }

        // Helper method: Advance reader past any whitespace
        private static int ConsumeWhitespaceAndPeek(TextReader reader)
        {
            var next = reader.Peek();
            while (next != -1 && Char.IsWhiteSpace((char)next))
            {
                reader.Read();
                next = reader.Peek();
            }

            return next;
        }
    }
}
