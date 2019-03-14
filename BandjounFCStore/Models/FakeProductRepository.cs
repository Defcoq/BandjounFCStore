using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandjounFCStore.Models
{
    public class FakeProductRepository : IProductRepository
    {
        public IQueryable<Product> Products => new List<Product>
        {
        new Product {Name="Football", Price=25} ,
        new Product {Name="Ballon", Price=30} ,
         new Product {Name="Casquette", Price=10} ,
          new Product {Name="Chausette", Price=25} ,

        }.AsQueryable();

        public Product DeleteProduct(int productID)
        {
            throw new NotImplementedException();
        }

        public void SaveProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
