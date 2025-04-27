using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.User_Commands;
using MovieAPI.models;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.User_Commands_Handlers
{
    public class RegisterUserCommandHandler (UserRepo _userRepo,IMapper _mapper)
        : IRequestHandler<RegisterUserCommand, RequestResult<bool>>
    {
        public async Task<RequestResult<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);
            user.Role = Role.User;
            var validate = await _userRepo.ValidateUserAsync(user);
            if (!validate.IsCorrect)
            {
                return RequestResult<bool>.Failure(400, validate.ErrorMessage);
            }
            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();
            return RequestResult<bool>.Success(true);
        }

    }
}
