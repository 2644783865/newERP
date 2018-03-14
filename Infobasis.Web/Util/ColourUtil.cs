using System;
using System.Drawing;

namespace Infobasis.Web.Util
{
	/// <summary>
	/// Contains methods for manipulating colours (<see cref="System.Drawing.Color"/>)
	/// such as <see cref="Darken"/> and <see cref="Lighten"/>.
	/// </summary>
	public class ColourUtil
	{
		private ColourUtil() { /*not creatable*/ }

		//=======================================================================
		/// <overloads>
		/// Darkens colours.
		/// </overloads>
		/// <summary>
		/// Darkens an HTML colour string such as <c>"#aaff22"</c>.
		/// </summary>
		/// <param name="htmlColor">A colour in HTML format.</param>
		/// <param name="amount">The amount by which to darken.</param>
		/// <returns>The darkened color in an HTML format <see cref="string"/>.
		/// </returns>
		public static string Darken(string htmlColor, int amount)
		{
			return Lighten(htmlColor, -amount);
		}

		//=======================================================================
		/// <summary>
		/// Darkens a <see cref="System.Drawing.Color"/> structure.
		/// </summary>
		/// <param name="c">a <see cref="System.Drawing.Color"/> structure</param>
		/// <param name="amount">The amount by which to darken.</param>
		/// <returns>The darkened <see cref="System.Drawing.Color"/>.</returns>
		public static Color Darken(Color c, int amount)
		{
			return Lighten(c, -amount);
		}

		//=======================================================================
		/// <overloads>
		/// Lightens colours.
		/// </overloads>
		/// <summary>
		/// Lightens an HTML colour string such as <c>"#aaff22"</c>.
		/// </summary>
		/// <param name="htmlColor">A colour in HTML format.</param>
		/// <param name="amount">The amount by which to lighten.</param>
		/// <returns>The lightened color in an HTML format <see cref="string"/>.
		/// </returns>
		public static string Lighten(string htmlColor, int amount)
		{
			Color c = ColorTranslator.FromHtml(htmlColor);
			c = Lighten(c, amount);
			return ColorTranslator.ToHtml(c);
		}

		//=======================================================================
		/// <summary>
		/// Lightens a <see cref="System.Drawing.Color"/> structure.
		/// </summary>
		/// <param name="c">A <see cref="System.Drawing.Color"/> structure to lighten.</param>
		/// <param name="amount">The amount by which to lighten.</param>
		/// <returns>The lightened <see cref="System.Drawing.Color"/>.</returns>
		public static Color Lighten(Color c, int amount)
		{
			int r,g,b;
			r = c.R;
			g = c.G;
			b = c.B;

			r = r + amount;
			if(r<0) r = 0; else if(r>255) r = 255;
			
			g = g + amount;
			if(g<0) g = 0; else if(g>255) g = 255;
			
			b = b + amount;
			if(b<0) b = 0; else if(b>255) b = 255;

			c = Color.FromArgb(r, g, b);
			return c;
		}

	}

	/// <summary>
	/// This class represents a colour using the Hue/Saturation/Luminance colour scheme.
	/// In this scheme Hue is the base colour, Luminance ranges from black (0) to the
	/// full colour (0.5) to white (1) and saturation goes from grey to the colour
	/// (modified by luminance). It also contains alpha channel information to allow
	/// proper conversion with ARGB and similar colours.
	/// </summary>
	public class HSLColor
	{
		// HSL are in a 0 to 1 range
		private double _hue;
		private double _saturation;
		private double _luminance;
		private byte _red;
		private byte _green;
		private byte _blue;
		private byte _alpha;

		/// <summary>
		/// This provides an interface to the private member _hue. _hue should be between 
		/// 0 and 1 so this property puts the value correctly into that range and then 
		/// updates the RGB values stored internally.
		/// </summary>
		public double Hue
		{
			get { return _hue; }
			set 
			{ 
				_hue = value;
				if (_hue < 0 || _hue > 1) 
				{
					_hue = _hue-Math.Floor(_hue);
				}
				HSLtoRGB(_hue, _saturation, _luminance, out _red, out _green, out _blue);
			}
		}

		/// <summary>
		/// This provides an interface to the private member _saturation. _saturation should be between 
		/// 0 and 1 so this property puts the value correctly into that range and then 
		/// updates the RGB values stored internally.
		/// </summary>
		public double Saturation
		{
			get { return _saturation; }
			set 
			{
				_saturation = value;
				if (_saturation < 0) 
				{
					_saturation=0;
				}
				else if (_saturation > 1) 
				{
					_saturation=1;
				}
				HSLtoRGB(_hue, _saturation, _luminance, out _red, out _green, out _blue);
			}
		}

		/// <summary>
		/// Provides a value between 0 and 1 that indicates the relative luminance of the colour, allowing for 
		/// the fact that greens and yellows are seen as much brighter for the same luminance value than blues and purples.
		/// 
		/// This originally used the PAL Luminance ('Y') channel calculation, Y = 0.30R + 0.59G + 0.11B, but this proved inadequate. 
		/// 
		/// To better handle the perception of colour, we employ the WCAG Luminosity calculation
		/// 
		/// http://www.w3.org/TR/2006/WD-WCAG20-20060427/appendixA.html#luminosity-contrastdef
		/// 
		/// "The luminosity of a color is defined as 0.2126 * ((R / FS) ^ 2.2) + 0.7152 * ((G / FS) ^ 2.2) + 0.0722 * ((B / FS) ^ 2.2).
		///     *      R, G, and B are the red, green, and blue RGB values of the color.
		///     *      FS is the maximum possible full scale RGB value for R, G, and B (255 for eight bit color channels).
		///     *      The "^" character is the exponentiation operator."
		/// 
		/// </summary>
		public double PerceivedLuminance
		{
			get
			{
				const double RED_FACTOR = 2126;
				const double GREEN_FACTOR = 7152;
				const double BLUE_FACTOR = 722;

				const double GAMMA = 2.2;

				const double FS = 255.0;

                return 
					Math.Pow(
					(RED_FACTOR * Math.Pow(_red / FS, GAMMA) + GREEN_FACTOR * Math.Pow(_green / FS, GAMMA) + BLUE_FACTOR * Math.Pow(_blue / FS, GAMMA)) 
					/ (RED_FACTOR + GREEN_FACTOR + BLUE_FACTOR), 1.0/GAMMA);
			}
			set
			{
				// When setting this value we primarily want to keep the Hue the same so the colour remains the same.
				// For the hue to remain the same the difference between the smaller colours must be proportional 
				// to the difference between the smallest and largest.  
				// The saturation can be kept constant also. For values of Luminance less than .5 this is as easy as
				// keeping the RGB ratio constant (ie multiply all the RGB components by a constant value). For values of
				// Luminance greater than .5 a similar thing can be done but the values need to be transformed and the
				// resulting ration kept constant (the summary being that rather than looking at ratios of r, G and B
				// you look at 1-R, 1-G and 1-B.

				// To do this we first need to know whether the required Perceived Brightness is going to be in the top 
				// or bottom half of the luminance range.
				// To do this we set the luminance to 0.5 and calculate the perceived brightness.
				// If the required PB is higher than the luminance will be >.5. If lower then <.5
				// We then use a scaling factor to reach the correct perceived brightness.

				double red;
				double green;
				double blue;

				HSLColor midLuminanceColor = new HSLColor(_hue, _saturation, 0.5);
				bool lowLuminance = (midLuminanceColor.PerceivedLuminance > value);
				if (lowLuminance)
				{
					double scale = value / midLuminanceColor.PerceivedLuminance;
					red = midLuminanceColor.Red/255.0 * scale;
					green = midLuminanceColor.Green/255.0 * scale;
					blue = midLuminanceColor.Blue/255.0 * scale;
				}
				else
				{
					// The scaling here is similar to above except that we want to scale 1-R, 1-G and 1-B and we use 1-PerceivedBrightness where before we used a brightness.
					double antiRed = 1 - (midLuminanceColor.Red / 255.0);
					double antiGreen = 1 - (midLuminanceColor.Green / 255.0);
					double antiBlue = 1 - (midLuminanceColor.Blue / 255.0);

					double scale = (1 - value) / (1 - midLuminanceColor.PerceivedLuminance);

					antiRed *= scale;
					antiGreen *= scale;
					antiBlue *= scale;

					red = 1 - antiRed;
					green = 1 - antiGreen;
					blue = 1 - antiBlue;
				}

				_red = (byte) (red*255);
				_green = (byte) (green*255);
				_blue = (byte) (blue*255);

				RGBtoHSL(_red, _green, _blue, out _hue, out _saturation, out _luminance);
			}
		}

		/// <summary>
		/// This provides an interface to the private member _luminance. _luminance should be between 
		/// 0 and 1 so this property puts the value correctly into that range and then 
		/// updates the RGB values stored internally.
		/// </summary>
		public double Luminance
		{
			get { return _luminance; }
			set 
			{
				_luminance = value;
				if (_luminance < 0) 
				{
					_luminance=0;
				}
				else if (_luminance > 1) 
				{
					_luminance=1;
				}
				HSLtoRGB(_hue, _saturation, _luminance, out _red, out _green, out _blue);
			}
		}

		/// <summary>
		/// This provides an interface to the private member _alpha. _alpha is a byte that
		/// represents the 8 bit alpha channel of a colour.
		/// </summary>
		public byte Alpha
		{
			get {return _alpha; }
			set {_alpha = value;}
		}

		/// <summary>
		/// This provides an interface to the private member _red. _red is a byte that
		/// represents the 8 bit red channel of a colour. It is read only since the value
		/// is calculated from the _hue, _saturation and _luminance fields.
		/// </summary>
		public byte Red
		{
			get { return _red; }
		}

		/// <summary>
		/// This provides an interface to the private member _green. _green is a byte that
		/// represents the 8 bit green channel of a colour. It is read only since the value
		/// is calculated from the _hue, _saturation and _luminance fields.
		/// </summary>
		public byte Green
		{
			get { return _green; }
		}

		/// <summary>
		/// This provides an interface to the private member _blue. _blue is a byte that
		/// represents the 8 bit blue channel of a colour. It is read only since the value
		/// is calculated from the _hue, _saturation and _luminance fields.
		/// </summary>
		public byte Blue
		{
			get { return _blue; }
		}



		/// <summary>
		/// Default Constructor for the class. Creates a black colour.
		/// </summary>
		public HSLColor()
		{
			_hue = 0;
			_saturation = 0;
			_luminance = 0;
			_red = 0;
			_green = 0;
			_blue = 0;
			_alpha = 0;
		}

		/// <summary>
		/// Constructor for the class.
		/// </summary>
		/// <param name="hue">Hue: from 0 to 1</param>
		/// <param name="saturation">Saturation: from 0 to 1.</param>
		/// <param name="luminance">Luminance: from 0 to 1.</param>
		public HSLColor(double hue, double saturation, double luminance) : this(hue, saturation, luminance, 255)
		{}

		/// <summary>
		/// Constructor for the class.
		/// </summary>
		/// <param name="hue">Hue: from 0 to 1</param>
		/// <param name="saturation">Saturation: from 0 to 1.</param>
		/// <param name="luminance">Luminance: from 0 to 1.</param>
		/// <param name="alpha">Alpha: from 0 to 255.</param>

		public HSLColor(double hue, double saturation, double luminance, byte alpha) 
		{
			_hue = hue;
			if (_hue < 0 || _hue > 1) 
			{
				_hue = _hue-Math.Floor(_hue);
			}

			_saturation = saturation;
			if (_saturation < 0) 
			{
				_saturation=0;
			}
			if (_saturation > 1) 
			{
				_saturation=1;
			}

			_luminance = luminance;
			if (_luminance < 0) 
			{
				_luminance=0;
			}
			else if (_luminance > 1) 
			{
				_luminance=1;
			}

			_alpha = alpha;
			HSLtoRGB(hue, saturation, luminance, out _red, out _green, out _blue);
		}

		/// <summary>
		/// Constructor for the class.
		/// </summary>
		/// <param name="red">Red: from 0 to 255</param>
		/// <param name="green">Green: from 0 to 255</param>
		/// <param name="blue">Blue: from 0 to 255</param>
		public HSLColor(byte red, byte green, byte blue) : this(red, green, blue, 255)
		{}

		/// <summary>
		/// Constructor for the class.
		/// </summary>
		/// <param name="red">Red: from 0 to 255</param>
		/// <param name="green">Green: from 0 to 255</param>
		/// <param name="blue">Blue: from 0 to 255</param>
		/// <param name="alpha">Alpha: from 0 to 255.</param>
		public HSLColor(byte red, byte green, byte blue, byte alpha) 
		{
			_red = red;
			_green = green;
			_blue = blue;
			_alpha = alpha;
			RGBtoHSL(red, green, blue, out _hue, out _saturation, out _luminance);
		}

		/// <summary>
		/// Constructor for the class.
		/// </summary>
		/// <param name="color">Color: The ARGB values of this color are used to initialise the object.</param>
		public HSLColor(Color color) : this(color.R, color.G, color.B, color.A)
		{}



		/// <summary>
		/// Return a <see cref="Color"/> structure version of the colour represented in this object.
		/// </summary>
		/// <returns>A <see cref="Color"/> equivalent of the colour stored in this class.</returns>
		public Color ToColor () 
		{
			return Color.FromArgb(_alpha, _red, _green, _blue);
		}

		/// <summary>
		/// Return a <see cref="HSBColor"/> structure version of the colour represented in this object.
		/// </summary>
		/// <returns>A <see cref="HSBColor"/> equivalent of the colour stored in this class.</returns>
		public HSBColor ToHSBColor () 
		{
			return new HSBColor(_red, _green, _blue, _alpha);
		}


		/// <summary>
		/// Takes red, green and blue values and converts them into Hue, Saturation and Luminance values.
		/// </summary>
		/// <param name="red">Red: from 0 to 255</param>
		/// <param name="green">Green: from 0 to 255</param>
		/// <param name="blue">Blue: from 0 to 255</param>
		/// <param name="Hue">Hue - Ouput Parameter: from 0 to 1</param>
		/// <param name="Saturation">Saturation - Ouput Parameter: from 0 to 1.</param>
		/// <param name="Luminance">Luminance - Ouput Parameter: from 0 to 1.</param>

		public static void RGBtoHSL (byte red, byte green, byte blue, out double Hue, out double Saturation, out double Luminance) 
		{
			double NormRed = red/255.0;
			double NormGreen = green/255.0;
			double NormBlue = blue/255.0;

			double min_col = Math.Min(NormRed, Math.Min(NormGreen, NormBlue));
			double max_col = Math.Max(NormRed, Math.Max(NormGreen, NormBlue));
			double spread_col = max_col - min_col;

			Luminance = (max_col + min_col)/2.0;

			if (Luminance > 1) Luminance = 1;
			if (Luminance < 0) Luminance = 0;

			if (spread_col == 0) 
			{
				Hue = 0;
				Saturation = 0;
			}
			else
			{
				if (Luminance < 0.5) 
				{
					Saturation = spread_col / (2*Luminance);
				} 
				else 
				{
					Saturation = spread_col / (2 - (2*Luminance));
				}

				if (Saturation < 0) Saturation = 0;
				if (Saturation > 1) Saturation = 1;

				double deltaRed = (NormRed/6)/spread_col;
				double deltaGreen = (NormGreen/6)/spread_col;
				double deltaBlue = (NormBlue/6)/spread_col;
				if (NormRed == max_col) 
				{
					Hue = deltaGreen - deltaBlue;
				} 
				else if (NormGreen == max_col) 
				{
					Hue = (1.0 / 3.0) + deltaBlue - deltaRed;
				}
				else if (NormBlue == max_col) 
				{
					Hue = (2.0 / 3.0) + deltaRed - deltaGreen;
				} 
				else 
				{
					throw(new ApplicationException("Error in program logic. At least one of RGB must equal the max."));
				}
				if (Hue < 0) {Hue = Hue+1;}
				if (Hue > 1) {Hue = Hue-1;}

			}
		}

		/// <summary>
		/// Takes red, green and blue values and converts them into Hue, Saturation and Luminance values.
		/// </summary>
		/// <param name="Hue">Hue: from 0 to 1</param>
		/// <param name="Saturation">Saturation: from 0 to 1.</param>
		/// <param name="Luminance">Luminance: from 0 to 1.</param>
		/// <param name="red">Red - Ouput Parameter: from 0 to 255</param>
		/// <param name="green">Green - Ouput Parameter: from 0 to 255</param>
		/// <param name="blue">Blue - Ouput Parameter: from 0 to 255</param>
		public static void HSLtoRGB (double Hue, double Saturation, double Luminance, out byte red, out byte green, out byte blue) 
		{
			double temp2, temp1;
			if (Saturation==0) 
			{
				byte colorValue = System.Convert.ToByte(Luminance*255);
				red = colorValue;
				green = colorValue;
				blue = colorValue;
			} 
			else 
			{
				if (Luminance < 0.5) 
				{
					temp2 = Luminance * (1 + Saturation);
				} 
				else 
				{
					temp2 = Luminance + Saturation - (Luminance * Saturation);
				}
				temp1 = 2 * Luminance - temp2;
                unchecked
                {
                    red = (byte)(255 * HueToColorComponent(temp1, temp2, Hue + 1.0 / 3.0));
                    green = (byte)(255 * HueToColorComponent(temp1, temp2, Hue));
                    blue = (byte)(255 * HueToColorComponent(temp1, temp2, Hue - 1.0 / 3.0));
                }
			}
		}

		private static double HueToColorComponent (double temp1, double temp2, double modHue) 
		{
			if (modHue > 1) { modHue = modHue - 1;}
			if (modHue < 0) { modHue = modHue + 1;}
 
			if ( (modHue*6) < 1 ) {return (temp1 + (temp2-temp1)*6*modHue);}
			if ( (modHue*2) < 1 ) {return (temp2);}
			if ( (modHue*3) < 2 ) {return (temp1 + (temp2 - temp1) * ( ( 2.0/3.0) - modHue) * 6);}

			if (temp1 > 1.0) temp1 = 1.0;
			if (temp1 < 0.0) temp1 = 0.0;

			return temp1;
		}

		/// <summary>
		/// Gets a <see cref="Color"/> structure from HSB (hue, saturation, brightness) values.
		/// </summary>
		/// <param name="hue">Hue: from 0 to 360</param>
		/// <param name="saturation">Saturation: from 0 to 1.</param>
		/// <param name="brightness">Brigtnness: fro 0 to 1.</param>
		/// <returns>A <see cref="Color"/> from the HSB value supplied.</returns>
		public static Color GetColor (double Hue, double Saturation, double Luminance) 
		{
			byte newRed, newGreen, newBlue;
			HSLtoRGB(Hue, Saturation, Luminance, out newRed, out newGreen, out newBlue);
			return Color.FromArgb(newRed, newGreen, newBlue);
		}


		public static Color addLuminance(Color color, double change) 
		{
			HSLColor hslColor = new HSLColor(color);
			hslColor.Luminance+=change;
			return hslColor.ToColor();
		}

		public static Color addLuminance(Color color, int change) 
		{
			HSLColor hslColor = new HSLColor(color);
			double delta = change/255.0;
			hslColor.Luminance+=delta;
			return hslColor.ToColor();
		}

		public static Color addSaturation(Color color, double change) 
		{
			HSLColor hslColor = new HSLColor(color);
			hslColor.Saturation+=change;
			return hslColor.ToColor();
		}

		public static Color addSaturation(Color color, int change) 
		{
			HSLColor hslColor = new HSLColor(color);
			double delta = change/255.0;
			hslColor.Saturation+=delta;
			return hslColor.ToColor();
		}

		public static Color addHue(Color color, double change) 
		{
			HSLColor hslColor = new HSLColor(color);
			hslColor.Hue+=change;
			return hslColor.ToColor();
		}

		public static Color addHue(Color color, int change) 
		{
			HSLColor hslColor = new HSLColor(color);
			double delta = change/360.0;
			hslColor.Hue+=delta;
			return hslColor.ToColor();
		}

		public static Color Blend(Color fromColor, Color toColor, double delta)
		{
			double oneMinusDelta = 1.0 - delta;
			return Color.FromArgb(
					(int)(fromColor.A * oneMinusDelta + toColor.A * delta),
					(int)(fromColor.R * oneMinusDelta + toColor.R * delta),
					(int)(fromColor.G * oneMinusDelta + toColor.G * delta),
					(int)(fromColor.B * oneMinusDelta + toColor.B * delta)
				);

		}

	}


	/// <summary>
	/// This class represents a colour using the Hue/Saturation/Brightness colour scheme.
	/// This colour scheme uses a colour system as follows: Hue is the base colour, saturation
	/// is a scale from grey to fully coloured and brightness is a scale from black to
	/// colour (modified by saturation). It also contains alpha channel information to
	/// allow proper conversion with ARGB and similar colours.
	/// </summary>
	public class HSBColor
	{
		// HSL are in a 0 to 1 range

		private double _hue;
		private double _saturation;
		private double _brightness;
		private byte _red;
		private byte _green;
		private byte _blue;
		private byte _alpha;

		/// <summary>
		/// This provides an interface to the private member _hue. _hue should be between 
		/// 0 and 1 so this property puts the value correctly into that range and then 
		/// updates the RGB values stored internally.
		/// </summary>
		public double Hue
		{
			get { return _hue; }
			set 
			{ 
				_hue = value;
				if (_hue <0 || _hue >=1) 
				{
					_hue = _hue-Math.Floor(_hue);
				}

				HSBtoRGB(_hue, _saturation, _brightness, out _red, out _green, out _blue);
			}
		}

		/// <summary>
		/// This provides an interface to the private member _saturation. _saturation should be between 
		/// 0 and 1 so this property puts the value correctly into that range and then 
		/// updates the RGB values stored internally.
		/// </summary>
		public double Saturation
		{
			get { return _saturation; }
			set 
			{
				_saturation = value;
				if (_saturation < 0) 
				{
					_saturation=0;
				}
				else if (_saturation > 1) 
				{
					_saturation=1;
				}
				HSBtoRGB(_hue, _saturation, _brightness, out _red, out _green, out _blue);
			}
		}

		/// <summary>
		/// This provides an interface to the private member _brightness. _brightness should be between 
		/// 0 and 1 so this property puts the value correctly into that range and then 
		/// updates the RGB values stored internally.
		/// </summary>
		public double Brightness
		{
			get { return _brightness; }
			set 
			{
				_brightness = value;
				if (_brightness < 0) 
				{
					_brightness=0;
				}
				if (_brightness > 1) 
				{
					_brightness=1;
				}
				HSBtoRGB(_hue, _saturation, _brightness, out _red, out _green, out _blue);
			}
		}

		/// <summary>
		/// This provides an interface to the private member _alpha. _alpha is a byte that
		/// represents the 8 bit alpha channel of a colour.
		/// </summary>
		public byte Alpha
		{
			get {return _alpha; }
			set {_alpha = value;}
		}

		/// <summary>
		/// This provides an interface to the private member _red. _red is a byte that
		/// represents the 8 bit red channel of a colour. It is read only since the value
		/// is calculated from the _hue, _saturation and _luminance fields.
		/// </summary>
		public byte Red
		{
			get { return _red; }
		}

		/// <summary>
		/// This provides an interface to the private member _green. _green is a byte that
		/// represents the 8 bit green channel of a colour. It is read only since the value
		/// is calculated from the _hue, _saturation and _luminance fields.
		/// </summary>
		public byte Green
		{
			get { return _green; }
		}

		/// <summary>
		/// This provides an interface to the private member _blue. _blue is a byte that
		/// represents the 8 bit blue channel of a colour. It is read only since the value
		/// is calculated from the _hue, _saturation and _luminance fields.
		/// </summary>
		public byte Blue
		{
			get { return _blue; }
		}


		/// <summary>
		/// Default Constructor for the class. Creates a black colour.
		/// </summary>
		public HSBColor()
		{
			_hue = 0;
			_saturation = 0;
			_brightness = 0;
			_red = 0;
			_green = 0;
			_blue = 0;
			_alpha = 0;
		}

		/// <summary>
		/// Constructor for the class.
		/// </summary>
		/// <param name="hue">Hue: from 0 to 1</param>
		/// <param name="saturation">Saturation: from 0 to 1.</param>
		/// <param name="brightness">Brightness: from 0 to 1.</param>
		public HSBColor(double hue, double saturation, double brightness) : this(hue, saturation, brightness, 255)
		{}

		/// <summary>
		/// Constructor for the class.
		/// </summary>
		/// <param name="hue">Hue: from 0 to 1</param>
		/// <param name="saturation">Saturation: from 0 to 1.</param>
		/// <param name="brightness">Brightness: from 0 to 1.</param>
		/// <param name="alpha">Alpha: from 0 to 255.</param>

		public HSBColor(double hue, double saturation, double brightness, byte alpha) 
		{
			_hue = hue;
			if (_hue <0 || _hue >=1) 
			{
				_hue = _hue-Math.Floor(_hue);
			}

			_saturation = saturation;
			if (_saturation < 0) 
			{
				_saturation=0;
			}
			else if (_saturation > 1) 
			{
				_saturation=1;
			}

			_brightness = brightness;
			if (_brightness < 0) 
			{
				_brightness=0;
			}
			else if (_brightness > 1) 
			{
				_brightness=1;
			}

			_alpha = alpha;
			HSBtoRGB(hue, saturation, brightness, out _red, out _green, out _blue);
		}

		/// <summary>
		/// Constructor for the class.
		/// </summary>
		/// <param name="red">Red: from 0 to 255</param>
		/// <param name="green">Green: from 0 to 255</param>
		/// <param name="blue">Blue: from 0 to 255</param>
		public HSBColor(byte red, byte green, byte blue) : this(red, green, blue, 255)
		{}

		/// <summary>
		/// Constructor for the class.
		/// </summary>
		/// <param name="red">Red: from 0 to 255</param>
		/// <param name="green">Green: from 0 to 255</param>
		/// <param name="blue">Blue: from 0 to 255</param>
		/// <param name="alpha">Alpha: from 0 to 255.</param>
		public HSBColor(byte red, byte green, byte blue, byte alpha) 
		{
			_red = red;
			_green = green;
			_blue = blue;
			_alpha = alpha;
			RGBtoHSB(red, green, blue, out _hue, out _saturation, out _brightness);
		}

		/// <summary>
		/// Constructor for the class.
		/// </summary>
		/// <param name="color">Color: The ARGB values of this color are used to initialise the object.</param>
		public HSBColor(Color color) : this(color.R, color.G, color.B, color.A)
		{}


		/// <summary>
		/// Return a <see cref="Color"/> structure version of the colour represented in this object.
		/// </summary>
		/// <returns>A <see cref="Color"/> equivalent of the colour stored in this class.</returns>
		public Color ToColor () 
		{
			return Color.FromArgb(_alpha, _red, _green, _blue);
		}

		/// <summary>
		/// Return a <see cref="HSLColor"/> structure version of the colour represented in this object.
		/// </summary>
		/// <returns>A <see cref="HSLColor"/> equivalent of the colour stored in this class.</returns>
		public HSLColor ToHSLColor () 
		{
			return new HSLColor(_red, _green, _blue, _alpha);
		}


		/// <summary>
		/// Takes red, green and blue values and converts them into Hue, Saturation and Brightness values.
		/// </summary>
		/// <param name="red">Red: from 0 to 255</param>
		/// <param name="green">Green: from 0 to 255</param>
		/// <param name="blue">Blue: from 0 to 255</param>
		/// <param name="Hue">Hue - Ouput Parameter: from 0 to 1</param>
		/// <param name="Saturation">Saturation - Ouput Parameter: from 0 to 1.</param>
		/// <param name="Brightness">Brightness - Ouput Parameter: from 0 to 1.</param>

		public static void RGBtoHSB (byte red, byte green, byte blue, out double Hue, out double Saturation, out double Brightness) 
		{
			double NormRed = red/255.0;
			double NormGreen = green/255.0;
			double NormBlue = blue/255.0;

			double min_col = Math.Min(NormRed, Math.Min(NormGreen, NormBlue));
			double max_col = Math.Max(NormRed, Math.Max(NormGreen, NormBlue));
			double spread_col = max_col - min_col;

			Brightness = max_col;

			if (spread_col == 0) 
			{
				Hue = 0;
				Saturation = 0;
			}
			else
			{
				Saturation = spread_col/max_col;

				double deltaRed = (NormRed / 6) / spread_col;
				double deltaGreen = (NormGreen / 6) / spread_col;
				double deltaBlue = (NormBlue / 6) / spread_col;
				if (NormRed == max_col)
				{
					Hue = deltaGreen - deltaBlue;
				}
				else if (NormGreen == max_col)
				{
					Hue = (1.0 / 3.0) + deltaBlue - deltaRed;
				}
				else if (NormBlue == max_col)
				{
					Hue = (2.0 / 3.0) + deltaRed - deltaGreen;
				}
				else
				{
					throw (new ApplicationException("Error in program logic. At least one of RGB must equal the max."));
				}
				if (Hue < 0) { Hue = Hue + 1; }
				if (Hue > 1) { Hue = Hue - 1; }

			}
		}

		/// <summary>
		/// Takes red, green and blue values and converts them into Hue, Saturation and Luminance values.
		/// </summary>
		/// <param name="Hue">Hue: from 0 to 1</param>
		/// <param name="Saturation">Saturation: from 0 to 1.</param>
		/// <param name="Brightness">Brightness: from 0 to 1.</param>
		/// <param name="red">Red - Ouput Parameter: from 0 to 255</param>
		/// <param name="green">Green - Ouput Parameter: from 0 to 255</param>
		/// <param name="blue">Blue - Ouput Parameter: from 0 to 255</param>
		public static void HSBtoRGB (double Hue, double Saturation, double Brightness, out byte red, out byte green, out byte blue) 
		{
			if (Saturation==0) 
			{
				byte colorValue = System.Convert.ToByte(Brightness*255);
				red = colorValue;
				green = colorValue;
				blue = colorValue;
			} 
			else 
			{
				Hue *= 6; // sector 0 to 5
				int i = (int)Math.Floor( Hue );
				double f = Hue - i;
				double p = Brightness * ( 1 - Saturation );
				double q = Brightness * ( 1 - Saturation * f );
				double t = Brightness * ( 1 - Saturation * ( 1 - f ) );
				double r,g,b;
				switch( i ) 
				{
					case 0:
						r = Brightness;
						g = t;
						b = p;
						break;
					case 1:
						r = q;
						g = Brightness;
						b = p;
						break;
					case 2:
						r = p;
						g = Brightness;
						b = t;
						break;
					case 3:
						r = p;
						g = q;
						b = Brightness;
						break;
					case 4:
						r = t;
						g = p;
						b = Brightness;
						break;
					case 5: default:
						r = Brightness;
						g = p;
						b = q;
						break;
				}
				red = (byte)(255*r);
				green = (byte)(255*g);
				blue = (byte)(255*b);
			}
		}

		/// <summary>
		/// Gets a <see cref="Color"/> structure from HSB (hue, saturation, brightness) values.
		/// </summary>
		/// <param name="hue">Hue: from 0 to 360</param>
		/// <param name="saturation">Saturation: from 0 to 1.</param>
		/// <param name="brightness">Brigtnness: fro 0 to 1.</param>
		/// <returns>A <see cref="Color"/> from the HSB value supplied.</returns>
		public static Color GetColor (double Hue, double Saturation, double Brightness) 
		{
			byte newRed, newGreen, newBlue;
			HSBtoRGB(Hue, Saturation, Brightness, out newRed, out newGreen, out newBlue);
			return Color.FromArgb(newRed, newGreen, newBlue);
		}

		public static Color addBrightness(Color color, double change) 
		{
			HSBColor hsbColor = new HSBColor(color);
			hsbColor.Brightness+=change;
			return hsbColor.ToColor();
		}

		public static Color addLuminance(Color color, int change) 
		{
			HSBColor hsbColor = new HSBColor(color);
			double delta = change/255.0;
			hsbColor.Brightness+=delta;
			return hsbColor.ToColor();
		}

		public static Color addSaturation(Color color, double change) 
		{
			HSBColor hsbColor = new HSBColor(color);
			hsbColor.Saturation+=change;
			return hsbColor.ToColor();
		}

		public static Color addSaturation(Color color, int change) 
		{
			HSBColor hsbColor = new HSBColor(color);
			double delta = change/255.0;
			hsbColor.Saturation+=delta;
			return hsbColor.ToColor();
		}

		public static Color addHue(Color color, double change) 
		{
			HSBColor hsbColor = new HSBColor(color);
			hsbColor.Hue+=change;
			return hsbColor.ToColor();
		}

		public static Color addHue(Color color, int change) 
		{
			HSBColor hsbColor = new HSBColor(color);
			double delta = change/360.0;
			hsbColor.Hue+=delta;
			return hsbColor.ToColor();
		}

	}

}
