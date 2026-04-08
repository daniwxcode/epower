using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Infrastructure.Contexts;
using BlazorHero.CleanArchitecture.Infrastructure.Helpers;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Identity;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using BlazorHero.CleanArchitecture.Shared.Constants.Role;
using BlazorHero.CleanArchitecture.Shared.Constants.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly IStringLocalizer<DatabaseSeeder> _localizer;
        private readonly BlazorHeroContext _db;
        private readonly UserManager<BlazorHeroUser> _userManager;
        private readonly RoleManager<BlazorHeroRole> _roleManager;

        public DatabaseSeeder(
            UserManager<BlazorHeroUser> userManager,
            RoleManager<BlazorHeroRole> roleManager,
            BlazorHeroContext db,
            ILogger<DatabaseSeeder> logger,
            IStringLocalizer<DatabaseSeeder> localizer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _logger = logger;
            _localizer = localizer;
        }

        public void Initialize()
        {
            AddAdministrator();
            AddBasicUser();
            AddSellerRole();
            AddSupervisorRole();
            _db.SaveChanges();
        }

        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var adminRole = new BlazorHeroRole(RoleConstants.AdministratorRole, _localizer["Administrator role with full permissions"]);
                var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                    _logger.LogInformation(_localizer["Seeded Administrator Role."]);
                }
                //Check if User Exists
                var superUser = new BlazorHeroUser
                {
                    FirstName = "Mukesh",
                    LastName = "Murugan",
                    Email = "mukesh@blazorhero.com",
                    UserName = "mukesh",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true,
                    MustChangePassword = false,
                    PasswordChangedOn = DateTime.UtcNow
                };
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, UserConstants.DefaultPassword);
                    var result = await _userManager.AddToRoleAsync(superUser, RoleConstants.AdministratorRole);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(_localizer["Seeded Default SuperAdmin User."]);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }
                foreach (var permission in Permissions.GetRegisteredPermissions())
                {
                    await _roleManager.AddPermissionClaim(adminRoleInDb, permission);
                }
            }).GetAwaiter().GetResult();
        }

        private void AddBasicUser()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var basicRole = new BlazorHeroRole(RoleConstants.BasicRole, _localizer["Basic role with default permissions"]);
                var basicRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
                if (basicRoleInDb == null)
                {
                    await _roleManager.CreateAsync(basicRole);
                    _logger.LogInformation(_localizer["Seeded Basic Role."]);
                }
                //Check if User Exists
                var basicUser = new BlazorHeroUser
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@blazorhero.com",
                    UserName = "johndoe",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true,
                    MustChangePassword = false,
                    PasswordChangedOn = DateTime.UtcNow
                };
                var basicUserInDb = await _userManager.FindByEmailAsync(basicUser.Email);
                if (basicUserInDb == null)
                {
                    await _userManager.CreateAsync(basicUser, UserConstants.DefaultPassword);
                    await _userManager.AddToRoleAsync(basicUser, RoleConstants.BasicRole);
                    _logger.LogInformation(_localizer["Seeded User with Basic Role."]);
                }
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Rôle Vendeur : accès POS, historique de ses ventes, gestion de sa caisse.
        /// </summary>
        private void AddSellerRole()
        {
            Task.Run(async () =>
            {
                var roleName = RoleConstants.SellerRole;
                var roleInDb = await _roleManager.FindByNameAsync(roleName);
                if (roleInDb == null)
                {
                    await _roleManager.CreateAsync(new BlazorHeroRole(roleName, "Vendeur ambulant — accès POS et caisse"));
                    roleInDb = await _roleManager.FindByNameAsync(roleName);
                    _logger.LogInformation($"Seeded {roleName} Role.");
                }

                string[] sellerPermissions =
                [
                    Permissions.CashPower.View,
                    Permissions.CashPower.Create,
                    Permissions.CashPower.Search,
                    Permissions.CashShifts.ViewOwn,
                    Permissions.CashShifts.Open,
                    Permissions.CashShifts.Close,
                    Permissions.Remittances.ViewOwn,
                ];

                foreach (var permission in sellerPermissions)
                {
                    await _roleManager.AddPermissionClaim(roleInDb, permission);
                }
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Rôle Superviseur : toutes les permissions vendeur + recouvrement, dashboard, gestion vendeurs.
        /// </summary>
        private void AddSupervisorRole()
        {
            Task.Run(async () =>
            {
                var roleName = RoleConstants.SupervisorRole;
                var roleInDb = await _roleManager.FindByNameAsync(roleName);
                if (roleInDb == null)
                {
                    await _roleManager.CreateAsync(new BlazorHeroRole(roleName, "Superviseur — recouvrement, dashboard et gestion vendeurs"));
                    roleInDb = await _roleManager.FindByNameAsync(roleName);
                    _logger.LogInformation($"Seeded {roleName} Role.");
                }

                string[] supervisorPermissions =
                [
                    // Cash Power
                    Permissions.CashPower.View,
                    Permissions.CashPower.Create,
                    Permissions.CashPower.Export,
                    Permissions.CashPower.Search,

                    // Caisses
                    Permissions.CashShifts.ViewOwn,
                    Permissions.CashShifts.ViewAll,
                    Permissions.CashShifts.Open,
                    Permissions.CashShifts.Close,

                    // Remises de fonds
                    Permissions.Remittances.ViewOwn,
                    Permissions.Remittances.ViewAll,
                    Permissions.Remittances.Create,
                    Permissions.Remittances.Validate,

                    // Dashboard
                    Permissions.Dashboards.View,

                    // Vendeurs
                    Permissions.Sellers.View,
                    Permissions.Sellers.Create,
                    Permissions.Sellers.Edit,

                    // Immobilier (lecture)
                    Permissions.BuildingParams.View,
                    Permissions.BuildingParams.Search,
                ];

                foreach (var permission in supervisorPermissions)
                {
                    await _roleManager.AddPermissionClaim(roleInDb, permission);
                }
            }).GetAwaiter().GetResult();
        }
    }
}