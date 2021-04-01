using Microsoft.Fee.WebMVC.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Microsoft.Fee.WebMVC.Services
{
    public class IdentityParser : IIdentityParser<ApplicationUser>
    {
        public ApplicationUser Parse(IPrincipal principal)
        {
            // Pattern matching 'is' expression
            // assigns "claims" if "principal" is a "ClaimsPrincipal"
            if (principal is ClaimsPrincipal claims)
            {
                return new ApplicationUser
                {
                    Gender = int.Parse(claims.Claims.FirstOrDefault(x => x.Type == "missing")?.Value ?? "0"),
                    Hobby = int.Parse(claims.Claims.FirstOrDefault(x => x.Type == "missing")?.Value ?? "0"),
                    Location = int.Parse(claims.Claims.FirstOrDefault(x => x.Type == "missing")?.Value ?? "0"),
                    School = int.Parse(claims.Claims.FirstOrDefault(x => x.Type == "missing")?.Value ?? "0"),
                    PaymentType = int.Parse(claims.Claims.FirstOrDefault(x => x.Type == "missing")?.Value ?? "0"),
                    Email = claims.Claims.FirstOrDefault(x => x.Type == "email")?.Value ?? "",
                    Id = claims.Claims.FirstOrDefault(x => x.Type == "sub")?.Value ?? "",
                    FirstName = claims.Claims.FirstOrDefault(x => x.Type == "first_name")?.Value ?? "",
                    Surname = claims.Claims.FirstOrDefault(x => x.Type == "surname")?.Value ?? "",
                    PhoneNumber = claims.Claims.FirstOrDefault(x => x.Type == "phone_number")?.Value ?? "",
                    DateofBirth = claims.Claims.FirstOrDefault(x => x.Type == "profile_dateofbirth")?.Value ?? "",
                    IDNumber = claims.Claims.FirstOrDefault(x => x.Type == "profile_idnumber")?.Value ?? "",
                    Request = decimal.Parse(claims.Claims.FirstOrDefault(x => x.Type == "missing")?.Value ?? "0")
                };
            }
            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}


