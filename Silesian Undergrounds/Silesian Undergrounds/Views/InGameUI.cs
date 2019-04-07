using Silesian_Undergrounds.Engine.UI;
using Silesian_Undergrounds.Engine.UI.Controls;

namespace Silesian_Undergrounds.Views
{
    public class InGameUI : UIArea
    {
        protected override void Initialize()
        {
            base.Initialize();

            AddElement(new Image(1, 1, 2, 3, "Items/Ores/gold/gold_1", this));
            AddElement(new Label("0 $", 3, 2, 3, 1, this));
            AddElement(new Image(8, 1, 2, 3, "Items/Keys/key_1", this));
            AddElement(new Label("0", 10, 2, 3, 1, this));
        }
    }
}
