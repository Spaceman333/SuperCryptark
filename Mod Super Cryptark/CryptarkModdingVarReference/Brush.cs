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
using Medusa.Physics.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using ProjectMercury;
using TexturePolygonLib;




namespace Medusa
{
    [Serializable]
    public class Brush : Entity
    {
        public string TextureName;
        public bool Stretch;
        public bool Solid;
        public bool Shadow;
        public bool FlippedHorizontal, FlipVertical;
        public bool Additive;
        public bool Opaque;
        public float Scale = 1;
        public float LoopRotation = 0;
        public float Loop = 0;
        public float LoopY = 0;
        public string BrushColor;
        public bool NonVisible;
        public Vector2[] PolygonPoints;

        internal bool FirstDraw;
        internal int displayWidth;
        internal int displayHeight;
        internal Rectangle RecTiled;
        internal Rectangle RecDest;

        internal Vector2 TextureOffset;

        internal bool CharacterCollideOnly;

        internal Texture2D Texture;
        internal Texture2D TextureNormalMap;
        internal int BrushNum;
        internal float BrushLayerDepth;

        public Brush()
        {
            Type = EntityType.BRUSH;
            this.Layer = Layer.FOREGROUND;
        }

        public Brush(Layer Layer)
        {
            Type = EntityType.BRUSH;
            this.Layer = Layer;
        }

        public void SetTexturePolygon()
        {
            var bounds = new Vector2(ConvertUnits.ToDisplayUnits(Width) / 2 , ConvertUnits.ToDisplayUnits(Height) / 2 );
            TextureFixture = new TexturedFixture(Texture, PolygonPoints, bounds, -bounds);
        }
    }
}
