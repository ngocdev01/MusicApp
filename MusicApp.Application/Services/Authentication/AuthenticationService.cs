

using MusicApp.Application.Common.Interface.Authentication;
using MusicApp.Application.Common.Interface.Persistence;
using MusicApp.Domain.Common.Entities;
using MusicApp.Domain.Common.Errors;
using System.Net;

namespace MusicApp.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokentGenerator _jwtTokentGenerator;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Role> _roleRepository;

    public AuthenticationService(IJwtTokentGenerator jwtTokentGenerator,IRepository<User> userRepository, IRepository<Role> roleRepository)
    {
        _jwtTokentGenerator = jwtTokentGenerator;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async  Task<AuthenticationResult> Login(string username,string email, string password)
    {
        //Check user 
        if ( await _userRepository.GetAsync(user => user.Email == email) is not User user)
        {
            throw new HttpResponseException(HttpStatusCode.NotFound, "User Not Found");
        }


        if(user.Password != password)
        {
            throw new HttpResponseException(HttpStatusCode.NotFound, "Wrong Password");
        }    
       

        //Create Token
        var token = _jwtTokentGenerator.GenerateToken(user);


        return new AuthenticationResult(
            user,
            token
            );
    }



    public async Task<AuthenticationResult> Register(string username, string email, string password)
    {
        //Check user exists
        if(await _userRepository.GetAsync(user => user.Email == email) is not null)
        {
            throw new HttpResponseException(HttpStatusCode.Conflict, "User exists");
        }
        //Role
        if( await _roleRepository.GetAsync("1") is not Role role)
            throw new HttpResponseException(HttpStatusCode.NotFound, "Get role fail");

        User user = new User
        {
            UserName = username,
            Email = email,
            Password = password,
            RoleId = "1",
        };

        await _userRepository.AddAsync(user);

        //Generate Token


        var token = _jwtTokentGenerator.GenerateToken(user);



        return new AuthenticationResult(
            user,
            token
        );
    }
}
