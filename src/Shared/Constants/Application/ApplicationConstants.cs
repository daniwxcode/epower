using System.Data;

namespace BlazorHero.CleanArchitecture.Shared.Constants.Application
{
    public static class ApplicationConstants
    {
        #region Building
        public static class BuildingsCache
        {

            public const string AllShops = "AllShops";
            public static string[] MetersCacheKeys(int id)
            {
                return new string[]
                 {
                    AllMeterCacheKey,
                    BuildindMetersCacheKey(id)
                 };
            }
            public const string AllBuildingCacheKey = "all-buildings";
            public static string BuildindMetersCacheKey(int id) => $"{id}all-meters";
            public const string AllMeterCacheKey = "all-meters";
        }


        #endregion
        public static class SignalR
        {
            public const string HubUrl = "/signalRHub";
            public const string SendUpdateDashboard = "UpdateDashboardAsync";
            public const string ReceiveUpdateDashboard = "UpdateDashboard";
            public const string SendRegenerateTokens = "RegenerateTokensAsync";
            public const string ReceiveRegenerateTokens = "RegenerateTokens";
            public const string ReceiveChatNotification = "ReceiveChatNotification";
            public const string SendChatNotification = "ChatNotificationAsync";
            public const string ReceiveMessage = "ReceiveMessage";
            public const string SendMessage = "SendMessageAsync";

            public const string OnConnect = "OnConnectAsync";
            public const string ConnectUser = "ConnectUser";
            public const string OnDisconnect = "OnDisconnectAsync";
            public const string DisconnectUser = "DisconnectUser";
            public const string OnChangeRolePermissions = "OnChangeRolePermissions";
            public const string LogoutUsersByRole = "LogoutUsersByRole";

            public const string PingRequest = "PingRequestAsync";
            public const string PingResponse = "PingResponseAsync";

        }
        public static class Cache
        {
           

            public const string GetAllBrandsCacheKey = "all-brands";
            public const string GetAllDocumentTypesCacheKey = "all-document-types";
            public static string GetAllEntityExtendedAttributesCacheKey(string entityFullName)
            {
                return $"all-{entityFullName}-extended-attributes";
            }

            public static string GetAllEntityExtendedAttributesByEntityIdCacheKey<TEntityId>(string entityFullName, TEntityId entityId)
            {
                return $"all-{entityFullName}-extended-attributes-{entityId}";
            }
        }
     

        public static class MimeTypes
        {
            public const string OpenXml = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }
    }
}