using System;
using System.Collections.Generic;
using System.Text;

namespace newsscraper.DAL.Entities
{
    /// <summary>
    /// News article
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int ArticleID { get; set; }

        /// <summary>
        /// Article URI. Defined as a Unique Constraint in the database
        /// </summary>
        public string Uri { get; set; }

        public string Title { get; set; }

        public string Contents { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}
