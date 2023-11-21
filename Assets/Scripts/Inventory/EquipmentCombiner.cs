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

    // rootBoneDictionary�� bornName�� ���ؼ� ��Ų��Ž��� ���� ����
    public Transform AddLimb(GameObject boneGO, List<string> boneNames)
    {
        Transform limb = ProcessBoneObject(boneGO.GetComponentInChildren<SkinnedMeshRenderer>(), boneNames);
        limb.SetParent(_transform);

        return limb;
    }

    // ��Ų��Ž��� ������ �����ۿ��� ����ϴ� bone���� ��Ī�ؼ�
    // ���Ӱ� ��Ų��Ž��������� �����Ͽ� �θ𿡰� �߰�
    Transform ProcessBoneObject(SkinnedMeshRenderer renderer, List<string> boneNames)
    {
        Transform itemTransform = new GameObject().transform;

        SkinnedMeshRenderer meshRenderer = itemTransform.gameObject.AddComponent<SkinnedMeshRenderer>();

        Transform[] boneTransforms = new Transform[boneNames.Count];
        for (int i = 0; i < boneNames.Count; i++)
        {
            boneTransforms[i] = _rootBonesDictionary[boneNames[i].GetHashCode()];
        }

        // ���ο� ������Ʈ�� �����ؼ� ���� ��Ų��Ž��� ī���� ����
        // ������Ʈ�� �߰��ϰ� ĳ������ �ڽ����� �߰����ִ� ���
        meshRenderer.bones = boneTransforms;
        meshRenderer.sharedMesh = renderer.sharedMesh;
        meshRenderer.materials = renderer.sharedMaterials;

        return itemTransform;
    }

    // ����ƽ �Ž� ó�� �Լ�
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

    // ���� ���뿡 ���� ��ȯ���� ������ ������ ������ �Լ�
    void TraverseHierarchy(Transform root)
    {
        foreach (Transform child in root)
        {
            _rootBonesDictionary.Add(child.name.GetHashCode(), child);

            TraverseHierarchy(child);
        }
    }
}
