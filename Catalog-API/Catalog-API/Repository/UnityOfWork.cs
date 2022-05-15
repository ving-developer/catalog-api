using Catalog_API.Context;
using Catalog_API.Repository.Interfaces;

namespace Catalog_API.Repository
{
    public class UnityOfWork : IUnityOfWork
    {
        private CatalogApiContext _context;
        private ProductRepository _productRepository;
        private CategoryRepository _categoryRepository;

        public UnityOfWork(CatalogApiContext context)
        {
            _context = context;
        }

        public IProductRepository ProductRepository
        {
            get { return _productRepository ?? new ProductRepository(_context); }
        }

        public ICategoryRepository CategoryRepository
        {
            get { return _categoryRepository ?? new CategoryRepository(_context); }
        }

        /**
         * Attempts to save all pending changes, if not, reverts all changes and returns an exception
         */
        public void Commit()
        {
            _context.SaveChanges();
        }

        /**
         * Dispose all recorded changes to be saved in the database
         */
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
