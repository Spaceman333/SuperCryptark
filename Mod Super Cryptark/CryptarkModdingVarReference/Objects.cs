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
using Medusa.Physics.Collision.Shapes;
using Medusa.Physics.Dynamics;
using Medusa.Physics.Dynamics.Joints;
using Medusa.Physics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using ProjectMercury;
using TexturePolygonLib;
using Spine;
using TexturePackingRuntime;

namespace Medusa
{
    [Serializable]
    public class GameObjectClass
    {
        // internal, cant be set in XML files
        internal string Classfile;
        internal bool BaseFileProcessed;
        internal GameObjectClass BaseObj;
        internal TextureBounds HudTexture;
        internal TextureBounds SidebarTexture;
        internal TextureBounds MinimapTexture;
        internal TextureBounds MapTexture;
        internal Texture2D TextureFile;
        internal Texture2D TextureNormalMapFile;
        internal Texture2D SpineWeaponTexture;
        internal Texture2D SpineWeaponNormal;
        internal Texture2D SpineWeaponRefraction;
        internal Texture2D SpineWeaponHeadTexture;
        internal Texture2D SpineWeaponHeadNormal;
        internal Texture2D SpineWeaponHeadRefraction;
        internal Texture2D RefractionMap;
        internal Texture2D MeshEffectTexture;
        internal Texture2D WeaponChargeTexture;
        internal Texture2D WeaponChargeTexture_Norm;
        internal Texture2D ProjectileMapTexture;
        internal ParticleEffect ParticleEffectRef;

        public List<string> RandomIngameClass;

        //**********************
        // MAIN
        public GameObjectType Type;
        public string BaseFile;
        public bool UseSpatialSystem = true;
        public float BoundingBoxWidth;
        public float BoundingBoxHeight;
        public float BoundingBoxUpdateRate;
        public bool SpaceWaypoint;
        public bool LiquidOnly;

        //**********************
        // DRAWING
        public string Texture;
        public string TextureBounds;
        public string TextureBoundsNorm;
        public string TextureDictionary;
        public bool CustomTexOrigin;
        public float TexOriginX;
        public float TexOriginY;
        public bool HasWeaponChargeTexture;
        public Vector2 TextureOffset;
        public string Displayname;
        public string VoiceUIName;
        public double VoiceUINameTime;
        public string ShortDisplayName; // displayed in HUD, load out
        public Layer SetLayer; // to set it as a specific layer (rather than default GAME), such as FULLBRIGHT
        public bool ShowsInteractOutline;
        public bool ShowsInteractDisplayname;
        public string HudTextureName;
        public string MapTextureName;
        public string NormalMap;
        public bool EditorOnly; // only draw in editor mode
        public bool ShowEditorDirection;
        public bool SendToBack; // send to the back of the drawing order in whatever layer we are in
        public bool SendToFront;
        public bool Tile;
        public float Scale = 1; // scale the texture
        public float ScaleModifier; // amount to modify the scale per frame
        public bool NoOffscreen; // dont disable when offscreen
        public bool SetNoOffscreenOnDamaged;
        public float OffscreenDistanceMultiply;
        public bool TextureSelect; // allow selection of texture in editor
        public bool LayerSelect; // select layer in editor
        public bool ManualFadeOut; // whether or not the fade out happens immediately after being added
        public float FadeOutDuration; // how long it takes for this object to fade out
        public float FadeOutStartDelay; // how long before the fade out starts
        public bool RemoveOnFadeOut = true; // whether or not this entity will be removed when it fades out (doesn't work when ManualFadeOut is true)
        public string RefractionMapName; // name of the refraction texture
        public bool UseRefractionOrigin;
        public Vector2 RefractionOrigin;
        public float RefractionScale;
        public float RefractionAlpha = 1f;
        public float RefractionAlphaMod;
        public string MeshEffectTextureName;
        public float MeshEffectShiftU;
        public float MeshEffectShiftV;
        public float MeshEffectTransInSpeed = 0.1f;
        public float MeshEffectTransOutSpeed = 0.1f;
        public string SidebarTextureName;
        public List<string> MeshEffects;
        public MeshEffectType MeshEffectType;
        public float MeshEffectRadius;
        public bool IgnoreSlowMo;
        public bool Additive;
        public int MeshWidthSegments;
        public int MeshHeightSegments;
        public float MeshEffectWidth;
        public float MeshEffectStartCapWidth;
        public float MeshEffectStartCapHeight;
        public float MeshEffectEndCapWidth;
        public float MeshEffectEndCapHeight;
        public float MeshEffectTileScale = 1f;
        public float MeshEffectRadiusScale = 1f;
        public float HealthBarScale = 1f;
        public string ProjectileMapTextureName;
        public float ProjectileMapTextureScale = 1f;
        public bool ProjectileMapTextureUseRotation = true;
        public int NumVerletPoints;
        public float VerletSpacing = 1f;
        public bool VerletSinMovement;
        public float VerletPinAngleForce;
        public float VerletTailFriction = 0.99f;
        public float VerletPinAngleOffset;
        public float VerletSinFrequency = 1f;
        public float VerletSinForce = 10f;
        public int VerletSinPointCount;
        public float VerletSinDistanceMod = 0.25f;
        public string ChargeParticleEffect;
        public string SlowExplosionCenterEffect = "slow_explosion_center";
        public string SlowExplosionSparkEffect = "slow_explosion_spark";
        public bool RandomStartRotation;

        //**********************
        // MINIMAP
        public string MinimapTextureName;
        public string MinimapDescription;
        public float MinimapTextureScale = 1;

        //**********************
        // ANIMATION
        public string SpineAnimation;
        public float SpineAnimationScale = 1;
        public string SpineAtlas;
        public string SpineAnimationSkin;
        public Vector2 AnimationOffset; // offset their animation from their physics body
        public bool DisableSpineUpdate;
        public bool FollowFacing;
        public string SetAnimationOnContact;
        public int SetAnimationOnContactTrack;
        public bool SetAnimationOnContactLoop;
        public string DeathAnimation; // only works for systems atm
        public string SpawnOnDeathAnim; // ^^

        //**********************
        // INTERACTIONS
        public bool ContinuousInteraction;
        public float InteractionDistance = 25f;
        public int InteractionCodeLength;
        public bool DisableInteractOnCodeComplete;
        public bool InteractThroughWalls;

        //**********************
        // PHYSICS / COLLISION
        public float Height; // height of physics body
        public float Width;
        public bool EclipseBody; // if it should have a eclipse physics body rather than rectanglar
        public bool SolidArcBody; // if it should have a solid arc body
        public float ArcRadians; // span of arc body
        public int ArcSides; // number of sides used to create arc body
        public bool EditorResizeable; // if it can be resized in the editor
        public float LinearDamping;
        public float AngularDamping;
        public float Restitution;
        public float Friction;
        public bool SetFriction;
        public bool CharacterCollide = true; // collide with characters
        public bool IgnoreBrushCollision;
        public bool CollideNonPlayer;
        public bool CollideNonScriptedCharacters;
        public bool CollidePlayer;
        public bool IgnoreCollision;
        public bool IgnoreExplosions;
        public bool IgnoreGameobjectCollision;
        public bool IgnoreDoorCollision;
        public bool IgnoreNonShieldGameobjectCollision;
        public bool IgnoreGravity;
        public bool IgnoreWeaponCollision;
        public bool IgnoreFluid;
        public bool AllowProjectileOnProjectileCollision;
        public bool AllowOtherProjectilesToRemove = true;
        public bool IgnoreProjectileCollision;
        public bool IgnoreSystemCollision;
        public bool IgnoreSystemChildrenCollision;
        public bool IgnoreSystemHealthProxyCollision;
        public bool IgnoreSameTypeCollision;
        public bool ProjectileHitDebris;
        public bool Solid;

        public bool FakeProjectile;
        public bool FakeProjectileNoBounce;
        
        public bool CustomBody;
        public string BoundingBoxBody; // generate physics body from a bounding box slot
        public bool TextureGeom; // generate physics body from texture
        public bool TextureGeomExactCenter;
        public bool NoRotation;
        public bool Kinematic;
        public bool Static;
        public float Mass;
        public Vector2 CollisionOffset;
        public string CollisionTexture; // if TextureGeom is set, another texture to use for the collision
        public float Density = 0.5f;
        public bool FollowBoneRotation; // have the box2d body follow the rotation of GameObject.BoneToFollow
        public bool Sensor; // sets body to be a sensor
        public bool BlockRaycasts;
        public bool DisableFollowBodyRotation; // disables automatic setting of Object.Rotation to Object.Body.Rotation
        public float SawSpinSpeed;
        public float FanSpinSpeed = 2f;
        public float StartIgnoreHitsTime = 1f;
        public bool NoFlipping;

        //**********************
        // LEVEL SECTION FIELDS
        public bool AllowFlipHorizontally; // if when placing it should be randomly flipped horizontally
        public bool AllowFlipVertically;
        public bool AllowRotation; // if when placing it should be randomly rotated
        public List<string> LevelSections; // list of level sections to randomly pick from
        public List<LevelSectionClass> ClassSections;
        public Vector3 DebugViewColor = new Vector3(0, 0.25f, 0); // color to display the level section in debug view
        public bool NoSystemLocationsNeeded;
        public bool OnlyCheckCoverWaypoints;
        public bool NoCheckingWaypoints;

        //**********************
        // LOAD OUT
        public LoadoutCategory LoadoutCategory;
        public float Price;
        public float BasefilePriceMulitply;
        public bool AmmoAsPercent;
        public string ItemInfo;
        public bool DevOnly; // if it should be only accessible when in developermode, good for items
        public bool DemoOnly;
        public bool CampaignModeOnly;
        public int TechChanceNumber = 1;
        public string OnLoadoutTrigger;

        //**********************
        // SYSTEMS
        public string Codename;
        public bool System; // if it's a system
        public bool SystemHealthProxy; // if this object has health that affects its parent
        public float SystemHealthProxyDamageMulitply = 1; // how to multiply damage done to proxies of this system
        public bool ZapOnTouch;
        public float NextSystemAbility;
        public float NextSystemAbilityRandom;
        public int SystemLevel = 1;
        public bool ShieldMeleeHits;
        public float AlarmRotateSpeed;
        public float AlarmSweepDuration;
        public float RepairDroneReleaseDelay; // amount of time between drone releases
        public int RepairMaxSwarm; // max number of entities that can be in a single swarm
        public float HealthRegenRate; // how often health regen happens
        public float HealthRegenAmount; // how much health to regen
        public Light AlarmSearchLight; // light used on each of alarm system's laser muzzles
        public string SpawnCharacter; // spawn a character instead of this game object
        public float ShuffleRate;

        //**********************
        // FACTORIES
        public bool FactoryMessageOnCreate;
        public bool FactoryKillCreationsOnDestroy;
        public SpawnLocationType FactorySpawnLocation;
        public FactoryType FactoryType;
        
        //**********************
        // SPAWN LOCATION
        public SpawnLocationType SpawnLocationType;

        //**********************
        // PROJECTILES / WEAPONS / SHIELD
        public float ZapTTL = 1f;
        public bool RemoveOffscreen;
        public bool HiddenInLoadout;
        public bool DamageEnemies;
        public bool DamageSystems = true;
        public bool DamagePlayer;
        public bool DamageObjects;
        public float HitForce;  // force to push on impact
        public float KnockbackForce;
        public float KnockbackTime;
        public float RefillRate; // if it's shield, how quickly to refill. If ammo how quickly ammo is added
        public float NextRefillTime; // if ammo, when to start refilling again after firing. If shield, when to refill again after a hit
        public float RechargeRate; // for charge up weapons, like blaster
        public bool HasChargeSound;
        public int StartingAmmoBoxes = 1; // number of ammo boxes this weapon starts with
        public float AmmoBoxQuantity; // how much ammo is in each box, 0 for infinite
        public string AmmoName = "AMMUNITION"; // what a weapon's ammo type is called
        public float RateOfFire;
        public float DamagePerShot;
        public float DamageRate;
        public float DamageAreaRadius;
        public float ProjectileZapDamage;
        public bool ScaleDamage;
        public float DecalScale = 1;
        public float NoFiringTime;
        public float CrusherDamageOnContact;
        public float DamageOnContact; // amount of damage a character takes when touching this
        public float DamageOnContactForce;
        public float DamageOnContactNoAirControlTime;
        public bool StunnedContactDamage = true; // whether contact damage happens while stunned
        public float CameraShakeOnHit;
        public float ProjectileSpread;      // angle in radians between multiple projectiles
        public float ProjectilePrecision = 1;   // 1 = perfect accuracy, 0 = worst accuracy
        public int ProjectilesPerShot = 1;
        public float ProjectileRange; // for rayguns, the distance of the beam. For projectiles how far it can go before being removed
        public int ProjectileRicochetCount; // Number of times a projectile can bounce before being destroyed
        public float ProjectileSpeed; // speed of fire
        public float ProjectileBoostMaxSpeed; // if this projectile gains speed, like a rocket
        public float ProjectileBoostForce; // adds force in whatever direction it's moving
        public float ProjectileSpacing; // used to create a space between multiple projectiles fired at once
        public double ProjectileBoostStart;
        public float AmmoPerShot = 1;
        public bool RayGun;
        public bool AllowFriendlyFire;
        public bool ConeGun;
        public float ConeArcAngle;
        public float ConeRadius;
        public int ConeSegments;
        public string ConeEffectClassfile;
        public string ProjectileClassfile;
        public string FireSound;
        public string ReleaseSound;
        public float ReleaseSoundDelay;
        public float FireBoneRecoil = 7;
        public PayloadType PayloadType;
        public bool Payload;
        public bool PayloadTracksOwnerRotation;
        public double RemoveAfterStarted;  // if it should be removed after a certain amount of time
        public double MinRandRemoveAfterStarted;
        public double MaxRandRemoveAfterStarted;
        public float FireRecoil;
        public float ShakeCameraOnFire;
        public float ShakeCameraAddition;
        public float ShakeCameraAdditionLimit;
        public float ShakeCameraAdditionCoolDown;
        public float RecoilCameraOnFire;
        public float RecoilCameraOnFireDuration = 0.5f;
        public float ProjectileSineFrequency;
        public float ProjectileSineStrength = 1f;
        public bool ProjectileFlipOddSine;
        public float ProjectileTargetSeekRange;
        public bool ProjectileFollowTarget;
        public bool ProjectileFollowAim;
        public float ProjectileFollowTargetForce;
        public float ProjectileFollowTargetMaxRotate;
        public float ProjectileFollowTargetRotateDelay;
        public float ProjectileMinRandSpeed;
        public float ProjectileMaxRandSpeed;
        public bool ProjectilePiercing;
        public bool ProjectileIsCone;
        public bool ProjectileUseMuzzleAngle;
        public bool ProjectileUseMovementAngle;
        public float ProjectileRotationOffset;
        public float StunTime;
        public float ProjectileAngularSpeed;
        public bool ProjectileGrappleOnHit;
        public bool ProjectileTurnStaticOnAttach;
        public bool ProjectileAllowInfiniteLifetime;
        public bool NoShieldDamage;
        public bool FireFromEntCenter;
        public bool NoDebrisOnHit;
        public bool FullShield;

        public string ReloadSound;
        public string ReleaseFiringSound;
        public string StartFiringSound;
        public string FiringLoop;
        public string FiringLoopAfterStartFiringSound;

        public bool ProjectileRemoveOnCollision = true;
        public bool ProjectileRemoveOnBrushCollision;
        public bool ProjectileRemoveOnGameObjectCollision;
        public bool ProjectileRemoveOnCharacterCollision;
        public bool ProjectileRemoveOnShieldCollision;
        public bool ProjectileRemovesOtherProjectiles;
        public bool ProjectileRemoveOffscreen = true;
        public bool DontRemoveOtherProjectileOnCollision;

        public bool ProjectileAttachToCharacters;
        public bool ProjectileAttachToBrushes;
        public bool ProjectileAttachToSystems;
        public bool ProjectileAttachToMuzzle;
        public bool ProjectileAttachToDoors;
        public bool ProjectileDoesKnockback;
        public bool RemoveOnProjectileTargetFound;
        public float RemoveOnProjectileTargetFoundDelay;
        public float MinNewRandTargetDelay;
        public float MaxNewRandTargetDelay;
        public float RandTargetRange;
        public float FireNoiseRadius = 65;
        public float HitNoiseRadius;
        public int NumShotsPerReload;
        public int AutoFireCount; // number of times this weapon shoots before stopping
        public bool FireOnRelease;
        public float ReloadTime;
        public float RateOfFireAddition;
        public float RateOfFireAdditionLimit;
        public float RateOfFireAdditionCoolDown;
        public float InaccuracyAddition;
        public float InaccuracyAdditionLimit;
        public float InaccuracyAdditionCoolDown;
        public bool RemoveProjectilesOnSecondFire;
        public bool RecoverAmmoOnSecondFire;
        public bool RecoverAmmoOnInteract;
        public bool RecoverAmmoAfterAttach;
        public bool RecordProjectiles;
        public bool OnlyShootOnNewInput;
        public string AttackAnimation;
        public string AttackSound;
        public bool ScaleWithCharge;
        public bool StartTimeOnCharge;
        public float MeleeHitScale;
        public Vector2 MeleeHitScaleVector;
        public double MeleeStrikeTime;
        public string MeleeParticleEffect;
        public bool InvulnerableDuringStrike;
        public bool Melee;
        public bool ProjKeepVelocity;
        public bool ProjAllowSpin; // if it should rotate in air
        public Vector3 ProjectileColor = new Vector3(1f, 1f, 1f);
        public string OnFireBeamClassfile;
        public string OnGrappleBeamClassfile;
        public bool ReflectProjectiles;
        public bool IgnoreProjectileRefectTeamSwitch;
        public bool SwapProjectileTeamOnContact; // used by reflect shield to make reflected projectiles damage enemies
        public bool CreateFluidOnFire; // temporary fluid test -- used by a gun to create particles in the fluid simulation
        public float FluidForceMultiply = 1;
        public float WeaponChargeLightFalloff = 0.2f; // used to determine how quickly a weapon's charge light disappears (0 - 1, 1 being fastest)
        public bool ZapGun; // tells a weapon to create a 'lightning zap' instead of a projectile
        public int ZapChainAmount; // number of times a zap can chain to other enemies
        public float MaxChainAngle = 1f;
        public float AimSpeedModifier; // how much a weapon affects the entity's aim speed when shot
        public float MinAimSpeedFactor = 0.5f; // how low a weapon can make the entity's aim speed
        public ParticleType FluidType;
        public float FluidForce;
        public float DisableAfter; // how long before this object is disabled
        public float ForwardPushForceMult;
        public float BackwardPushForceMult;
        public bool GrappleOnFire;
        public bool GrappleOnLunge;
        public bool StopGrappleOnLungeEnd;
        public bool GrappleDamageOnSolidContact; // whether or not to damage a grappled enemy when they hit something solid
        public float GrappleDamage;
        public float GrappleRangeModifier;
        public float MaxGrappleRange = 1000f;
        public float MinGrappleRange = 2f;
        public bool ThrowGrappled;
        public float ThrowForce;
        public bool SwapTeamDuringGrapple;
        public string ShieldFollowBone;
        public List<string> WeaponMeshEffects;
        public bool RemoveEarlierProjectiles; // for recorded projectiles, remove any projectile created earlier than this one when this one is removed
        public float EarlierProjectileMaxDistance; // for recorded projectiles, the maximum distance an earlier projectile can be from later projectiles before being removed
        public float LaserGrowth = 0.3f;
        public float TeleportFragRange;
        public bool SetToProjectile;
        public float HealAmount;
        public bool MarkTarget;

        //**********************
        // WEAPON STATS
        public float SetAccuracyStat = -1f;
        public float SetDamageStat = -1f;
        public float SetRangeStat = -1f;
        public float SetFireRateStat = -1f;
        public float SetSpeedStat = -1f;
        public float SetNoiseStat = -1f;
        public bool OverrideShowAccuracyStat;
        public bool OverrideShowDamageStat;
        public bool OverrideShowRangeStat;
        public bool OverrideShowFireRateStat;
        public bool OverrideShowSpeedStat;
        public bool ShowAccuracyStat;
        public bool ShowDamageStat;
        public bool ShowRangeStat;
        public bool ShowFireRateStat;
        public bool ShowSpeedStat;

        //**********************
        // CHARGING
        public float MaxCharge;
        public float MinimumCharge;
        public bool ReleaseOnMaxCharge;

        //**********************
        // HAZARDS
        public float ZapFlailDelay; // base amount of time between applying zap cable's flail force
        public float ZapFlailRandomMin; // minimum amount of random time to add to flail delay
        public float ZapFlailRandomMax; // maximum amount
        public float ZapFlailForceMin; // minimum amount of random flail force
        public float ZapFlailForceMax; // maximum amount
        public float ZapFlailOutwardForce; // how hard the zap tail is pushed outward
        public float MineTrackRange; // how close a mine needs to be before tracking a target
        public float MineTrackSpeed; // multiplies the force that moves a mine towards a target
        public float MineTriggerRange; // how close the mine gets before triggering detonation
        public float MineDetonateRange; // how close the mine tries to get
        public float MineDetonateDelay; // how long to wait after being in range of target to detonate
        public float SprayerRate; // how often to spray
        public int SprayerParticlesPerSpray; // how many particles to create per spray
        public int SprayerTotalParticles; // how many total particles to spray (0 for infinity)
        public float SprayerForce; // multiplies with the object's rotation direction to come up with liquid force
        public float TimerOnCodeComplete; // sets an objects timer to time + this value when interaction code is complete
        public bool ForceTakeDamage;
        public float CageRadius;
        public float CageThickness;
        public int CageSegments;
        public string CageShield;
        public float CloseDuration;
        public float OpenDuration;
        public float BeeHiveRadius;
        public int BeeHiveSpawnCount = 1;
        public float SlowExplosionSparkCountMod = 0.5f;

        //**********************
        // DEBRIS
        public List<string> SmallerDebris;
        public int NumSmallerDebris;
        public float MinDebrisSpawnBuffer;
        public float MaxDebrisSpawnBuffer;
        public float MinDebrisSpawnForce;
        public float MaxDebrisSpawnForce;
        public List<string> DebrisToSpawn;

        //**********************
        // LIQUIDS
        public Vector2 LiquidAreaSize;
        public int LiquidAmount;

        //**********************
        // CASING
        public string CasingClassfile;
        public float CasingEjectSpeed;
        public float CasingEjectAngle;
        public float CasingEjectSpread;

        //**********************
        // LEVEL
        public float Health; // if set, how much damage until it's removed
        public bool HideHealth;

        //**********************
        // WEAPON DRAWING
        public string SpineWeaponTextureName;
        public string SpineWeaponHeadTextureName;
        public string WeaponAnimation;

        //**********************
        // ITEMS
        public ItemType ItemType;
        public int StartItemQuantity;
        public int InstantHealAmount;
        public float RateOfUse; // how quickly they can use another item in the slot
        public string OnUseSound;
        public bool PassiveItem;
        public List<PowerupEffectClass> PowerupEffects;
        public bool TwoPlayerPowerup;
        public bool UseInSpace;
        public bool ItemRecharges;
        public float DepleteChargeMult = 1f;
        public bool InfiniteItemQuantity;
        public bool DrawChargeBar;

        //**********************
        // SPAWNING
        public string SpawnOnItemUse;
        public string SpawnCharacterOnItemUse;
        public string SpawnObjectOnRemove;
        public string SpawnObjectOnAdd;
        public string SpawnObjectOnDamaged;
        public string CustomObjectSpawn;
        public string SpawnAtBone;
        public float SpawnForce;
        public float SpawnForceRotationOffset;
        public float MinSpawnRandAngularVel;
        public float MaxSpawnRandAngularVel;
        public string BoneToFollowSpawn;
        public bool RemoveOwnerOnRemove;
        public string SetSpineTrackOnSpawn;
        public int SetSpineTrackNumberOnSpawn;
        public int SquadCount = 1;
        public string SquadClass = "Juggernaut";
        public string SquadClassDisplay;

        //**********************
        // AUDIO
        public string AmbientSound;
        public double AmbientSoundStart;
        public string AmbientSoundStartSound;
        public bool AmbientSound_AmbientSoundbank;
        public string OnRemoveSound;
        public string OnAddSound;

        //**********************
        // AI
        public float AIFiringTime; // how long a enemy can use this weapon for
        public float AINextFiringTime; // how long the enemy waits to use the weapon again after it's use
        public float AIMinRandNextFiringTime;
        public float AIMaxRandNextFiringTime;
        public string Weapon1;
        public string Weapon2;
        public string Weapon3;
        public string Weapon4;
        public double StationaryAfterFiring;
        public double NoAimAfterFiring;
        public double MovementMultiplyAfterFiringTime;
        public float MovementMultiplyAfterFiringAmount;
        public float RetreatAfterFiringTime;

        //**********************
        // BEAMS
        public string AimingBeamClassfile;
        public Vector4 BeamColor;
        public string BeamStartParticleEffect;
        public string BeamEndParticleEffect;

        //**********************
        // LIGHTS
        public Light Light;
        public bool OnFireProjectileLightLimit;
        public Light OnFireProjectileLight;
        public Light OnFireMuzzleLight;
        public Light OnExplodeLight;
        public Light OnWeaponChargeLight;
        public Light OnRemoveLight;
        public Light RaygunImpactLight;

        //**********************
        // EXPLOSIONS
        public bool ExplodeOnRemove; // do explosion on removal
        public float ExplodeForce;
        public float ExplodeDamage;
        public float ExplodeSize;
        public float ExplodeDamageObjectMultiply = 1;
        public string ExplosionSound;
        public float ExplosionSoundDistance = 50;
        public string ExplosionRefraction; // refraction classfile
        public bool DamagesOwner;
        public bool SimpleExplosion; // bypasses explosion logic and just checks entity distances
        public float ExplosionGrowRate; // used by slow explosions to grow the radius
        public float ExplosionMaxRadius = float.PositiveInfinity; // used to limit the slow explosion's radius
        public float SlowExplosionDamageRate;

        //**********************
        // VORTICES
        public float VortexForce; // amount of force to apply to bodies when within range of a vortex
        public float VortexForceModifier; // amount to modify the vortex force by every frame
        public float VortexRadius; // starting radius of a vortex
        public float VortexRadiusModifier; // amount to modify the vortex radius every frame
        public bool VortexTeleportOnRemove; // whether or not to teleport collected entities on removal

        //**********************
        // PARTICLES
        public float ParticleOffsetX;
        public float ParticleOffset;
        public string ParticleEffect;
        public string ParticleEffectBone;
        public bool RotateParticleEffect;
        public float RotateParticleEffectOffset;
        public string RemoveParticleEffect;
        public string WeaponTrailEffect;
        public string MuzzleFlashEffect;
        public bool NoMuzzleFlashRotation;
        public string BreakParticleEffect;
        public string ExplosionParticleEffect;
        public string GibParticleEffect;
        public string RaygunImpactParticleEffect;
        public int ParticleEffectAmount = 1;
        public bool ParticleLocal;

        //**********************
        // DECALS
        public DecalType DecalType = DecalType.Bullet;

        //**********************
        // GIBS
        public bool GibOnDeath;
        public int DebrisAmount = 1;
        public Vector2 DebrisRandomOffset;
        public bool RandomDebrisDirection;
        public float DebrisMinRandForceMod = 1f;
        public float DebrisMaxRandForceMod = 1f;
        public bool DisableGibSlotOnRemove;

        //**********************
        // DOORS
        public bool CoreDoor;
        public bool OutsideDoor;
        public string InteractSound;

        //**********************
        // STATIONS
        public StationType StationType;
        public CoreType CoreType;
    }

    public class LevelSectionClass
    {
        public string Class;
        public List<string> LevelNames;
        public bool OverRide;
        public bool Tutorial;
    }

    public enum ItemType
    {
        None,
        Basic,
        Health,
        Invis,
        Key,
        DemoBomb,
        DemoBombRemote,
        Drone,
        Teleporter,
        AlarmTrigger,
        KnockBack,
        SensorSuite,
        EmpBomb,
        Shield,
        Blink,
        Slowmo,
    }

    [Flags]
    public enum FactoryType
    {
        None,
        Drone,
        Juggernaut,
        Sentry,
    }

    [Flags]
    public enum StationType
    {
        AMMO = 0,
        HEALTH = 1,
    }

    public enum CoreType
    {
        MAIN,
        HEART,
        CHOIR,
    }

    [Flags]
    public enum SpawnLocationType
    {
        NONE = 0,
        ROOM = 1,
        ON_WALL = 2 << 1,
        SENTRY = 2 << 2,
        TRIPWIRE = 2 << 3,
        ENEMY = 2 << 4,
        LARGE_ENEMY = 2 << 5,
        CORE = 2 << 6,
        SYSTEM = 2 << 7,
        OUTSIDE_WALL = 2 << 8,
    }

    [Flags]
    public enum DecalType
    {
        None,
        Bullet,
        Bomb,
        Flame,
    }

    // Various properties based on type. Such as how to update, how to draw,
    public enum GameObjectType
    {
        NONE,
        WEAPON,
        ITEM,
        GIB,
        LEVELSECTION,
        CORE,
        SYSTEM_FACTORY,
        SYSTEM_DOOR,
        SYSTEM_ALARM,
        SYSTEM_SENTRY,
        SYSTEM_SHIELD,
        SYSTEM_JAMMER,
        SYSTEM_REPAIR,
        SYSTEM_DRONEHEALTH,
        SYSTEM_DRONEREPAIR,
        SYSTEM_FLAKCANNON,
        SYSTEM_DESTRUCT,
        SYSTEM_FAILSAFE,
        SYSTEM_TANKER,
        SYSTEM_SHUFFLE,
        SYSTEM_HAZARD,
        SYSTEM_FACTORY_ADVANCED,
        FORCE_SHIELD,
        SPAWN_LOCATION,
        TRIPWIRE,
        FIREHAZARD,
        TRIGGER,
        WAYPOINT,
        INVISIBLEWALL,
        CARRY_ITEM,
        DOOR,
        DESTROYABLE,
        STATION,
        ARMOURY,
        BASIC,
        SHIELD,
        SHIELD_FOR_SYSTEMS,
        TERMINAL,
        BEAM,
        CASING,
        SYSTEM_CHILD,
        REFRACTION,
        VORTEX,
        CHARACTER_PROP,
        DOOR_CODE_INTERACTION,
        ZAPCABLE,
        ZAPCABLE_SEGMENT,
        HAZARD_POD,
        HAZARD_MINE,
        TARGET,
        FLUID_SPRAYER,
        DEMO_BOMB,
        DEMO_BOMB_REMOTE,
        LIGHT,
        LIQUID,
        SLOW_EXPLOSION,
        TELEPORTER,
        ALARMTRIGGER,
        SAW,
        EMP,
        WEAPON_ANIMATION,
        KNOCKBACK,
        TECH_PICKUP,
        WARP_SPAWNER,
        ALARMED_EFFECTS,
        TRANSLOCATOR,
        STASIS_ARM,
        EMP_GENERATOR,
        BUMPER,
        INCINERATOR_WHEEL,
        INCINERATOR_FORK,
        INCINERATOR_STATIONARY,
        CAGE_GENERATOR,
        CAGE_GENERATOR_SHIELD,
        SAWBLADE_SINGLE,
        SAWBLADE_TRIPLE,
        SAWBLADE_WALLARM,
        SAWBLADE_TRIANGLE,
        SAWBLADE_BOTTLENECK,
        SAWBLADE_CHILD,
        CRUSHER_SINGLE,
        CRUSHER_TRIPLE,
        CRUSHER_CHILD,
        DEBRIS,
        SPIKEWALL,
        SPIKES,
        FAN_DOODAD,
        ARM_DOODAD,
        CORE_DOODAD,
        VERLET_TAIL,
        PLAYERSHIP_DOODAD,
        PLAYERSHIP_PLAYER,
        BEE_HIVE,
        MULE_SKIFF,
        MULE_SKIFF_CHILD,
        ARTIFACT_POD,
        ALIEN_POD,
    }

    public enum LoadoutCategory
    {
        None,
        Melee,
        Test,
        Item,
        Machineguns,
        Bombs,
        Shotguns,
        Cannons,
        Rockets,
        Projectors,
        Shields,
        Energy,
    }

    public enum PayloadType
    {
        PROJECTILE_FIRE,
        OBJECT_SPAWN,
        EXPLOSION
    }

    public enum InteractionCode
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    [Serializable]
    public class GameObject : Entity
    {
        // SERIALIZED FIELDS
        public bool StartOnWall;
        public bool Respawn;
        public bool Reset;
        public string Classfile;
        public bool FlippedHorizontal, FlipVertical;
        public string LightColor;
        public List<long> WaypointIDList;
        public string Trigger;
        public string Name;
        public string DoorID;
        public bool On;
        public bool On_EditorSet;
        public string TextureName;
        public bool UsedSaved;
        public bool Projectile;
        public int ProjectileCurrentRicochetCount;
        public List<Entity> ProjectileCollidedList;
        public bool ActivatePayloadAltPos;
        public Vector2 AltPayloadPos;
        public Vector2 ProjectileStartVelocity;
        public float ProjectileSineCurrent;
        public float ProjectileNextFollowTargetRotate;
        public float ProjectileDamageMultiplier = 1f;
        public double RemoveAtTime;
        public bool BeamEnabled;
        public float NextNewRandTarget;
        public Vector2 CurrentRandTarget;
        public bool DrawShield;
        public Entity WarpEntityToSpawn;
        public List<Entity> WarpTeleportEntities;
        public GameObject WarpTeleportLocation;
        public bool WarpPositionOnWall;
        public int WarpEntitySeed;
        public float WarpSpawnerScale = 1f;
        public bool DisableSpawnOnRemove;
        public Color OriginalBeamColor;
        public bool SpawnOnDamagedUsed;
        public bool Fixed;
        public bool Fixed_EditorSet;
        public Light Light;

        internal bool EnemyAIWaypointClosed;
        internal bool PlayerWaypointClosed;

        // NON SERIALIZED FIELDS
        internal bool Locked;
        internal bool LockedAtStart;
        internal bool Sealed;
        internal bool FollowBoneRotation;

        internal Bone VerletPinBone;
        internal float VerletSinOffset;

        internal List<Entity> NukeAffectedEnts;
        internal bool ProjectileRemoveOnCollision;
        internal float Scale = 1;
        internal Body[] ChildBodies;
        internal InventoryItem ProjectileWeapon;
        //internal bool ForceShieldDraw;
        internal bool WarpObjectSpawned;
        internal GameObject AlarmedByEffects;
        internal float ProjectileSpeed;
        internal bool IsSpraying;
        internal float RefractionAlpha = 1f;
        internal float RefractionAlphaMod;
        internal bool ForceKill;
        internal bool DisableExplodeOnRemove;
        internal float DeathAnimFinishTime;
        internal bool IsSlotColorDirty = true;
        internal string[] BoneTargets;
        internal int BoneTargetIndex = -1;

        internal float NextHitWallEffect;
        internal float NextDamage;

        internal float AmbientZone_In = 1;
        internal float AmbientZone_Out = 0.4f;

        internal float DisableShieldAt;
        internal float ShieldShineAt;
        internal bool ShieldDeactivating;
        internal float DisableEffectsAt;

        internal float CurrentInteractOutlineAlpha;
        internal float TargetInteractOutlineAlpha;

        internal double RepairedPercentage;

        internal bool Broken;
        internal List<GameObject> WaypointList;
        internal float WaypointTempDist;

        internal Character LastAttacker;

        internal GameObjectClass Class;
        internal Texture2D CollisionTexture;

        internal Vector2 Origin;
        internal List<Vertices> VertexArray;

        internal Cue ProjectileSound;

        internal double NextSystemAbility;
        internal int NextSystemAbilityInt;
        internal GameObject LastRepairing;
        internal float NextAbility;
        internal float SawbladeSpinSpeed;

        internal bool IsOpen;
        internal float NextFire;

        internal double NextAlarm;
        internal bool Used;
        internal int UsedAmount;
        internal List<Entity> LocationEntities;
        internal bool CrateContentsSeen;
        internal bool CageOpening;
        internal bool CageClosing;
        internal bool CageActive;
        internal bool CrusherPistonEnabled;

        internal Vector2 LastWeaponTrailParticlePosition = Vector2.Zero;
        internal Entity ProjectileTarget;
        
        internal bool RotatingWallObject;

        internal bool HasBeenPickedUp;

        internal float FadeOutStart;
        internal float FadeOutEnd;

        internal Vector2 TripwireEnd;

        internal Slot GibSlot;
        internal List<Vector2> GibParentOffsets;

        internal Entity BoneOwner;
        internal Bone BoneToFollow;
        internal float FollowBoneRotationOffset;
        internal Bone BoneFollowingSelf;
        internal bool BoneFollowingSelfRotation;
        internal float BoneFollowingSelfRotationOffset;

        internal ParticleEffect ParticleEffect;

        internal string ArtifactPickup;

        internal Body BodyToFollow;
        internal Vector2 BodyToFollowLocal; // the local offset to be used when following a body

        internal Entity EntToFollow;

        internal float NextHealthRegen;
        internal bool HitShield;
        internal float VortexForce;
        internal float VortexRadius;
        internal List<Entity> VortexTeleportEntities;
        internal InteractionCode[] InteractionCodes;
        internal int InteractionCodeProgress;
        internal float NextZapFlail;
        internal float NextDebrisEffect;
        internal float NextMineDetonate;
        internal float NextFluidSpray;
        internal float NextHazardFire;
        internal float NextHazardFireReload;
        internal int SprayerCurrentParticleCount;
        internal int SprayerTotalParticleCount;
        internal float Timer;
        internal float NextTimerSound;
        internal bool TimerSet;
        internal Vector2 ImpactNormal; // projectile impact normal stored for OnRemoveLight's direction
        internal Vector2 ImpactPosition; // projectile impact position (world coordinates)
        internal Entity ImpactEntity;
        internal float SlowExplosionRadius;
        internal float NextSlowExplosionDamage; // next time a slow explosion can cause damage
        internal float NextSlowExplosionSpark;
        internal bool IsFailsafe; // used to tell whether or not a system was created by the failsafe
        internal List<MeshEffect> MeshEffects;
        internal string TechPickup;
        internal float TechPickupAmmo;
        internal int TechPickupItemQuantity;
        internal float TechPickupFullAmmo;
        internal string[] MuzzleNames;
        internal bool CanSeeTarget;
        internal VerletPoint[] VerletPoints;
        //internal Body VerletPinBody;

        // System-specific AI
        internal string CoreTurretAnimation;
        internal float NextCoreTurretSwitch;
        internal string[] CoreMuzzleNames;
        internal bool CoreCanFire;
        internal float CoreStopFiringAt;
        internal bool CoreLevel1FireSwitch;
        internal float NextAlarmRotation;
        internal bool AlarmCanFire;
        internal bool AlarmCanSee;
        internal string[] AlarmMuzzleNames;
        internal bool AlarmCanRotate;
        internal float AlarmCurrentRotation;
        internal float AlarmPreventCloseUntil;
        internal bool AlarmIsOpen;
        internal string[] FactoryMuzzleNames;
        internal bool FactoryCanFire;
        internal List<SpawnType> FactorySpawnOptions;
        internal SpawnType FactoryNextSpawnCharacter;
        internal GameObject FactorySpawnProp;
        internal string FactorySpawnPropAnimation;
        internal string[] DoorInteractBoneNames;
        internal int DoorSystemDoorId;
        internal bool Door1Locked = true;
        internal bool Door2Locked = true;
        internal bool Door3Locked = true;
        internal bool DoorSystemFailure;
        internal bool SentryCanSee;
        internal bool SentryIsOpen;
        internal bool SentryTrackingAllowed;
        internal float SentryCurrentRotation;
        internal float SentryStartChargingAt;
        internal float SentryCloseSound;
        internal bool SentryIsCharging;
        internal bool SentryIsFiring;
        internal float SentryStopFiringAt;
        internal Vector2 SentryLastTargetPos = new Vector2(1, 0);
        internal SwarmGroup RepairCurrentGroup;
        internal SwarmGroup HiveCurrentGroup;
        internal bool RepairFireDrone;
        internal int RepairSpawnCount;
        internal string[] RepairMuzzleNames;
        internal int RepairCurrentMuzzleIndex;
        internal bool HealthRegenEnabled;
        internal float DroneRepairNextRegen;
        internal GameObject ShieldedByPreference;
        internal GameObject AlarmByPreference;
        internal float NextShuffle;
        internal bool Shuffled;

        public GameObject()
        {
            Type = EntityType.OBJECT;
            Layer = Layer.GAME;
        }
    }

    [Serializable]
    public class PowerupEffectClass
    {
        public PowerupEffectType Type;
        public float Duration;
        public bool RemoveOnFire;
        public bool OverwritesSameType = true;
        public string PowerupTextureName;
        public bool HudDraw = true;
        public float HealthRegenAmount;
        public float HealthRegenRate;
        public float Amount;

        internal TextureBounds PowerupTexture;
    }

    public enum PowerupEffectType
    {
        None,
        Invisible,
        HealthRegen,
        RateOfFireMod,
        DamageMod,
        PrecisionMod,
        ProjectileSpeedMod,
        MovementSpeedMod,
        DensityMod,
        ResistFire,
        ResistSlime,
        KeyToHealth,
        Marked,
    }

    public class PowerupEffect
    {
        public PowerupEffectClass Class;
        public float TimeToExpire;
        public bool HasDuration;

        public float NextHealthRegen;
    }

    [Serializable]
    public class TailEffect
    {
        public string BoneName;
        public string TailClass;
        public float SinOffset;
    }
}
