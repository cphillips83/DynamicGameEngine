using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Json;

namespace GameName1.Roguelike.Scripts
{
    public class JsonAnimation : SpriteAnimation
    {
        public string file;

        private void init()
        {
            //var jo = Root.instance.resources.findJson(file);
            //if (jo != null)
            //{
            //    this.fps = 5;
            //    textureName = "content/img/1/" + (string)jo["id"] + ".png";

            //    var textureRef = Root.instance.resources.loadTexture(textureName);
            //    var textureSize = new Vector2(textureRef.Width, textureRef.Height);

            //    this.offset = new Vector2((int)jo["offset_x"], (int)jo["offset_y"]);
            //    this.size = new Vector2((int)jo["width"], (int)jo["height"]);

            //    var normalizedSize = this.size / textureSize;
                
            //    //this.origin = Vector2.One - (this.size / this.offset);

            //    var animations = (JsonObject)jo["animations"];
            //    foreach (var _anim in animations)
            //    {
            //        var anim = (JsonObject)_anim.Value;
            //        var len = (int)anim["length"];
            //        var row = (int)anim["row"];
            //        var frames = new int[len];

            //        for (var i = 0; i < len; i++)
            //            frames[i] = this.addFrame(new Vector2(normalizedSize.X * i, normalizedSize.Y * row), size);

            //        this.addAnimation(_anim.Key, frames);
            //    }

            //    this.setAnimation("idle_down");
            //}
        }
    }
}
