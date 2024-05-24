namespace ApplicationCore.Helpers;

public static class EnumHelper
{
    public static T GetEnum<T>(string value) =>
        (T)Enum.Parse(typeof(T), value.Trim(), true);

#pragma warning disable CS8601 // Possible null reference assignment.
    public static bool TryGetEnum<T>(string value, out object enumValue) =>
        Enum.TryParse(typeof(T), value.Trim(), true, out enumValue);
#pragma warning restore CS8601 // Possible null reference assignment.

}
