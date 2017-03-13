using System;

namespace Melody.Error {
	public class SettingsException : Exception {

		public SettingsException(string message) : base("Settings Exception: " + message) {
		}
	}
}

