using System.ComponentModel.DataAnnotations;

namespace FaraOne.Domain;

public class Message
{
    public int Id { get; set; }
    [MaxLength(10)]
    public string RoomId { get; set; } = string.Empty;
    public int ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; }

    public int SenderId { get; set; }

    [MaxLength(150)]
    public string SenderName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    [MaxLength(100)]
    public string MessageType { get; set; } = "text"; // text, image, file, system
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
}