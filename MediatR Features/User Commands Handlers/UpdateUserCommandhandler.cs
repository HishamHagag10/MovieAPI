using AutoMapper;
using MediatR;
using MovieAPI.MediatR_Features.User_Commands;
using MovieAPI.models;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.User_Commands_Handlers
{
    public class UpdateUserCommandhandler(UserRepo _userRepo, IMapper _mapper)
        : IRequestHandler<UpdateUserCommand, RequestResult<bool>>
    {
        public async Task<RequestResult<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.Id);
            if (user == null)
            {
                return RequestResult<bool>.Failure(404, "The User doesn't exist");
            }
            user = _mapper.Map<UpdateUserCommand, User>(request,user);
            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();
            return RequestResult<bool>.Success(true);
        }
    }
}
