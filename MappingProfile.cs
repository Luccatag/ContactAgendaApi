using AutoMapper;
using ContactAgendaApi.Models;
using ContactAgendaApi.DTOs;

namespace ContactAgendaApi;

// AutoMapper profile for Contact <-> DTO mappings
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map from Contact to ContactReadDto and vice versa
        CreateMap<Contact, ContactReadDto>().ReverseMap();
        // Map from ContactCreateDto to Contact
        CreateMap<ContactCreateDto, Contact>();
        // Map from ContactUpdateDto to Contact
        CreateMap<ContactUpdateDto, Contact>();
    }
}
