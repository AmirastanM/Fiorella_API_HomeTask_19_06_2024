using AutoMapper;
using FiorelloAPI.DTOs.Blogs;
using FiorelloAPI.DTOs.Categories;
using FiorelloAPI.DTOs.Experts;
using FiorelloAPI.DTOs.Products;
using FiorelloAPI.DTOs.Settings;
using FiorelloAPI.DTOs.Sliders;
using FiorelloAPI.DTOs.Socials;
using FiorelloAPI.Models;

namespace FiorelloAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
        
            CreateMap<Blog, BlogDto>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Blog, BlogCreateDto>().ReverseMap();
            CreateMap<Blog, BlogEditDto>().ReverseMap();


            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("MM.dd.yyyy")))
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(p => p.Name)));

            CreateMap<Category, CategoryProductDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("MM.dd.yyyy")))
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products.Select(p => p.Name)));

            CreateMap<CategoryCreateDto, Category>();

         
            CreateMap<Expert, ExpertDto>().ReverseMap();
            CreateMap<Expert, ExpertCreateDto>().ReverseMap();
            CreateMap<Expert, ExpertEditDto>().ReverseMap();

            CreateMap<Setting, SettingDto>().ReverseMap();

            CreateMap<Social, SocialDto>().ReverseMap();
            CreateMap<Social, SocialCreateDto>().ReverseMap();
            

            CreateMap<Slider, SliderDto>().ReverseMap();
            CreateMap<SliderCreateDto, Slider>().ReverseMap();

            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore()); 
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.Name)));


          

        }
    }
}
