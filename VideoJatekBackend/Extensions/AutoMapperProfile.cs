using AutoMapper;
using VideoJatekBackend.Dto;
using VideoJatekBackend.Models;

namespace VideoJatekBackend.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Publisher, PublisherDto>()
                .ReverseMap();

            CreateMap<Videogame, VideogameDto>()
                .ReverseMap();
        }
    }
}
