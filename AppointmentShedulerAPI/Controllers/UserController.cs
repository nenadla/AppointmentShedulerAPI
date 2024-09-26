using AppointmentShedulerAPI.Models.Domain;
using AppointmentShedulerAPI.Models.DTO;
using AppointmentShedulerAPI.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace AppointmentShedulerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request)
        {
            var user = new Models.Domain.User
            {
                Username = request.Username,
                Email = request.Email,
                Phone = request.Phone,
                Counter = 0,
                Edited = 0,
                Cancelled = 0
            };
            await userRepository.CreateUserAsync(user);

            var response = new UserDto
            {
              Id=user.Id,
              Username = user.Username,
              Email = user.Email,
              Phone = user.Phone,
              Counter = user.Counter,
              Edited = user.Edited,
              Cancelled = user.Cancelled
            };
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userRepository.GetAllUsersAsync();
            var response= new List<UserDto>();

            foreach (var user in users)
            {
                response.Add(new UserDto
                {
                    Id = user.Id,
                    Username=user.Username,
                    Email = user.Email,
                    Phone = user.Phone,
                    Counter = user.Counter,
                    Edited = user.Edited,
                    Cancelled = user.Cancelled
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var exsistingUser = await userRepository.FindByIdAsync(id);

            if(exsistingUser == null)
            {
                return NotFound();
            }
            var response = new UserDto
            {
                Id = exsistingUser.Id,
                Username = exsistingUser.Username,
                Email = exsistingUser.Email,
                Phone = exsistingUser.Phone,
                Counter = exsistingUser.Counter,
                Edited = exsistingUser.Edited,
                Cancelled = exsistingUser.Cancelled
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateUserById([FromRoute] Guid id, UpdateUserRequestDto request)
        {
            var user = new User
            {
                Id=id,
                Username=request.Username,
                Email = request.Email,
                Phone = request.Phone,
                Counter=request.Counter,
                Edited = request.Edited,
                Cancelled = request.Cancelled
            };

            user = await userRepository.UpdateByIdAsync(user);

            if(user == null)
            {
                return NotFound();
            }
            var response = new UserDto
            {
                Id=user.Id,
                Username=user.Username,
                Email = request.Email,
                Phone = request.Phone,
                Counter = request.Counter,
                Edited = request.Edited,
                Cancelled = request.Cancelled
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var user = await userRepository.DeleteUser(id);

            if(user == null)
            {
                return NotFound();
            }

            var response = new UserDto
            {
                Id= user.Id,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                Counter = user.Counter,
                Edited = user.Edited,
                Cancelled = user.Cancelled
            };
            return Ok(response);
        }
    }
}
