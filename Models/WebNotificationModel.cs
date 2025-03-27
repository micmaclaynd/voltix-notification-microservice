using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Voltix.Shared.Models;


namespace Voltix.NotificationMicroservice.Models;

[Table("web_notifications")]
public class WebNotificationModel : BaseModel {
    [Required]
    [StringLength(256)]
    [Column("message", TypeName = "varchar(256)")]
    public required string Message { get; set; }

    [Required]
    [Column("is_readed", TypeName = "bool")]
    public bool IsReaded { get; set; } = false;

    [Required]
    [Column("user_id", TypeName = "int")]
    public required int UserId { get; set; }

    [Required]
    [Column("added_datetime", TypeName = "datetime")]
    public required DateTime AddedDateTime { get; set; }
}
