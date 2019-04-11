using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Silesian_Undergrounds.Engine.Common;

namespace Silesian_Undergrounds.Engine.Components
{
    public interface IComponent
    {
        Vector2 Position { get; set; }
        Rectangle Rect { get; set; }
        GameObject Parent { get; }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch batch);

        void RegisterSelf();
        void UnRegisterSelf();
    }
}
