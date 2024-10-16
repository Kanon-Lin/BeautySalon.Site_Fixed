using AutoMapper;
using BeautySalon.Backstage.Site.Models.Dtos;
using BeautySalon.Backstage.Site.Models.EFModels;
using BeautySalon.Backstage.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.Backstage.Site.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ServiceCategory, ProductCategoryDto>().ReverseMap();

            CreateMap<ProductCategoryVm, ProductCategoryDto>().ReverseMap();
        }
    }
}