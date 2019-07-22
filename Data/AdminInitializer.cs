using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using MvcBlog.Models;

namespace MvcBlog.Data
{
    public static class AdminInitializer
    {
        public static void SeedAdmin(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync("szalatamarzena@gmail.com").Result == null)
            {
                var user = new ApplicationUser { UserName = "szalatamarzena@gmail.com", Email = "szalatamarzena@gmail.com" };
                var result = userManager.CreateAsync(user, "adminq1@W").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}