using System.Collections.Generic;
using System.Linq;
using Common.Repositiory;

namespace Shop
{
    public class ProductsRepository : Repository<ProductItem>
    {
        public ProductsRepository(IEnumerable<ProductsSet> sets)
        {
            AddRange(sets.SelectMany(x => x.Items));
        }
    }
}