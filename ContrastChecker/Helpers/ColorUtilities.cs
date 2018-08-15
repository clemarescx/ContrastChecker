using System.Globalization;
using System.Windows.Media;

namespace ContrastChecker.Helpers
{
    public static class ColorUtilities
    {
        /// <summary>
        /// Parses a string containing a hexadecimal value into its corresponding colour.
        /// The byte string can be prefixed with '#', '0x' or '0X'.
        /// If only 3 bytes are provided, the Alpha value is defaulted to 0xFF (opaque).
        /// </summary>
        /// <param name="colorString">
        /// the string containing the colour bytes.<para/>
        /// Format: (#|0x|0X)(A)RGB
        /// </param>
        /// <param name="color">The parsed color.</param>
        /// <returns>the Color corresponding to the (A)RGB input</returns>
        public static bool TryParseColorString(string colorString, out Color color)
        {
            if (string.IsNullOrWhiteSpace(colorString))
            {
                color = Colors.Transparent;
                return false;
            }

            colorString = colorString.Replace(" ", string.Empty).ToLower();
            if (colorString.StartsWith("#"))
            {
                colorString = colorString.Substring(1);
            }
            else if (colorString.StartsWith("0x"))
            {
                colorString = colorString.Substring(2);
            }

            byte a = 0xff;
            byte r = 0xff;
            byte g = 0xff;
            byte b = 0xff;

            string GetValueByte(int idx) => colorString.Substring(idx, 2);
            const int argbLength = 8;
            const int rgbLength = 6;
            const int shortArgbLength = 4;
            const int shortRgbLength = 3;
            var alphaIndexOffset = 0;

            switch (colorString.Length)
            {
                case argbLength:
                    alphaIndexOffset = 2;
                    byte.TryParse(GetValueByte(0), NumberStyles.HexNumber, null, out a);
                    goto case rgbLength;
                case rgbLength:
                    byte.TryParse(GetValueByte(0 + alphaIndexOffset), NumberStyles.HexNumber, null, out r);
                    byte.TryParse(GetValueByte(2 + alphaIndexOffset), NumberStyles.HexNumber, null, out g);
                    byte.TryParse(GetValueByte(4 + alphaIndexOffset), NumberStyles.HexNumber, null, out b);
                    break;
                case shortArgbLength:
                    alphaIndexOffset = 1;
                    byte.TryParse(colorString.Substring(0, 1), NumberStyles.HexNumber, null, out a);
                    goto case shortRgbLength;
                case shortRgbLength:
                    byte.TryParse(colorString.Substring(0 + alphaIndexOffset, 1), NumberStyles.HexNumber, null, out r);
                    byte.TryParse(colorString.Substring(1 + alphaIndexOffset, 1), NumberStyles.HexNumber, null, out g);
                    byte.TryParse(colorString.Substring(2 + alphaIndexOffset, 1), NumberStyles.HexNumber, null, out b);
                    break;
            }

            color = Color.FromArgb(a, r, g, b);
            return true;
        }
    }
}
