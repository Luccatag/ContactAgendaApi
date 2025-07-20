using AutoMapper;
using ContactAgendaApi.Models;
using ContactAgendaApi.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Contact, ContactReadDto>();
        CreateMap<ContactCreateDto, Contact>();
        CreateMap<ContactUpdateDto, Contact>();
        // Add more mappings as needed
    }
}