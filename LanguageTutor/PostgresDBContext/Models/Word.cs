using System;
using System.Collections.Generic;

namespace DBContext.Models
{
    public partial class Word
    {
        public long IdWord { get; set; }
        public int IdTopic { get; set; }
        public string NameWord { get; set; }

        public virtual Topic IdTopicNavigation { get; set; }
    }
}
