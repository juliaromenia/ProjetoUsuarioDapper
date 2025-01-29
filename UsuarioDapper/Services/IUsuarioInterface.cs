using UsuarioDapper.Dto;
using UsuarioDapper.Models;

namespace UsuarioDapper.Services;

public interface IUsuarioInterface
{
    Task<ResponseModel<List<UsuarioListarDto>>> BuscarUsuarios();
    Task<ResponseModel<Usuario>> BuscarUsuarioPorId(int usuarioId);
    Task<ResponseModel<List<UsuarioListarDto>>> CriarUsuario(UsuarioCriarDto usuarioCriar);
    Task<ResponseModel<List<UsuarioListarDto>>> EditarUsuario(UsuarioEditarDto usuarioEditar);
    Task<ResponseModel<List<UsuarioListarDto>>> RemoverUsuario(int usuarioId);
}