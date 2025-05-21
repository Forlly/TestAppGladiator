using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class Build
{
    private const BuildOptions ANDROID_BUILD_OPTIONS = BuildOptions.CompressWithLz4HC;
    private const BuildOptions IOS_BUILD_OPTIONS = BuildOptions.CompressWithLz4HC;
    [MenuItem("Build/Build Android AAB")]
    public static void AndroidAAB()
    {
        EditorUserBuildSettings.overrideMaxTextureSize = 1024;
        AssetDatabase.Refresh();
        PlayerSettings.Android.useCustomKeystore = true;
        EditorUserBuildSettings.buildAppBundle = true;
        PlayerSettings.Android.minifyRelease = true;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;

        // Set bundle version. NEW_BUILD_NUMBER environment variable is set in the codemagic.yaml 
        var versionIsSet = int.TryParse(Environment.GetEnvironmentVariable("NEW_BUILD_NUMBER"), out int version);
        if (versionIsSet)
        {
            Debug.Log($"Bundle version code set to {version}");
            PlayerSettings.Android.bundleVersionCode = version;
        }
        else
        {
            Debug.Log("Bundle version not provided");
        }

        // Set keystore name
        string keystoreName = Environment.GetEnvironmentVariable("CM_KEYSTORE_PATH");
        if (!String.IsNullOrEmpty(keystoreName))
        {
            Debug.Log($"Setting path to keystore: {keystoreName}");
            PlayerSettings.Android.keystoreName = keystoreName;
        }
        else
        {
            Debug.Log("Keystore name not provided");
        }

        // Set keystore password
        string keystorePass = Environment.GetEnvironmentVariable("CM_KEYSTORE_PASSWORD");
        if (!String.IsNullOrEmpty(keystorePass))
        {
            Debug.Log("Setting keystore password");
            PlayerSettings.Android.keystorePass = keystorePass;
        }
        else
        {
            Debug.Log("Keystore password not provided");
        }

        // Set keystore alias name
        string keyaliasName = Environment.GetEnvironmentVariable("CM_KEY_ALIAS");
        if (!String.IsNullOrEmpty(keyaliasName))
        {
            Debug.Log("Setting keystore alias");
            PlayerSettings.Android.keyaliasName = keyaliasName;
        }
        else
        {
            Debug.Log("Keystore alias not provided");
        }

        // Set keystore password
        string keyaliasPass = Environment.GetEnvironmentVariable("CM_KEY_PASSWORD");
        if (!String.IsNullOrEmpty(keyaliasPass))
        {
            Debug.Log("Setting keystore alias password");
            PlayerSettings.Android.keyaliasPass = keyaliasPass;
        }
        else
        {
            Debug.Log("Keystore alias password not provided");
        }
        PlayerSettings.SplashScreen.show = false;
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = $"android/{PlayerSettings.productName}v{PlayerSettings.bundleVersion}_{PlayerSettings.Android.bundleVersionCode}.aab";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.targetGroup = BuildTargetGroup.Android;
        buildPlayerOptions.options = ANDROID_BUILD_OPTIONS;
        buildPlayerOptions.scenes = GetScenes();

        Debug.Log("Building Android AAB");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log("Built Android AAB");
    }

    [MenuItem("Build/Build iOS")]
    public static void BuildIos()
    {
        EditorUserBuildSettings.overrideMaxTextureSize = 512;
        AssetDatabase.Refresh();
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "ios";
        buildPlayerOptions.target = BuildTarget.iOS;
        buildPlayerOptions.targetGroup = BuildTargetGroup.iOS;
        buildPlayerOptions.options = IOS_BUILD_OPTIONS;
        buildPlayerOptions.scenes = GetScenes();

        PlayerSettings.SplashScreen.show = false;
        PlayerSettings.iOS.buildNumber = $"{ int.Parse(PlayerSettings.iOS.buildNumber) + 1}";
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log("Built iOS");
    }

    private static string[] GetScenes()
    {
        return (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
    }
}
