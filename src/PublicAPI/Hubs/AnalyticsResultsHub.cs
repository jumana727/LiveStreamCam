using Microsoft.AspNetCore.SignalR;

namespace PublicAPI.Hubs;

public class AnalyticsResultsHub : Hub
{
    public class GroupMembershipRequest
    {
        public string? streamId;
        public string? analyticsSettingsId;
    }

    // public class GroupMembershipResponse
    // {

    // }

    public async Task JoinGroup(GroupMembershipRequest request)
    {
        if (request.streamId is null || request.analyticsSettingsId is null)
            throw new ArgumentException("Missing Id.", nameof(request));

        var groupId = (request.streamId.Trim() + request.analyticsSettingsId.Trim()).ToLower();
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId);

        await Clients.User(Context.ConnectionId).SendAsync("MembershipResponse", $"You have joined the group for {request.streamId}, {request.analyticsSettingsId}.");
    }

    public async Task LeaveGroup(GroupMembershipRequest request)
    {
        if (request.streamId is null || request.analyticsSettingsId is null)
            throw new ArgumentException("Missing Id.", nameof(request));

        var groupId = (request.streamId.Trim() + request.analyticsSettingsId.Trim()).ToLower();
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);

        await Clients.User(Context.ConnectionId).SendAsync("MembershipResponse", $"You have left the group {request.streamId}, {request.analyticsSettingsId}.");
    }

}
