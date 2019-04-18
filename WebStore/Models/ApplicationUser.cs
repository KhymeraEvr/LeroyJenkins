using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebStore.Models
{
    public class ApplicationUserManager : UserManager<IdentityUser>
    {

        static IEnumerable<IPasswordValidator<IdentityUser>> _passwordValidators = new List<IPasswordValidator<IdentityUser>>{new PasswordValidator<IdentityUser>()};

        public ApplicationUserManager(IUserStore<IdentityUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<IdentityUser> passwordHasher, 
            IEnumerable<IUserValidator<IdentityUser>> userValidators, 
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<IdentityUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, _passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
