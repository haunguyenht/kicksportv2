﻿using KickSport.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace KickSport.Web.SeedData
{
    public class SeedAdmin
    {
        private readonly RequestDelegate _next;

        public SeedAdmin(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider provider)
        {
            var userManager = provider.GetService<UserManager<ApplicationUser>>();

            if (!userManager.Users.Any())
            {
                var roleManager = provider.GetService<RoleManager<IdentityRole>>();
                var adminRoleExists = roleManager.RoleExistsAsync("Administrator").Result;
                if (!adminRoleExists)
                {
                    var result = roleManager.CreateAsync(new IdentityRole("Administrator")).Result;
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException();
                    }
                }

                var userRoleExists = roleManager.RoleExistsAsync("User").Result;
                if (!userRoleExists)
                {
                    var result = roleManager.CreateAsync(new IdentityRole("User")).Result;
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException();
                    }
                }

                var user = new ApplicationUser
                {
                    Email = "admin@admin.com",
                    UserName = "Admin",
                    FirstName = "Admin",
                    LastName = "Admin"
                };

                var userResult = await userManager.CreateAsync(user, "12345678");
                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException();
                }

                var roleResult = await userManager.AddToRoleAsync(user, "Administrator");
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException();
                }
            }

            await _next(context);
        }
    }
}
