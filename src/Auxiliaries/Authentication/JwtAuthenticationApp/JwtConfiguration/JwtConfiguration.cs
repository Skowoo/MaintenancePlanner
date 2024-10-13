﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtAuthenticationApp.JwtConfiguration
{
    public static class CommonJwtConfiguration
    {
        private const string ValidIssuer = "JwtAuthenticationApp";
        private const string SecurityKey = "FtlpB4ljwLW9aqj/jS/x5u7Hxu7823563278956896789&%(^&%&(*RCKKztqUbOxvBtIx0MsXYM+pEwb2kzG0yz99tD";

        public static void AddCommonJwtConfiguration(this IServiceCollection services)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey)),
                    ValidateIssuerSigningKey = true
                };
            });
        }
    }
}
