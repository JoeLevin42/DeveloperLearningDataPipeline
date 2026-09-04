namespace CsharpConsumer.Models;

public class ProcessedMessage
{
    public int responseId { get; set; }
    public string age { get; set; }
    public int yearsCode { get; set; }
    public string devType { get; set; }
    public string learnCodeChoose { get; set; }
    public string learningMethods { get; set; }
    public string learnCodeAI { get; set; }
    public string aiLearningMethods { get; set; }
    public string aiUsage { get; set; }
    public string aiTrust { get; set; }
    public string aiSentiment { get; set; }
    public string experienceLevel { get; set; }
    public bool usesDocumentation { get; set; }
    public bool usesAIForLearning { get; set; }
    public bool usesStackOverflow { get; set; }
}