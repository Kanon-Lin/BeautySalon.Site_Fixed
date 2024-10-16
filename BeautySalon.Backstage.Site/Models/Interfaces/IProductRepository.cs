using BeautySalon.Backstage.Site.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Backstage.Site.Models.Interfaces
{
    public interface IProductRepository
    {
        void Create(ProductDto dto);
        void Delete(int id);
        ProductDto Get(int productId);
        bool IsProductExist(string productName);
        void Update(ProductDto productInDb);
    }
}
