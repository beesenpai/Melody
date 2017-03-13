using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using TagLib;
using System.Data.SQLite;

using Melody.Error;

namespace Melody.Core {
	public class Song {
		private string _filepath;
		private string _title;
		private string _artist;
		private string _album;
		private IPicture _albumart;
		private uint _year;
		private string _genre;
		private uint _track;
		private uint _trackct;
		private uint _disc;
		private uint _discct;
		private string _lyrics;
		private string _copyright;
		private string _composer;
		private string _conductor;
		private uint _bpm;
		private bool _broken;
		private bool _heard;
		private bool _favorite;

		public string FilePath { 
			get { return _filepath; }
			set {
				if(!Path.GetInvalidPathChars(value) || !File.Exists(_filepath))
					throw new FileNotFoundException("The source file could not be found.");

				_filepath = value;

			}
		}

		public string Title { get; private set; }

		public string Artist { get; private set; }

		public string Album { get; private set; }

		public IPicture AlbumArt { get; private set; }

		public uint Year { get; private set; }

		public string Genre { get; private set; }

		public uint Track { get; private set; }

		public uint TrackCount { get; private set; }

		public uint Disc { get; private set; }

		public uint DiscCount { get; private set; }

		public string Lyrics { get; private set; }

		public string Copyright { get; private set; }

		public string Composer { get; private set; }

		public string Conductor { get; private set; }

		public uint BPM { get; private set; }

		public bool Broken { get; private set; }

		public bool Heard;
		public bool Favorite;

		private Song() {
		}

		public Song(string filePath) {
			FilePath = filePath;
			var file = TagLib.File.Create(FilePath);
			if(file == null)
				throw new FileNotFoundException();
			Title = file.Tag.Title;
			Artist = file.Tag.JoinedAlbumArtists;
			AlbumArt = file.Tag.Pictures[0];
			Genre = file.Tag.JoinedGenres;
			Track = file.Tag.Track;
			TrackCount = file.Tag.TrackCount;
			Disc = file.Tag.Disc;
			DiscCount = file.Tag.DiscCount;
			Lyrics = file.Tag.Lyrics;
			Copyright = file.Tag.Copyright;
			Composer = file.Tag.JoinedComposers;
			Conductor = file.Tag.Conductor;
			BPM = file.Tag.BeatsPerMinute;
			Broken = false;
			Heard = false;
			Favorite = false;
		}
	}
}
