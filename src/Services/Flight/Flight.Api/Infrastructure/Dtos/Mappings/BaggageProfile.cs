using AutoMapper;
using Flight.Domain.Entities;

namespace Flight.Api.Infrastructure.Dtos.Mappings
{
    public class BaggageProfile : Profile
    {
        public BaggageProfile()
        {
            CreateMap<BaggageDto, Baggage>();
        }
    }
}
