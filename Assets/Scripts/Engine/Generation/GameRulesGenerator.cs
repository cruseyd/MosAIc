using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GameRulesGenerator", menuName = "Scriptable Objects/GameRulesGenerator")]
public class GameRulesGenerator : DynamicEnum
{
    public override void UpdateValues()
    {
        this.generateClassMaps = true;
        this.classMapFileName = "VALUERules";
        base.UpdateValues();
    }
    protected override string GenerateClassCode(string classname)
    {
        return ScriptGenerator.GenerateGameRulesScript(classname);
    }
}
