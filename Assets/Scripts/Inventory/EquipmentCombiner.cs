using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentCombiner
{
    readonly Dictionary<int, Transform> _rootBonesDictionary = new Dictionary<int, Transform>();
    readonly Transform _transform;

    const string BONE_TAG = "bone";

    public EquipmentCombiner(GameObject rootGO)
    {
        _transform = rootGO.transform;
        TraverseHierarchy(_transform);
    }

    // rootBoneDictionary와 bornName을 비교해서 스킨드매쉬를 새로 설정
    public Transform AddLimb(GameObject boneGO, List<string> boneNames)
    {
        Transform limb = ProcessBoneObject(boneGO.GetComponentInChildren<SkinnedMeshRenderer>(), boneNames);
        limb.SetParent(_transform);

        return limb;
    }

    // 스킨드매쉬로 구성된 아이템에서 사용하는 bone들을 매칭해서
    // 새롭게 스킨드매쉬랜더러를 생성하여 부모에게 추가
    Transform ProcessBoneObject(SkinnedMeshRenderer renderer, List<string> boneNames)
    {
        Transform itemTransform = new GameObject().transform;

        SkinnedMeshRenderer meshRenderer = itemTransform.gameObject.AddComponent<SkinnedMeshRenderer>();

        Transform[] boneTransforms = new Transform[boneNames.Count];
        for (int i = 0; i < boneNames.Count; i++)
        {
            boneTransforms[i] = _rootBonesDictionary[boneNames[i].GetHashCode()];
        }

        // 새로운 오브젝트를 생성해서 기존 스킨드매쉬를 카피한 다음
        // 컴포넌트로 추가하고 캐릭터의 자식으로 추가해주는 방식
        meshRenderer.bones = boneTransforms;
        meshRenderer.sharedMesh = renderer.sharedMesh;
        meshRenderer.materials = renderer.sharedMaterials;

        return itemTransform;
    }

    // 스태틱 매쉬 처리 함수
    public Transform[] AddMesh(GameObject itemGO)
    {
        Transform[] itemTransform = ProcessMeshObject(itemGO.GetComponentsInChildren<MeshRenderer>());

        return itemTransform;
    }

    Transform[] ProcessMeshObject(MeshRenderer[] meshRenderers)
    {
        List<Transform> itemTransforms = new List<Transform>();

        foreach (MeshRenderer renderer in meshRenderers)
        {
            if (renderer.transform.parent != null)
            {
                Transform parent = _rootBonesDictionary[renderer.transform.parent.name.GetHashCode()];

                GameObject itemGO = GameObject.Instantiate(renderer.gameObject, parent);

                itemTransforms.Add(itemGO.transform);
            }
        }

        return itemTransforms.ToArray();
    }

    // 하위 뼈대에 대한 순환으로 뼈대의 정보를 얻어오는 함수
    void TraverseHierarchy(Transform root)
    {
        foreach (Transform child in root)
        {
            _rootBonesDictionary.Add(child.name.GetHashCode(), child);

            TraverseHierarchy(child);
        }
    }
}
