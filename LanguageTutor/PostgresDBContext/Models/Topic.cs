using System;
using System.Collections.Generic;

namespace DBContext.Models
{
    public partial class Topic
    {
        public Topic()
        {
            Word = new HashSet<Word>();
        }

        public int IdTopic { get; set; }
        public string NameTopic { get; set; }

        public virtual ICollection<Word> Word { get; set; }
    }
}
