using Microsoft.EntityFrameworkCore;
using Questionnairor.Data;
using Questionnairor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questionnairor.Services
{
    public class DatabaseService : IDatabaseService
    {
        private DbContextOptions<QuestionnaireDbContext> dbContextOptions;

        public DatabaseService(DbContextOptions<QuestionnaireDbContext> dbContextOptions)
        {
            this.dbContextOptions = dbContextOptions;
        }

        public bool IsValid()
        {
            try
            {
                using (QuestionnaireDbContext dbContext = new QuestionnaireDbContext(dbContextOptions))
                {
                    dbContext.Database.GetDbConnection().Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<Questionnaire> Load()
        {
            try
            {
                using (QuestionnaireDbContext dbContext = new QuestionnaireDbContext(dbContextOptions))
                {
                    return await dbContext.Questionnaires.FirstOrDefaultAsync();
                }
            }
            catch
            {
                return new Questionnaire().Id(Guid.Empty);
            }
        }

        public async Task<int> Save(Questionnaire data)
        {
            try
            {
                using (QuestionnaireDbContext dbContext = new QuestionnaireDbContext(dbContextOptions))
                {
                    dbContext.Update(data);
                    return await dbContext.SaveChangesAsync();
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
