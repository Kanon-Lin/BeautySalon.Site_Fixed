using AutoMapper;
using BeautySalon.FrontEnd.Site.Models.DTO;
using BeautySalon.FrontEnd.Site.Models.EFModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models
{
	public class MappingProfile: Profile
	{
		public MappingProfile()
		{
			//單向映射

			CreateMap<RegisterDto, Member>()
            .ForMember(dest => dest.EncryptedPassword, opt => opt.MapFrom(src => src.EncryptedPassword))
            .ForMember(dest => dest.ConfirmCode, opt => opt.MapFrom(src => src.ConfirmCode))
            .ForMember(dest => dest.IsConfirmed, opt => opt.MapFrom(src => src.IsConfirmed))
            .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<LoginDto, Member>();

            //雙向映射
            CreateMap<Member, MemberDto>().ReverseMap();

		}
	}
}