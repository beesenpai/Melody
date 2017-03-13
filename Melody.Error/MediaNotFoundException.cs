using System;

namespace Melody.Error {
	public class MediaNotFoundException : Exception {
		/// <summary>
		/// The requested URI.
		/// </summary>
		public string URI { get; private set; }

		public MediaNotFoundException(string uri) : base("The request was not found in the current library.") {
			URI = uri;
		}
	}
}

