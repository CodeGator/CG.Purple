﻿
namespace CG.Purple.Models;

/// <summary>
/// This class represents a message property model.
/// </summary>
public class MessageProperty : ModelBase
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the associate message.
    /// </summary>
    public virtual Message Message { get; set; } = null!;

    /// <summary>
    /// This property contains the associate property type.
    /// </summary>
    public virtual PropertyType PropertyType { get; set; } = null!;

    /// <summary>
    /// This property contains the value for the message property.
    /// </summary>
    public string Value { get; set; } = null!;

    #endregion
}
