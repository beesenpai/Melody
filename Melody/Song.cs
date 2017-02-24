﻿using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using TagLib;
using System.Data.SQLite;

namespace Melody.Core {
    public class Song {
        public string FilePath { get; private set; }
        public string Title { get; private set; }
        public string[] Artists { get; private set; }
        public string Album { get; private set; }
        public IPicture AlbumArt { get; private set; }
        public uint Year { get; private set; }
        public string[] Genres { get; private set; }
        public uint Track { get; private set; }
        public uint TrackCount { get; private set; }
        public uint Disc { get; private set; }
        public uint DiscCount { get; private set; }
        public string Lyrics { get; private set; }
        public string Copyright { get; private set; }
        public string[] Composers { get; private set; }
        public string Conductor { get; private set; }
        public uint BPM { get; private set; }
        public bool Broken { get; private set; }
        public bool Heard;
        public bool Favorite;

        private Song() { }
        public Song(string filePath) {
            FilePath = filePath;
            var file = TagLib.File.Create(FilePath);
            if (file == null) throw new FileNotFoundException();
            Title = file.Tag.Title;
            Artists = file.Tag.AlbumArtists;
            AlbumArt = file.Tag.Pictures[0];
            Genres = file.Tag.Genres;
            Track = file.Tag.Track;
            TrackCount = file.Tag.TrackCount;
            Disc = file.Tag.Disc;
            DiscCount = file.Tag.DiscCount;
            Lyrics = file.Tag.Lyrics;
            Copyright = file.Tag.Copyright;
            Composers = file.Tag.Composers;
            Conductor = file.Tag.Conductor;
            BPM = file.Tag.BeatsPerMinute;
            Broken = false;
            Heard = false;
            Favorite = false;
        }

        public static Song Get(SQLiteCommand song) {
            SQLiteDataReader r = song.ExecuteReader();
            if (!r.HasRows) return null;
            r.Read();
            Song ret = new Song();
            ret.Album = (string)r["Album"];
            ret.AlbumArt = (IPicture)r["AlbumArt"];
            ret.Artists = ((string)r["Artists"]).Split(',');
            ret.BPM = (uint)r["BPM"];
            ret.Broken = (bool)r["Broken"];
            ret.Composers = ((string)r["Composers"]).Split(',');
            ret.Conductor = (string)r["Conductor"];
            ret.Copyright = (string)r["Copyright"];
            ret.Disc = (uint)r["Disc"];
            ret.DiscCount = (uint)r["DiscCount"];
            ret.Favorite = (bool)r["Favorite"];
            ret.FilePath = (string)r["FilePath"];
            ret.Genres = ((string)r["Genres"]).Split(',');
            ret.Heard = (bool)r["Heard"];
            ret.Lyrics = (string)r["Lyrics"];
            ret.Title = (string)r["Title"];
            ret.Track = (uint)r["Track"];
            ret.TrackCount = (uint)r["TrackCount"];
            ret.Year = (uint)r["Year"];
            r.Close();
            return ret;
        }
    }
}
