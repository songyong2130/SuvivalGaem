using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillManager : MonoBehaviour
{
    [System.Serializable]
    public class SkillSlot
    {
        public PlayerSkill skillData;
        [HideInInspector] public bool isCoolingDown;
    }
    // 제네릭 사용으로 SkillSlot의 형식대로 skillSlots 리스트에 저장됨
    [SerializeField] private List<SkillSlot> skillSlots = new List<SkillSlot>();
    [SerializeField] private Transform firePoint;
    private PlayerStamina stamina;
    private SkillCaster caster;
    
    private void Awake()
    {
        stamina = GetComponent<PlayerStamina>();
        caster = GetComponent<SkillCaster>();
    }
    public void OnSkillE(InputAction.CallbackContext ctx) {if (ctx.started) TryUseSkill(0);}
    public void OnSkillR(InputAction.CallbackContext ctx) {if (ctx.started) TryUseSkill(1);}
    public void OnSkillT(InputAction.CallbackContext ctx) {if (ctx.started) TryUseSkill(2);} 

    /// <summary>
    /// 스킬 사용시 나옴
    /// </summary>
    private void TryUseSkill(int slotIndex)
    {
        if (slotIndex < 0 || skillSlots.Count <= slotIndex) return; // 슬롯 범위 조건 (3칸이니 0~2)
        SkillSlot slot = skillSlots[slotIndex]; // skillSlots에 슬롯 넣음
        if (slot.skillData == null || slot.isCoolingDown) return; // skillData
        if (!stamina.HasEnoughStamina(slot.skillData.staminaCost)) return;

        stamina.UseStamina(slot.skillData.staminaCost);
        StartCoroutine(SkillCooltimeRoutine(slot));
    }
    private IEnumerator SkillCooltimeRoutine(SkillSlot slot)
    {
        slot.isCoolingDown = true;

        caster.CastSkill(slot.skillData, firePoint); // 스킬 사용

        yield return new WaitForSeconds(slot.skillData.cooldown);

        slot.isCoolingDown = false;
    }
}
