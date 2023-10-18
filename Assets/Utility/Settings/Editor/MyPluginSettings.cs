using Dhs5.Utility.Settings;
using Dhs5.Utility.Settings.Editor;
using UnityEditor;
using UnityEngine;

[Settings(SettingsUsage.RuntimeProject, "My Plug-in Settings")]
public sealed class MyPluginSettings : Settings<MyPluginSettings>
{
    public int integerValue => _integerValue;
    [SerializeField, Range(0, 10)] int _integerValue = 5;

    public float floatValue => _floatValue;
    [SerializeField, Range(0, 100)] float _floatValue = 25.0f;

    public string stringValue => _stringValue;
    [SerializeField, Tooltip("A string value.")] string _stringValue = "Hello";

    [SettingsProvider]
    static SettingsProvider GetSettingsProvider() => instance.GetSettingsProvider();
}