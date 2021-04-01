using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Microsoft.Fee.Services.CorporateSponsorIdentity.API.Configuration
{
    public class Config
    {
        // ApiResources define the apis in the system
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("scholarships", "Scholarships Service"),
                new ApiResource("mobilescholarshipagg", "Mobile Scholarship Aggregator"),
                new ApiResource("webscholarshipagg", "Web Scholarship Aggregator"),
            };
        }

        // Identity resources are data like user ID, name, or email address of a user
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        // A client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
        {
            return new List<Client>
            {
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "Fee SPA Scholarship OpenId Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris =           { $"{clientsUrl["SponsorSpa"]}/" },
                    RequireConsent = false,
                    PostLogoutRedirectUris = { $"{clientsUrl["SponsorSpa"]}/" },
                    AllowedCorsOrigins =     { $"{clientsUrl["SponsorSpa"]}" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "scholarships",
                        "webscholarshipagg"
                    },
                },
                new Client
                {
                    ClientId = "xamarin",
                    ClientName = "Fee Xamarin Scholarship OpenId Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,                    
                    // Used to retrieve the access token on the back channel.
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RedirectUris = { clientsUrl["SponsorXamarin"] },
                    RequireConsent = false,
                    RequirePkce = true,
                    PostLogoutRedirectUris = { $"{clientsUrl["SponsorXamarin"]}/Account/Redirecting" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "scholarships",
                        "mobilescholarshipagg"
                    },
                    // Allow requesting refresh tokens for long lived API access
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "Sponsor MVC Client",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientUri = $"{clientsUrl["SponsorMvc"]}",                             // Public uri of the client
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowAccessTokensViaBrowser = false,
                    RequireConsent = false,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = new List<string>
                    {
                        $"{clientsUrl["SponsorMvc"]}/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{clientsUrl["SponsorMvc"]}/signout-callback-oidc"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "scholarships",
                        "webscholarshipagg"
                    },
                    AccessTokenLifetime = 60*60*2, // 2 hours
                    IdentityTokenLifetime= 60*60*2 // 2 hours
                },
                new Client
                {
                    ClientId = "scholarshipswaggerui",
                    ClientName = "Scholarships Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientsUrl["ScholarshipApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientsUrl["ScholarshipApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "scholarships"
                    }
                },
                //new Client
                //{
                //    ClientId = "applyingswaggerui",
                //    ClientName = "Applying Swagger UI",
                //    AllowedGrantTypes = GrantTypes.Implicit,
                //    AllowAccessTokensViaBrowser = true,

                //    RedirectUris = { $"{clientsUrl["ApplyingApi"]}/swagger/oauth2-redirect.html" },
                //    PostLogoutRedirectUris = { $"{clientsUrl["ApplyingApi"]}/swagger/" },

                //    AllowedScopes =
                //    {
                //        "applications"
                //    }
                //},
                new Client
                {
                    ClientId = "mobilescholarshipaggswaggerui",
                    ClientName = "Mobile Scholarship Aggregattor Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientsUrl["MobileScholarshipAgg"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientsUrl["MobileScholarshipAgg"]}/swagger/" },

                    AllowedScopes =
                    {
                        "mobilescholarshipagg"
                    }
                },
                new Client
                {
                    ClientId = "webscholarshipaggswaggerui",
                    ClientName = "Web Scholarship Aggregattor Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientsUrl["WebScholarshipAgg"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientsUrl["WebScholarshipAgg"]}/swagger/" },

                    AllowedScopes =
                    {
                        "webscholarshipagg"
                    }
                }
            };
        }
    }
}
