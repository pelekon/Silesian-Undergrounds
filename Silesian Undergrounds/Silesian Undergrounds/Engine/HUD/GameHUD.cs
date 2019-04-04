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
using Silesian_Undergrounds.Engine.Common;

namespace Silesian_Undergrounds.Engine.HUD {
    public class GameHUD {

        SpriteFont font;
        Texture2D coalImage;
        Texture2D keyImage;
        Color tintColor = Color.White;
        private int size;

        public GameHUD(int size)
        {
            this.size = size / 2;
        }

        public void Load(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/font");
            coalImage = content.Load<Texture2D>("Items/Ores/gold/gold_1");
            keyImage = content.Load<Texture2D>("Items/Keys/key_1");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null);

            string moneyString = " : " + Player.moneyAmount.ToString() + " $";

            spriteBatch.DrawString(font, moneyString, Vector2.Zero + new Vector2(this.size, this.size / 4), tintColor);
            spriteBatch.Draw(coalImage, new Rectangle((int)Vector2.Zero.X, (int)Vector2.Zero.Y + this.size / 4, this.size, this.size), tintColor);
            // draw keys counter
            //  measure font which was drawn sd
            Vector2 moneyStringSize = font.MeasureString(moneyString);

            spriteBatch.DrawString(font, " : " + Player.keyAmount.ToString(), new Vector2(this.size + coalImage.Width, this.size / 4), tintColor);
            spriteBatch.Draw(keyImage, new Rectangle(coalImage.Width + (int)moneyStringSize.Y, (int)Vector2.Zero.Y + this.size / 4, this.size, this.size), tintColor);

            spriteBatch.End();
        }
    }
}
