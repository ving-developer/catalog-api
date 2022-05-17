namespace Catalog_API.Repository.Interfaces
{
    public interface IUnityOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        Task CommitAsync();
    }
}
