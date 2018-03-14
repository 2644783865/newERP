using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Globalization;
using System.Collections.Generic;
using Infobasis.Web;
using System.Xml;
using Infobasis.Web;

namespace Infobasis.Web.Util
{
	/// <summary>
	/// Summary description for StringUtil.
	/// </summary>
	public static class StringUtil
	{

		static readonly Regex _formatHashRegex = new Regex(@"{(\w+)}");

		/// <summary>
		/// Similar to String.Format except instead of positional markers "{0}", this 
		/// takes named markers "{Name}".
		/// </summary>
		/// <param name="format">String with format markers.</param>
		/// <param name="hash">Dictionary with data to insert into new string.</param>
		/// <returns>String with tokens from hash inserted.</returns>
		public static string FormatHash(string format, IDictionary hash)
		{
			//TODO: Probably more efficient just to scan string directly here instead of RegEx

			StringBuilder output = new StringBuilder();

			int endOfLastMatch = 0;
			foreach (Match m in _formatHashRegex.Matches(format))
			{
				string tokenName = m.Groups[1].Value;

				// Add bit before the matched token
				output.Append(format.Substring(endOfLastMatch, m.Index - endOfLastMatch));

				// Insert replacement
				output.Append(hash[tokenName]);

				endOfLastMatch = m.Index + m.Length;
			}

			// Last bit;
			output.Append(format.Substring(endOfLastMatch));

			return output.ToString();
		}

		//=======================================================================
		public static string FormatRow(string format, DataRow row)
		{
			_FormatRow_MatchEvaluator evaluator = new _FormatRow_MatchEvaluator(row);
			string result = _formatHashRegex.Replace(format, new MatchEvaluator(evaluator.MatchEvaluator));
			return result;
		}
		#region class _FormatRow_MatchEvaluator
		private class _FormatRow_MatchEvaluator
		{
			DataRow _row;
			public _FormatRow_MatchEvaluator(DataRow row)
			{
				_row = row;
			}
			public string MatchEvaluator(Match match)
			{
				string columnName = match.Groups[1].Value;
				return _row[columnName].ToString();
			}
		}
		#endregion

		//=======================================================================
		public static bool IsNullOrEmpty(string s)
		{
			return (s == null || s == string.Empty);
		}

		//=====================================================================
		/// <summary>
		/// Joins an object array into a string so that it can be safely split 
		/// using <see cref="EscapedSplit"/>.
		/// </summary>
		/// <param name="delimiter">Character used to delimit array items.</param>
		/// <param name="escape">Character used to prefix the delimiter character when present in an array item.</param>
		/// <param name="items">The array to join.</param>
		/// <returns>A string containing the joined items.</returns>
		/// <remarks>
		/// EscapedJoin and EscapedSplit are versions of String.Join and String.Split, 
		/// which are more robust in certain situations. The key feature is that they 
		/// allow joining and splitting of strings to and from arrays even if the array 
		/// items contain the delimiter character. This is enabled by specifying use 
		/// of an 'escape' character to prefix the delimiter when it in one of the items.
		/// </remarks>
		public static string EscapedJoin(char delimiter, char escape, params string[] items)
		{
			if (delimiter == escape)
				throw new ArgumentException("Delimiter and escape chars must be different.", "delimiter, escape");

			if (items == null)
				throw new ArgumentNullException("items");

			if (items.Length == 0) return string.Empty;

			// we'll need some strings...
			string escapeStr = escape.ToString();
			string escapedEscapeStr = new string(escape, 2);
			string delimStr = delimiter.ToString();
			string escapedDelimStr = new string(new char[] { escape, delimiter });


			string firstItem = items[0] ?? string.Empty;

			StringBuilder result = new StringBuilder(firstItem, items.Length * (firstItem).Length * 2 + items.Length - 1);
					// initial capacity estimated from length of the first item, and the number of items. StringBuilder created initialised with the firstItem already in place.
			
			// escape the first item
			result.Replace(escapeStr, escapedEscapeStr);
			result.Replace(delimStr, escapedDelimStr);

			for (int i = 1; i < items.Length; i++)
			{
				result.Append(delimiter);

				string item = items[i] ?? string.Empty;

				StringBuilder escaped = new StringBuilder(item);

				escaped.Replace(escapeStr, escapedEscapeStr);
				escaped.Replace(delimStr, escapedDelimStr);

				// result.Append(escaped.ToString()) uses an (expensive) ToString() operation on the buffer. Quicker to move it to a char array, and append that

				char[] escapedChars = new char[escaped.Length];
				escaped.CopyTo(0, escapedChars, 0, escaped.Length);

				result.Append(escapedChars);
			}
			return result.ToString();
		}

		//=====================================================================
		/// <summary>
		/// Splits a string into an array that was previously joined 
		/// using <see cref="EscapedJoin"/>.
		/// </summary>
		/// <param name="delimiter">Character used to delimit array items.</param>
		/// <param name="escape">Character used to prefix the delimiter character when present in an array item.</param>
		/// <param name="str">The delimited string.</param>
		/// <returns>An array with the split items.</returns>
		/// <remarks>
		/// EscapedJoin and EscapedSplit are versions of String.Join and String.Split, 
		/// which are more robust in certain situations. The key feature is that they 
		/// allow joining and splitting of strings to and from arrays even if the array 
		/// items contain the delimiter character. This is enabled by specifying use 
		/// of an 'escape' character to prefix the delimiter when it in one of the items.
		/// </remarks>
		public static string[] EscapedSplit(char delimiter, char escape, string str)
		{
			if (delimiter == escape)
				throw new ArgumentException("Delimiter and escape chars must be different.", "delimiter, escape");

			if (str == null)
				throw new ArgumentNullException("str");

			StringCollection collection = new StringCollection();

			StringBuilder item = new StringBuilder();

			for (int i = 0; i < str.Length; i++)
			{
				char currChar = str[i];

				char nextChar = char.MinValue;
				if (i < str.Length - 1) nextChar = str[i + 1];

				if (currChar != delimiter && currChar != escape)
				{
					item.Append(currChar);
				}
				else if (currChar == escape && nextChar == escape)
				{
					item.Append(currChar);
					i++;
				}
				else if (currChar == escape && nextChar == delimiter)
				{
					item.Append(nextChar);
					i++;
				}
				else if (currChar == delimiter)
				{
					collection.Add(item.ToString());
					item.Length = 0; // reset item
				}
			}
			// Add last item
			collection.Add(item.ToString());

			string[] array = new string[collection.Count];
			collection.CopyTo(array, 0);
			return array;
		}


		/// <summary>
		/// Works similarly to String.Split except that the items are trimmed of any whitespace.
		/// </summary>
		public static string[] TrimmedSplit(string s, params char[] separators)
		{
			ArrayList list = new ArrayList();
			foreach (string item in s.Split(separators))
				list.Add(item.Trim());

			string[] array = new string[list.Count];
			list.CopyTo(array);
			return array;
		}

		//=====================================================================================
		/// <summary>
		///	Removes all the HTML tags from the string and returns specified no. of characters.
		/// </summary>
		/// <param name="data">The string to be processed.</param>
		/// <returns></returns>		
		public static string TrimHtmlTags(string html)
		{
			return TrimHtmlTags(html, -1);
		}

		public static string TrimHtmlTags(string html, int maxNoOfChars)
		{
			if (html == null || html == string.Empty)
				return string.Empty;

			// First pass to resolve HTML entities (e.g. "&nbsp;" etc)
			html = HttpUtility.HtmlDecode(html);

			StringBuilder output = new StringBuilder();

			bool beginTag = false;
			for (int i = 0; i < html.Length; i++)
			{
				if (maxNoOfChars != -1 && output.Length == maxNoOfChars)
					return output.ToString() + "...";

				char inputChar = html[i];
				if (inputChar == '<')
					beginTag = true;
				else
				{
					if (inputChar == '>')
						beginTag = false;
					else
					{
						if (!beginTag)
							output.Append(inputChar);
					}
				}
			}
			return output.ToString();
		}


		//=======================================================================
		/// <summary>
		/// Encodes a string into a format that is safe to use as a JavaScript 
		/// string literal. Note that this will not enclose the string in single 
		/// or double quotes as this may be different depending on the context.
		/// Single or double quotes are always encoded into "\x27" or "\x22" 
		/// respectively so it's always safe to use this with whatever you use 
		/// to enclose your string literals.
		/// </summary>
		/// <param name="text">The text to encode.</param>
		/// <returns>The JavaScript-encoded string.</returns>
		public static string JSEncode(string text)
		{
			if (text == null)
				return null;
			if (text.Length == 0)
				return text;

			StringBuilder safe = new StringBuilder();
			foreach (char ch in text)
			{
				// Newline etc?
				if (ch == '\r')
					safe.Append("\\r");
				else if (ch == '\n')
					safe.Append("\\n");
				else if (ch == '\t')
					safe.Append("\\t");
				else if (ch == '\\')
					safe.Append("\\\\");
				// is it a common, safe char?
				else if ((ch >= '0' && ch <= '9') ||
					(ch >= 'A' && ch <= 'Z') ||
					(ch >= 'a' && ch <= 'z') ||
					ch == ' ' || ch == ',' || ch == '.' || ch == ':' ||
					ch == ';' || ch == '!' || ch == '?' || ch == '(' ||
					ch == ')' || ch == '[' || ch == ']' || ch == '/' ||
					ch == ')' || ch == '[' || ch == ']' || ch == '_' ||
					ch == '='
					)
					safe.Append(ch);
				// Hex encode "\xFF"
				else if (ch <= 127)
					safe.Append("\\x" + ((int)ch).ToString("x2"));
				// Unicode hex encode "\uFFFF"
				else
					safe.Append("\\u" + ((int)ch).ToString("x4"));
			}
			return safe.ToString();
		}

		//===========================================================
		public static byte[] HexStringToByteArray(string hexString)
		{
			if (hexString == null)
				throw new ArgumentNullException("str");
			if (hexString.Length % 2 != 0)
				throw new FormatException("String \"" + hexString + "\" is invalid length for Hex string. Length should be evenly divisible by 2.");

			byte[] bytes = new byte[hexString.Length / 2];

			for (int i = 0; i < hexString.Length; i += 2)
			{
				string hexPair = hexString.Substring(i, 2);
				try
				{
					bytes[i / 2] = byte.Parse(hexPair, NumberStyles.AllowHexSpecifier);
				}
				catch (FormatException)
				{
					throw new FormatException("\"" + hexPair + "\" in string \"" + hexString + "\" is not in a recognised hexadecimal format.");
				}
			}
			return bytes;
		}

		//=======================================================================
		/// <summary>
		/// Converts HTML to almost pre-formatted plain text. Basically all tags 
		/// are removed, but tags such as line breaks (&lt;br&gt;) and paragraphs 
		/// (&lt;p&gt;) are preserved as newlines and carriage-returns.
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static string HtmlToPlainText(string html)
		{
			return HtmlToPlainText(html, -1);
		}

		//=======================================================================
		/// <summary>
		/// Converts HTML to almost pre-formatted plain text. Basically all tags 
		/// are removed, but tags such as line breaks (&lt;br&gt;) and paragraphs 
		/// (&lt;p&gt;) are preserved as newlines and carriage-returns.
		/// </summary>
		/// <param name="html"></param>
		/// <param name="lineWrapWidth">Any lines longer than this size are word-wrapped.</param>
		/// <returns></returns>
		public static string HtmlToPlainText(string html, int lineWrapWidth)
		{
			// remove unwanted blocks
			html = Regex.Replace(html, @"((<!DOCTYPE.*?>)|(<html.*?>).*?<body.*?>)|(<!--.*?-->)|(<style.*?>.*?</style>)|(<head.*?>.*?</head>)", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

			// remove duplicate whitespace
			html = Regex.Replace(html, @"\s+", " ");

			// replace tags with apropriate formatting
			html = Regex.Replace(html, @"(<(?<END_TAG_SLASH>\/?)(?<TAG>.*?)(\s+(?<ATTRIBS>.*?))?/?>)|(?<HTML_ENTITY>&#?\w.+?;)", new MatchEvaluator(_HtmlToPlainText_Match), RegexOptions.Singleline);

			if (lineWrapWidth > 0)
				html = wrapLines(html, lineWrapWidth);

			return html;

		}


		//=======================================================================
		// Used by Regex in HtmlToPlainText()
		static string _HtmlToPlainText_Match(Match match)
		{
			string tag = match.Groups["TAG"].Value.ToUpper();
			string htmlEntity = match.Groups["HTML_ENTITY"].Value;
			string endTagSlash = match.Groups["END_TAG_SLASH"].Value;

			bool isEndTag = endTagSlash.Length > 0;

			const string doubleLineBreak = "\r\n\r\n";
			const string singleLineBreak = "\r\n";

            if (htmlEntity.Length > 0)
            {
                if (htmlEntity.Equals("&nbsp;", StringComparison.InvariantCultureIgnoreCase))
                {
                    return " ";
                }
                else
                {
                    return System.Web.HttpUtility.HtmlDecode(htmlEntity);
                }
            }

			if (!isEndTag)
			{
				if (tag == "P")
					return doubleLineBreak;
				else if (tag == "BR")
					return singleLineBreak;
				else if (tag == "DIV")
					return singleLineBreak;
				else if (tag == "H1" || tag == "H2" || tag == "H3" || tag == "H4")
					return doubleLineBreak;

			}
			else // isEndTag
			{
				if (tag == "DIV")
					return singleLineBreak;
				if (tag == "H1" || tag == "H2" || tag == "H3" || tag == "H4")
					return doubleLineBreak;
			}
			return string.Empty;
		}


		//=======================================================================
		// Used for word-wrapping by HtmlToPlainText()
		static string wrapLines(string text, int lineWidth)
		{
			StringBuilder output = new StringBuilder();

			int lineStart = 0;
			int lastSpace = 0;

			//scan for break
			for (int cursor = 0; cursor < text.Length; cursor++)
			{
				char ch = text[cursor];

				// remember where last word ended
				if (char.IsWhiteSpace(ch))
					lastSpace = cursor + 1;

				// new line?
				if (ch == '\r' || ch == '\n')
				{
					// Just output whatever we've got so far on the current line
					output.Append(text.Substring(lineStart, cursor - lineStart));

					lineStart = cursor;
					lastSpace = cursor;
				}
				// Do we need to word wrap?
				else if (cursor > (lineStart + lineWidth))
				{
					// If the last space we found was after the start of this line then move cursor back to last space
					if (lastSpace != lineStart)
						cursor = lastSpace;
					else
						// it's an overlong line without any spaces: don't break the line
						continue;

					output.Append(text.Substring(lineStart, cursor - lineStart) + "\r\n");

					lineStart = cursor;
					lastSpace = cursor;
				}
			}
			return output.ToString();
		}

		//=======================================================================
		/// <summary>
		/// Same as String.EndsWith except case-insensitively. 
		/// </summary>
		/// <param name="str">The string to search.</param>
		/// <param name="ending">The ending to find.</param>
		/// <returns></returns>
		public static bool EndsWithIgnoreCase(string str, string ending)
		{
			int strLen = str.Length;
			int endingLen = ending.Length;
			if (endingLen > strLen)
				return false;
			return (0 == string.Compare(str, strLen - endingLen, ending, 0, strLen, true));
		}

		/// <summary>
		/// Indicates whether two strings are equal, ignoring their case (in the invariant culture).
		/// </summary>
		public static bool EqualsIgnoreCase(string str1, string str2)
		{
			return CultureInfo.InvariantCulture.CompareInfo.Compare(str1, str2, CompareOptions.IgnoreCase) == 0;
		}

		//=====================================================================
		static HtmlSanitizer _htmlSanitizer;
		/// <summary>
		/// Strips HTML of potentially harmful:
		///   * SCRIPT APPLET etc elements
		///   * 'on__' event handlers
		///   * javascript: or vbscript: href attributes.
		/// </summary>
		public static string SanitizeHtml(string html)
		{
            if (html == null)
                return null;

			// Short circuit if there's no tags
			if (html.IndexOf('<') == -1)
				return html;

			if (_htmlSanitizer == null)
			{
				string allowedHtmlTags = Global.Config["AllowedHtmlTags"];
				_htmlSanitizer = new HtmlSanitizer(allowedHtmlTags);
			}
			return _htmlSanitizer.SanitizeHtml(html);
		}

		/// <summary>
		/// Returns a string safe for inclusion in HTML attributes. Note that this also does whitespace formatting 
		/// i.e. tabs are converted to non-breaking spaces. Linebreaks are not converted to &lt;br/&gt;, however
		/// </summary>
        public static string HtmlAttributeEncode(string text)
        {
            if (text == null)
                return null;
            if (text.Length == 0)
                return text;


            StringWriter html = new FormattedHtmlStringWriter(HtmlFormat.AttributeValue);

            html.Write(text);

            return html.ToString();
        }

        /// <summary>
		/// Returns a string safe for inclusion in HTML. Note that this also does whitespace formatting 
		/// i.e. line breaks are converted to &lt;br/&gt; and tabs are converted to non-breaking spaces.
		/// </summary>
		public static string HtmlEncode(string text)
		{
			if (text == null)
				return null;
			if (text.Length == 0)
				return text;


			StringWriter html = new FormattedHtmlStringWriter(HtmlFormat.Content);

			html.Write(text);

            return html.ToString();
		}

        internal enum HtmlFormat
        {
            Content,
            AttributeValue
        }

		/// <summary>
		/// A StringWriter that writes line breaks and whitespace in HTML format.
		/// </summary>
		internal class FormattedHtmlStringWriter : StringWriter
		{
			int _spaceCount;
            HtmlFormat _format;

            internal FormattedHtmlStringWriter(HtmlFormat format)
            {
                _format = format;
            }

            public override void Write(char[] buffer, int index, int count)
            {
                for (int i = index; i < index + count && i < buffer.Length; i++)
                {
                    Write(buffer[i]);
                }
            }

			public override void Write(char ch)
			{
                if (ch == ' ')
                {
                    _spaceCount++;
                }
                else
                {
                    writePendingSpaces();
                    if (ch == '\'')
                    {
                        base.Write("&#39;"); // not &apos;, that's an XML entity, not an HTML one.
                    }
                    else if (ch == '<')
                    {
                        base.Write("&lt;");
                    }
                    else if (ch == '>')
                    {
                        base.Write("&gt;");
                    }
                    else if (ch == '"')
                    {
                        base.Write("&quot;");
                    }
                    else if (ch == '&')
                    {
                        base.Write("&amp;");
                    }
                    else if (ch == '\n')
                    {
                        if (_format == HtmlFormat.Content)
                        {
                            base.Write("<br/>");
                        }
                        else
                        {
                            base.Write(ch);
                        }
                    }
                    else if (ch == '\t')
                    {
                        base.Write("&nbsp;&nbsp;&nbsp; ");
                    }
                    else if (ch > ' ' && ch <= '~')
                    {
                        base.Write(ch);
                    }
                    else if (((int) ch) >= 0x80)
                    {
                        base.Write(ch); // we'll assume that UTF-8 output encoding will handle anything outside the ASCII range.
                    }   
                    // note we are throwing out ch if it's < ' ' and not one we've already handled (so, \n or \t then), or \7f (DEL)
                }
			}

			public override void Write(string value)
			{
				writePendingSpaces();
				foreach (char ch in value)
					this.Write(ch);
			}

			void writePendingSpaces()
			{
				if (_spaceCount > 0)
				{
					_spaceCount--; // account for last space we add
					while (_spaceCount-- > 0)
						base.Write("&nbsp;");
					base.Write(' ');

					_spaceCount = 0;
				}
			}

			public override string ToString()
			{
				writePendingSpaces();
				return base.ToString();
			}

		}
	}
}
