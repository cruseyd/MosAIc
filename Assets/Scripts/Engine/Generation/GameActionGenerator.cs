using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionGenerator", menuName = "Scriptable Objects/ActionGenerator")]
public class GameActionGenerator : DynamicEnum
{
    public override void UpdateValues()
    {
        this.generateClassMaps = true;
        this.classMapFileName = "VALUEAction";
        base.UpdateValues();
    }
    protected override string GenerateClassCode(string classname)
    {
        return ScriptGenerator.GenerateGameActionScript(classname);
    }
}
