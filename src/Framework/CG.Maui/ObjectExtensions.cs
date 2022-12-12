
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System;

/// <summary>
/// This class contains extension methods related to the <see cref="Object"/>
/// type.
/// </summary>
public static partial class ObjectExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method performs a quick clone of the specified object.
    /// </summary>
    /// <param name="source">The object to be cloned.</param>
    /// <param name="sourceType">The type of the object to be cloned.</param>
    /// <returns>The cloned object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are invalid, or missing.</exception>
    public static object QuickClone(
        this object source,
        Type sourceType
        )
    {
        // Serialize the object to JSON.
        var json = JsonSerializer.Serialize(
            source, 
            sourceType,
            new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });

        // Deserialize the JSON to an object.
        var obj = JsonSerializer.Deserialize(
            json, 
            sourceType,
            new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });

        // Return the results.
        return obj;
    }

    // *******************************************************************

    /// <summary>
    /// This method performs a quick clone of the specified object.
    /// </summary>
    /// <typeparam name="T">The type of object to be cloned.</typeparam>
    /// <param name="source">The object to be cloned.</param>
    /// <returns>The cloned object.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are invalid, or missing.</exception>
    public static T QuickClone<T>(
        this T source
        ) where T : class
    {
        // Is source a non object type passed to us as an object reference?
        if (typeof(T) == typeof(object) && source.GetType() != typeof(object))
        {
            // The problem here is, 'source' is a non object type but it's 
            //   been passed to us as an object reference. That makes the T
            //   type parameter = object. That's not right, so let's call
            //   GetType ourselves and pass the correct type into the
            //   overload. Then we can cast the results manually.

            // Call the overload. The cast to T should still work ...
            var obj = source.QuickClone(source.GetType()) as T;

            // Return the results.
            return obj;
        }
        else
        {
            // Serialize the object to JSON.
            var json = JsonSerializer.Serialize<T>(
                source,
                new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                });

            // Deserialize the JSON to an object.
            var obj = JsonSerializer.Deserialize<T>(
                json,
                new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                });

            // Return the results.
            return obj;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method performs a quick copy from the source object to the 
    /// destination object.
    /// </summary>
    /// <param name="source">The object to read from.</param>
    /// <param name="dest">The object to write to.</param>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are invalid, or missing.</exception>
    public static void QuickCopyTo(
        this object source, 
        object dest
        )
    {
        // Get the list of properties that can be copied.
        var sourceProps = source.GetType().GetProperties()
            .Where(x => x.CanWrite && x.CanRead);

        // Loop through the properties.
        foreach (var pi in sourceProps)
        {
            // Get the property value (if any).
            var sourcePropValue = pi.GetValue(source, null);
            if (null != sourcePropValue)
            {
                // Deal with value types and strings.
                if (pi.PropertyType == typeof(string) ||
                    pi.PropertyType == typeof(decimal) ||
                    pi.PropertyType == typeof(int) ||
                    pi.PropertyType == typeof(double) ||
                    pi.PropertyType == typeof(float) ||
                    pi.PropertyType == typeof(DateTime) ||
                    pi.PropertyType == typeof(TimeSpan) ||
                    pi.PropertyType.IsEnum
                    )
                {
                    // Set the destination value.
                    pi.SetValue(dest, sourcePropValue, null);
                }

                // Deal with object types.
                else
                {
                    // Get the property value (if any).
                    var destPropValue = pi.GetValue(dest, null);
                    if (null != destPropValue)
                    {
                        // Deep copy the object.
                        sourcePropValue.QuickCopyTo(
                            destPropValue
                            );
                    }
                    else
                    {
                        // Set the destination to null.
                        pi.SetValue(dest, null, null);
                    }
                }
            }
            else
            {
                // Set the destination to null.
                pi.SetValue(dest, null, null);
            }
        }
    }

    #endregion
}