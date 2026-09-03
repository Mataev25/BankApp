using Microsoft.AspNetCore.Mvc;
using BankApp.Services;
using BankApp.Models;
using BankApp.Data;

namespace BankApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
    private readonly AuthService authService;

    public AuthController() {
        var context = new AppDbContext();
        authService = new AuthService(context);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request) {
        if (!authService.CardExists(request.CardNumber))
            return Unauthorized("Карта не найдена");

        if (authService.IsCardBlocked(request.CardNumber))
            return Unauthorized("Карта заблокирована");
        
        var user = authService.GetUserByCard(request.CardNumber);
        if (user == null)
            return Unauthorized("Пользователь не найден");
        
        if (user.IsFirstLogin)
            return Unauthorized("Требуется активация карты");

        if (!authService.ValidatePin(request.CardNumber, request.Pin)) {
            authService.IncrementFailedAttempts(request.CardNumber);
            return Unauthorized("Неверный PIN-код");
        }

        authService.ResetFailedAttempts(request.CardNumber);

        return Ok(new {
            Message = "Успешный вход",
            UserName = user.FullName,
            UserId = user.Id
        });
    }

    [HttpPost("activate")]
    public IActionResult Activate([FromBody] ActivateRequest request) {
        if (!authService.CardExists(request.CardNumber))
            return BadRequest("Карта не найдена");

        var success = authService.ActivateCard(request.CardNumber, request.Pin);
        if (!success)
            return BadRequest("Не удалось активировать карту");
        return Ok(new {Message = "Карта активирована"});
    }
}

public class LoginRequest {
    public string CardNumber {get; set;}
    public string Pin {get; set;}
}

public class ActivateRequest {
    public string CardNumber {get; set;}
    public string Pin {get; set;}
}