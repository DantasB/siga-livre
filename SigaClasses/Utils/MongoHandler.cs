using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace SigaClasses.Utils
{
    public class MongoHandler
    {
        private static string DatabaseName   { get; set; }
        private static string CollectionName { get; set; }
        private static string HostName       { get; set; }
        private static string Login          { get; set; }
        private static string Password       { get; set; }

        private static IMongoCollection<BsonDocument> Collection { get; set; }

        public static bool ConnectToMongo()
        {
            try
            {
                string atlasString = @"mongodb+srv://{0}:{1}@{2}-kfui3.mongodb.net/test";
                atlasString        = String.Format(atlasString, Login, Password, HostName);

                MongoClient dbClient                      = new MongoClient(atlasString);
                IMongoDatabase database                   = dbClient.GetDatabase(DatabaseName);
                Collection                                = database.GetCollection<BsonDocument>(CollectionName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace + " " + ex.Message);
                return false;
            }

            return true;
        }

        public static bool StartOnMongo(string login, string password, string hostName, string databaseName, string collectionName)
        {
            Login          = login;
            Password       = password;
            HostName       = hostName;
            DatabaseName   = databaseName;
            CollectionName = collectionName;
            return true;
        }


    }
}
