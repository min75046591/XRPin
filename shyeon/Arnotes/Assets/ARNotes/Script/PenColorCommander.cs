using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenColorCommander : MenuCommander
{
    public StationeryController stationeryController;
    public List<Material> materials;
    public override void Command(string commandParam)
    {
        this.stationeryController.ConvertToDrawingMode();
        for(int i = 0; i < materials.Count; i++)
        {
            this.stationeryController.ChangeColor(FindMaterial(commandParam));
        }
    }

    private Material FindMaterial(string colorName)
    {
        for(int i = 0; i < materials.Count; i++)
        {
            if (materials[i].name == colorName) return materials[i];
        }
        return materials[0]; // default Color
    }

}
