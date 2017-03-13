using System;

namespace Melody.Error {
	public class LibraryException : Exception {
		public LibraryException(string message) : base("Library Exception: " + message) {
		}
	}
}

