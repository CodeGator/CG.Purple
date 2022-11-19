﻿
namespace CG.Purple.Options;

/// <summary>
/// This class contains configuration settings for the <see cref="IMailMessageManager"/> 
/// type.
/// </summary>
internal class MailMessageOptions
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties
       
    /// <summary>
    /// This property contains the default cache duration for <see cref="MailMessage"/>
    /// objects.
    /// </summary>
    public TimeSpan? DefaultCacheDuration { get; set; }

    #endregion
}