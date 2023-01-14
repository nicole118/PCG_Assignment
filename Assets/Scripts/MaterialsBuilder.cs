using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialsBuilder
{
    private List<Material> materialsList = new List<Material>();

    public MaterialsBuilder(){

        Material redMaterial = new Material(Shader.Find("Specular"));
        redMaterial.color = Color.red;

        Material blueMaterial = new Material(Shader.Find("Specular"));
        blueMaterial.color = Color.blue;

        Material greenMaterial = new Material(Shader.Find("Specular"));
        greenMaterial.color = Color.green;

        Material yellowMaterial = new Material(Shader.Find("Specular"));
        yellowMaterial.color = Color.yellow;

        Material magentaMaterial = new Material(Shader.Find("Specular"));
        magentaMaterial.color = Color.magenta;

        Material whiteMaterial = new Material(Shader.Find("Specular"));
        whiteMaterial.color = Color.white;

        materialsList.Add(redMaterial);
        materialsList.Add(blueMaterial);
        materialsList.Add(greenMaterial);
        materialsList.Add(yellowMaterial);
        materialsList.Add(magentaMaterial);
        materialsList.Add(whiteMaterial);
    }

    public List<Material> MaterialsList(){
        return materialsList;
    }

}
