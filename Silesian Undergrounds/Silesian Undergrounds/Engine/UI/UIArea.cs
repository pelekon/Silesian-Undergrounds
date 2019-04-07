using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silesian_Undergrounds.Engine.UI
{
    public class UIArea : UIElement
    {
        private List<UIElement> _elements;
        
        public UIArea() : base(0, 0, 100, 100, null, null)
        {
            _elements = new List<UIElement>();
            Initialize();
        }

        protected virtual void Initialize() { }

        public void AddElement(UIElement element)
        {
            _elements.Add(element);
        }

        public void RemoveElement(UIElement element)
        {
            _elements.Remove(element);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var element in _elements)
                element.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (var element in _elements)
                element.Draw(batch);
        }
    }
}
