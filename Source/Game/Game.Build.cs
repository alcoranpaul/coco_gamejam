using Flax.Build;
using Flax.Build.NativeCpp;

public class Game : GameModule
{
    /// <inheritdoc />
    public override void Init()
    {
        base.Init();

        // C#-only scripting
        BuildNativeCode = false;
    }

    /// <inheritdoc />
    public override void Setup(BuildOptions options)
    {
        base.Setup(options);

        options.ScriptingAPI.IgnoreMissingDocumentationWarnings = true;
        options.PublicDependencies.Add("Util");

        // Here you can modify the build options for your game module
        // To add C++ define use: options.PublicDefinitions.Add("COMPILE_WITH_FLAX");
        // To learn more see scripting documentation.
    }
}
