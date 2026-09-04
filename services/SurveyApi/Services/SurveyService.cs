using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SurveyApi.Models;
using SurveyApi.Models;

namespace SurveyApi.Services;

public class SurveyService(IOptions<SurveyDatabaseSettings> DatabaseSettings)
{
    private readonly IMongoCollection<SurveyModel> _collection = new MongoClient(DatabaseSettings.Value.ConnectionString)
        .GetDatabase(DatabaseSettings.Value.DatabaseName)
        .GetCollection<SurveyModel>(DatabaseSettings.Value.CollectionName);


    public async Task<List<SurveyModel>> GetAllAsync() =>
        await _collection.Find(_ => true).Limit(100).ToListAsync();

    public async Task<IEnumerable<SurveyModel>> GetWhoUseDocs()
    {
        var result = await _collection.AsQueryable()
            .Where(e => e.usesDocumentation).Take(100).ToListAsync();
        return result;
    }

    public async Task<IEnumerable<SurveyModel>> GetUseDocsAndAi()
    {
        var result = await _collection.AsQueryable()
            .Where(e => e.usesDocumentation || e.usesAIForLearning).Take(100)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<SurveyModel>> GetTrustAi()
    {
        var result = await _collection
        .AsQueryable()
        .Where(x => x.aiTrust == "Somewhat trust")
        .Take(100)
        .ToListAsync();
        return result;
    }
    public async Task<IEnumerable<SurveyModel>> GetSeniorUp10()
    {
        var result = await _collection
        .AsQueryable()
        .Where(x => x.yearsCode >= 10)
        .Take(100)
        .ToListAsync();

        return result;
    }
    public async Task<IEnumerable<SurveyModel>> Get20Async()
    {
        var result = await _collection
    .AsQueryable()
    .Where(x =>
     x.devType == "Developer, back-end" &&
     x.learnCodeAI != null &&
     x.learnCodeAI != "")
     .OrderByDescending(x => x.yearsCode)
     .Take(20)
     .ToListAsync();
    return result;
    }

}
