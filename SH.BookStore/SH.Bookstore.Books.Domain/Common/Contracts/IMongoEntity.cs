using SH.Bookstore.Books.Domain.Mongo;

namespace SH.Bookstore.Books.Domain.Common.Contracts;
public interface IMongoEntity
{
}

public interface IMongoEntity<T> : IMongoEntity
    where T : IMongo
{

}