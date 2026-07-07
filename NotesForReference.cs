using Azure.Core;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Resend;
using System.Drawing;
using System.Net;
using System.Numerics;
using System.Timers;

namespace IdentityManager
{
    public class NotesForReference
    {
        //ASP.NET Authentication
        //    Cookie Based Authentication
        //        Cookie-based authentication has been there for handling user authentication since a long time.
        //        Cookie-based authentication is stateful.
                  //Let's look at the flow of cookie-based authentication:
                    //  User enters their login credentials.
                    //  Server verifies the credentials are correct and creates a session which is then stored in a database.
                    //  A cookie with the session ID is placed in the user's browser.
                    //  On subsequent requests, the session ID is verified against the database and if valid the request processed.
                    //  Once a user logs out of the app, the session is destroyed both client-side and server-side


        //    Token Based Authentication
        //        Token-based authentication is more recent as compared to cookie-based authentication.
        //        It came in spotlight with more focus on API’s , SPA Applications and IOT.
        //        Token-based authentication is stateless
                  //Let's look at the flow of token-based authentication:
                  //      User enters their login credentials.
                  //      Server verifies the credentials are correct and returns a signed token.
                  //      This token is stored client-side
                  //      Subsequent requests to the server includes this token
                  //      The server decodes the JWT and if the token is valid processes the request.
                  //      Once a user logs out, the token is destroyed client-side.




    }
}
