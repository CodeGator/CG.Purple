﻿
namespace CG.Purple.Seeding.Options;

/// <summary>
/// This class contains configuration options related to text message seeding.
/// </summary>
public class TextMessageOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the 'To' address for the message.
    /// </summary>
    [Required]
    [MaxLength(1024)]
    public string To { get; set; } = null!;

    /// <summary>
    /// This property contains the subject for the message.
    /// </summary>
    [Required]
    public string Body { get; set; } = null!;

    /// <summary>
    /// This property indicates whether the message is disabled, or not.
    /// </summary>
    public bool IsDisabled { get; set; }

    #endregion
}
