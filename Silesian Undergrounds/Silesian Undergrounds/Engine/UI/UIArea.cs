using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Engine.UI
{
    public class UIArea
    {
        private List<UIElement> _elements;
        private float xAxisUnit = 1;
        private float yAxisUnit = 1;
        
        public UIArea()
        {
            _elements = new List<UIElement>();

            // Calculate inner unit to set elements positions in range 0-100
            yAxisUnit = ResolutionMgr.GameHeight / 100;
            xAxisUnit = ResolutionMgr.GameWidth / 100;
        }

        public virtual void Initialize() { }

        void AddElement(UIElement element)
        {
            _elements.Add(element);
        }

        void RemoveElement(UIElement element)
        {
            _elements.Remove(element);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var element in _elements)
                element.Update(gameTime);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin();

            foreach (var element in _elements)
                element.Draw(batch, xAxisUnit, yAxisUnit);

            batch.End();
        }
    }
}
