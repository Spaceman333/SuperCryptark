using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using Medusa;
using Medusa.Physics.Common;
using Medusa.Physics.Collision;
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
using Spine;

namespace Medusa
{
    public delegate void EntityFunc(Entity Ent);
    public delegate void BoneRotationFunc(Character C, Spine.Bone B, double LookR);
    public delegate void HearSoundFunc(Entity Ent, Vector2 Position, Entity Source, bool ThroughWalls, string Sound);
    public delegate void BrokenFunc(Entity Ent);
    public delegate void PickupFunc(Entity Ent, Character C);
    public delegate void InteractFunc(Entity Ent, Fixture fixture, Character C, bool New);
    public delegate void GrabFunc(Entity Ent, Fixture fixture, Character C);

    public enum EntityType
    {
        BRUSH,
        CHARACTER,
        PARTICLE,
        OBJECT,
        SOUNDEFFECT,
        ZAP
    }

    [Flags]
    public enum Layer
    {
        GAME = 1,
        CHARACTERS = 2,
        FOREGROUND = 4,
        BACKGROUND = 8,
        HORIZON1 = 16,
        HORIZON2 = 32,
        HORIZON3 = 64,
        FULLBRIGHT = 128, 
        DECAL = 256,


        // OLD:
        LIGHTMAP = 512,
        LIGHTMAPADDITIVE = 1024,
        BUMPMAP = 2048,
    }

    [Serializable]
    public class SoundEffectEnt : Entity
    {
        public string SoundCue;

        public bool On = true;
        public bool Not3d;
        public float Volume = 1;
        public float Distance = 250;
        public float MinRan = 0;
        public float MaxRan = 0;

        internal bool Error;
        internal double NextSound;

        public SoundEffectEnt()
        {
            Type = EntityType.SOUNDEFFECT;
            Layer = Layer.GAME;
        }
    }

    [Serializable]
    public class Collision : Entity
    {

    }

    [Serializable]
    public class ParticleEnt : Entity
    {
    }

    public class Rope
    {
        public List<Body> RopeBodies;
        public List<RevoluteJoint> RopeJoints;
    }

    public class SearchTarget
    {
        public Character Character;
        public Vector2 FocusRay;

        public SearchTarget(Character Character)
        {
            this.Character = Character;
        }
    }

    [Serializable]
    public class Entity
    {
        // SERIALIZED FIELDS:
        public long ID = 0;
        public long LastModified = 0;
        public EntityType Type;
        public Layer Layer;
        public string Tag;
        public Vector2 Position;
        public Vector2 StartPosition;
        public float StartRotation;
        public float Rotation;
        public float Width;
        public float Height;
        public bool FacingLeft;
        public bool PreviousFacingLeft;
        public bool OriginalFlipX;
        public bool ContinuousInteraction;
        public float InteractionDistance;
        public Vector2 PrefabOffset;
        public bool Teleporting;
        public float RandStartValue;

        // NON SERIALIZED FIELDS:
        internal bool ForceBoundingBoxUpdate;
        internal bool SkipOffscreenReset;
        internal bool AllowVortexWarp = true;
        internal GameObject LightObject;
        internal List<GameObject> ExtraLightObjects;
        internal GameObject ThrusterLight;
        internal bool Added;
        internal bool OffScreenRemove;
        internal int OwnerID;
        internal float OriginalDensity;
        internal bool InteractDisabled;
        internal bool HighlightDescDirty;
        internal bool HighlightNameDirty;
        internal long GridKey;
        internal List<long> OtherGridKeys;
        internal bool KeepUpdatingGrid;
        internal bool InitialGridUpdate = true;
        internal CustomBoundingBox BoundingBox;
        internal float BoundingBoxUpdateRate;
        internal float NextBoundingBoxUpdate;
        internal bool UseSpatialSystem;

        internal float Health;

        internal Inventory Inventory;
        internal bool HasRefractiveItems;
        internal double LastFire;
        internal InventoryItem LastItemFired;
        internal List<PowerupEffect> PowerupEffects;
        internal double NextItemUse;
        internal float LaserLength;
        internal string[] Animations;

        internal Entity Parent;
        internal List<GameObject> Children;
        internal Vector2 LastMovementForce;
        internal List<Entity> ZappedEntities;

        //internal GameObject AimingBeam;
        internal List<GameObject> AimingBeams;
        internal List<GameObject> FireBeams;

        internal float AimSpeedFactor = 1f;

        internal bool IsFlashing;   // is this entity flashing white from damage this frame
        internal float NoFlashingUntil;

        internal AnimState AnimState;

        internal int Team = -1;
        internal int OriginalTeam;

        internal Character Character; // this ent as a character, to avoid constant casting
        internal GameObject GameObject; // this Ent as a gameobject, to avoid constant casting
        internal Brush Brush; // this Ent as a brush, to avoid constant casting

        internal bool ImmuneToProjectiles;
        internal bool ImmuneToExplosions;
        internal bool ImmuneToSlowExplosions;
        internal bool ImmuneToFluid;

        internal List<int> ParticlesAffecting; // list of all particle indices affecting this entity
        internal int NumSlimeParticles; // number of cryogel particles affecting this entity

        internal double LastHit;
        internal Entity LastAttacker;
        internal double NoAirControlTime;
        internal double NoAIAimingTime;
        internal double NoMovingTime;
        internal float StunTime;
        internal float HazardDisabledTime;
        internal float NextParticleEffect;

        internal float MovementMultiplyAfterFiringAmount;
        internal double MovementMultiplyAfterFiringTime;

        internal double NoInteractWithTime;
        internal Vector2 LastLocalFocus;
        internal double LastExplosionHit;
        internal GameObject ShieldedBy;
        internal GameObject SystemShieldedEffect;
        internal Cue ShieldBySound;
        internal GameObject AlarmBy;
        internal GameObject NukedBy;

        internal List<Entity> InSection;
        internal GameObject Section;
        internal LevelSection FromLevelSection;
        internal string SectionName;
        internal bool SectionFlipH;
        internal bool SectionFlipV;
        internal float SectionRotated;

        internal float ShowHealthAlpha;
        internal bool Removed;

        internal bool EmissivesDisabled;

        internal TexturedFixture TextureFixture;

        internal Color Color = Color.White;

        internal Entity Owner;
        internal InventorySlot OwnerSlot;
        internal Entity OwnerOriginal;
        internal InventoryItem OwnerInventoryItem;
        internal Entity ShieldFollowEntity;

        internal Vector2 LastPosition;
        internal int UndoEntIndex;
        internal int UndoEntLayerIndex;

        internal double IgnoreHitsTime;

        internal AudioEmitter Emitter;

        internal Body Body;
        internal Fixture WallHitFixture;

        internal double StartTime;

        internal bool OffScreen = true;
        internal bool ForceOnscreenSetup = true;

        internal bool NoOffscreen;
        internal float NoOffscreenRemoveUntil;
        internal double RemoveTime;

        internal double NextRotateSound;

        internal InventoryItem ActiveMeleeWeapon;
        internal Vector2 LastRayEnd;
        internal bool PlayerStartingShip;

        internal Entity OriginFactory;

        internal Zap Zap;

        internal float AddedTurretRotation;

        internal bool EnemyDeactivated;

        internal Texture2D Texture;
        internal Texture2D NormalmapTexture;
        internal Texture2D TextureSheet;
        internal TextureBounds TextureBounds;
        internal TextureBounds NormalmapTextureBounds;

        internal Character LastMrFixItHealer;
        internal double LastMrFixItHealerTime;

        // for editor
        internal float OriginalHeight, OriginalWidth, OriginalRotation;
        internal bool OriginalRotateSet;
        internal Vector2 MouseOffset, OriginalPosition, MouseStart;
        internal float OriginalMass;

        internal Vector2 FakeProjectile_Speed;

        internal Dictionary<string, float> BoneOffsets;
        internal Dictionary<string, Vector2> BonePosOffsets;

        internal double DeactiveDelay;
        internal double AIRetreatTime;
        internal double NoFiringTime;
        internal double CustomAnimationTime;
        internal bool CustomAnimationAllowMovement;
        internal bool CustomAnimationNoAttacking;
        internal bool CustomAnimationNoTarget;
        internal bool CustomAnimationNoAiming;
        internal bool CustomAnimationLooping;
        internal string CustomAnimation;
        internal string CustomAnimationLast;

        internal double AINextJukeCheck;
        internal bool AIJukeUp;

        internal EntityFunc Draw;
        internal EntityFunc PreUpdate;
        internal EntityFunc OffScreenUpdate;
        internal EntityFunc Update;
        internal bool ToUpdate;
        internal EntityFunc UpdateGrid;
        internal EntityFunc SpineUpdate;
        internal EntityFunc Movement;
        internal EntityFunc Control;
        internal EntityFunc Animation;
        internal EntityFunc AnimationRotate;
        internal EntityFunc Die;

        internal EntityFunc Start;

        internal BoneRotationFunc BoneRotation;
        internal InteractFunc Interact;
        internal PickupFunc Pickup;
        internal BrokenFunc Break;
        internal HearSoundFunc HearSound;
        internal bool SoundOff;
        internal Cue Sound;
        internal Cue Sound2; // these are just used randomly for whatever entities, just using the same varaible so they are always removed
        internal Cue Sound3;
        internal Cue Sound4;
        internal Cue OnFire;
        internal Skeleton skeleton;
        internal AnimationState AnimationState;
        internal AnimationStateData AnimationStateData;
        internal SkeletonBounds bounds;
        internal Character InteractBy;
        internal Character LastInteractBy;

        internal Bone Bone_Laser;
        internal Bone Bone_Fan;
        internal Bone Bone_ParticleEffect;
        internal Bone Bone_Muzzle;

        internal bool Blocker;
        internal float ForwardPushForceMult = 1f;
        internal float BackwardPushForceMult = 1f;

        internal string ReleaseSound;
        public double ReleaseSoundTime;

        internal double NextLiquidHurt;

        internal float StartInertia;

        internal float FocusRotation;
        internal Vector2 FocusDistance;
        private Vector2 mFocus;
        internal Vector2 Focus
        {
            get
            {
                return mFocus;
            }
            set
            {
                mFocus = value;
                FocusDistance = mFocus - Position;
                FocusRotation = (float)Math.Atan2(FocusDistance.Y, FocusDistance.X);
                if (float.IsNaN(FocusRotation))
                {
                    FocusRotation = 0;
                    FocusDistance = Vector2.One;
                }
                if (FocusDistance == Vector2.Zero)
                    FocusDistance = new Vector2(1, 0);
            }
        }
        internal Vector2 CustomPushDirection;

        // Swarm variables
        internal SwarmGroup MemberSwarmGroup; // the swarm group this entity is a member of
        internal Vector2 SwarmPosition;
        internal Vector2 SwarmVelocity;
        internal int SwarmCellI;
        internal int SwarmCellJ;
        internal int SwarmCohesionNeighborCount;
        internal int SwarmSeparationNeighborCount;
        internal int SwarmAlignmentNeighborCount;
        internal Entity[] SwarmCohesionNeighbors;
        internal Entity[] SwarmSeparationNeighbors;
        internal Entity[] SwarmAlignmentNeighbors;
        internal bool SwarmBehaviorEnabled;
        internal Vector2 SwarmGoal;

        // AI variables
        internal Vector2 LastWaypointNeededGoal;
        internal Vector2 GoalPosition;
        internal Vector2 FocusRay;
        internal double NextFindEnemyRayCast;
        internal double NextStationaryRayCast;
        internal double NextRetreatRayCast;
        internal string LastMuzzle;
        internal Character FindCharacter;
        internal Entity FindEntity;
        internal Vector2 FindCharacterLastPosition;
        internal double FindCharacterTime;
        internal List<SearchTarget> SearchTargets;

        Vector2 MapOffsetLast;
        internal Vector2 MapOffsetLastPosition;
        float MapOffSetLastRotation;

        public Vector2 GetMapOffset()
        {
            if (GameObject == null)
                return Position;
            else
            {
                if (MapOffsetLastPosition == Position && MapOffSetLastRotation == Rotation)
                    return MapOffsetLast;
                MapOffsetLastPosition = Position;
                MapOffSetLastRotation = Rotation;

                if (GameObject.Class.CoreType == CoreType.HEART)
                    MapOffsetLast = Position + Vector2.Transform(new Vector2(0, 20), Matrix.CreateRotationZ(Rotation));
                else if (GameObject.Class.System)
                    MapOffsetLast = Position + Vector2.Transform(new Vector2(0, -20), Matrix.CreateRotationZ(Rotation));
                else if (GameObject.Class.Type == GameObjectType.TERMINAL)
                    MapOffsetLast = Position + Vector2.Transform(new Vector2(0, -15f), Matrix.CreateRotationZ(Rotation));
                else if (GameObject.Class.Type == GameObjectType.TECH_PICKUP)
                    MapOffsetLast = Position + Vector2.Transform(new Vector2(0, -20), Matrix.CreateRotationZ(Rotation));
                else if (GameObject.Class.Type == GameObjectType.ARTIFACT_POD)
                    MapOffsetLast = Position + Vector2.Transform(new Vector2(0, -20), Matrix.CreateRotationZ(Rotation));
                else
                    MapOffsetLast = Position;

                return MapOffsetLast;
            }
        }

        public void AnimationEvent(AnimationState state, int trackIndex, Event e)
        {
            float Amount = e.Float;
            if (Amount == 0) // so it can be the float or the int
                Amount = (float)e.Int;

            if (((Engine.instance.DebugView.Flags & Physics.DebugViewFlags.Game) == Physics.DebugViewFlags.Game))
                Engine.instance.AddDebugText(e.Data.Name, Position, Color.White);

            if (e.Data.Name.Contains("particle_"))
            {
                string ParticleName = e.Data.Name.Replace("particle_", "");
                Vector2 Pos = Position;
                if (!string.IsNullOrEmpty(e.String))
                    Pos = SpineFunc.GetBonePosition(this, e.String.Replace("\n",""));
                Particles.GameParticles[ParticleName].Trigger(Pos, -1);

                if (ParticleName == "spark")
                    Engine.instance.PlaySound("FX_M_Systems_Spark", this, Pos);
                else if (ParticleName == "_new_explosion_rock")
                    Engine.instance.PlaySound("FX_Hazard_Crusher_Impact", this, Pos);
            }
            else if (e.Data.Name == "shake_fluid")
            {
                if (Engine.instance.FluidSimulation != null)
                    Engine.instance.FluidSimulation.ShakeParticles(this, e.Int, e.Float);
            }
            else if (e.Data.Name == "lunge")
            {
                Character.IsLunging = e.Int == 1;
            }
            else if (e.Data.Name == "push")
            {
                if (Character != null && !Character.Player && NoAirControlTime > Engine.instance.Time)
                    return;
                bool WasIgnore = Body.IgnoreForces;
                float multiplier = Amount > 0 ? ForwardPushForceMult : BackwardPushForceMult;
                Vector2 dir = FocusDistance;

                if (CustomPushDirection != Vector2.Zero)
                    dir = CustomPushDirection;

                Body.IgnoreForces = false;
                if (FocusDistance != Vector2.Zero)
                    Body.ApplyForce(Amount * Body.Mass * 400 * Vector2.Normalize(dir) * multiplier);
                Body.IgnoreForces = WasIgnore;
                CustomPushDirection = Vector2.Zero;
            }
            else if (e.Data.Name == "melee")
            {
                if (ActiveMeleeWeapon != null)
                {
                    if (Amount > 0)
                    {
                        if (!ActiveMeleeWeapon.MeleeStrike)
                        {
                            if (!string.IsNullOrEmpty(ActiveMeleeWeapon.GameObjectClass.AttackSound))
                                Engine.instance.PlaySound(ActiveMeleeWeapon.GameObjectClass.AttackSound, this);
                        }

                        if (ActiveMeleeWeapon.MeleeHits != null)
                            ActiveMeleeWeapon.MeleeHits.Clear();

                        ActiveMeleeWeapon.MeleeStrike = true;
                        Body.IgnoreForces = true;
                    }
                    else if (Amount < 0)
                    {
                        ActiveMeleeWeapon.MeleeStrike = false;
                        ActiveMeleeWeapon = null;
                        Body.IgnoreForces = false;
                    }
                }
                else
                {
                    Engine.instance.AddDevConsoleText("No active melee weapon for melee attack");
                }

                /*
                if (Amount > 0)
                {
                    ActiveMeleeWeapon.MeleeHits.Clear();
                    ActiveMeleeWeapon.MeleeStrike = true;
                    Body.IgnoreForces = true;
                }
                else
                {
                    ActiveMeleeWeapon.MeleeStrike = false;
                    ActiveMeleeWeapon = null;
                    Body.IgnoreForces = false;
                }*/
            }
            else if (e.Data.Name == "blow_left_door" && Amount > 0)
            {
                Vector2 force = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * -30000f;
                GameObject gib = LevelObjects.CreateGib(this, skeleton.FindSlot("door_l_box"), force, MiscFunc.RandomBetween(-0.5f, 0.5f), "Gibs\\gib", 3f);

                gib.Body.LinearDamping = 4f;
                gib.Body.AngularDamping = 2f;
            }
            else if (e.Data.Name == "blow_right_door" && Amount > 0)
            {
                Vector2 force = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * 30000f;
                GameObject gib = LevelObjects.CreateGib(this, skeleton.FindSlot("door_r_box"), force, MiscFunc.RandomBetween(-0.5f, 0.5f), "Gibs\\gib", 3f);

                gib.Body.LinearDamping = 4f;
                gib.Body.AngularDamping = 2f;
                //Stations.GetSupplyPodContents(LastInteractBy, GameObject);
            }
            else if (e.Data.Name == "getkey")
            {
                //Stations.GetTerminalContents(LastInteractBy, GameObject);
            }
            else if (skeleton.Data.Name == "core" && e.Data.Name == "fire")
            {
                Engine.instance.PlaySound("FX_M_Core_Shot", this);
                if (GameObject.CoreCanFire && SystemsFunc.SystemHasHealth(GameObject))
                {
                    foreach (string muzzleName in GameObject.CoreMuzzleNames)
                    {
                        if (GameObject.Class.SystemLevel == 1)
                        {
                            if (GameObject.CoreLevel1FireSwitch && (muzzleName == "muzzle01" || muzzleName == "muzzle03"))
                                continue;
                            if (!GameObject.CoreLevel1FireSwitch && (muzzleName == "muzzle02" || muzzleName == "muzzle04"))
                                continue;
                        }
                        Vector2 position = SpineFunc.GetBonePosition(GameObject, muzzleName);
                        float rotation = SpineFunc.GetBoneRotation(GameObject, muzzleName);
                        Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                        GameObject projectile = WeaponsFunc.CreateProjectile(this, position, "EnemyWeapons\\core_p", direction);

                        projectile.Class.IgnoreSystemCollision = true;
                        projectile.Class.IgnoreSystemHealthProxyCollision = true;
                    }
                    GameObject.CoreLevel1FireSwitch = !GameObject.CoreLevel1FireSwitch;
                }
            }
            else if (skeleton.Data.Name == "alarm")
            {
                if (e.Data.Name == "fire")
                {
                    GameObject.AlarmCanFire = true;
                }
                else if (e.Data.Name == "fire_stop")
                    GameObject.AlarmCanFire = false;
                else if (e.Data.Name == "vulnerable")
                {
                    foreach (GameObject child in GameObject.Children)
                    {
                        child.ImmuneToExplosions = false;
                        child.ImmuneToProjectiles = false;
                    }
                }
                else if (e.Data.Name == "invulnerable")
                {
                    foreach (GameObject child in GameObject.Children)
                    {
                        child.ImmuneToExplosions = true;
                        child.ImmuneToProjectiles = true;
                    }
                }
            }
            else if (skeleton.Data.Name == "sentry_new" || skeleton.Data.Name == "sentry_flakk")
            {
                if (e.Data.Name == "start_tracking")
                {
                    GameObject.SentryTrackingAllowed = true;
                    GameObject.SentryStartChargingAt = (float)Engine.instance.Time + 1f;
                }
                else if (e.Data.Name == "stop_tracking")
                {
                    GameObject.SentryTrackingAllowed = false;
                }
                else if (e.Data.Name == "fire")
                {
                    GameObject.SentryIsCharging = false;
                    GameObject.SentryIsFiring = true;
                    GameObject.SentryStopFiringAt = (float)Engine.instance.Time + 2f;
                }
                else if (e.Data.Name == "invulnerable")
                {
                    foreach (GameObject child in GameObject.Children)
                    {
                        child.ImmuneToExplosions = true;
                        child.ImmuneToProjectiles = true;
                    }
                }
                else if (e.Data.Name == "vulnerable")
                {
                    foreach (GameObject child in GameObject.Children)
                    {
                        child.ImmuneToExplosions = false;
                        child.ImmuneToProjectiles = false;
                    }
                }
            }
            else if (skeleton.Data.Name == "factory" || skeleton.Data.Name == "factory_advanced")
            {
                if (e.Data.Name == "invulnerable")
                {
                    if (!OffScreen)
                        Engine.instance.PlaySound("FX_M_DroneFactory_Close", this);

                    foreach (GameObject child in GameObject.Children)
                    {
                        child.ImmuneToExplosions = true;
                        child.ImmuneToProjectiles = true;
                    }
                }
                else if (e.Data.Name == "vulnerable")
                {
                    if (!OffScreen)
                        Engine.instance.PlaySound("FX_M_DroneFactory_Open", this);

                    foreach (GameObject child in GameObject.Children)
                    {
                        child.ImmuneToExplosions = false;
                        child.ImmuneToProjectiles = false;
                    }
                }
                else if (e.Data.Name == "fire")
                {
                    GameObject.FactoryCanFire = true;
                }
            }
            else if (skeleton.Data.Name == "repair_system")
            {
                if (e.Data.Name == "fire_01")
                {
                    GameObject.RepairCurrentMuzzleIndex = 0;
                    GameObject.RepairFireDrone = true;
                }
                else if (e.Data.Name == "fire_02")
                {
                    GameObject.RepairCurrentMuzzleIndex = 1;
                    GameObject.RepairFireDrone = true;
                }
                else if (e.Data.Name == "fire_03")
                {
                    GameObject.RepairCurrentMuzzleIndex = 2;
                    GameObject.RepairFireDrone = true;
                }
                else if (e.Data.Name == "fire_04")
                {
                    GameObject.RepairCurrentMuzzleIndex = 3;
                    GameObject.RepairFireDrone = true;
                }
                else if (e.Data.Name == "fire_05")
                {
                    GameObject.RepairCurrentMuzzleIndex = 4;
                    GameObject.RepairFireDrone = true;
                }
                else if (e.Data.Name == "stop_regen")
                {
                    GameObject.Children[0].HealthRegenEnabled = false;
                }
                else if (e.Data.Name == "start_regen")
                {
                    if (GameObject.Class.SystemLevel >= 2 && GameObject.Class.Type != GameObjectType.SYSTEM_REPAIR)
                        GameObject.Children[0].HealthRegenEnabled = true;
                }
            }
            else if (skeleton.Data.Name == "flak_turret")
            {
                if (e.Data.Name == "fire_left")
                {
                    Character.FlakCurrentMuzzle = "left_muzzle";
                    WeaponsFunc.Fire(this, Inventory.Items[InventorySlot.WEAPON_1]);
                }
                else if (e.Data.Name == "fire_right")
                {
                    Character.FlakCurrentMuzzle = "right_muzzle";
                    WeaponsFunc.Fire(this, Inventory.Items[InventorySlot.WEAPON_1]);
                }
            }
            /*else if (skeleton.Data.Name == "tanker")
            {
                if (e.Data.Name == "enable_fire")
                {
                    if (Character.AIState == AIState.PATROL)
                    {
                        if (Character.Class.HazardType == HazardType.MINES)
                        {
                            GameObject mine = new GameObject();

                            mine.Classfile = "Hazards\\mine";
                            mine.Position = SpineFunc.GetBonePosition(this, "muzzle");
                            Engine.instance.AddEntity(mine);
                            mine.Team = 1;
                        }
                    }
                }
            }*/
            else if (skeleton.Data.Name == "heavy_sentinel")
            {
                if (e.Data.Name == "fire")
                {
                    WeaponsFunc.Fire(this, Inventory.Items[InventorySlot.WEAPON_1]);
                }
                else if (e.Data.Name == "activate_shield")
                {
                    WeaponsFunc.ActivateShield(Character, Inventory.Items[InventorySlot.WEAPON_2]);
                }
                else if (e.Data.Name == "deactivate_shield")
                {
                    WeaponsFunc.DeactivateShield(Inventory.Items[InventorySlot.WEAPON_2].ShieldObject);
                }
            }
            else if (skeleton.Data.Name == "tattletale")
            {
                //if (Character.CanAlarm && e.Data.Name == "alarm")
                //    CFunc.CharacterAlarm(Character, Character.FindCharacter, Position);
            }
            else if (skeleton.Data.Name == "seeker_launcher")
            {
                if (e.Data.Name == "close")
                {
                    Engine.instance.PlaySound("Combat_M_SeekerLauncher_Close", this);
                }
                else if (e.Data.Name == "open")
                {
                    Engine.instance.PlaySound("Combat_M_SeekerLauncher_Open", this);
                }
                else
                {
                    SwarmManager manager = SwarmManager.Instance;
                    string muzzleName = null;
                    Vector2 spawnPos;
                    Character spawn;

                    if (e.Data.Name == "fire01")
                        muzzleName = "muzzle01";
                    else if (e.Data.Name == "fire02")
                        muzzleName = "muzzle02";
                    else if (e.Data.Name == "fire03")
                        muzzleName = "muzzle03";
                    else if (e.Data.Name == "fire04")
                        muzzleName = "muzzle04";

                    if (Character.OwnerSwarmGroup == null)
                        Character.OwnerSwarmGroup = manager.CreateGroup(50);

                    spawnPos = SpineFunc.GetBonePosition(this, muzzleName);
                    spawn = new Character();
                    spawn.Position = spawnPos;
                    spawn.Classname = "Bee";
                    Engine.instance.AddEntity(spawn);
                    manager.AddEntityToGroup(Character.OwnerSwarmGroup, spawn);

                    Particles.GameParticles["_new_muzzle_enemyblaster"].Trigger(spawnPos);
                    Engine.instance.PlaySound("FX_M_RepairSystem_Drones_Eject", spawn, spawnPos);
                }
            }
            else if (skeleton.Data.Name == "bully")
            {
                if (e.Data.Name == "fire")
                    WeaponsFunc.Fire(this, Inventory.Items[InventorySlot.WEAPON_1]);
            }
            else if (skeleton.Data.Name == "stasis_gel_arm")
            {
                if (e.Data.Name == "start_fire")
                    GameObject.IsSpraying = true;
                else if (e.Data.Name == "stop_fire")
                    GameObject.IsSpraying = false;
            }
            else if (skeleton.Data.Name == "cage_generator")
            {
                if (e.Data.Name == "activate")
                    GameObject.CageActive = true;
                else if (e.Data.Name == "deactivate")
                    GameObject.CageActive = false;
            }
            else if (Type == EntityType.CHARACTER)
            {
                if (Character.Class.Type == CharacterType.Leviathan)
                {
                    int OnscreenSpawned = 0;
                    foreach (Character Spawned in Character.CharsSpawned)
                    {
                        if (!Spawned.OffScreen)
                            OnscreenSpawned++;
                    }
                    if (e.Data.Name == "enable_fire" && OnscreenSpawned < 3)
                    {
                        Character spawn = new Character();

                        spawn.Classname = Character.Class.CharToSpawn;
                        spawn.Position = SpineFunc.GetBonePosition(this, "muzzle");
                        Engine.instance.AddEntity(spawn);
                        Character.CharsSpawned.Add(spawn);
                    }
                }
                else if (Character.Class.Type == CharacterType.Blacksuit)
                {
                    if (e.Data.Name == "start_fire")
                        Character.IsFiring = true;
                    else if (e.Data.Name == "stop_fire")
                        Character.IsFiring = false;
                    else if (e.Data.Name == "activate_shield" && Character.State != CharacterState.DEATH_ANIM)
                    {
                        ItemsFunc.Use(this, Inventory.Items[InventorySlot.ABILITY]);
                        Character.CurrentShield.ShieldObject.NextAbility = float.MaxValue;
                        Character.IsSecondaryFiring = true;
                    }
                    else if (e.Data.Name == "deactivate_shield" && Character.CurrentShield != null)
                    {
                        Character.CurrentShield.ShieldObject.NextAbility = 0.1f;
                        Character.IsSecondaryFiring = false;
                    }
                }
                else if (Character.Class.Type == CharacterType.Viper)
                {
                    if (e.Data.Name == "fire")
                        WeaponsFunc.Fire(this, Inventory.Items[InventorySlot.WEAPON_1]);
                }
            }
            else if (Type == EntityType.OBJECT)
            {
                if (GameObject.Class.Type == GameObjectType.CRUSHER_SINGLE)
                {
                    GameObject child = GameObject.Children[0];

                    if (e.Data.Name == "enable")
                        child.CrusherPistonEnabled = true;
                    else
                        child.CrusherPistonEnabled = false;
                }
                else if (GameObject.Class.Type == GameObjectType.CRUSHER_TRIPLE)
                {
                    GameObject child = GameObject.Children[1 + e.Int];

                    if (e.Data.Name == "enable")
                        child.CrusherPistonEnabled = true;
                    else
                        child.CrusherPistonEnabled = false;
                }
                else if (GameObject.Class.Type == GameObjectType.INCINERATOR_STATIONARY)
                {
                    if (e.Data.Name == "is_open")
                        GameObject.IsOpen = true;
                    else if (e.Data.Name == "is_closing")
                        GameObject.IsOpen = false;
                }
                else if (GameObject.Class.Type == GameObjectType.INCINERATOR_FORK ||
                    GameObject.Class.Type == GameObjectType.INCINERATOR_WHEEL)
                {
                    if (e.Data.Name == "fireSound")
                    {
                        Engine.instance.PlaySound("FX_Hazard_Incinerator_Fire", GameObject);
                    }
                    else if (e.Data.Name == "fire")
                    {
                        GameObject.IsSpraying = true;
                    }
                    else if (e.Data.Name == "fire_stop")
                        GameObject.IsSpraying = false;
                }
                else if (GameObject.Class.Type == GameObjectType.SAWBLADE_WALLARM)
                {
                    if (e.Data.Name == "fire")
                    {
                        Vector2 debrisPos = SpineFunc.GetBonePosition(this, "muzzle");
                        float debrisAngle = SpineFunc.GetBoneRotation(this, "muzzle");
                        GameObject debris;
                        float debrisSpeed = MiscFunc.RandomBetween(6f, 9f);

                        debrisAngle += MiscFunc.RandomBetween(-0.4f, 0.4f);

                        //if (GameObject.FlippedHorizontal)
                        //    debrisAngle = -debrisAngle + MathHelper.Pi;
                        //if (GameObject.FlipVertical)
                        //    debrisAngle = -debrisAngle;

                        debris = SpineFunc.CreateDebris(this, skeleton.FindBone("muzzle"), MiscFunc.mRandom.Next(99999999), Vector2.Zero, 1f, 1f, false, "Hazards\\sawblade_debris");
                        debris.Body.LinearVelocity = new Vector2((float)Math.Cos(debrisAngle), (float)Math.Sin(debrisAngle)) * debrisSpeed;
                    }
                }
                else if (GameObject.Class.Type == GameObjectType.SYSTEM_SHUFFLE)
                {
                    if (e.Data.Name == "vulnerable")
                    {
                        GameObject weakObj = Children[0];

                        weakObj.ImmuneToExplosions = false;
                        weakObj.ImmuneToFluid = false;
                        weakObj.ImmuneToProjectiles = false;
                        weakObj.ImmuneToSlowExplosions = false;
                    }
                    else if (e.Data.Name == "invulnerable")
                    {
                        GameObject weakObj = Children[0];

                        weakObj.ImmuneToExplosions = true;
                        weakObj.ImmuneToFluid = true;
                        weakObj.ImmuneToProjectiles = true;
                        weakObj.ImmuneToSlowExplosions = true;
                    }
                }
                else if (GameObject.Class.Type == GameObjectType.PLAYERSHIP_PLAYER)
                {
                    if (e.Data.Name == "teleport_effect")
                    {
                        GameObject refraction = new GameObject();
                        GameObject light = new GameObject();

                        refraction.Classfile = "Refractions\\shockwave_2";
                        refraction.Position = Position;
                        Engine.instance.AddEntity(refraction);

                        light.Classfile = "Lights\\death_teleport_light";
                        light.Position = Position;
                        Engine.instance.AddEntity(light);
                    }
                }
                else if (GameObject.Class.Type == GameObjectType.BEE_HIVE)
                {
                    if (e.Data.Name == "fire")
                    {
                        Vector2 basePos = SpineFunc.GetBonePosition(this, "spawn");
                        Vector2 spawnPos = basePos + MiscFunc.RandomInUnitCircle() * 10f;
                        Character bee = new Character();

                        if (GameObject.HiveCurrentGroup == null)
                            GameObject.HiveCurrentGroup = SwarmManager.Instance.CreateGroup(5);

                        bee.Classname = "Bee";
                        bee.Position = spawnPos;
                        Engine.instance.AddEntity(bee);

                        SwarmManager.Instance.AddEntityToGroup(GameObject.HiveCurrentGroup, bee);
                    }
                }
            }
        }
    }
}
