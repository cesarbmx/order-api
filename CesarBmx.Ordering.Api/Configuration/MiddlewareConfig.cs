﻿using CesarBmx.Shared.Api.Configuration;
using Microsoft.AspNetCore.Builder;

namespace CesarBmx.Ordering.Api.Configuration
{
    public static class MiddlewareConfig
    {
        public static IApplicationBuilder ConfigureMiddleware(this IApplicationBuilder app)
        {
            app.ConfigureSharedMiddleware();

            return app;
        }
    }
}
