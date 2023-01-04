namespace SH.Bookstore.Orders.Host.Mongo;
internal class MongoSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string BooksCoollection { get; set; }
    public string ClientOrdersCollection { get; set; }
}
