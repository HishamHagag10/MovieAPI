

namespace MovieAPI.Repository
{
    public class UserRepo : BaseRepository<User>
    {
        public UserRepo(AppDBContext context) : base(context)
        {
        }
        public async Task<RepoResult<User>> ValidateUserAsync(User user)
        {
            var isUserNameExist = await _context.Users.AnyAsync(x=>x.UserName==user.UserName);
            if (isUserNameExist)
            {
                return RepoResult<User>.ValidationError("This User Name exist choose anthor one");
            }
            return RepoResult<User>.SuccessValidation();
        }

        internal async Task<User?> GetUserByUserNameAndPasswordAsync(string userName, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);
        }
    }
}
