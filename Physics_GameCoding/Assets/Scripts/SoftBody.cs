using System;
using UnityEngine;


//forve tje GO to always have a component
// if it doesnt, unity attaches one
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class SoftBody : MonoBehaviour
{
    [Range(0, 2f)]
    //how Far vertices can move, higher=more floppy
    public float softness = 1;

    //how much motion slows down the friction
    [Range(0.01f, 1f)]
    public float damping = 0.1f;

    //how resistanr to banding
    public float stiffness = 1f;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateSoftBodyPhysics();
    }
    void CreateSoftBodyPhysics()
    {
        SkinnedMeshRenderer smr = GetComponent<SkinnedMeshRenderer>();
        if (smr == null) return;

        //add unity cloth physics component to object at runtime
        Cloth cloth = gameObject.AddComponent<Cloth>();
        cloth.damping = damping;
        cloth.bendingStiffness = stiffness;

        //evere vertex in the mesh gets a physics rule
        //generate the rules w function
        cloth.coefficients = GenerateClothCoefficients(smr.sharedMesh.vertices.Length);
    }

    //making an array so we have multiple coefficients for all verticies
    //ex: mesh has 500 verticies, so cloth needs 500 coeffitients (one per vert)
    //thats why we are returning an array
    private ClothSkinningCoefficient[] GenerateClothCoefficients(int vertexCount)
    {
        //[] cretaes an array one entry per vertex
        //make a list with vertexcount per slot
        ClothSkinningCoefficient[] coefficients = new ClothSkinningCoefficient[vertexCount];
        //loop theorugh ever vertex
        //set rules for each vert 1 by 1
        for(int i = 0; i< vertexCount; i++)
        {
            //how far vertex can move
            coefficients[i].maxDistance = softness;
            //collision buffe 0 = tight
            coefficients[i].collisionSphereDistance = 0f;
        }
        //send back to cloth component
        return coefficients;
    }
}
