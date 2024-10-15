using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Brainflow;

public class CollaborationHub : Hub
{
    private static Dictionary<string, Room> rooms = new Dictionary<string, Room>();
    
    /// <summary>
    /// Creating a new room
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task CreateRoom(string roomId, string userName)
    {
        if (!rooms.ContainsKey(roomId))
        {
            var newRoom = new Room { id = roomId };
            rooms[roomId] = newRoom;

            // Add the user to the room
            await AddUserToRoom(roomId, userName);
        }
        else
        {
            await Clients.Caller.SendAsync("Error", "Room already exists.");
        }
    }

    /// <summary>
    /// Used to join to an existing room or create new if it doesn't exists
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task JoinRoom(string roomId, string userName)
    {
        if (rooms.ContainsKey(roomId))
        {
            await AddUserToRoom(roomId, userName);
        }
        else
        {
            await CreateRoom(roomId, userName);
            //await Clients.Caller.SendAsync("Error", "Room does not exist.");
        }
    }

    /// <summary>
    /// To leave the room
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task LeaveRoom(string roomId, string userName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

        if (rooms.ContainsKey(roomId))
        {
            var room = rooms[roomId];

            var userToRemove = room.Users.FirstOrDefault(u => u.Name == userName);
            if (userToRemove != null)
            {
                room.Users.Remove(userToRemove);
            }

            await Clients.Group(roomId).SendAsync("UserLeft", userName);
        }
    }

    /// <summary>
    /// Used to notify users about notes
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="note"></param>
    /// <returns></returns>
    public Task SendNoteUpdate(string roomId, string note)
    {
        if (!rooms.ContainsKey(roomId))
        {
            rooms[roomId] = new Room { id = roomId };
        }

        var room = rooms[roomId];
        room.Notes.Add(note);

        return Clients.Group(roomId).SendAsync("ReceiveNoteUpdate", room.Notes);
    }

    /// <summary>
    /// Node updates
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public Task SendNodeUpdate(string roomId, NodeData node)
    {
        if (!rooms.ContainsKey(roomId))
        {
            rooms[roomId] = new Room { id = roomId };
        }

        var room = rooms[roomId];
        var existingNode = room.Nodes.Find(n => n.id == node.id);
        if (existingNode != null)
        {
            room.Nodes.Remove(existingNode);
        }
        room.Nodes.Add(node);

        return Clients.Group(roomId).SendAsync("ReceiveNodeUpdate", node);
    }

    /// <summary>
    /// Edge Updates
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="edge"></param>
    /// <returns></returns>
    public Task SendEdgeUpdate(string roomId, EdgeData edge)
    {
        if (!rooms.ContainsKey(roomId))
        {
            rooms[roomId] = new Room { id = roomId };
        }

        var room = rooms[roomId];
        var existingEdge = room.Edges.Find(e => e.id == edge.id);
        if (existingEdge != null)
        {
            room.Edges.Remove(existingEdge);
        }
        room.Edges.Add(edge);

        return Clients.Group(roomId).SendAsync("ReceiveEdgeUpdate", edge);
    }

    /// <summary>
    /// Delete node updates
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="nodeId"></param>
    /// <returns></returns>
    public async Task DeleteNode(string roomId, string nodeId)
    {
        if (rooms.ContainsKey(roomId))
        {
            var room = rooms[roomId];
            var node = room.Nodes.FirstOrDefault(n => n.id == nodeId);
            if (node != null)
            {
                room.Nodes.Remove(node);
                await Clients.Group(roomId).SendAsync("NodeDeleted", nodeId);
            }
        }
    }

    /// <summary>
    /// Delete edge updates
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="edgeId"></param>
    /// <returns></returns>
    public async Task DeleteEdge(string roomId, string edgeId)
    {
        if (rooms.ContainsKey(roomId))
        {
            var room = rooms[roomId];
            var edge = room.Edges.FirstOrDefault(e => e.id == edgeId);
            if (edge != null)
            {
                room.Edges.Remove(edge);
                await Clients.Group(roomId).SendAsync("EdgeDeleted", edgeId);
            }
        }
    }


    #region Private Methods

    private string GetRandomColor()
    {
        Random random = new Random();
        return $"#{random.Next(0x1000000):X6}";
    }

        /// <summary>
    /// Internal function to add user to an existing room and send all updates
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    private async Task AddUserToRoom(string roomId, string userName)
    {
        var room = rooms[roomId];

        var user = new UserData
        {
            Name = userName,
            Color = GetRandomColor()
        };

        room.Users.Add(user);

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("UserJoined", userName, user.Color);

        await Clients.Caller.SendAsync("ReceiveAllNotes", room.Notes);
        await Clients.Caller.SendAsync("ReceiveAllUsers", room.Users);
        await Clients.Caller.SendAsync("ReceiveAllNodes", room.Nodes);
        await Clients.Caller.SendAsync("ReceiveAllEdges", room.Edges);
    }
    #endregion

}
