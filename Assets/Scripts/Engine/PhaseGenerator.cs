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
        string code = ($"public class {classname} : Phase" + "{\n");
        code += "    public override void Enter(Phase prevPhase){}\n";
        code += "    public override void Exit(Phase nextPhase){}\n";
        code += "};";
        return code;
    }
}
