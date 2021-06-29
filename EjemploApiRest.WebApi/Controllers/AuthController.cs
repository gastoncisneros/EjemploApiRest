using EjemploApiRest.Services;
using EjemploApiRest.WebApi.Configuration;
using EjemploApiRest.WebApi.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjemploApiRest.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        ITokenHandlerService _tokenHandlerService;

        public AuthController(UserManager<IdentityUser> userManager, ITokenHandlerService tokenHandlerService)
        {
            _userManager = userManager;
            _tokenHandlerService = tokenHandlerService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDTO user)
        {
            if (ModelState.IsValid)
            {
                var existUser = await _userManager.FindByEmailAsync(user.Email);

                if(existUser != null)
                {
                    return BadRequest("El email ya existe");
                }

                var isCreated = await _userManager.CreateAsync(new IdentityUser() { Email = user.Email, UserName = user.Name }, user.Password);

                if (isCreated.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(isCreated.Errors.Select(x => x.Description).ToList());
                }
            }
            else
            {
                return BadRequest("Error en el Modelo");
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDTO user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if(existingUser == null)
                {
                    return BadRequest(new UserLoginResponseDTO()
                    {
                        Login = false,
                        Errors = new List<string>() { "Usuario o Contraseña Incorrecto"}
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (isCorrect)
                {
                    var pars = new TokenParameter()
                    {
                        Id = existingUser.Id,
                        PasswordHash = existingUser.PasswordHash,
                        UserName = existingUser.UserName
                    };

                    var jwtToken = _tokenHandlerService.GenerateJwtToken(pars);

                    return Ok(new UserLoginResponseDTO()
                    {
                        Login = true,
                        Token = jwtToken
                    });
                }
                else
                {
                    return BadRequest(new UserLoginResponseDTO()
                    {
                        Login = false,
                        Errors = new List<string>() { "Usuario o Contraseña Incorrecto" }
                    });
                }
            }
            else
            {
                return BadRequest(new UserLoginResponseDTO()
                {
                    Login = false,
                    Errors = new List<string>() { "Usuario o Contraseña Incorrecto" }
                });
            }
        }
    }
}
