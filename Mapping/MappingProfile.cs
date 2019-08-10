using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pieshop.Models;
using Pieshop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pieshop.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            //CreateMap<AddUserViewModel, ApplicationUser>(MemberList.Source);
            ////    //.ForMember(x => x.PasswordHash, opt => opt.Ignore())
            ////    //.ForMember(x => x.AccessFailedCount, opt => opt.Ignore())
            ////    //.ForMember(x => x.ConcurrencyStamp, opt => opt.Ignore())
            ////    //.ForMember(x => x.EmailConfirmed, opt => opt.Ignore())
            ////    //.ForMember(x => x.LockoutEnabled, opt => opt.Ignore())
            ////    //.ForMember(x => x.LockoutEnd, opt => opt.Ignore())
            ////    //.ForMember(x => x.NormalizedEmail, opt => opt.Ignore())
            ////    //.ForMember(x => x.NormalizedUserName, opt => opt.Ignore())
            ////    //.ForMember(x => x.PhoneNumber, opt => opt.Ignore())
            ////    //.ForMember(x => x.PhoneNumberConfirmed, opt => opt.Ignore())
            ////    //.ForMember(x => x.SecurityStamp, opt => opt.Ignore())
            ////    //.ForMember(x => x.TwoFactorEnabled, opt => opt.Ignore());

            //CreateMap<EditUserViewModel, ApplicationUser>(MemberList.Source);
            ////    //.ForMember(x => x.PasswordHash, opt => opt.Ignore())
            ////    //.ForMember(x => x.AccessFailedCount, opt => opt.Ignore())
            ////    //.ForMember(x => x.ConcurrencyStamp, opt => opt.Ignore())
            ////    //.ForMember(x => x.EmailConfirmed, opt => opt.Ignore())
            ////    //.ForMember(x => x.LockoutEnabled, opt => opt.Ignore())
            ////    //.ForMember(x => x.LockoutEnd, opt => opt.Ignore())
            ////    //.ForMember(x => x.NormalizedEmail, opt => opt.Ignore())
            ////    //.ForMember(x => x.NormalizedUserName, opt => opt.Ignore())
            ////    //.ForMember(x => x.PhoneNumber, opt => opt.Ignore())
            ////    //.ForMember(x => x.PhoneNumberConfirmed, opt => opt.Ignore())
            ////    //.ForMember(x => x.SecurityStamp, opt => opt.Ignore())
            ////    //.ForMember(x => x.TwoFactorEnabled, opt => opt.Ignore());

            //CreateMap<ApplicationUser, AddUserViewModel>(MemberList.Source);
            //CreateMap<ApplicationUser, EditUserViewModel>(MemberList.Source);

        }
    }
}
