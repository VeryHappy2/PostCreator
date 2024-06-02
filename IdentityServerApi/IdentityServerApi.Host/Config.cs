// using IdentityServer4.Models;

// namespace IdentityServerApi.Host
// {
//     public static class Config
//     {
//         public static IEnumerable<IdentityResource> GetIdentityResources()
//         {
//             return new IdentityResource[]
//             {
//                 new IdentityResources.OpenId(),
//                 new IdentityResources.Profile(),
//             };
//         }

//         public static IEnumerable<ApiResource> GetApis()
//         {
//             return new ApiResource[]
//             {
//                 new ApiResource("alevelwebsite.com")
//                 {
//                     Scopes = new List<Scope>
//                     {
//                         new Scope("angular"),
//                     },
//                 },
//                 new ApiResource("post")
//                 {
//                     Scopes = new List<Scope>
//                     {
//                         new Scope("postItem"),
//                         new Scope("postCategory"),
//                         new Scope("postComment"),
//                     },
//                 },
//             };
//         }

//         public static IEnumerable<Client> GetClients(IConfiguration configuration)
//         {
//             return new[]
//             {
//                 new Client
//                 {
//                     ClientId = "angular",
//                     ClientName = "Angular",
//                     AllowedGrantTypes = GrantTypes.Code,
//                     ClientSecrets = { new Secret("secret".Sha256()) },
//                     AllowedScopes = { "openid", "profile", "angular" },
//                 },
//                 new Client
//                 {
//                     ClientId = "post",

//                     // no interactive user, use the clientid/secret for authentication
//                     AllowedGrantTypes = GrantTypes.ClientCredentials,

//                     // secret for authentication
//                     ClientSecrets =
//                     {
//                         new Secret("secret".Sha256()),
//                     },
//                 },
//                 new Client
//                 {
//                     ClientId = "postswaggerui",
//                     ClientName = "Post Swagger UI",
//                     AllowedGrantTypes = GrantTypes.Implicit,
//                     AllowAccessTokensViaBrowser = true,
//                 },
//             };
//         }
//     }
// }