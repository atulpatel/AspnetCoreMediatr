namespace MembershipEligibilitySearch.Api.PipelineBehavior
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="BasicAuthMiddleware" />
    /// </summary>
    public class BasicAuthMiddleware
    {
        /// <summary>
        /// Defines the next
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        /// Defines the realm
        /// </summary>
        private readonly string realm;

        /// <summary>
        /// Defines the USERNAME
        /// </summary>
        internal const string USERNAME = "MembershipEligibilitySearchUser";

        /// <summary>
        /// Defines the PASSWORD
        /// </summary>
        internal const string PASSWORD = "E76Wmz<:uD_n";
            
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next<see cref="RequestDelegate"/></param>
        /// <param name="realm">The realm<see cref="string"/></param>
        public BasicAuthMiddleware(RequestDelegate next, string realm)
        {
            this.next = next;
            this.realm = realm;
        }

        /// <summary>
        /// The Invoke
        /// current basic Authorization header is  "Basic Q29uc3VtZXJBcHBsaWNhdGlvblNlYXJjaFVzZXI6amVGQDs+Sjl2ZHhO"
        /// </summary>
        /// <param name="context">The context<see cref="HttpContext"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                // Get the encoded username and password
                var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                // Decode from Base64 to string
                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                //// Split username and password
                var username = decodedUsernamePassword.Split(':', 2)[0];
                var password = decodedUsernamePassword.Split(':', 2)[1];
                // Check if login is correct
                if (IsAuthorized(username, password))
                {
                    await next.Invoke(context);
                    return;
                }
            }
            // Return authentication type (causes browser to show login dialog)
            context.Response.Headers["WWW-Authenticate"] = "Basic";
            // Add realm if it is not null
            if (!string.IsNullOrWhiteSpace(realm))
            {
                context.Response.Headers["WWW-Authenticate"] += $" realm=\"{realm}\"";
            }
            // Return unauthorized
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        /// <summary>
        /// The IsAuthorized
        /// </summary>
        /// <param name="username">The username<see cref="string"/></param>
        /// <param name="password">The password<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool IsAuthorized(string username, string password)
        {

            //// Check that username and password are correct
            return username.Equals(USERNAME, StringComparison.InvariantCultureIgnoreCase)
                   && password.Equals(PASSWORD);
        }
    }
}
