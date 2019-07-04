// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Axle.Configurators
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using NJsonSchema;
    using NSwag;
    using NSwag.AspNetCore;
    using NSwag.SwaggerGeneration.AspNetCore;
    using NSwag.SwaggerGeneration.Processors.Security;

    public static class SwaggerConfigurator
    {
        public static Action<SwaggerUi3Settings<AspNetCoreToSwaggerGeneratorSettings>> Configure(Assembly assembly, string authority, string apiName, string clientId)
        {
            return settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling =
                    PropertyNameHandling.CamelCase;
                settings.GeneratorSettings.DefaultEnumHandling = EnumHandling.String;

                settings.PostProcess = document =>
                {
                    document.Info.Title = assembly.Attribute<AssemblyTitleAttribute>(attribute => attribute.Title);

                    document.Info.Version = assembly.Attribute<AssemblyInformationalVersionAttribute>(attribute => attribute.InformationalVersion);
                };

                var swaggerSecurityScheme = new SwaggerSecurityScheme
                {
                    Type = SwaggerSecuritySchemeType.OAuth2,
                    Name = "Authorization",
                    Description = "Press Authorize button to login with a valid user",
                    In = SwaggerSecurityApiKeyLocation.Header,

                    AuthorizationUrl = $"{authority}/connect/authorize",
                    Scopes = new Dictionary<string, string>
                    {
                        { apiName, "CFD Platform (Nova)" },
                        { $"{apiName}:server", "CFD Platform (Nova) server side methods" },
                        { $"{apiName}:mobile", "CFD Platform (Nova) mobile side methods" }
                    }
                };

                settings.GeneratorSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));

                settings.GeneratorSettings.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT Token", swaggerSecurityScheme));

                settings.OAuth2Client = new OAuth2ClientSettings
                {
                    ClientId = clientId
                };
            };
        }
    }
}
