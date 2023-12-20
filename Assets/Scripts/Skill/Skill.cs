using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class SkillInfo
{
    [SerializeField]
    public List<string> precedeSkillName;
    public string skillName;
    public Skill skill;

    public Sprite skillIcon;

    public float atk;
    public float mana;

    public int maxPoint;
    public int curSkillPoint;

    public float cooldown;

    public float curCooldown;
}
public class Skill : MonoBehaviour
{
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    Collider _skillCollision;
    [SerializeField]
    float _delay;
    [SerializeField]
    ParticleSystem _ps;
    private SkillInfo _skillInfo;
    private void Awake()
    {
        _skillCollision = GetComponent<Collider>();
        _ps = GetComponentInChildren<ParticleSystem>();
    }
    public void CastSkill(SkillInfo skillInfo)
    {
        transform.localPosition = offset;
        _skillInfo = skillInfo;
        _ps.Play();
        StartCoroutine(Coroutine_SkillRoutine());

    }
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 타격하는 대상의 태그, 컴포넌트, 함수는 바뀔 수 있다
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage((int)_skillInfo.atk);
        }
    }
    private IEnumerator Coroutine_SkillRoutine()
    {
        yield return new WaitForSeconds(_delay);
        _skillCollision.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _skillCollision.enabled = false;
        yield return new WaitForSeconds(_ps.main.duration);
        Destroy(gameObject);
        
    }
}