using Microsoft.AspNetCore.Mvc;
using UsuarioDapper.Dto;
using UsuarioDapper.Services;

namespace UsuarioDapper.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioInterface _usuarioService;
    public UsuarioController(IUsuarioInterface usuarioInterface, IUsuarioInterface usuarioService = null)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<IActionResult> BuscarUsuarios()
    {
        var usuarios = await _usuarioService.BuscarUsuarios();

        if (usuarios.Status == false)
        {
            return NotFound(usuarios);
        }

        return Ok(usuarios);
    }

    [HttpGet("{usuarioId}")]
    public async Task<IActionResult> BuscarUsuarioPorId(int usuarioId)
    {
        var usuario = await _usuarioService.BuscarUsuarioPorId(usuarioId);

        if (usuario.Status == false)
        {
            return NotFound(usuario);
        }

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> CriarUsuario(UsuarioCriarDto usuarioCriar)
    {
        var usuarios = await _usuarioService.CriarUsuario(usuarioCriar);

        if (usuarios.Status == false)
        {
            return BadRequest(usuarios);
        }

        return Ok(usuarios);
    }

    [HttpPut]
    public async Task<IActionResult> EditarUsuario(UsuarioEditarDto usuarioEditar)
    {
        var usuarios = await _usuarioService.EditarUsuario(usuarioEditar);

        if (usuarios.Status == false)
        {
            return BadRequest(usuarios);
        }

        return Ok(usuarios);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoverUsuario(int usuarioId)
    {
        var usuarios = await _usuarioService.RemoverUsuario(usuarioId);

        if (usuarios.Status == false)
        {
            return BadRequest(usuarios);
        }

        return Ok(usuarios);
    }
}