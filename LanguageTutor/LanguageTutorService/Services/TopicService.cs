using DBContext.Connect;
using DBContext.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LanguageTutorService
{
    public class TopicService
    {
        private readonly dc58kv94isevv4Context postgres;

        public TopicService(dc58kv94isevv4Context postgresDB)
        {
            postgres = postgresDB;
        }

        public List<Topic>GetTopics()
        {
            return postgres.Topic.AsNoTracking().ToList();
        }

    }
}
