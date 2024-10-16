using BeautySalon.Backstage.Site.Models.Dtos;
using BeautySalon.Backstage.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.Backstage.Site.Models.Interfaces
{
    public interface IProductCategoryRepository
    {
        void Create(ServiceCategory category);
        void Delete(int id);
        ProductCategoryDto Get(int name);
        IEnumerable<ProductCategoryDto> GetAll();
        void Update(ProductCategoryDto categoryInDb);
    }
}
