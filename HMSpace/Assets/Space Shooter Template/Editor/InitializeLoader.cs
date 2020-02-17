using UnityEditor;

[InitializeOnLoad]
public class InitializeLoader : Editor {

    static InitializeLoader()
    {
        CheckDefineSymbols();
    }

    static void CheckDefineSymbols()
    {
        if (Services.ServicesAreEnable)
        {
           DefineSymbols.AddDefineSymbol(DefineSymbols.services, true);
        }
    }
}
