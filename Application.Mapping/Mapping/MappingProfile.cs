using AutoMapper;
using Domain.Models;
using Application.Dto.Request;
using Application.Dto.Response;
using Application.Dto.Request.Full;
using Application.Dto.Response.Full;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Inventory, InventoryResponseDto>().ReverseMap();
            CreateMap<InventoryRequestDto, Inventory>()
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ForMember(dest => dest.InventoryType, opt => opt.Ignore());
            CreateMap<Inventory, InventoryFullResponseDto>()
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.UserName));

            CreateMap<InventoryType, InventoryTypeResponseDto>();
            CreateMap<InventoryTypeRequestDto, InventoryType>().ReverseMap();

            CreateMap<ItemValue, ItemValueResponseDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.InventoryType.Name));
            CreateMap<ItemValueRequestDto, ItemValue>().ReverseMap();

            CreateMap<Item, ItemFullResponseDto>();
            CreateMap<ItemFullRequestDto, Item>();
        }
    }
}