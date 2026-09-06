
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
    ?? "mongodb://mongo:27017";

var mongoDatabaseName =
    Environment.GetEnvironmentVariable("MONGO_DATABASE")
    ?? "DeveloperLearning";

var services = new ServiceCollection();

var mongoClient = new MongoClient(mongoConnection);
var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);

services.AddSingleton(mongoClient);
services.AddSingleton(mongoDatabase);

var serviceProvider = services.BuildServiceProvider();

var database =
    serviceProvider.GetRequiredService<IMongoDatabase>();


// =========================
// Kafka
// =========================

var topicName = "processed_data";

var config = new ConsumerConfig
{
    BootstrapServers = "kafka:9092",
    GroupId = "some-group-3",
    AutoOffsetReset = AutoOffsetReset.Earliest,
    EnableAutoCommit = false
};


// =========================
// MongoDB Collection
// =========================

var collection =
    database.GetCollection<BsonDocument>(topicName);


// =========================
// Kafka Consumer
// =========================

using var consumer =
    new ConsumerBuilder<Ignore, string>(config).Build();

Console.WriteLine("Kafka consumer started...");
Console.WriteLine($"Kafka broker: {config.BootstrapServers}");
Console.WriteLine($"Kafka topic: {topicName}");
Console.WriteLine($"Kafka group: {config.GroupId}");


// =========================
// Assign Partition Directly
// =========================

var topicPartition =
    new TopicPartition(
        topicName,
        new Partition(0));

var topicPartitionOffset =
    new TopicPartitionOffset(
        topicPartition,
        Offset.Beginning);

consumer.Assign(topicPartitionOffset);

Console.WriteLine(
    "Assigned to processed_data partition 0 from beginning.");

Console.WriteLine("Waiting for messages...");


// =========================
// Consume Messages
// =========================

try
{
    while (true)
    {
        var result = consumer.Consume();

        Console.WriteLine(
            $"Received message from topic: {result.Topic}");

        string json = result.Message.Value;

        var document =
            BsonDocument.Parse(json);

        await collection.InsertOneAsync(document);

        Console.WriteLine(
            "Message inserted into MongoDB.");
    }
}
catch (ConsumeException ex)
{
    Console.WriteLine("Kafka consume error:");
    Console.WriteLine(ex.Error.Reason);
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred:");
    Console.WriteLine(ex.Message);
}
finally
{
    consumer.Close();
}
