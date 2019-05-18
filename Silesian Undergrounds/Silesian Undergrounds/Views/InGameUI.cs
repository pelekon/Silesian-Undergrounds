using Silesian_Undergrounds.Engine.UI;
using Silesian_Undergrounds.Engine.UI.Controls;
using Silesian_Undergrounds.Engine.Common;

namespace Silesian_Undergrounds.Views
{
    public class InGameUI : UIArea
    {
        private Label moneyLabel;
        private Label keyLabel;
        private Label hungerLabel;
        private Label liveLabel;
        private Label maxLiveValueLabel;
        private Label maxHungerValueLabel;

        public InGameUI(Player plr) : base()
        {
            plr.MoneyChangeEvent += Player_MoneyChangeEvent;
            plr.KeyChangeEvent += Player_KeyChangeEvent;
            plr.HungerChangeEvent += Player_HungerChangeEvent;
            plr.LiveChangeEvent += Player_LiveChangeEvent;

            plr.LiveMaxValueChangeEvent += Player_LiveMaxValueChangeEvent;
            plr.HungerMaxValueChangeEvent += Player_HungerMaxValueChangeEvent;

            moneyLabel.Text = plr.MoneyAmount.ToString() + " $";
            keyLabel.Text = plr.KeyAmount.ToString();
            hungerLabel.Text = plr.HungerValue.ToString() + " / ";
            liveLabel.Text = plr.LiveValue.ToString() + " / ";

            maxLiveValueLabel.Text = plr.MaxLiveValue.ToString();
            maxHungerValueLabel.Text = plr.MaxHungerValue.ToString();
        }

        private void Player_MoneyChangeEvent(object sender, PropertyChangedArgs<int> e)
        {
            moneyLabel.Text = e.NewValue.ToString() + " $";
        }

        private void Player_KeyChangeEvent(object sender, PropertyChangedArgs<int> e)
        {
            keyLabel.Text = e.NewValue.ToString();
        }

        private void Player_HungerChangeEvent(object sender, PropertyChangedArgs<int> e)
        {
            hungerLabel.Text = e.NewValue.ToString() + " / ";
        }

        private void Player_LiveChangeEvent(object sender, PropertyChangedArgs<int> e)
        {
            liveLabel.Text = e.NewValue.ToString() + " / ";
        }

        private void Player_HungerMaxValueChangeEvent(object sender, PropertyChangedArgs<int> e)
        {
            maxHungerValueLabel.Text = e.NewValue.ToString();
        }

        private void Player_LiveMaxValueChangeEvent(object sender, PropertyChangedArgs<int> e)
        {
            maxLiveValueLabel.Text = e.NewValue.ToString();
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

            liveLabel = new Label("100 / ", 3, 5f, 5, 5, this);
            liveLabel.SetTextAlign(TextAlign.ALIGN_LEFT);
            liveLabel.SetBackground(null);

            hungerLabel = new Label("100 / ", 3, 9.5f, 5, 5, this);
            hungerLabel.SetTextAlign(TextAlign.ALIGN_LEFT);
            hungerLabel.SetBackground(null);

            maxLiveValueLabel = new Label("100", 6, 5f, 5, 5, this);
            maxLiveValueLabel.SetTextAlign(TextAlign.ALIGN_LEFT);
            maxLiveValueLabel.SetBackground(null);

            maxHungerValueLabel = new Label("100", 5.75f, 9.5f, 5, 5, this);
            maxHungerValueLabel.SetTextAlign(TextAlign.ALIGN_LEFT);
            maxHungerValueLabel.SetBackground(null);

            AddElement(new Image(1, 1, 2, 3, "Items/Ores/gold/gold_1", this));
            AddElement(moneyLabel);
            AddElement(new Image(8, 1, 2, 3, "Items/Keys/key_1", this));
            AddElement(keyLabel);
            AddElement(new Image(1, 6,2,3,"Items/Heart/heart_1",this));
            AddElement(liveLabel);
            AddElement(new Image(1, 11, 2, 3,"Items/Food/meat", this));
            AddElement(hungerLabel);
            AddElement(maxLiveValueLabel);
            AddElement(maxHungerValueLabel);
        }
    }
}
