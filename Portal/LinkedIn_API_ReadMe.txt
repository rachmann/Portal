Welcome to LinkedIn APIs for .NET
---------------------------------
LinkedIn APIs for .NET is a robust and clean library that allows a developer to easily make LinkedIn API calls in Microsoft .NET application. The library is built on top of the Owin framework which means integration with LinkedIn external login provider comes out of the box. In other words, logging with LinkedIn works the same way as it works with the default login providers like Facebook, Google, and Twitter. 

The current version of the library provides the following functionality:
- Seamless integration with ASP.NET identity management.
- Retrieve basic profile of a LinkedIn user.
- Retrieve full profile of a LinkedIn user.

Project Support
-------------------
You can use LinkedIn APIs for .NET either in ASP.NET Web Forms or in ASP.NET MVC. However, the library as of now has been tested only in ASP.NET MVC 5 with .NET Framework 4.5.
Registering your LinkedIn App

You must have unique set of API Key and Secret Key to make the API calls. In order to get the API Key and the Secret Key, you need to register a LinkedIn app using your own LinkedIn account. Or someone else can register the LinkedIn app for you and then share the details with you.

Create a LinkedIn Application
-------------------------------
Ensure that you enter http://rootpath/signin-linkedin in the OAuth 2.0 Redirect URLs field where rootpath is the root path of your web application. Typically it would be your localhost during development and later on the actual public domain once you publish your web application. Example value for OAuth 2.0 Redirect URLs could be: http://localhost:18991/signin-linkedin. Keep in mind that you would be able to make API calls only from the application whose root path matches with the root path specified in the OAuth 2.0 Redirect URLs field. Of course, you can always change the value whenever you need to.

Once you are done with registering your LinkedIn app, you will get your API Key and Secret Key. You can always revisit the app on LinkedIn by going to https://www.linkedin.com/secure/developer.
Installing LinkedIn APIs for .NET Package in Your Project

The best and the recommended way to install the latest version of LinkedIn APIs for .NET in your Visual Studio project is through Nuget. From the Nuget's Package Manager Console in your Visual Studio .NET, simply execute the following command:

PM> Install-Package LinkedIn

You can open the Package Manager Console in Microsoft Visual Studio by clicking on the Tools > Library Package Manager > Package Manager Console menu.

The LinkedIn APIs for .NET package depends on the Owin.Security.Providers package which Nuget will automatically install in your project.

Configure LinkedIn Options
---------------------------
In the ConfigureAuth method of the Startup partial class in your web project, you need to tell Owin that you want to access a LinkedIn user's access token once the user is successfully authenticated with LinkedIn. You can achieve this by the following code:

public void ConfigureAuth(IAppBuilder app)
{
	...

	var linkedInOptions = new LinkedInAuthenticationOptions();

	linkedInOptions.ClientId = "Your LinkedIn API Key";
	linkedInOptions.ClientSecret = "Your LinkedIn Secret Key";
	
    linkedInOptions.Scope.Add("r_fullprofile");	

	linkedInOptions.Provider = new LinkedInAuthenticationProvider()
	{
		OnAuthenticated = async context =>
                {
                    context.Identity.AddClaim(new System.Security.Claims.Claim("LinkedIn_AccessToken", context.AccessToken));
                }
	};

	linkedInOptions.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;

	app.UseLinkedInAuthentication(linkedInOptions);

	...
}

The class LinkedInAuthenticationOptions is a member of the Owin.Security.Providers.LinkedIn namespace.
Store LinkedIn Access Token in the Identity Database

One last step before you would be ready to make API calls. You need to store user's access token in your Identity database. You need to do this when the external login provider (in this case LinkedIn) redirects to your application after successful login. Typically its the ExternalLoginCallback action method of the AccountController class. You can store access token in the Identity database using the following code:

var claimsIdentity = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

if (claimsIdentity != null)
{	
	var currentClaims = await UserManager.GetClaimsAsync(user.Id);
	var accessToken = claimsIdentity.FindAll(loginProvider + "_AccessToken").First();
        
	if (currentClaims.Count() <= 0)
	{
		await UserManager.AddClaimAsync(user.Id, accessToken);
	}
}

The above code store access token in the database only if it doesn't exist for the user.

Get the Profile Information
---------------------------
If you have followed all the above steps, it is just a matter of few lines of code to make API calls such as retrieving user's LinkedIn profile information. You need to first import two namespaces as follows:

using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using LinkedIn.Api.Client.Owin;
using LinkedIn.Api.Client.Owin.Profiles;

Next, you need to get the LinkedIn access token:

var am = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
var user = am.FindById(this.User.Identity.GetUserId());
var claim = user.Claims.ToList().Where(m => m.ClaimType == "LinkedIn_AccessToken").SingleOrDefault();

Once you have the access token, you can use the following code to get the LinkedIn user's full profile:

var client = new LinkedInApiClient(HttpContext.GetOwinContext().Request, claim.ClaimValue);
var profileApi = new LinkedInProfileApi(client);
var userProfile = await profileApi.GetFullProfileAsync();

If you are just interested in the basic profile, you can call the GetBasicProfileAsync() method instead. The GetBasicProfileAsync() method returns an instance of the LinkedInBasicProfile class while GetFullProfileAsync() returns an instance of the LinkedInFullProfile class. The LinkedInFullProfile class inherits from the LinkedInBasicProfile class. 