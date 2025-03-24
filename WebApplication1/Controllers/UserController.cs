using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Api.DTOs;
using WebApplication1.Api.Errors;
using WebApplication1.Repositry.Data;
using WebApplication1.Core.Repositries;
using WebApplication1.Service.WebApplication1.Repositry.WebApplication1.Core.services;

namespace WebApplication1.Api.Controllers
{
    public class UserController : ApiBaseController
    {
        private readonly AuthService _authService;
        private readonly EcommerceContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserController> _logger;
        private readonly IGenericRepository<User> userRepository;

        public UserController(AuthService authService,
                              EcommerceContext dbContext,
                              UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IEmailService emailService,
                              ILogger<UserController> logger,
                              IGenericRepository<User> _userRepository)
        {
            _authService = authService;
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _logger = logger;
            userRepository = _userRepository;
        }

        #region Register
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest(new ApiErrorResponse(400, "This email is already in use"));

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new ApiErrorResponse(400, $"Error: {errors}"));
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                { "token", token },
                { "email", user.Email }
            };

            var callback = QueryHelpers.AddQueryString("https://localhost:7173/User/EmailConfirmation", param);
            await _emailService.sendEmailAsync(user.Email, "Confirm Email", $"Click here to verify: {callback}");

            return Ok("Registration successful! Please check your email.");
        }
        #endregion

        #region Email Confirmation
        [HttpGet("EmailConfirmation")]

        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            Console.WriteLine($"Received Token: {token}");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)

                return BadRequest(new ApiErrorResponse(404, "User not found"));

            var isconfirmed = await _userManager.ConfirmEmailAsync(user, token);

            if (!isconfirmed.Succeeded)

            {
                Console.WriteLine($"Email confirmation failed. Errors: {string.Join(", ", isconfirmed.Errors.Select(e => e.Description))}");

                return BadRequest(new ApiErrorResponse(400, "An error occurred while confirming your email"));

            }
            return Ok(new { message = "Email verified successfully" });

        }

        #endregion

        #region Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized(new ApiErrorResponse(401, "Invalid email or password"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new ApiErrorResponse(401, "Invalid credentials"));

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized(new ApiErrorResponse(401, "Verify your email first"));

            var token = await _authService.RegisterOrLogin(user);
            return Ok(new LoginResponseDto { Message = "Login Successful", Token = token });
        }
        #endregion


        #region Forget Password
        [HttpPost("Forget-Password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(new ApiErrorResponse(400, "Email not found"));

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = $"https://localhost:7173/User/Reset-Password?token={Uri.EscapeDataString(resetToken)}&email={Uri.EscapeDataString(request.Email)}";

            await _emailService.sendEmailAsync(
                request.Email,
                "Reset Password",
                $"Click the link below to securely reset your password:\r\n{resetLink}\r\n\r\n" +
                $"If you did not request a password reset, please ignore this message.\r\n\r\nBest regards,\r\nThe PlantCare Team"
            );

            return Ok(new { Message = "You will receive reset instructions. Check your email." });
        }

        #endregion

        #region Reset Password
        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string token, [FromQuery] string email, [FromBody] ResetPasswordRequestDto request)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                return BadRequest(new ApiErrorResponse(400, "Token or email is missing."));

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new ApiErrorResponse(401, "Invalid email address."));

            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new ApiErrorResponse(400, $"Password reset failed: {errors}"));
            }

            return Ok(new { Message = "Password updated successfully!" });
        }
        #endregion


        #region Logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            var token = await _dbContext.Tokens.FirstOrDefaultAsync(t => t.token == request.Token && t.isValid);
            if (token == null)
                return Unauthorized(new ApiErrorResponse(401, "Invalid or expired token"));

            token.isValid = false;
            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "Logout successful" });
        }
        #endregion


        #region Get_By_ID

        [HttpGet("{Id}")]
        public async Task<ActionResult> Get_User_ById(Guid Id, [FromHeader] string tk)
        {
            var token = await _dbContext.Tokens.FirstOrDefaultAsync(T => T.token == tk && T.isValid);
            if (token == null)
            {
                return Unauthorized(new ApiErrorResponse(401, "Invalid Token"));
            }
            var result = await userRepository.GetByIdAsync(Id);
            if (result == null)
                return NotFound(new ApiErrorResponse(404, "there is no product found"));
            return Ok(result);

        }
        #endregion

      
        #region Delete_By_ID
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProductById(Guid Id, [FromHeader] string tk)
        {
            var token = await _dbContext.Tokens.FirstOrDefaultAsync(t => t.token == tk && t.isValid);
            if (token is null)
                return Unauthorized(new ApiErrorResponse(401, "Invalid Token"));
            var user = await _dbContext.Users.FindAsync(Id);
            if (user is null)
                return NotFound(new ApiErrorResponse(404, "Product Not Found"));
            var isDeleted = await userRepository.DeleteAsync(user);
            if (isDeleted == 0)
                return NotFound(new ApiErrorResponse(405, "something went wrong"));
            return Ok("Product Deleted Successfully");
          

        }
        #endregion


    }
}
