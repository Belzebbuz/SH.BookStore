namespace SH.Bookstore.Books.Infrastructure.Mongo.Settings;
public class MongoSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string BooksCoollection { get; set; }
    public string ClientOrdersCollection { get; set; }
}
