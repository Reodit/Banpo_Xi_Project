using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;

    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandDamageCollider;

    private void Awake()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots) 
        {
            if(weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if(weaponSlot.isRightHandSlot) 
            {
                rightHandSlot = weaponSlot;    
            
            }
        }
    }
    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if(isLeft)
        {
            leftHandSlot.LoadWeaponModel(weaponItem);

        }
        else
        {
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
        }
    }
     #region Handle Weapon Damage Collider
    private void LoadLeftWeaponDamageCollider()
    {

    }
    private void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }
    public void OpenRightHandDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }
    public void CloseRightHandDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }
    #endregion
}
