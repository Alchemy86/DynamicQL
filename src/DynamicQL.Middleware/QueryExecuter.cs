using System.Threading.Tasks;

namespace DynamicQL.Middleware
{
    internal class QueryExecuter
    {
        public QueryExecuter()
        {

        }

        public async Task<string> ExecuteAsync()
        {
            return "Moo";
        }
    }
}