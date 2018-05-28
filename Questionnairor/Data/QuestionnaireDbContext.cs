using Microsoft.EntityFrameworkCore;
using Questionnairor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questionnairor.Data
{
    public class QuestionnaireDbContext : DbContext
    {
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Response> Responses { get; set; }

        public QuestionnaireDbContext(DbContextOptions<QuestionnaireDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {

        }
    }
}
