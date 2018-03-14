using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Infobasis.Web.Util
{

	/*
	 * NOTE: this should regularly be tested against exploits such as those at 
	 *		http://ha.ckers.org/xss.html
	 * */
	/// <summary>
	/// Strips HTML of potentially harmful:
	///   * SCRIPT APPLET etc elements
	///   * 'on__' event handlers
	///   * javascript: or vbscript: href attributes.
	/// </summary>
	public class HtmlSanitizer
	{

		public static readonly string[] DefaultTagWhiteList = 
		{
			"A","ABBR","ACRONYM","ADDRESS","AREA","B","BASE","BASEFONT","BDO",
			"BIG","BLOCKQUOTE","BODY","BR","BUTTON","CAPTION","CENTER","CITE",
			"CODE","COL","COLGROUP","DD","DEL","DFN","DIR","DIV","DL","DT",
			"EM","FIELDSET","FONT","H1","H2","H3","H4","H5","H6","HEAD","HR",
			"HTML","I","IMG","INS","ISINDEX","KBD","LABEL","LEGEND","LI","MAP",
			"MENU","OL","P","PARAM","PRE","Q","S","SAMP","SMALL","SPAN",
			"STRIKE","STRONG","SUB","SUP","TABLE","TBODY","TD","TEXTAREA",
			"TFOOT","TH","THEAD","TITLE","TR","TT","U","UL","VAR"
		};


		/// <summary>
		/// List of tags which will be allowed. All other tags will be stripped from HTML.
		/// </summary>
		public string[] TagWhiteList
		{
			get { return _tagWhiteList; }
			set { _tagWhiteList = value; }
		} string[] _tagWhiteList = DefaultTagWhiteList;


		//=======================================================================
		public HtmlSanitizer()
			: this(null)
		{
		}

		//=======================================================================
		/// <param name="allowedTagList">Space-delimited, all upper-case list of allowed tags.</param>
		public HtmlSanitizer(string tagWhiteList)
		{
			if (tagWhiteList != null && tagWhiteList.Length > 0)
				_tagWhiteList = tagWhiteList.Split(' ', '\r', '\n', '\t');
		}


		//=======================================================================
		/// <summary>
		/// Returns a ready-to-use instance of the HtmlSanitizer using the <see cref="DefaultTagWhiteList"/>.
		/// </summary>
		public static HtmlSanitizer Default
		{
			get
			{
				if (_defaultSanitizer == null)
					_defaultSanitizer = new HtmlSanitizer();
				return _defaultSanitizer;
			}
		} static HtmlSanitizer _defaultSanitizer;


		//=======================================================================
		const string PATTERN = @"
(?<TagStart> < /? )
(?<TagName> [\w-:?\x00]+ )
# Attribs
(
	(\s+
	(?<Attribute>
		[""']*
		[\w-]+ # Attrib name: word chars or '-'
		(\s* = \s*)?
		(
		("" [^""]* "") # Double quoted value
		|
		(' [^']* ')	   # Single quoted value
		|
		(` [^`]* `)	   # Grave quoted value
		|
		([^\s>]*)      # Non-quoted value
		)?
		[""']*
		))+
		\s*
)?

(?<TagEnd> 
	\s*
	/? >
)?

";

		//=======================================================================
		Regex _tagsRegex = new Regex(PATTERN, RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.Compiled);


		//=======================================================================
		public string SanitizeHtml(string html)
		{
			html = HtmlUtil.MakeValidHtml(html);
			StringBuilder safeHtml = new StringBuilder();

			int lastMatchEnd = 0;

			Match match = _tagsRegex.Match(html);

			while (match.Success)
			{
				// Add all text between last match and this match
				string priorText = html.Substring(lastMatchEnd, match.Index - lastMatchEnd);
				safeHtml.Append(priorText);

				Group tagOpen = match.Groups["TagStart"];
				Group tagName = match.Groups["TagName"];
				Group attribute = match.Groups["Attribute"];
				Group tagClose = match.Groups["TagEnd"];

				//				Console.WriteLine("tagOpen:   " + tagOpen.Value);
				//				Console.WriteLine("tagName:   " + tagName.Value);
				//				Console.WriteLine("attribute: " + attribute.Captures.Count);
				//				Console.WriteLine("tagClose:  " + tagClose.Value + " " + tagClose.Success);


				// Skip if we didn't match the opening and closing tags - they may be up to something to bypass the parsing
				if (tagOpen.Success && tagClose.Success && isAllowedTag(tagName.Value))
				{
					safeHtml.Append(tagOpen.Value);
					safeHtml.Append(tagName.Value);

					if (attribute.Success)
					{
						foreach (Capture attributeCapture in attribute.Captures)
						{
							//Console.WriteLine(" * " + attributeCapture.Value);
							if (isAllowedAttribute(attributeCapture.Value))
								safeHtml.Append(" " + attributeCapture.Value);
						}
					}
					safeHtml.Append(tagClose.Value);
				}

				lastMatchEnd = match.Index + match.Length;

				match = match.NextMatch();
			}

			// add any trailing text
			safeHtml.Append(html.Substring(lastMatchEnd));

			return safeHtml.ToString();
		}


		//=======================================================================
		bool isAllowedTag(string tag)
		{
			tag = removeNonWordChars(tag);
			return Array.IndexOf(_tagWhiteList, tag.ToUpper()) > -1;
		}

		//=======================================================================
		bool isAllowedAttribute(string attribute)
		{
			bool allowed = true;

			string attributeName = attribute;
			int equalsPos = attribute.IndexOf("=");
			if (equalsPos > -1)
				attributeName = attribute.Substring(0, equalsPos);

			attributeName = removeNonWordChars(attributeName); // remove any spaces, tabs, or other junk

			attributeName = attributeName.ToLower();
			if (attributeName.StartsWith("on"))
				allowed = false;
			else if (attributeName.StartsWith("href") || attributeName.EndsWith("src") || attributeName.StartsWith("background"))
				allowed = !containsScriptProtocol(htmlDecode(attribute));
			else if (attributeName.StartsWith("style"))
				allowed = isSafeCssStyle(htmlDecode(attribute));

			//Console.WriteLine("isAllowedAttribute('"+ attribute +"') = " + allowed);
			return allowed;
		}

		//==========================================
		string removeNonWordChars(string text)
		{
			StringBuilder trimmed = new StringBuilder();
			foreach (char ch in text)
			{
				if (char.IsLetterOrDigit(ch))
					trimmed.Append(ch);
			}
			return trimmed.ToString();
		}

		//==========================================
		/// <summary>
		/// Does CSS contain either "expression:" or "url:" ("url:" can be pointed at a "javascript:" URL).
		/// </summary>
		bool isSafeCssStyle(string cssStyle)
		{
			// Remove comments from CSS
			cssStyle = Regex.Replace(cssStyle, @"/\*.*\*/", "");

			return !Regex.IsMatch(cssStyle, @"\bexpression\s*\(|\burl\s*\(", RegexOptions.IgnoreCase);
		}

		//=======================================================================
		// match "javascript:" or "vbscript:" including ones that are split with newlines or carriage returns, which IE recognises
		Regex _scriptProtocolRegex = new Regex(@"((j[\s\0]*a[\s\0]*v[\s\0]*a)|(v[\s\0]*b))([\s\0]*s[\s\0]*c[\s\0]*r[\s\0]*i[\s\0]*p[\s\0]*t[\s\0]*\:)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		//=======================================================================
		bool containsScriptProtocol(string attributeValue)
		{
			bool matches = _scriptProtocolRegex.Match(attributeValue).Success;
			//Console.WriteLine("containsScriptProtocol( "+ attributeValue +" ) = " + matches);
			return matches;
		}



		//=======================================================================
		// Adapted (and improved) from Reflected source of .NET 2.0 System.Web.HttpUtility.HtmlDecode
		string htmlDecode(string s)
		{
			if (s == null)
			{
				return null;
			}
			if (s.IndexOf('&') < 0)
			{
				return s;
			}
			StringBuilder builder1 = new StringBuilder();
			StringWriter output = new StringWriter(builder1);

			int length = s.Length;
			for (int currentPos = 0; currentPos < length; currentPos++)
			{
				char entityValue = s[currentPos];
				if (entityValue == '&')
				{
					int endCharPos = s.IndexOfAny(_entityEndChars, currentPos + 1);

					if (endCharPos > 0)
					{
						string entityName = s.Substring(currentPos + 1, (endCharPos - currentPos) - 1);
						if ((entityName.Length > 1) && (entityName[0] == '#'))
						{
							try
							{
								if ((entityName[1] == 'x') || (entityName[1] == 'X'))
								{
									entityValue = (char)((ushort)int.Parse(entityName.Substring(2), NumberStyles.AllowHexSpecifier));
								}
								else
								{
									entityValue = (char)((ushort)int.Parse(entityName.Substring(1)));
								}
								// Improvement to .NET 2.0 code
								if (s[endCharPos] == '&')
									currentPos = endCharPos - 1; // therefore the '&' will be interpreted as the starter for the next entity
								else
									currentPos = endCharPos;
							}
							catch (FormatException)
							{
								currentPos++;
							}
							catch (ArgumentException)
							{
								currentPos++;
							}
						}
						else
						{
							currentPos = endCharPos;
							char ch = HtmlEntities.Lookup(entityName);
							if (ch != '\0')
							{
								entityValue = ch;
							}
							else
							{
								output.Write('&');
								output.Write(entityName);
								output.Write(';');
								continue;
							}
						}
					}
				}
				output.Write(entityValue);
			}
			return output.ToString();
		}

		static char[] _entityEndChars = new char[] { ';', '&' };


		#region HtmlEntities class used by htmlDecode() method. Adapted from Reflected source of .NET 2.0 System.Web.HtmlEntities
		//============================================================================
		// 
		class HtmlEntities
		{
			// Fields
			private static string[] _entitiesList;
			private static Hashtable _entitiesLookupTable;
			private static object _lookupLockObject;

			//============================================================================
			static HtmlEntities()
			{
				HtmlEntities._lookupLockObject = new object();
				string[] textArray1 = new string[] 
				{ 
					"\"-quot", "&-amp", "<-lt", ">-gt", "\x00a0-nbsp", "\x00a1-iexcl", "\x00a2-cent", "\x00a3-pound", "\x00a4-curren", "\x00a5-yen", "\x00a6-brvbar", "\x00a7-sect", "\x00a8-uml", "\x00a9-copy", "\x00aa-ordf", "\x00ab-laquo", 
					"\x00ac-not", "\x00ad-shy", "\x00ae-reg", "\x00af-macr", "\x00b0-deg", "\x00b1-plusmn", "\x00b2-sup2", "\x00b3-sup3", "\x00b4-acute", "\x00b5-micro", "\x00b6-para", "\x00b7-middot", "\x00b8-cedil", "\x00b9-sup1", "\x00ba-ordm", "\x00bb-raquo", 
					"\x00bc-frac14", "\x00bd-frac12", "\x00be-frac34", "\x00bf-iquest", "\x00c0-Agrave", "\x00c1-Aacute", "\x00c2-Acirc", "\x00c3-Atilde", "\x00c4-Auml", "\x00c5-Aring", "\x00c6-AElig", "\x00c7-Ccedil", "\x00c8-Egrave", "\x00c9-Eacute", "\x00ca-Ecirc", "\x00cb-Euml", 
					"\x00cc-Igrave", "\x00cd-Iacute", "\x00ce-Icirc", "\x00cf-Iuml", "\x00d0-ETH", "\x00d1-Ntilde", "\x00d2-Ograve", "\x00d3-Oacute", "\x00d4-Ocirc", "\x00d5-Otilde", "\x00d6-Ouml", "\x00d7-times", "\x00d8-Oslash", "\x00d9-Ugrave", "\x00da-Uacute", "\x00db-Ucirc", 
					"\x00dc-Uuml", "\x00dd-Yacute", "\x00de-THORN", "\x00df-szlig", "\x00e0-agrave", "\x00e1-aacute", "\x00e2-acirc", "\x00e3-atilde", "\x00e4-auml", "\x00e5-aring", "\x00e6-aelig", "\x00e7-ccedil", "\x00e8-egrave", "\x00e9-eacute", "\x00ea-ecirc", "\x00eb-euml", 
					"\x00ec-igrave", "\x00ed-iacute", "\x00ee-icirc", "\x00ef-iuml", "\x00f0-eth", "\x00f1-ntilde", "\x00f2-ograve", "\x00f3-oacute", "\x00f4-ocirc", "\x00f5-otilde", "\x00f6-ouml", "\x00f7-divide", "\x00f8-oslash", "\x00f9-ugrave", "\x00fa-uacute", "\x00fb-ucirc", 
					"\x00fc-uuml", "\x00fd-yacute", "\x00fe-thorn", "\x00ff-yuml", "\u0152-OElig", "\u0153-oelig", "\u0160-Scaron", "\u0161-scaron", "\u0178-Yuml", "\u0192-fnof", "\u02c6-circ", "\u02dc-tilde", "\u0391-Alpha", "\u0392-Beta", "\u0393-Gamma", "\u0394-Delta", 
					"\u0395-Epsilon", "\u0396-Zeta", "\u0397-Eta", "\u0398-Theta", "\u0399-Iota", "\u039a-Kappa", "\u039b-Lambda", "\u039c-Mu", "\u039d-Nu", "\u039e-Xi", "\u039f-Omicron", "\u03a0-Pi", "\u03a1-Rho", "\u03a3-Sigma", "\u03a4-Tau", "\u03a5-Upsilon", 
					"\u03a6-Phi", "\u03a7-Chi", "\u03a8-Psi", "\u03a9-Omega", "\u03b1-alpha", "\u03b2-beta", "\u03b3-gamma", "\u03b4-delta", "\u03b5-epsilon", "\u03b6-zeta", "\u03b7-eta", "\u03b8-theta", "\u03b9-iota", "\u03ba-kappa", "\u03bb-lambda", "\u03bc-mu", 
					"\u03bd-nu", "\u03be-xi", "\u03bf-omicron", "\u03c0-pi", "\u03c1-rho", "\u03c2-sigmaf", "\u03c3-sigma", "\u03c4-tau", "\u03c5-upsilon", "\u03c6-phi", "\u03c7-chi", "\u03c8-psi", "\u03c9-omega", "\u03d1-thetasym", "\u03d2-upsih", "\u03d6-piv", 
					"\u2002-ensp", "\u2003-emsp", "\u2009-thinsp", "\u200c-zwnj", "\u200d-zwj", "\u200e-lrm", "\u200f-rlm", "\u2013-ndash", "\u2014-mdash", "\u2018-lsquo", "\u2019-rsquo", "\u201a-sbquo", "\u201c-ldquo", "\u201d-rdquo", "\u201e-bdquo", "\u2020-dagger", 
					"\u2021-Dagger", "\u2022-bull", "\u2026-hellip", "\u2030-permil", "\u2032-prime", "\u2033-Prime", "\u2039-lsaquo", "\u203a-rsaquo", "\u203e-oline", "\u2044-frasl", "\u20ac-euro", "\u2111-image", "\u2118-weierp", "\u211c-real", "\u2122-trade", "\u2135-alefsym", 
					"\u2190-larr", "\u2191-uarr", "\u2192-rarr", "\u2193-darr", "\u2194-harr", "\u21b5-crarr", "\u21d0-lArr", "\u21d1-uArr", "\u21d2-rArr", "\u21d3-dArr", "\u21d4-hArr", "\u2200-forall", "\u2202-part", "\u2203-exist", "\u2205-empty", "\u2207-nabla", 
					"\u2208-isin", "\u2209-notin", "\u220b-ni", "\u220f-prod", "\u2211-sum", "\u2212-minus", "\u2217-lowast", "\u221a-radic", "\u221d-prop", "\u221e-infin", "\u2220-ang", "\u2227-and", "\u2228-or", "\u2229-cap", "\u222a-cup", "\u222b-int", 
					"\u2234-there4", "\u223c-sim", "\u2245-cong", "\u2248-asymp", "\u2260-ne", "\u2261-equiv", "\u2264-le", "\u2265-ge", "\u2282-sub", "\u2283-sup", "\u2284-nsub", "\u2286-sube", "\u2287-supe", "\u2295-oplus", "\u2297-otimes", "\u22a5-perp", 
					"\u22c5-sdot", "\u2308-lceil", "\u2309-rceil", "\u230a-lfloor", "\u230b-rfloor", "\u2329-lang", "\u232a-rang", "\u25ca-loz", "\u2660-spades", "\u2663-clubs", "\u2665-hearts", "\u2666-diams"
				};
				HtmlEntities._entitiesList = textArray1;
			}


			//============================================================================
			internal static char Lookup(string entity)
			{
				if (HtmlEntities._entitiesLookupTable == null)
				{
					lock (HtmlEntities._lookupLockObject)
					{
						if (HtmlEntities._entitiesLookupTable == null)
						{
							Hashtable hashtable1 = new Hashtable();
							string[] textArray1 = HtmlEntities._entitiesList;
							for (int num1 = 0; num1 < textArray1.Length; num1++)
							{
								string text1 = textArray1[num1];
								hashtable1[text1.Substring(2)] = text1[0];
							}
							HtmlEntities._entitiesLookupTable = hashtable1;
						}
					}
				}
				object obj1 = HtmlEntities._entitiesLookupTable[entity];
				if (obj1 != null)
				{
					return (char)obj1;
				}
				return '\0';
			}
		}
		#endregion


	}
}
