using System.Threading.Tasks;
using Questionnairor.Models;

namespace Questionnairor.Services
{
    public interface IDatabaseService
    {
        bool IsValid();
        Task<Questionnaire> Load();
        Task<int> Save(Questionnaire data);
    }
}