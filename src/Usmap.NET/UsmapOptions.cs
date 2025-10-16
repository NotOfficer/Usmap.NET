using OodleDotNet;

namespace UsmapDotNet;

/// <summary>
/// Options/Configuration for parsing usmaps
/// </summary>
public class UsmapOptions
{
    /// <summary>
    /// Required for:<br/>
    /// - parsing oodle compressed usmaps<br/>
    /// </summary>
    public Oodle? Oodle { get; set; }

    /// <summary>
    /// Whether or not to save names of the usmap<br/>
    /// Defaults to <see langword="true"/>.
    /// </summary>
    public bool SaveNames { get; set; } = true;
}
