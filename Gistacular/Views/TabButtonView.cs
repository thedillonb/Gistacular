using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace Gistacular.Views
{
    public class TabButtonView : CellBackgroundView
    {
        protected UISegmentedControl _segment;
        public Action<int> SegmentChanged;

        public int Selected
        {
            get { return _segment.SelectedSegment; }
        }

        public TabButtonView(RectangleF frame, params string[] tabs)
        {
            this.Frame = frame;

            _segment = new UISegmentedControl(tabs);
            _segment.ControlStyle = UISegmentedControlStyle.Bar;
            _segment.SelectedSegment = 0;
            _segment.AutosizesSubviews = true;
            _segment.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            _segment.Frame = this.Frame;

            //Themeing
            var textAttrs = new UITextAttributes { TextColor = UIColor.FromRGB(122, 122, 122), TextShadowColor = UIColor.White, TextShadowOffset = new UIOffset(0, 1) };
            _segment.SetTitleTextAttributes(textAttrs, UIControlState.Normal);
            //var textAttrsHighlighted = new UITextAttributes { TextColor = UIColor.White, TextShadowColor = UIColor.FromRGB(122, 122, 122), TextShadowOffset = new UIOffset(0, 1) };
            //_segment.SetTitleTextAttributes(textAttrsHighlighted, UIControlState.Highlighted);

            _segment.SetDividerImage(Images.Components.TabsVertical, UIControlState.Normal, UIControlState.Normal, UIBarMetrics.Default);
            _segment.SetBackgroundImage(Images.Components.TabsBackground, UIControlState.Normal, UIBarMetrics.Default);
            _segment.SetBackgroundImage(Images.Components.TabsHighlighted, UIControlState.Selected, UIBarMetrics.Default);

            AddSubview(_segment);

            _segment.ValueChanged += (sender, e) => { 
                if (SegmentChanged != null)
                    SegmentChanged(_segment.SelectedSegment);
            };

//            //Fucking bug in the divider
//            BeginInvokeOnMainThread(delegate {
//                _segment.SelectedSegment = 1;
//                _segment.SelectedSegment = 0;
//                _segment.SelectedSegment = MonoTouch.Utilities.Defaults.IntForKey(MultipleSelectionsKey);
//                Title = GetTitle(_segment.SelectedSegment);
//                
//            });
        }
    }
}

