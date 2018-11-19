using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using Medusa;
using Medusa.Physics.Common;
using Medusa.Physics.Collision.Shapes;
using Medusa.Physics.Dynamics;
using Medusa.Physics.Dynamics.Joints;
using Medusa.Physics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using ProjectMercury;
using TexturePackingRuntime;

namespace Medusa
{
    [Serializable]
    public class CharacterClass
    {
        // internal, cant be set in XML files
        internal string Classfile;
        internal bool BaseFileProcessed;
        internal bool ClassSetup;
        internal TextureBounds MinimapTexture;
        internal TextureBounds SidebarTexture;

        //**********************
        // MAIN
        public CharacterType Type;
        public string BaseFile;
        public string DisplayName;
        public bool NoOffscreen;
        public float OffscreenDistanceMultiply;

        public string CharacterAmbientSound;

        //**********************
        // DRAWING
        public float BodyRotateToFocusPercent = 0.5f;
        public float MinFireBoneRecoilDelay = 0.05f;
        public string MinimapTextureName;
        public string SidebarTextureName;
        public bool HideOnMinimap;
        public float MinimapScale = 1;
        public bool MinimapRotateIcon;
        public string OnFireBeamClassfile;
        public string Description;
        public float HealthBarScale;
        public List<TailEffect> TailEffects;

        //**********************
        // ANIMATION
        public string SpineAnimation;
        public float SpineAnimationScale = 1;
        public string SpineAnimationSkin;
        public string SpineAtlas;
        public Vector2 AnimationOffset; // offset their animation from their physics position
        public float ArmMovementScaleX = 1f;
        public float ArmMovementScaleY = 1f;
        public List<SkinAtlas> SkinAtlases;
        public string DeathAnimation;

        //**********************
        // PHYSICS / COLLISION
        public bool RectangleBody;
        public float Width = 2;
        public float Height = 2;
        public bool IgnoreBrushCollision;
        public bool IgnoreAllCollision;
        public bool IgnoreExplosionPush;
        public bool IgnoreFluid;
        public bool IgnoreSystemCollision;
        public bool IgnoreSameTeamCollision;
        public bool CharacterCollide;
        public float Density = 0.25f;
        public float Inertia; // if not set the inertia will be auto set by the density/size
        public float InertiaNoAirControlMultiply = 1; // what to multiple the inertia by, if they are in 'no air control' mode (got hit with a explosion)
        public float WallFixtureScale; // if they should have a part of their physics body just for colliding with walls, size percent of their main body
        public float LinearDamping = 4;
        public float LinearDampingNoAirControlMultiply = 0.5f;
        public float AngularDamping = 10;
        public float AngularDampingNoAirControlMultiply = 0.4f;
        public bool NonBoundingBoxCollisionsEnabled;
        public bool Static;

        //**********************
        // MOVEMENT
        public float BaseMovementForce = 700;
        public bool AIJukeCombatMovement;
        public float ForceMultiply_Fly = 1;
        public float ForceMultiply_Fly_Offscreen = 1;
        public float Maxspeed_Fly = 25;
        public float AimSpeedRestore = 1f;
        public float Maxspeed_Fly_Offscreen;
        public float WanderSpeedMod = 0.5f;
        public float LungeRange = 30f;

        //**********************
        // DAMAGE / HEALTH
        public float Health;
        public float DamageOnContact; // amount of damage a character takes when touching this
        public float DamageOnContactForce;
        public float DamageOnContactNoAirControlTime;
        public string HitBox;
        public float HealAmount;
        public float HealDelay;
        public float HealRange;
        public bool AllowGrapple;
        public float DieAfter; // gives characters a life timespan
        public float RandDieAfter; // add a random value when setting DieAt
        public string SpawnObjectOnRemove;

        //**********************
        // PLAYER CHARACTERS
        public List<string> StartLoadOut;
        public List<int> StartLoadOutAmounts;
        public List<string> StartItemLoadOut;
        public List<string> StartLoadOutBase;
        public List<string> PassiveItems;
        public PlayerType PlayerType;
        public int ArtifactCost;
        public bool DevOnly;
        public int MaxHull = 15;
        public int StartHull = 9;

        //**********************
        // ITEMS
        public bool RecoverItemOnInteract;
        public float InteractionDistance = 25f;
        public bool StoreHealthOnRecover;

        //**********************
        // AI
        public List<string> DefaultItems;
        public string Weapon1;
        public string Weapon2;
        public string Weapon3;
        public string Weapon4;
        public string NoLiquidWeapon;
        public float MinCombatDistance = 60;    // at what distance to start combat
        public float AI_SearchTime = 15;    // how long to search for enemy
        public float AI_SearchTimeAlarmMultiply = 2;
        public float AI_AimSpeed = 0.1f;    // speed of their focus on enemy
        public float AI_DeactivateDelay;
        public bool AI_DisableReturn;
        public bool AI_DisableFindEnemy;
        public float FieldOfVision = 180f;     // Used for finding characters in AI
        public float FieldOfVisionClose = 360f;     // Used for finding characters in AI when closer than 10 units
        public string AimingBeamClassfile;
        public bool Swarmer;
        public bool SwarmDisableSetGoal;
        public float SwarmFireRate; // how long to wait before letting a swarm member fire
        public bool Weapon1FireDisabled;
        public bool DroneFactory;
        public bool SentryFactory;
        public int FactoryAmount = 1;
        public HazardType HazardType;
        public float SawbladeSpinAmount; // how much the sawblade's angular velocity is modified each frame
        public float SawbladeMaxSpin;
        public float SawbladeLaunchDelay;
        public float SawbladeLaunchForce;
        public bool RespondsToAlarm = true;
        public float AlarmDistance;
        public float ToWaypointTimeMultiply = 1;
        public float AbilityDelay;
        public float SecondaryAbilityDelay;
        public bool DisableOffscreenAI;
        public float LaserGrowth = 0.15f;
        public int NumCharsToSpawn;
        public string CharToSpawn;
        public bool ShieldedTurret;

        //**********************
        // LIGHT
        public Light Light;
        public Light AimingLight;
        public Light ThrusterLight;
        public Light OnRemoveLight;
    }

    // The 'State' of the character is mostly used for animation, and also restricting and changing movement/abilties
    public enum CharacterState
    {
        NONE = 0,
        UP = 1,
        DEAD = 2,
        ON_WALL = 3,
        DEATH_ANIM = 4,
    }

    public enum PlayerType
    {
        None,
        Gunhead,
        Enforcer,
        Scout,
        Engineer,
        Pyro,
        Spy,
        Tank,
        Slowmo,
        Teleporter,
    }

    public enum CharacterType
    {
        None,
        Player,
        Gundog,
        Bomber,
        Jackal,
        Jockey,
        Juggernaut,
        Picket,
        Bee,
        FlakTurret,
        Entrencher,
        Tanker,
        Sawblade,
        Sentinel,
        Tattletale,
        MrFixit,
        Bully,
        HelperDrone,
        HelperSentry,
        SeekerLauncher,
        Slimer,
        Pirate,
        Hunter,
        Leviathan,
        Blacksuit,
        Viper,
        Tank,
    }

    // The AIState sets how the character will act
    [Flags]
    public enum AIState
    {
        NONE =  0,
        STATIONARY = 1,
        FINDENEMY = 2 << 1,
        WANDER = 2 << 2,
        PATROL = 2 << 3,
        RETURN = 2 << 4,
        LOSTENEMY = 2 << 5,
        COMBAT = 2 << 6,
        GOTO = 2 << 7,
        ONGUARD = 2 << 8,
        RETREAT = 2 << 9,
        WANDER_CONTINUE = 2 << 10,
    }

    public enum AnimState
    {
        NONE,
        ACTIVE,
        DEACTIVE,
        MOVING,
        MOVINGBACKWARDS,
        FLINCH,
        IDLE,
        SHIP,
        CUSTOM,
    }

    public enum HazardType
    {
        MINES,
        SLIME,
        ALARM_BEACONS,
        BEES
    }

    //
    // END OF CLASS DOCUMENTATION
    //

    [Serializable]
    public struct Controls
    {
        public float CursorX;
        public float CursorY;
        public bool up;
        public bool down;
        public bool dash;
        public bool right;
        public bool left;
        public bool interact;
        public bool interact_new;
        public bool GamepadAiming;
        public bool Attack1;
        public bool Attack2;
        public bool Attack3;
        public bool Attack4;
        public bool Item1;
        public bool Item2;
        public bool Item3;
        public bool Item4;
        public bool Item5;
        public bool Item6;

        public bool IsDown(InventorySlot slot)
        {
            if (slot == InventorySlot.WEAPON_1)
                return Attack1;
            if (slot == InventorySlot.WEAPON_2)
                return Attack2;
            if (slot == InventorySlot.WEAPON_3)
                return Attack3;
            if (slot == InventorySlot.WEAPON_4)
                return Attack4;
            if (slot == InventorySlot.ABILITY)
                return dash;
            return false;
        }

        public bool IsNewDown(Controls previous, InventorySlot slot)
        {
            if (slot == InventorySlot.WEAPON_1)
                return !previous.Attack1 && Attack1;
            if (slot == InventorySlot.WEAPON_2)
                return !previous.Attack2 && Attack2;
            if (slot == InventorySlot.WEAPON_3)
                return !previous.Attack3 && Attack3;
            if (slot == InventorySlot.WEAPON_4)
                return !previous.Attack4 && Attack4;
            if (slot == InventorySlot.ABILITY)
                return !previous.dash && dash;
            return false;
        }

        public bool IsReleased(Controls previous, InventorySlot slot)
        {
            if (slot == InventorySlot.WEAPON_1)
                return previous.Attack1 && !Attack1;
            if (slot == InventorySlot.WEAPON_2)
                return previous.Attack2 && !Attack2;
            if (slot == InventorySlot.WEAPON_3)
                return previous.Attack3 && !Attack3;
            if (slot == InventorySlot.WEAPON_4)
                return previous.Attack4 && !Attack4;
            return false;
        }

        public void SetSlot(bool Value, InventorySlot slot)
        {
            if (slot == InventorySlot.WEAPON_1)
            {
                Attack1 = Value;
            }
            if (slot == InventorySlot.WEAPON_2)
            {
                Attack2 = Value;
            }
            if (slot == InventorySlot.WEAPON_3)
            {
                Attack3 = Value;
            }
            if (slot == InventorySlot.WEAPON_4)
            {
                Attack4 = Value;
            }
        }
    }

    public class CharPlayer
    {
        public bool Spacemode;
        public float ShipRotation;

        public float FireNoise;

        public Vector2 LastThumbsticks = new Vector2(1, 0);
        public double GamepadAimingTime;
        public Vector2 CursorMovement;
        public int GamepadPlayer = -1;
        public bool LoadOutMode;
        public Controls Controls;
        public Controls PreviousControls;
        public Entity FocusEntity;
        public Entity LastInteractEntity;
        public double InteractWaitTime;
        public double InteractWaitTimeTotal;
        public InventorySlot LastWeaponFired = InventorySlot.NONE;
        public bool IsInteractCode;
        public float CursorDistance;

        public float PyroBoostPower;
        public double DashTime;
        public Vector2 LastDashDirection;
        public double NextDash;
        public double TotalDashTime;
        public float StopInteractCodeAt;
        public float NoItemUseTime;
        public Vector2 LastJet;
        public double NextPyroFlame;

        public bool SlowmoOn;
    }

    [Serializable]
    public class Character : Entity
    {
        public string Classname;

        internal float MaxHealth;
        internal Character CharHealthSource;
        internal bool PlayerClone;

        private AIState m_AIState;

        internal AIState AIState
        {
            get
            {
                return m_AIState;
            }
            set
            {
                if (OffScreen && m_AIState != value && (value == Medusa.AIState.FINDENEMY || value == Medusa.AIState.RETURN || value == Medusa.AIState.COMBAT))
                {
                    if (!Engine.instance.OffscreenChasingCharacters.Contains(this))
                        Engine.instance.OffscreenChasingCharacters.Add(this);
                }
                m_AIState = value;
            }
        }

        internal AIState AIStateLast;
        internal AIState AIState_Start;
        internal float AITimeToDeactivate;
        internal int PirateAIState;

        internal bool Dead;

        internal Vector2 MovementRotation;

        internal CharacterState State;
        internal CharacterState State_Start;

        internal double DelayTillShield;

        internal bool OnLeftWall;
        internal bool OnRightWall;
        internal bool Player;
        internal float SpeedForceMultiplier = 1f;
        internal float SpeedMaxMultiplier = 1f;
        internal GameObject TankerSystemProxy;
        internal Character GrapplingWith;
        internal Character GrappledBy;
        internal float GrappleRange;
        internal float GrappleRangeModifier;
        internal float MinGrappleRange;
        internal float MaxGrappleRange;
        internal InventoryItem GrappleWeapon;
        internal GameObject GrappleBeam;
        internal bool DamageOnNextContact;
        internal float DisableDamageOnNextContactAt;
        internal bool CanAlarm = true;
        internal float DieAt;
        internal float NextAbility;
        internal float NextSecondaryAbility;
        internal float NextTertiaryAbility;
        internal double NextWaypointSearch;
        internal GameObject WaypointCurrent;
        internal GameObject WaypointLast;
        internal List<GameObject> WaypointList;
        internal Vector2 WaypointGoal;
        internal double LastWaypointSearch;
        internal GameObject WaypointStart;
        internal Vector2 RetreatFrom;
        internal Vector2 PatrolTarget;
        internal List<Entity> FindEntitiesInRangeResults;
        internal List<Entity> ValidMrFixitHealTargets;
        internal SwarmGroup OwnerSwarmGroup; // a swarm group that this entity owns (as opposed to being a member)
        internal bool IsLunging;
        internal bool IsFiring;
        internal bool IsSecondaryFiring;
        internal bool StopGrappleOnLungeEnd;
        internal bool GrappleDamageOnSolidContact;
        internal float GrappleDamage;
        internal float MinCombatDistance;
        internal bool FlakTurretCanFire;
        internal string FlakCurrentMuzzle;
        internal bool IsOffscreenTeleporting;
        internal List<Character> CharsSpawned;
        internal List<Character> CharsSpawnedToRemove;
        internal GameObject LastProjectileHit;
        internal bool FixOnscreenTailEffects;

        internal float SawbladeAttackTime;
        internal float SawbladeNextAttackTime;
        internal float SawbladeLaunchTime;
        internal bool IsTurretOpen;
        internal float CloseTurretAt;

        internal float AimRotationMix;

        internal float NoFireBoneRecoilUntil;

        internal InventoryItem CurrentShield;

        internal CharacterClass Class;

        internal Cue CharacterAmbientSound;

        internal List<string> StartingObjects;
        internal int ActivePlayerSlot;
        internal Vector2 CamTargetFocusPeek;
        internal Vector2 CamCurrentFocusPeek;

        // Classes for character type specific fields
        internal CharPlayer CharPlayer;

        public Character()
        {
            Type = EntityType.CHARACTER;
            Layer = Layer.GAME;
        }
    }
}
