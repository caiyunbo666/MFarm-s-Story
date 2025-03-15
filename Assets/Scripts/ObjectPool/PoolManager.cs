using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public List<GameObject> poolPrefabs;

    private List<ObjectPool<GameObject>> poolEffectList = new List<ObjectPool<GameObject>>();

    private void OnEnable()
    {
        EventHandler.ParticleEffectEvent += OnParticleEffectEvent;
    }

    private void OnDisable()
    {
        EventHandler.ParticleEffectEvent -= OnParticleEffectEvent;
    }

    private void Start()
    {
        CreatePool();
    }

    /// <summary>
    /// ���ɶ����
    /// </summary>
    private void CreatePool()
    {
        foreach(GameObject item in poolPrefabs)
        {
            Transform parent = new GameObject(item.name).transform;
            parent.SetParent(transform);

            var newPool = new ObjectPool<GameObject>(
                ()=> Instantiate(item, parent),
                e => { e.SetActive(true); },
                e => { e.SetActive(false); },
                e => { Destroy(e); }
            );

            poolEffectList.Add(newPool);
        }
    }

    private void OnParticleEffectEvent(ParticalEffectType effectType, Vector3 pos)
    {
        //������Ч��ȫ
        ObjectPool<GameObject> objPool = effectType switch
        {
            ParticalEffectType.LeavesFalling01 => poolEffectList[0],
            ParticalEffectType.LeavesFalling02 => poolEffectList[1],
            ParticalEffectType.Rock => poolEffectList[2],
            ParticalEffectType.ReapableScenery => poolEffectList[3],
            _ => null,
        };

        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        StartCoroutine(ReleaseRoutine(objPool, obj));
    }

    private IEnumerator ReleaseRoutine(ObjectPool<GameObject> pool, GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        pool.Release(obj);
    }
}
