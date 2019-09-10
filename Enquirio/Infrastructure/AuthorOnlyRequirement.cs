using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Enquirio.Data;
using Microsoft.AspNetCore.Authorization;

// Requirement for ensuring only the authors of posts can modify them. Users with 
// role in BypassRoles can bypass the requirement and modify any post.
namespace Enquirio.Infrastructure {

    public class AuthorOnlyRequirement : IAuthorizationRequirement {
        public string[] BypassRoles { get; set; }

        public AuthorOnlyRequirement(string[] bypassRoles) {
            BypassRoles = bypassRoles;
        }
    }

    public class AuthorOnlyHandler : AuthorizationHandler<AuthorOnlyRequirement> {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                AuthorOnlyRequirement requirement) {

            if (context.Resource is IPost post) {
                var user = context.User;

                if (IsInBypassRole(user, requirement.BypassRoles) 
                    || IsAuthor(user, post)) {
                    context.Succeed(requirement);
                } else {
                    context.Fail();
                }

            } else if (context.Resource != null) {
                throw new NotSupportedException();
            }

            return Task.CompletedTask;
        }

        private bool IsInBypassRole(ClaimsPrincipal user, string[] roles) 
            => user.FindAll(ClaimTypes.Role)
                .Select(c => c.Value)
                .Intersect(roles)
                .Any();

        private bool IsAuthor(ClaimsPrincipal user, IPost post) 
            => user.FindFirst(ClaimTypes.NameIdentifier).Value == post.UserId;
    }
}
