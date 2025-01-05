using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "InitializerGenerator", menuName = "Scriptable Objects/InitializerGenerator")]
public class InitializerGenerator : DynamicEnum
{
    public override void UpdateValues()
    {
        this.generateClassMaps = true;
        this.classMapFileName = "VALUEInitializer";
        base.UpdateValues();
    }
    protected override string GenerateClassCode(string classname)
    {
        return ScriptGenerator.GenerateInitializerScript(classname);
    }
}
