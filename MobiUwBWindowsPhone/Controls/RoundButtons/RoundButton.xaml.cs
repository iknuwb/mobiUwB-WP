#region

using System;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace MobiUwB.Controls.RoundButtons
{
    public partial class RoundButton : UserControl
    {
        public static readonly DependencyProperty ImageWidthHeightProperty =
        DependencyProperty.Register(
            "ImageWidthHeightProperty",
            typeof(Double),
            typeof(RoundButton),
            new PropertyMetadata(50d));

        public Double ImageWidthHeight
        {
            get
            {
                return (Double)GetValue(ImageWidthHeightProperty);
            }
            set
            {
                SetValue(ImageWidthHeightProperty, value);
            }
        }

        public static readonly DependencyProperty ImageMarginProperty =
        DependencyProperty.Register(
            "ImageMarginProperty",
            typeof(Thickness),
            typeof(RoundButton),
            new PropertyMetadata(new Thickness(5)));

        public Thickness ImageMargin
        {
            get { return (Thickness)GetValue(ImageMarginProperty); }
            set { SetValue(ImageMarginProperty, value); }
        }


        public static readonly DependencyProperty TextMarginProperty =
        DependencyProperty.Register(
            "TextMarginProperty",
            typeof(Thickness),
            typeof(RoundButton),
            new PropertyMetadata(new Thickness(5)));

        public Thickness TextMargin
        {
            get { return (Thickness)GetValue(TextMarginProperty); }
            set { SetValue(TextMarginProperty, value); }
        }


        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            "TextProperty",
            typeof(String),
            typeof(RoundButton),
            new PropertyMetadata(""));

        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public static readonly DependencyProperty ImageTextProperty =
        DependencyProperty.Register(
            "ImageTextProperty",
            typeof(String),
            typeof(RoundButton),
            new PropertyMetadata(""));

        public String ImageText
        {
            get { return (String)GetValue(ImageTextProperty); }
            set { SetValue(ImageTextProperty, value); }
        }

        public static readonly DependencyProperty ControlHeightProperty =
        DependencyProperty.Register(
            "ControlHeightProperty",
            typeof(Double),
            typeof(RoundButton),
            new PropertyMetadata(10d));

        public Double ControlHeight
        {
            get
            {
                return (Double)GetValue(ControlHeightProperty);
            }
            set
            {
                SetValue(ControlHeightProperty, value);
            }
        }

        public static readonly DependencyProperty DescriptionTextSizeProperty =
        DependencyProperty.Register(
            "DescriptionTextSizeProperty",
            typeof(Double),
            typeof(RoundButton),
            new PropertyMetadata(10d));

        public Double DescriptionTextSize
        {
            get
            {
                return (Double)GetValue(DescriptionTextSizeProperty);
            }
            set
            {
                SetValue(DescriptionTextSizeProperty, value);
            }
        }

        public RoundButton()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}
