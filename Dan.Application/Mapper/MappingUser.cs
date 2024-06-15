using Dan.Dtos.User;
using Dan.Models;
using AutoMapper;
using Dan.Dtos.User;
using Dan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan.Application.Mapper
{
    public class MappingUser: Profile
    {
        public MappingUser() 
        {
            CreateMap<DtoUser, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<ApplicationUser,ResponceUserDto>();

        }
    }
}
