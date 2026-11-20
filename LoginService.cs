using demo.Context;
using Microsoft.EntityFrameworkCore;

namespace demo
{
    public class LoginService
    {
        private readonly DemoDbContext _context;

        public LoginService(DemoDbContext context)
        {
            _context = context;
        }

        public UserContext? GetUser(string login)
        {
            var user = _context.Users
                .Include(u => u.IdRoleNavigation)
                .FirstOrDefault(u => u.Login == login);

            if (user == null)
                return null;

            UserContext logUser = new()
            {
                Id = user.IdUser,
                Name = $"{user.FirstName} {user.LastName[0]}. {user.MiddleName?[0]}.",
                Role = user.IdRoleNavigation.RoleName
            };

            return logUser;
        }
    }
}
