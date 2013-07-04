using System;
using MonoTouch.UIKit;

namespace Gistacular.Elements
{
    public class TrueFalseElement : StyledElement
    {
        bool _val;

        public new bool Value
        {
            get { return _val; }
            set
            {
                Accessory = value ? MonoTouch.UIKit.UITableViewCellAccessory.Checkmark : MonoTouch.UIKit.UITableViewCellAccessory.None;
                _val = value;

                if (GetImmediateRootElement() != null)
                    GetImmediateRootElement().Reload(this, UITableViewRowAnimation.None);
            }
        }

        public TrueFalseElement(string title)
            : base(title)
        {
            Font = UIFont.BoldSystemFontOfSize(14f);
            Tapped += () => {
                Value = !Value;
            };
        }
    }
}

