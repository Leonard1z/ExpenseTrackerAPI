using AutoMapper;
using Domain.DTO.Category;
using Domain.DTO.ExpenseDto;
using Domain.DTO.UserAccount;
using Domain.Entities;

namespace Services.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();

            CreateMap<UserRegisterDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());

            CreateMap<Expense, ExpenseDto>().ReverseMap();
            CreateMap<Expense, ExpenseCreateDto>().ReverseMap();
            CreateMap<Expense, ExpenseUpdateDto>().ReverseMap();


        }
    }
}
