using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace AspNetNetwork.Application.Core.Settings.User;

/// <summary>
/// Represents the identity configuration class.
/// </summary>
public static class IdentityConfiguration
{
    /// <summary>
    /// The api scopes list.
    /// </summary>
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("ProgrammerAnswers.Micro.QuestionsAPI", "Question API"),
            new("ProgrammerAnswers.Micro.ImageAPI", "Image API")
        };

    /// <summary>
    /// The identity resources list.
    /// </summary>
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    /// <summary>
    /// The api resources list.
    /// </summary>
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new("ProgrammerAnswers.Micro.QuestionsAPI", "Question API", new []
                { JwtClaimTypes.Name})
            {
                Scopes = {"ProgrammerAnswers.Micro.QuestionsAPI"}
            },
            new("ProgrammerAnswers.Micro.ImageAPI", "Image API", new []
                { JwtClaimTypes.Name})
            {
                Scopes = {"ProgrammerAnswers.Micro.ImageAPI"}
            },
        };

    /// <summary>
    /// The clients list.
    /// </summary>
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "ProgrammerAnswers.Micro-image",
                ClientName = "ProgrammerAnswers.Micro Image",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                RedirectUris =
                {
                    "http://localhost:44460/signin"
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:44460"
                },
                PostLogoutRedirectUris =
                {
                    "http://localhost:44460/signout"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "ProgrammerAnswers.Micro.ImageAPI"
                },
                AllowAccessTokensViaBrowser = true
            },
            new Client
            {
                ClientId = "ProgrammerAnswers.Micro-question",
                ClientName = "ProgrammerAnswers.Micro Question",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                RedirectUris =
                {
                    "http://localhost:44460/signin"
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:44460"
                },
                PostLogoutRedirectUris =
                {
                    "http://localhost:44460/signout"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "ProgrammerAnswers.Micro.QuestionAPI"
                },
                AllowAccessTokensViaBrowser = true
            }
        };
}