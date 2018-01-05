namespace Jt76.Identity.Models.MappedModels
{
	using AutoMapper;
	using Common.Constants;
	using Microsoft.AspNetCore.Identity;
	using Models;

	public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, User>()
				.ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<User, ApplicationUser>()
                .ForMember(d => d.Roles, map => map.Ignore());

            CreateMap<ApplicationUser, UserEdit>()
                .ForMember(d => d.Roles, map => map.Ignore());
            CreateMap<UserEdit, ApplicationUser>()
                .ForMember(d => d.Roles, map => map.Ignore());

            CreateMap<ApplicationUser, UserPatch>()
                .ReverseMap();

            CreateMap<ApplicationRole, Role>()
                .ForMember(d => d.Permissions, map => map.MapFrom(s => s.Claims))
                .ForMember(d => d.UsersCount, map => map.ResolveUsing(s => s.Users?.Count ?? 0))
                .ReverseMap();
            CreateMap<Role, ApplicationRole>();

            CreateMap<IdentityRoleClaim<string>, Claim>()
                .ForMember(d => d.Type, map => map.MapFrom(s => s.ClaimType))
                .ForMember(d => d.Value, map => map.MapFrom(s => s.ClaimValue))
                .ReverseMap();

            CreateMap<ApplicationPermission, Permission>()
                .ReverseMap();

            CreateMap<IdentityRoleClaim<string>, Permission>()
                .ConvertUsing(s => Mapper.Map<Permission>(ApplicationPermissions.GetPermissionByValue(s.ClaimValue)));
        }
    }
}
