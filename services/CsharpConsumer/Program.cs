using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

//create the mongo config

var configuration = new ConfigurationBuilder().
    SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();


var services = new ServiceCollection();

var mongoClient = new MongoClient("mongodb://localhost:27017");
var mongoDatabase = mongoClient.GetDatabase("DeveloperLearning");

services.AddSingleton(mongoClient);
services.AddSingleton(mongoDatabase);

var serviceProvider = services.BuildServiceProvider();

//until here this is the creating of the serviceProvider
// now we will use the mongo database 

var topicName = configuration["Kafka:Topics"];
var database = serviceProvider.GetRequiredService<IMongoDatabase>();
var collection = database.GetCollection<BsonDocument>(topicName);

var bootstrapServers = configuration["Kafka:BootstrapServers"];
var groupId = configuration["Kafka:GroupId"];
//now we will config the consumer 
var config = new ConsumerConfig
{
    BootstrapServers = bootstrapServers,
    GroupId = groupId,
    AutoOffsetReset = AutoOffsetReset.Earliest,
    EnableAutoCommit = false

    //mybe we will do late the auto commit - off = false
};

using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

Console.WriteLine("Kafka consumer started..."); 
Console.WriteLine("Waiting for messages...");

try
{
    var result = consumer.Consume();

    Console.WriteLine($"Received message from topic: {result.Topic}");
    string json = result.Message.Value;

    var document = BsonDocument.Parse(json);

    await collection.InsertOneAsync(document);
    Console.WriteLine("Message inserted into MongoDB.");

    consumer.Commit(result);

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