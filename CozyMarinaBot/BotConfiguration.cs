

#pragma warning disable CA1050 // Declare types in namespaces
public class BotConfiguration
#pragma warning restore CA1050 // Declare types in namespaces
{
    public static readonly string Configuration = "BotConfiguration";

    public string TokenName { get; set; } = "";
}