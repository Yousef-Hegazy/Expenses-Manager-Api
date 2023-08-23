using System.Security.Claims;
using API.Data;
using API.Models.Domain;
using API.Models.DTOs.User;
using API.Models.Enums;
using API.Security;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;
    private readonly DataContext _context;

    public AccountController(UserManager<AppUser> userManager, IMapper mapper, TokenService tokenService,
        DataContext context)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
        _context = context;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is null) return NotFound("This email does not exist");

        var result = await _userManager.CheckPasswordAsync(user: user, password: loginDto.Password);

        if (!result) return Unauthorized();

        var userDto = _mapper.Map<UserDto>(user);

        userDto.Token = _tokenService.CreateToken(user);

        return Ok(userDto);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(u => u.Email == registerDto.Email))
            return BadRequest("Email already exists");

        if (await _userManager.Users.AnyAsync(u => u.UserName == registerDto.Username))
            return BadRequest("Username already exists");

        var user = _mapper.Map<AppUser>(registerDto);

        var res = await _userManager.CreateAsync(user: user, password: registerDto.Password);

        if (!res.Succeeded) return BadRequest("Failed to create user");

        var userDto = _mapper.Map<UserDto>(user);

        userDto.Token = _tokenService.CreateToken(user);
        
        var mailRes = await MailServices.SendEmail(registerDto.Email, registerDto.Username,
            $"{registerDto.Link}/{user.Id}");

        if (!mailRes) return BadRequest("Something went wrong while sending a confirmation email");

        return Ok(userDto);
    }

    [HttpPost("send-confirmation-email")]
    public async Task<IActionResult> SendConfirmationEmail(EmailConfirmationDto emailConfirmationDto)
    {
        var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

        if (user is null) return Unauthorized();
        
        var mailRes = await MailServices.SendEmail(user.Email, user.UserName,
            $"{emailConfirmationDto.Link}/{user.Id}");

        if (!mailRes) return BadRequest("Something went wrong while sending a confirmation email");

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrentUser()
    {
        var email = HttpContext.User.FindFirstValue(claimType: ClaimTypes.Email);

        if (email is null) return Unauthorized();

        var user = await _userManager.FindByEmailAsync(email);

        var userDto = _mapper.Map<UserDto>(user);

        userDto.Token = _tokenService.CreateToken(user);

        return Ok(userDto);
    }

    [HttpPatch("confirm-email")]
    public async Task<IActionResult> ConfirmEmail()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user is null) return Unauthorized();

        user.EmailConfirmed = true;

        await _context.SaveChangesAsync();

        var userDto = _mapper.Map<UserDto>(user);

        userDto.Token = _tokenService.CreateToken(user);

        return Ok(userDto);
    }

    [AllowAnonymous]
    [HttpPost("send-password-email")]
    public async Task<IActionResult> SendPasswordEmail(PasswordResetEmailDto passwordResetEmailDto)
    {
        var user = await _userManager.FindByEmailAsync(passwordResetEmailDto.Email);

        if (user is null) return NotFound("No user with this email exists");

        var mailRes = await MailServices.SendEmail(user.Email, user.UserName,
            $"{passwordResetEmailDto.Link}/{user.Id}", EmailType.ChangePassword);

        if (!mailRes) return BadRequest("Something went wrong while sending a confirmation email");

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("id-login")]
    public async Task<IActionResult> IdLogin(IdLoginDto idLoginDto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == idLoginDto.Id);

        if (user is null) return NotFound("No such user");

        var userDto = _mapper.Map<UserDto>(user);

        userDto.Token = _tokenService.CreateToken(user);

        return Ok(userDto);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u =>
            u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user is null) return Unauthorized();

        await _userManager.RemovePasswordAsync(user: user);

        await _userManager.AddPasswordAsync(user: user, password: changePasswordDto.NewPassword);

        var userDto = _mapper.Map<UserDto>(user);

        userDto.Token = _tokenService.CreateToken(user);

        return Ok(userDto);
    }

    [HttpPut("edit-profile")]
    public async Task<IActionResult> EditProfile(EditProfileDto editProfileDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user is null) return Unauthorized();

        if (user.Email != editProfileDto.Email)
        {
            var mailRes = await MailServices.SendEmail(editProfileDto.Email, editProfileDto.Username,
                $"{editProfileDto.Link}/{user.Id}");

            if (!mailRes) return BadRequest("Something went wrong while sending a confirmation email");

            user.EmailConfirmed = false;
        }

        _mapper.Map<EditProfileDto, AppUser>(editProfileDto, user);

        await _context.SaveChangesAsync();

        var userDto = _mapper.Map<UserDto>(user);

        userDto.Token = _tokenService.CreateToken(user);

        return Ok(userDto);
    }
}