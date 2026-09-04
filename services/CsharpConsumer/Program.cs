using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;


// =========================
// Configuration
// =========================

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();


// =========================
// MongoDB
// =========================

var mongoConnection =
    Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING")
    ?? "mongodb://localhost:27017";

var mongoDatabaseName =
    Environment.GetEnvironmentVariable("MONGO_DATABASE")
    ?? "DeveloperLearning";

var services = new ServiceCollection();

var mongoClient = new MongoClient(mongoConnection);
var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);

services.AddSingleton(mongoClient);
services.AddSingleton(mongoDatabase);

var serviceProvider = services.BuildServiceProvider();



// MongoDB Database & Collection


var database = serviceProvider.GetRequiredService<IMongoDatabase>();

var topicName = configuration["Kafka:Topics"];

var collection = database.GetCollection<BsonDocument>(topicName);



var bootstrapServers =
    Environment.GetEnvironmentVariable("KAFKA_BROKER")
    ?? configuration["Kafka:BootstrapServers"];

var groupId = configuration["Kafka:GroupId"];

var config = new ConsumerConfig
{
    BootstrapServers = bootstrapServers,
    GroupId = groupId,
    AutoOffsetReset = AutoOffsetReset.Earliest,
    EnableAutoCommit = false
};




using var consumer =
    new ConsumerBuilder<Ignore, string>(config).Build();

Console.WriteLine("Kafka consumer started...");
Console.WriteLine("Waiting for messages...");

consumer.Subscribe(topicName);



try
{
    while (true)
    {
        var result = consumer.Consume();

        Console.WriteLine(
            $"Received message from topic: {result.Topic}");

        string json = result.Message.Value;

        var document = BsonDocument.Parse(json);

        // Insert into MongoDB
        await collection.InsertOneAsync(document);

        Console.WriteLine("Message inserted into MongoDB.");

        consumer.Commit(result);
    }
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred.");
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    consumer.Close();
}