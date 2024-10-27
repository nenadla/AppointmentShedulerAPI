using AppointmentShedulerAPI.Models.Domain;
using AppointmentShedulerAPI.Models.DTO;
using AppointmentShedulerAPI.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace AppointmentShedulerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request)
        {
            
            var user = mapper.Map<User>(request);
            user.Counter = 0;
            user.Edited = 0;
            user.Cancelled = 0;
            
            try
            {
                await userRepository.CreateUserAsync(user);
            }
            catch (InvalidOperationException ex)
            {
                
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { message = "Došlo je do greške prilikom kreiranja korisnika." });
            }


            var response = mapper.Map<UserDto>(user);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
           
            var users = await userRepository.GetAllUsersAsync();

            
            var response = mapper.Map<List<UserDto>>(users);
            return Ok(response);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            
            var existingUser = await userRepository.FindByIdAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

           
            var response = mapper.Map<UserDto>(existingUser);
            return Ok(response);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateUserById([FromRoute] Guid id, [FromBody] UpdateUserRequestDto request)
        {
            var user = mapper.Map<User>(request);
            user.Id = id;

            try
            {
                var updatedUser = await userRepository.UpdateByIdAsync(user);

                if (updatedUser == null)
                {
                    return NotFound();
                }
                var response = mapper.Map<UserDto>(updatedUser);
                return Ok(response); 
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Došlo je do greške prilikom ažuriranja korisnika.", details = ex.Message });
            }
        }


        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            
            var user = await userRepository.DeleteUser(id);

            if (user == null)
            {
                return NotFound();
            }

            
            var response = mapper.Map<UserDto>(user);
            return Ok(response);
        }
    }
}
