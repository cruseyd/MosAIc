using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PhaseGenerator", menuName = "Scriptable Objects/PhaseGenerator")]
public class PhaseGenerator : DynamicEnum
{
    public override void UpdateValues()
    {
        this.generateClassMaps = true;
        this.classMapFileName = "VALUEPhase";
        base.UpdateValues();
    }
    protected override string GenerateClassCode(string classname)
    {
        return ScriptGenerator.GeneratePhaseScript(classname);
    }
}
