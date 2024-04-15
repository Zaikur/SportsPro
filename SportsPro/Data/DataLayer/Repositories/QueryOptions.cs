/* Ayden Hofts
 * 04/14/2024
 * This class contains the Query Options used to implement the repository pattern.
 */


using System;
using System.Linq.Expressions;

namespace SportsPro.Data.DataLayer
{
    public class QueryOptions<T>
    {
        // public properties for sorting, filtering, and paging
        public Expression<Func<T, Object>> OrderBy { get; set; } = null!;
        public Expression<Func<T, bool>> Where { get; set; } = null!;
        public string OrderByDirection { get; set; } = "asc";  // default

        /* Code for working with Include strings */
        private string[] includes = Array.Empty<string>();

        // public write-only property for Include strings – accepts a string, converts it to
        // a string array, and stores in private string array field
        public string Includes
        {
            set => includes = value.Replace(" ", "").Split(',');
        }

        // public get method for Include strings - returns private string array, or
        // empty string array if private backing field is null
        public string[] GetIncludes() => includes;

        // read-only properties 
        public bool HasWhere => Where != null;
        public bool HasOrderBy => OrderBy != null;
    }

    // basically an alias for a list of where expressions - to make code clearer
    public class WhereClauses<T> : List<Expression<Func<T, bool>>> { }

}