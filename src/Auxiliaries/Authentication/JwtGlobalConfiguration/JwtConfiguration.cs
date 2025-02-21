﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtGlobalConfiguration
{
    public static class JwtConfiguration
    {
        public const string ValidIssuer = "JwtAuthenticationApp"; // Refactor both as external variables
        public const string SecurityKey = "FtlpB4ljwLW9aqj/jS/x5u7Hxu7823563278956896789&%(^&%&(*RCKKztqUbOxvBtIx0MsXYM+pEwb2kzG0yz99tD";
        public const int ExpirationTimeInMinutes = 60;

        public static void AddCommonJwtConfiguration(this IServiceCollection services)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = ValidIssuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey)),
                    ValidateAudience = false
                };
            });
        }
    }
}
