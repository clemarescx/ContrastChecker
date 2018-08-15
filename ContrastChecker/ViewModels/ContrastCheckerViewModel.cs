using System;
using System.Windows.Media;
using ContrastChecker.Helpers;

namespace ContrastChecker.ViewModels
{
    public class ContrastCheckerViewModel : ViewModelBase
    {
        private const int ColorDifferenceThreshold = 500;
        private const int BrightnessDifferenceThreshold = 125;
        private string _backgroundColorString;
        private double _bgBrightness;
        private Color _bgColor;
        private double _fgBrightness;

        private Color _fgColor;
        private string _foregroundColorString;

        public ContrastCheckerViewModel()
        {
            PressEnterInTextBoxCommand = new DelegateCommand(PressEnterInTextBox);
            BackgroundColorString = "FF0000FF";
            ForegroundColorString = "FFFF69B4";
        }

        public DelegateCommand PressEnterInTextBoxCommand { get; }

        public string BackgroundColorString
        {
            get => _backgroundColorString;
            set
            {
                if (SetProperty(ref _backgroundColorString, value))
                {
                    CalculateValues();
                }
            }
        }

        public string ForegroundColorString
        {
            get => _foregroundColorString;
            set
            {
                if (SetProperty(ref _foregroundColorString, value))
                {
                    CalculateValues();
                }
            }
        }

        public Color BgColor
        {
            get
            {
                ColorUtilities.TryParseColorString(_backgroundColorString, out _bgColor);
                return _bgColor;
            }
        }

        public Color FgColor
        {
            get
            {
                ColorUtilities.TryParseColorString(_foregroundColorString, out _fgColor);
                return _fgColor;
            }
        }


        public double BackgroundBrightness
        {
            get => _bgBrightness;
            set => SetProperty(ref _bgBrightness, value);
        }

        public double ForegroundBrightness
        {
            get => _fgBrightness;
            set => SetProperty(ref _fgBrightness, value);
        }

        public double ColorDifference => CalculateColorDifference(FgColor, BgColor);

        public double BrightnessDifference => Math.Abs(BackgroundBrightness - ForegroundBrightness);


        public bool ColorDifferenceIsAcceptable => ColorDifference > ColorDifferenceThreshold;

        public bool BrightnessDifferenceIsAcceptable => BrightnessDifference > BrightnessDifferenceThreshold;

        public bool ContrastRatioIsAcceptable => ContrastRatio > ContrastRatioAcceptanceThreshold;
        public static double ContrastRatioAcceptanceThreshold => 4.5;


        public double ContrastRatio => CalculateContrastRatio(RelativeLuminance(BgColor), RelativeLuminance(FgColor));

        private void PressEnterInTextBox()
        {
            Console.WriteLine("ENTER PRESSED!!");
            CalculateValues();
        }

        private void CalculateValues()
        {
            BackgroundBrightness = Brightness(BgColor);
            ForegroundBrightness = Brightness(FgColor);
            NotifyPropertyChanged(nameof(ColorDifference));
            NotifyPropertyChanged(nameof(BrightnessDifference));
            NotifyPropertyChanged(nameof(ContrastRatio));
            NotifyPropertyChanged(nameof(ColorDifferenceIsAcceptable));
            NotifyPropertyChanged(nameof(BrightnessDifferenceIsAcceptable));
            NotifyPropertyChanged(nameof(ContrastRatioIsAcceptable));
        }

        private double CalculateColorDifference(Color c1, Color c2) => Math.Abs(c1.R - c2.R) +
                                                                       Math.Abs(c1.G - c2.G) +
                                                                       Math.Abs(c1.B - c2.B);

        private double Brightness(Color c) => c.R * 0.299 + c.G * 0.587 + c.B * 0.114;

        //the darkest color is the divisor
        private static double CalculateContrastRatio(double l1, double l2) =>
            l1 > l2 ? (l1 + 0.05) / (l2 + 0.05) : (l2 + 0.05) / (l1 + 0.05);

        private static double RelativeLuminance(Color c) => 0.2126 * R(c) + 0.7152 * G(c) + 0.0722 * B(c);

        private static double R(Color c) => SrgbChannel(Srgb(c.R));

        private static double G(Color c) => SrgbChannel(Srgb(c.G));

        private static double B(Color c) => SrgbChannel(Srgb(c.B));

        private static double SrgbChannel(float srgb) =>
            srgb <= 0.03928 ? srgb / 12.92 : Math.Pow((srgb + 0.055) / 1.055, 2.4f);

        private static float Srgb(byte channel) => channel / 255.0f;
    }
}
