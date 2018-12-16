using System;
using System.Collections.Generic;
using Medusa.Physics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Spine;

namespace Medusa
{
    public class InventoryItem
    {
        public GameObjectClass GameObjectClass;
        public Inventory Inventory;
        public InventorySlot Slot;

        public Cue FiringLoop;
        public Cue StartFiring;

        // Weapons
        public string AmmoLabel = "";
        public bool AmmoLabelLowAmmo;

        public bool NoAmmoVoiceDone;

        public void UpdateAmmoLabel()
        {
            if (GameObjectClass.Type == GameObjectType.ITEM)
            {
                if (GameObjectClass.PassiveItem)
                {
                    AmmoLabel = "";
                }
                else if (ItemQuantity == 0)
                {
                    AmmoLabel = "---";
                }
                else if (GameObjectClass.ItemRecharges)
                {
                    AmmoLabel = ((int)((CurrentItemCharge / GameObjectClass.MaxCharge) * 100f)) + "%";
                }
                else if (ItemUsed)
                {
                    AmmoLabel = CodeComplete ? "ARMED" : "SET";
                }
                else
                {
                    AmmoLabel = "x" + ItemQuantity;
                }
            }
            else
            {
                int ammo = (int)Math.Ceiling(m_Ammo);
                if (GameObjectClass.AmmoAsPercent)
                {
                    int Percent = (int)((m_Ammo / GameObjectClass.AmmoBoxQuantity) * 100);
                    AmmoLabel = Percent.ToString() + "%";
                    AmmoLabelLowAmmo = Percent <= 25;
                }
                else if (ammo <= 0)
                    AmmoLabel = "---";
                else
                {
                    bool Left = (Slot == InventorySlot.WEAPON_3) || (Slot == InventorySlot.WEAPON_1);
                    AmmoLabel = Left ? "x" + ammo : ammo + "x";
                    AmmoLabelLowAmmo = (Ammo / FullAmmo) <= 0.25f;
                }
            }
        }

        private float m_Ammo;
        public float Ammo
        {
            get
            {
                return m_Ammo;
            }
            set
            {
                if (m_Ammo != value)
                {
                    if (NoAmmoVoiceDone && value > m_Ammo)
                        NoAmmoVoiceDone = false;

                    m_Ammo = value;
                    UpdateAmmoLabel();
                }
            }
        }
        private float m_CurrentWeaponCharge;
        public float CurrentWeaponCharge
        {
            get
            {
                return m_CurrentWeaponCharge;
            }
            set
            {
                if (m_CurrentWeaponCharge != value)
                {
                    m_CurrentWeaponCharge = value;
                    UpdateAmmoLabel();
                }
            }
        }

        private float m_CurrentItemCharge;
        public float CurrentItemCharge
        {
            get
            {
                return m_CurrentItemCharge;
            }
            set
            {
                if (m_CurrentItemCharge != value)
                {
                    m_CurrentItemCharge = value;
                    UpdateAmmoLabel();
                }
            }
        }

        private int m_ItemQuantity;
        public int ItemQuantity
        {
            get
            {
                return m_ItemQuantity;
            }
            set
            {
                if (m_ItemQuantity != value)
                {
                    m_ItemQuantity = value;
                    UpdateAmmoLabel();
                }
            }
        }

        public float FullAmmo;
        public float TimeOfNextShot;
        public double NextMeleeClear;
        public float AIFiringEndTime;
        public float AIFiringBreakEndTime;
        public float CurrentInaccuracyAddition;
        public float CurrentRateOfFireAddition;
        public float CurrentShakeCameraAddition;
        public int NumShotsThisReload;
        public float NextReloadTime;
        public bool IsWeaponCharging;
        public List<GameObject> Projectiles;
        public List<GameObject> ProjectilesToRemove;
        public GameObject WeaponChargeLightObj;
        public bool MeleeStrike;
        public double MeleeStrikeTime;
        public Body WeaponHitBody;
        public Vector2 WeaponHitBodyLastPosition;
        public float WeaponHitBodyCurrentScale;
        public List<Entity> MeleeHits;
        public bool HidingChargeLight;
        public int CurrentAutoFireCount;
        //public GameObject FireBeam;
        //public Entity FireBeamEntity;
        //public Bone FireBeamBone;
        //public float FireBeamAngleOffset;
        public GameObject ConeObject;
        public double NextRaygunImpactEffect;
        public double NextChargingSound;
        public List<MeshEffect> WeaponMeshEffects;
        public List<Entity> ZappedEntities;
        public float ClearZappedEntitiesAt;
        public bool ShownLowAmmoWarning;
        public bool ProjectileUseMuzzleAngle;
        public float RefractionMapScale = 1f;
        public float RefractionAlpha = 0f;
        public bool SetRefractionAlpha;

        // Shields
        public GameObject ShieldObject;
        public double NextRecharge;
        public double NextRefill;

        // Items
        public float TimeOfNextUse;
        public bool ItemUsed;
        public bool CodeComplete;
        public GameObject SpawnedObject;
        public float StoredHealth;
        public GameObject AimingBeam; // used by passive laser sight item
        public bool Blinking;
        public Vector2 BlinkPosition;
        public bool DepleteCharge;
        public float CurrentChargeBarAlpha;
        public float TargetChargeBarAlpha;

        public bool WeaponIsReleased;
        public bool WeaponIsFiring;
        public bool WeaponIsFiringNew;

        public InventoryItem(Inventory inventory, InventorySlot slot)
        {
            Inventory = inventory;
            Slot = slot;
            Projectiles = new List<GameObject>();
            ProjectilesToRemove = new List<GameObject>();
        }
    }
}
