using Silesian_Undergrounds.Engine.UI;
using Silesian_Undergrounds.Engine.UI.Controls;
using Silesian_Undergrounds.Engine.Common;

namespace Silesian_Undergrounds.Views
{
    public class InGameUI : UIArea
    {
        private Label moneyLabel;
        private Label keyLabel;

        public InGameUI(Player plr) : base()
        {
            plr.MoneyChangeEvent += Player_MoneyChangeEvent;
            plr.KeyChangeEvent += Player_KeyChangeEvent;

            moneyLabel.Text = plr.MoneyAmount.ToString() + " $";
            keyLabel.Text = plr.KeyAmount.ToString();
        }

        private void Player_MoneyChangeEvent(object sender, PlayerPropertyChangedEvent<int> e)
        {
            moneyLabel.Text = e.NewValue.ToString() + " $";
        }

        private void Player_KeyChangeEvent(object sender, PlayerPropertyChangedEvent<int> e)
        {
            keyLabel.Text = e.NewValue.ToString();
        }

        protected override void Initialize()
        {
            base.Initialize();

            moneyLabel = new Label("0 $", 3, 1.5f, 3, 2, this);
            moneyLabel.SetTextAlign(TextAlign.ALIGN_LEFT);
            moneyLabel.SetBackground(null);
            keyLabel = new Label("0", 10, 1.5f, 3, 2, this);
            keyLabel.SetTextAlign(TextAlign.ALIGN_LEFT);
            keyLabel.SetBackground(null);

            AddElement(new Image(1, 1, 2, 3, "Items/Ores/gold/gold_1", this));
            AddElement(moneyLabel);
            AddElement(new Image(8, 1, 2, 3, "Items/Keys/key_1", this));
            AddElement(keyLabel);
        }
    }
}
