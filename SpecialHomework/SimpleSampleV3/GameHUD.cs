using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SimpleSampleV3
{
    public class GameHUD
    {
        SpriteFont font;
        string text = "Score: ";
        Color tintColor = Color.White;

        public void Load(ContentManager content)
        {
            font = content.Load<SpriteFont>(@"Fonts/font");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Fixed position - not moving with the camera, but scaled with the resolution
            var transformMatrix = ResolutionManager.getTransformMatrix();
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, transformMatrix);
            spriteBatch.DrawString(font, text + Player.score.ToString(), Vector2.Zero, tintColor);
            spriteBatch.End();
        }
    }
}
