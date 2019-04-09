using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Silesian_Undergrounds.Engine.Common;

namespace Silesian_Undergrounds.Engine.Components
{
    public interface IComponent
    {
        Vector2 position { get; set; }
        Rectangle rect { get; set; }
        GameObject parent { get; set; }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch batch);

        GameObject GetParent();
    }
}
