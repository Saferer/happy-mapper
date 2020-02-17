using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Services))]
public class ServicesInspector : Editor {
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(10);
        DrawServicesButton();
        GUILayout.Space(10);
        DrawDownloadGoogleButton();
    }

    void DrawServicesButton()
    {
        string buttonLabel = Services.ServicesAreEnable ? "DISABLE SERVICES" : "ENABLE SERVICES";
        if (GUILayout.Button(buttonLabel, GUILayout.Height(20)))
        {
            if (!Services.ServicesAreEnable)
            {
                DefineSymbols.AddDefineSymbol(DefineSymbols.services, true);
            }
            else
                DefineSymbols.AddDefineSymbol(DefineSymbols.services, false);
            Services.ServicesAreEnable = !Services.ServicesAreEnable;
        }
    }

    void DrawDownloadGoogleButton()
    {
        if (GUILayout.Button("DOWNLOAD GOOGLE SDK", GUILayout.Height(20)))
        {
            Application.OpenURL("https://github.com/playgameservices/play-games-plugin-for-unity");
        }
    }
}
