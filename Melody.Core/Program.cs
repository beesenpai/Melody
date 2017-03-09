using System;

namespace Melody.Core {
	public static class Program {
		/// <summary>
		/// Returns the running instance of the media library.
		/// </summary>
		/// <value>The library instance.</value>
		public static Library Lib { get; private set; }

		public static void Main(string[] args){
			lib = new Library();
		}
	}
}

