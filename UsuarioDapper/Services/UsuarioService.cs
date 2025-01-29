using AutoMapper;
using Dapper;
using System.Data.SqlClient;
using UsuarioDapper.Dto;
using UsuarioDapper.Models;

namespace UsuarioDapper.Services;

public class UsuarioService : IUsuarioInterface
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    public UsuarioService(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<ResponseModel<Usuario>> BuscarUsuarioPorId(int usuarioId)
    {
        ResponseModel<Usuario> response = new ResponseModel<Usuario>();

        using (var connection = GetConnection())
        {
            var usuarioBanco = await connection.QueryFirstOrDefaultAsync<Usuario>(
                "SELECT * FROM Usuarios WHERE id = @Id", new { Id = usuarioId });

            if (usuarioBanco == null)
            {
                response.Mensagem = "Nenhum usuário localizado!";
                response.Status = false;
                return response;
            }

            response.Dados = usuarioBanco;
            response.Mensagem = "Usuário localizados com sucesso!";
        }

        return response;
    }

    public async Task<ResponseModel<List<UsuarioListarDto>>> BuscarUsuarios()
    {
        ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

        using (var connection = GetConnection())
        {
            var usuariosBanco = await connection.QueryAsync<Usuario>(
                "SELECT * FROM Usuarios");

            if (usuariosBanco.Count() == 0)
            {
                response.Mensagem = "Nenhum usuário localizado!";
                response.Status = false;
                return response;
            }

            var usuarioMapeado = _mapper.Map<List<UsuarioListarDto>>(usuariosBanco);

            response.Dados = usuarioMapeado;
            response.Mensagem = "Usuários localizados com sucesso!";
        }

        return response;
    }

    public async Task<ResponseModel<List<UsuarioListarDto>>> CriarUsuario(UsuarioCriarDto usuarioCriar)
    {
        ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

        using (var connection = GetConnection())
        {
            var novoUsuario = await connection.ExecuteAsync(
                "INSERT INTO Usuarios (NomeCompleto, Email, Cargo, Salario, CPF, Senha, Situacao) " +
                "VALUES (@NomeCompleto, @Email, @Cargo, @Salario, @CPF, @Senha, @Situacao)", usuarioCriar);

            if (novoUsuario == 0)
            {
                response.Mensagem = "Ocorreu um erro ao criar um usuário!";
                response.Status = false;
                return response;
            }

            var usuarios = await ListarUsuarios(connection);
            var usuariosMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);

            response.Dados = usuariosMapeados;
            response.Mensagem = "Usuários Listados com sucesso!";
        }

        return response;
    }

    public async Task<ResponseModel<List<UsuarioListarDto>>> EditarUsuario(UsuarioEditarDto usuarioEditar)
    {
        ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

        using (var connection = GetConnection())
        {
            var editarUsuario = await connection.ExecuteAsync(
                "UPDATE Usuarios " +
                "SET " +
                "NomeCompleto = @NomeCompleto," +
                "Email = @Email, " +
                "Cargo = @Cargo," +
                "Salario = @Salario," +
                "CPF = @CPF," +
                "Situacao = @Situacao " +
                "WHERE Id = @Id", usuarioEditar);

            if (editarUsuario == 0)
            {
                response.Mensagem = "Ocorreu um erro ao editar um usuário!";
                response.Status = false;
                return response;
            }

            var usuarios = await ListarUsuarios(connection);
            var usuariosMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);

            response.Dados = usuariosMapeados;
            response.Mensagem = "Usuários Listados com sucesso!";
        }

        return response;
    }

    public async Task<ResponseModel<List<UsuarioListarDto>>> RemoverUsuario(int usuarioId)
    {
        ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

        using (var connection = GetConnection())
        {
            var editarUsuario = await connection.ExecuteAsync(
                "DELETE FROM Usuarios WHERE Id = @Id", new { Id = usuarioId });

            if (editarUsuario == 0)
            {
                response.Mensagem = "Ocorreu um erro ao excluir o usuário!";
                response.Status = false;
                return response;
            }

            var usuarios = await ListarUsuarios(connection);
            var usuariosMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);

            response.Dados = usuariosMapeados;
            response.Mensagem = "Usuários Listados com sucesso!";
        }

        return response;
    }

    #region MÉTODOS PRIVADOS

    private static async Task<IEnumerable<Usuario>> ListarUsuarios(SqlConnection connection)
    {
        return await connection.QueryAsync<Usuario>("SELECT * FROM Usuarios");
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }

    #endregion
}