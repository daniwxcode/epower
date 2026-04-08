using System.Collections.Generic;
using System.Linq;
using BlazorHero.CleanArchitecture.Application.Models.Chat;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Audit;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Audit;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Identity;

namespace BlazorHero.CleanArchitecture.Infrastructure.Mappings;

internal static class UserMappingExtensions
{
    public static UserResponse ToUserResponse(this BlazorHeroUser user) => new()
    {
        Id = user.Id,
        UserName = user.UserName,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        IsActive = user.IsActive,
        EmailConfirmed = user.EmailConfirmed,
        PhoneNumber = user.PhoneNumber,
        ProfilePictureDataUrl = user.ProfilePictureDataUrl
    };

    public static List<UserResponse> ToUserResponseList(this IEnumerable<BlazorHeroUser> users)
        => users.Select(u => u.ToUserResponse()).ToList();

    public static ChatUserResponse ToChatUserResponse(this BlazorHeroUser user) => new()
    {
        Id = user.Id,
        UserName = user.UserName,
        ProfilePictureDataUrl = user.ProfilePictureDataUrl,
        FirstName = user.FirstName,
        LastName = user.LastName,
        EmailAddress = user.Email
    };

    public static IEnumerable<ChatUserResponse> ToChatUserResponseList(this IEnumerable<BlazorHeroUser> users)
        => users.Select(u => u.ToChatUserResponse());
}

internal static class RoleClaimMappingExtensions
{
    public static RoleClaimResponse ToRoleClaimResponse(this BlazorHeroRoleClaim claim) => new()
    {
        Id = claim.Id,
        RoleId = claim.RoleId,
        Type = claim.ClaimType,
        Value = claim.ClaimValue,
        Description = claim.Description,
        Group = claim.Group
    };

    public static List<RoleClaimResponse> ToRoleClaimResponseList(this IEnumerable<BlazorHeroRoleClaim> claims)
        => claims.Select(c => c.ToRoleClaimResponse()).ToList();

    public static BlazorHeroRoleClaim ToBlazorHeroRoleClaim(this RoleClaimRequest request) => new()
    {
        Id = request.Id,
        RoleId = request.RoleId,
        ClaimType = request.Type,
        ClaimValue = request.Value,
        Description = request.Description,
        Group = request.Group
    };

    public static BlazorHeroRoleClaim ToBlazorHeroRoleClaim(this RoleClaimResponse response) => new()
    {
        Id = response.Id,
        RoleId = response.RoleId,
        ClaimType = response.Type,
        ClaimValue = response.Value,
        Description = response.Description,
        Group = response.Group
    };
}

internal static class RoleMappingExtensions
{
    public static RoleResponse ToRoleResponse(this BlazorHeroRole role) => new()
    {
        Id = role.Id,
        Name = role.Name,
        Description = role.Description
    };

    public static List<RoleResponse> ToRoleResponseList(this IEnumerable<BlazorHeroRole> roles)
        => roles.Select(r => r.ToRoleResponse()).ToList();
}

internal static class ChatHistoryMappingExtensions
{
    public static ChatHistory<BlazorHeroUser> ToBlazorHeroUserChatHistory(
        this ChatHistory<Application.Interfaces.Chat.IChatUser> source) => new()
        {
            Id = source.Id,
            FromUserId = source.FromUserId,
            ToUserId = source.ToUserId,
            Message = source.Message,
            CreatedDate = source.CreatedDate
        };
}

internal static class AuditMappingExtensions
{
    public static AuditResponse ToAuditResponse(this Audit audit) => new()
    {
        Id = audit.Id,
        UserId = audit.UserId,
        Type = audit.Type,
        TableName = audit.TableName,
        DateTime = audit.DateTime,
        OldValues = audit.OldValues,
        NewValues = audit.NewValues,
        AffectedColumns = audit.AffectedColumns,
        PrimaryKey = audit.PrimaryKey
    };

    public static List<AuditResponse> ToAuditResponseList(this IEnumerable<Audit> audits)
        => audits.Select(a => a.ToAuditResponse()).ToList();
}
