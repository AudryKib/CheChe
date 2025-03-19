using Duende.IdentityServer.Models;

namespace IdentityService
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("CheCheApp", "CheChe full Access"),
               
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "postman",
                    ClientName = "postman",
                    AllowedScopes = { "openid", "profile","CheCheApp" },
                    RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                    ClientSecrets = new[] {new Secret("NotASecret".Sha256()) },
                    AllowedGrantTypes = { GrantType.ResourceOwnerPassword}

                }


            };
    }
}
