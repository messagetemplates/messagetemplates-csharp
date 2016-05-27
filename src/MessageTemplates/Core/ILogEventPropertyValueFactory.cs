using MessageTemplates.Structure;

namespace MessageTemplates.Core
{
  /// <summary>
  /// Supports the policy-driven construction of <see cref="TemplatePropertyValue"/>s given
  /// regular .NET objects.
  /// </summary>
  public interface IMessageTemplatePropertyValueFactory
    {
    /// <summary>
    /// Create a <see cref="TemplatePropertyValue"/> given a .NET object and destructuring
    /// strategy.
    /// </summary>
    /// <param name="value">The value of the property.</param>
    /// <param name="destructureObjects">If true, and the value is a non-primitive, non-array type,
    /// then the value will be converted to a structure; otherwise, unknown types will
    /// be converted to scalars, which are generally stored as strings.</param>
    /// <returns>The value.</returns>
    TemplatePropertyValue CreatePropertyValue(object value, bool destructureObjects = false);
    }
}
