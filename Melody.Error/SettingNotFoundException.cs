using System;

namespace Melody.Error {
	public class SettingNotFoundException : Exception {
		/// <summary>
		/// The requested URI.
		/// </summary>
		public string Setting { get; private set; }

		public SettingNotFoundException(string key) :
			base("The setting key requested could not be found. The database could be corrupt.") {
			Setting = key;
		}
	}
}

