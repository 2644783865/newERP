using System;
using System.Web;
using System.Text;
using System.Collections.Specialized;
using System.Diagnostics;
using Infobasis.Web;

namespace Infobasis.Web.Util
{
	/// <summary>
	/// For manipulatating the <strong>query string</strong> portion of a URL.
	/// Normally you can use <see cref="Uri"/> and/or <see cref="UriBuilder"/> for 
	/// manipulating URLs etc but they don't support manipulation of <strong>query string</strong> 
	/// without resorting to string parsing.
	/// </summary>
	/// <example>
	/// <code>
	/// string url = "http://my.server.com/MyPage.aspx?a=123&amp;b=456";
	/// HRef href = new HRef(url);
	/// 
	/// // Read a query string param
	/// Console.WriteLine( <strong>href["a"]</strong> ); // prints "123"
	/// 
	/// // Add a query string param
	/// <strong>href["c"] = "789";</strong>
	/// Console.WriteLine( <strong>href.ToString()</strong> ); // prints "http://my.server.com/MyPage.aspx?a=123&amp;b=456&amp;<strong>c=789</strong>"
	/// </code>
	/// </example>
	public class HRef
	{
		const char DOT_CHARACTER = '.';
		NameValueCollection _queryDictionary = new NameValueCollection();

		public StringCollection NamelessValues
		{
			get { return _namelessValues; }
		} StringCollection _namelessValues = new StringCollection();

		string _urlRoot = string.Empty;
		string _tabSection = string.Empty;
		string _sectionPath = string.Empty;
		string _handler = string.Empty;
		bool _rootRelative = false;

		string _query = string.Empty;

		const char PATH_SEPARATOR = '/';
		const char QUERY_SEPARATOR = '?';
		const char QUERY_DELIMITER = '&';
		const char VALUE_DELIMITER = '=';
		const char FRAGMENT_SEPARATOR = '#';

        public HRef(Uri url)
            : this(url.PathAndQuery + url.Fragment)
        {
        }

        public HRef(string pathQueryAndFragment)
        {
            string pathAndQuery;

            // Truncate any fragment/bookmark
            int fragmentPos = pathQueryAndFragment.IndexOf(FRAGMENT_SEPARATOR);
            if (fragmentPos > -1)
            {
                _fragment = pathQueryAndFragment.Substring(fragmentPos + 1);
                pathAndQuery = pathQueryAndFragment.Substring(0, fragmentPos);
            }
            else
            {
                pathAndQuery = pathQueryAndFragment;
            }

            // Parse query
            int queryDelimiterPos = pathAndQuery.IndexOf(QUERY_SEPARATOR);

            if (queryDelimiterPos > -1)
            {
                BaseUrl = pathAndQuery.Substring(0, queryDelimiterPos);
                _query = pathAndQuery.Substring(queryDelimiterPos + 1);
            }
            else
            {
                BaseUrl = pathAndQuery;
            }

            if (_query.Length > 1)
            {
                foreach (string part in _query.Split(QUERY_DELIMITER))
                {
                    int valueDelimiterPos = part.IndexOf(VALUE_DELIMITER);
                    string name, value;
                    if (valueDelimiterPos != -1)
                    {
                        name = part.Substring(0, valueDelimiterPos);
                        value = part.Substring(valueDelimiterPos + 1);
                        _queryDictionary[HttpUtility.UrlDecode(name)] = HttpUtility.UrlDecode(value);
                    }
                    else
                    {
                        value = part.Substring(valueDelimiterPos + 1);
                        _namelessValues.Add(value);
                    }
                }
            }

        }

        public string UrlRoot
		{
			get
			{
				return _urlRoot;
			}
			set
			{
				_urlRoot = value;
			}
		}


		public string Handler
		{
			get
			{
				return _handler;
			}
			set
			{
				_handler = value;
			}
		}

		public string SectionPath
		{
			get
			{
				return _sectionPath;
			}
			set
			{
				_sectionPath = value;
			}
		}

		private static string _defaultSiteRoot = "";
		internal static string SiteRoot
		{
			get
			{
				return Global.SiteRootPath ?? _defaultSiteRoot;
			}
			set
			{
				_defaultSiteRoot = value;
			}
		}

		////////////////////////////////////////////////////////////////////////////
		public string BaseUrl
		{
			get
			{
				string url = UrlRoot + (SectionPath == null ? "" : SectionPath) + Handler;

				if (!_rootRelative)
				{
					url = url.Substring(1);
				}

				return url;
			}
			set
			{
				Uri currentUri;
				if (Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out currentUri) && currentUri.IsAbsoluteUri)
				{
					_urlRoot = currentUri.GetLeftPart(UriPartial.Authority);

					value = currentUri.PathAndQuery + currentUri.Fragment;
				}
				else
				{
					_urlRoot = "";
				}
				_rootRelative = value.StartsWith(PATH_SEPARATOR.ToString());

				if (value.ToUpper().StartsWith(SiteRoot.ToUpper()))
				{
					_urlRoot += SiteRoot;
					value = value.Substring(SiteRoot.Length);
				}

				if (value.IndexOf(DOT_CHARACTER) >= 0)
				{
					int lastPathSeparatorBeforeHandler = value.LastIndexOf(PATH_SEPARATOR, value.IndexOf(DOT_CHARACTER));

					if (lastPathSeparatorBeforeHandler >= 0)
					{
						_handler = value.Substring(lastPathSeparatorBeforeHandler);
						value = value.Substring(0, value.Length - _handler.Length);
					}
					else
					{
						_handler = PATH_SEPARATOR + value;
						value = "";
					}
				}
				else
				{
					_handler = PATH_SEPARATOR.ToString(); // directory path only

					if (value.EndsWith(PATH_SEPARATOR.ToString())) value = value.Substring(0, value.Length - 1);
				}

				if (value.Length > 0)
				{
					_sectionPath = value;
				}
				else
				{
					_sectionPath = string.Empty;
				}

			}
		}


		////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Gets/sets a value from/to the query string. Assigning null means the 
		/// name/value pair is effectively removed. 
		/// </summary>
		public string this[string name]
		{
			set { _queryDictionary[name] = value; }
			get { return _queryDictionary[name]; }
		}


		public string Fragment
		{
			get { return _fragment; }
			set { _fragment = value; }
		} string _fragment = string.Empty;

		////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Adds a name/value pair and returns the current instance.
		/// </summary>
		/// <param name="name">Name of variable.</param>
		/// <param name="value">Value of variable. If null the name/value pair is not added.</param>
		/// <returns>The current instance.</returns>
		/// <remarks>This method allows you to add multiple variables in one line, 
		/// eg.: <c>myHref.Add("x","1").Add("y","2")...</c>.
		/// <p>Any values that are null mean the name/value pair is not added. 
		/// This is consistent with Request.QueryString[].</p>
		/// </remarks>
		public HRef Add(string name, string value)
		{
			if (name == null)
			{
				_namelessValues.Add(value);
			}
			else
			{
				if (value != null)
					_queryDictionary[name] = value;
			}
			return this;
		}
		public HRef Add(string name, object value)
		{
			return Add(name, value == null ? null : value.ToString());
		}

		public void Remove(string name)
		{
			_queryDictionary.Remove(name);
		}

		////////////////////////////////////////////////////////////////////////////
		public void Merge(string queryString)
		{
			if (queryString == null || queryString.Length == 0)
			{
				return;
			}
			if (queryString.IndexOf(QUERY_SEPARATOR) > 0)
				throw new ArgumentException("Argument must be query string without path. Found unescaped '" + QUERY_SEPARATOR + "'.", "queryString");

			foreach (string part in queryString.Split(QUERY_DELIMITER))
			{
				string[] nameValue = part.Split(VALUE_DELIMITER);
				_queryDictionary[HttpUtility.UrlDecode(nameValue[0])] = HttpUtility.UrlDecode(nameValue[1]);
			}
		}

		public override string ToString()
		{
			return ToString(false);
		}
		public string ToString(bool includeFragment)
		{
			StringBuilder href = new StringBuilder();

			href.Append(BaseUrl);


			if (_queryDictionary.Count > 0 || _namelessValues.Count > 0)
			{


				href.Append(QUERY_SEPARATOR);

				bool isFirst = true;
				foreach (string key in _queryDictionary.Keys)
				{
					object value = _queryDictionary[key];
					if (value != null)
					{
						if (!isFirst)
							href.Append(QUERY_DELIMITER);
						appendUrlEncoded(href, key);
						href.Append(VALUE_DELIMITER);
						appendUrlEncoded(href, value.ToString());
						isFirst = false;
					}
				}
				foreach (string namelessValue in _namelessValues)
				{
					if (!isFirst)
						href.Append(QUERY_DELIMITER);
					appendUrlEncoded(href, namelessValue);
					isFirst = false;
				}

			}

			if (includeFragment && !string.IsNullOrEmpty(_fragment))
			{
				href.Append(FRAGMENT_SEPARATOR);
				href.Append(_fragment);
			}

			return href.ToString();
		}


		//===================================================================
		// We want to keep our URLs reasonably readable so we don't use HttpUtilty.UrlEncode 
		// as that obfuscates common "safe" chars that we use commonly in the app as delimiters.
		void appendUrlEncoded(StringBuilder sb, string s)
		{
			foreach (char ch in s)
			{
				// is one of our safe common delimiters or a regular 
				// ASCII letter number that we don't want to obfuscate in the URL?
				if (ch == ':'
					|| ch == ','
					|| ch == DOT_CHARACTER
					|| ch == '_'
					|| ch == '-'
					|| ch == '*'
					|| ch == ';'
					|| (ch >= '0' && ch <= '9')
					|| (ch >= 'a' && ch <= 'z')
					|| (ch >= 'A' && ch <= 'Z')
					|| ch == ')'
					|| ch == '('
					|| ch == '@'
					|| ch == '$'
					)
				{
					sb.Append(ch);
				}
				else if (ch <= 255)
				{
					sb.Append('%' + ((int)ch).ToString("x2"));
				}
				else
				{
					byte[] bytes = System.Text.Encoding.UTF8.GetBytes(ch.ToString());
					foreach (byte b in bytes)
						sb.Append('%' + b.ToString("x2"));
				}
			}
		}

		/// <summary>Checks if the given URL is local/relative (and therefore does not link to another server).</summary>
		public static string EnsureRelativeUrl(string url)
		{
			if (url == null)
				return null;

			if (!IsRelativeUrl(url))
				throw new FormatException("URL must be relative, local URL. Value was \"" + url + "\".");
			return url;
		}


		public static bool IsRelativeUrl(string url)
		{
			Uri uri;
			return Uri.TryCreate(url, UriKind.Relative, out uri);
		}


		/// <summary>Checks if the given URI is local to the current ASP.NET web site.</summary>
		public static Uri EnsureLocalUri(Uri uri)
		{
			if (uri == null)
				return null;

			if (uri.Host != HttpContext.Current.Request.ServerVariables["SERVER_NAME"]) // they're always lowercase
				throw new FormatException("Given URL is not a local URL. Must be on host '" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + "'.");
			return uri;
		}

		/// <summary>Creates a copy of a URL using the "https://" scheme.</summary>
		public static Uri ToHttps(Uri url)
		{
			UriBuilder newUrl = new UriBuilder(url);
			newUrl.Scheme = Uri.UriSchemeHttps;
			newUrl.Port = 443;
			return newUrl.Uri;
		}

        /// <summary>
        /// Ensures that the given virtual path matches the application's virtual path case-sensitively so that you can be sure that case-sensitive cookie paths work.
        /// </summary>
        public static bool CanonicalisePath(string pathAndQuery, out string newPath)
        {
            return CanonicalisePath(pathAndQuery, HttpRuntime.AppDomainAppVirtualPath, out newPath);
        }
        public static bool CanonicalisePath(string pathAndQuery, string appPath, out string newPath)
        {
            if (pathAndQuery.Length < appPath.Length)
                throw new ArgumentException("Path '" + pathAndQuery + "' must not be shorter than app path '" + appPath + "'.", "pathAndQuery");

            if (pathAndQuery.StartsWith(appPath, StringComparison.Ordinal))
            {
                newPath = pathAndQuery;
                return false;
            }
            else
            {
                newPath = appPath + pathAndQuery.Substring(appPath.Length);
                return true;
            }
        }
	}


}