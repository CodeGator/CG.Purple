﻿namespace CG.Purple.SqlServer.Entities;

/// <summary>
/// This class represents a notification text entity.
/// </summary>
internal class TextMessage : Message
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the to address for the message.
    /// </summary>
    public string To { get; set; } = null!;

    /// <summary>
    /// This property contains the body for the message.
    /// </summary>
    public string Body { get; set; } = null!;

    #endregion
}
