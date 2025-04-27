using MediatR;
using MovieAPI.MediatR_Features.Actor_Commands;
using MovieAPI.MediatR_Features.User_Commands;
using MovieAPI.Repository;

namespace MovieAPI.MediatR_Features.User_Commands_Handlers
{
    public class DeleteUserCommandHandler(UserRepo _userRepo) :
        IRequestHandler<DeleteUserCommand, RequestResult<bool>>
    {
        public async Task<RequestResult<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.Id);
            if (user == null)
            {
                return RequestResult<bool>.Failure(400, "User Unauthorized");
            }

            _userRepo.Delete(user);
            await _userRepo.SaveChangesAsync();
            return RequestResult<bool>.Success(true);
        }
    }
}
