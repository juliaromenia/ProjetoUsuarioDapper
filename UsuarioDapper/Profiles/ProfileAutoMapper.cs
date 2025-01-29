using AutoMapper;
using UsuarioDapper.Dto;
using UsuarioDapper.Models;

namespace UsuarioDapper.Profiles;

public class ProfileAutoMapper : Profile
{
    public ProfileAutoMapper()
    {
        CreateMap<Usuario, UsuarioListarDto>();
    }
}