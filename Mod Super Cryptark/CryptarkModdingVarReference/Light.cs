using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Medusa
{
    public enum LightType
    {
        Point,
        Directional,
        Spotlight
    }

    [Serializable]
    public class Light
    {
        public LightType Type;
        public Vector3 Position = new Vector3(0f, 0f, 100f);
        public Vector4 Color = new Vector4(1f, 1f, 1f, 1f);
        public Vector4 SpecularColor = new Vector4(1f, 1f, 1f, 1f);
        public Vector3 Direction;
        public float Strength = 1f;
        public float MinStrength;
        public float MaxStrength;
        public float StrengthModifier;
        public float StrengthCycleScale = 1f;
        public bool OscillateStrength;
        public float Radius = 100f;
        public float MinRadius;
        public float MaxRadius = float.PositiveInfinity;
        public float RadiusCycleScale = 1f;
        public bool OscillateRadius;
        public bool RemoveOnZeroRadius = true;
        public bool RemoveOnZeroStrength;
        public float RadiusModifier;
        public string FollowBoneName;
        public bool FollowBoneRotation;
        public bool IsEnabled = true;
        public float InnerConeAngle = 0.3f;  // radians
        public float OuterConeAngle = 0.4f;  // radians
        public float Shininess = 100f;
        public bool FollowOwnRotation;
        public bool UseOwnTime;
        public bool DisableRandomTime;
        public float TimeOffset;
        public bool UseSpatialSystem = true;
        public float BoundingBoxUpdateRate = 0.5f;

        internal Entity FollowEntity;
        internal float RandTimeOffset;
        internal float Time;
        internal float StartingRadius;
        internal bool StartingValuesSet;
        internal Rectangle Rect;
        internal Vector3 CurrentDirection;

        internal Spine.Bone FollowBone;

        public Light()
        {
            RandTimeOffset = (float)MiscFunc.mRandom.NextDouble();
        }

        public Light(Light light)
        {
            Type = light.Type;
            Position = light.Position;
            Direction = light.Direction;
            Color = light.Color;
            SpecularColor = light.SpecularColor;
            Strength = light.Strength;
            MinStrength = light.MinStrength;
            MaxStrength = light.MaxStrength;
            StrengthModifier = light.StrengthModifier;
            StrengthCycleScale = light.StrengthCycleScale;
            OscillateStrength = light.OscillateStrength;
            Radius = light.Radius;
            MinRadius = light.MinRadius;
            MaxRadius = light.MaxRadius;
            RadiusCycleScale = light.RadiusCycleScale;
            OscillateRadius = light.OscillateRadius;
            IsEnabled = light.IsEnabled;
            RadiusModifier = light.RadiusModifier;
            InnerConeAngle = light.InnerConeAngle;
            OuterConeAngle = light.OuterConeAngle;
            Shininess = light.Shininess;
            FollowBoneName = light.FollowBoneName;
            FollowBoneRotation = light.FollowBoneRotation;
            FollowOwnRotation = light.FollowOwnRotation;
            RemoveOnZeroRadius = light.RemoveOnZeroRadius;
            RemoveOnZeroStrength = light.RemoveOnZeroStrength;
            UseOwnTime = light.UseOwnTime;
            DisableRandomTime = light.DisableRandomTime;
            TimeOffset = light.TimeOffset;
            UseSpatialSystem = light.UseSpatialSystem;
            BoundingBoxUpdateRate = light.BoundingBoxUpdateRate;
            RandTimeOffset = (float)MiscFunc.mRandom.NextDouble();
        }
    }
}
