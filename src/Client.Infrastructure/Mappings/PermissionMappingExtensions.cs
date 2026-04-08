using System.Collections.Generic;
using System.Linq;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Mappings;

public static class PermissionMappingExtensions
{
    public static RoleClaimRequest ToRoleClaimRequest(this RoleClaimResponse response) => new()
    {
        Id = response.Id,
        RoleId = response.RoleId,
        Type = response.Type,
        Value = response.Value,
        Description = response.Description,
        Group = response.Group,
        Selected = response.Selected
    };

    public static PermissionRequest ToPermissionRequest(this PermissionResponse response) => new()
    {
        RoleId = response.RoleId,
        RoleClaims = response.RoleClaims?.Select(c => c.ToRoleClaimRequest()).ToList()
            ?? (IList<RoleClaimRequest>)new List<RoleClaimRequest>()
    };
}
