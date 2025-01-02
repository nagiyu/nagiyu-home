using System.Collections.Generic;

namespace Nagiyu.Policy.Web.Models
{
    public class Term
    {
        public string Title { get; set; }
        public List<TermContent> Contents { get; set; }
    }

    public class TermContent
    {
        public string MainContent { get; set; }
        public List<string> SubItems { get; set; }
    }
}
