﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace mpvInputEdit
{
    public partial class App : Application
    {
        public static string InputConfPath { get; } = MpvConfFolder + "input.conf";

        static string StartupFolder { get; } = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\";

        static string _MpvConfFolder;

        public static string MpvConfFolder {
            get {
                if (_MpvConfFolder == null)
                {
                    if (Directory.Exists(StartupFolder + "portable_config"))
                        _MpvConfFolder = StartupFolder + "portable_config\\";
                    else
                        _MpvConfFolder = Environment.GetFolderPath(
                            Environment.SpecialFolder.ApplicationData) + "\\mpv\\";
                }
                return _MpvConfFolder;
            }
        }

        private static ObservableCollection<InputItem> _InputItems;

        public static ObservableCollection<InputItem> InputItems
        {
            get
            {
                if (_InputItems is null)
                {
                    _InputItems = new ObservableCollection<InputItem>();

                    if (File.Exists(InputConfPath))
                    {
                        foreach (string line in File.ReadAllLines(InputConfPath))
                        {
                            string l = line.Trim();
                            if (l.StartsWith("#")) continue;
                            if (!l.Contains(" ")) continue;
                            InputItem item = new InputItem();
                            item.Input = l.Substring(0, l.IndexOf(" "));
                            if (item.Input == "") continue;
                            l = l.Substring(l.IndexOf(" ") + 1);

                            if (l.Contains("#menu:"))
                            {
                                item.Menu = l.Substring(l.IndexOf("#menu:") + 6).Trim();
                                l = l.Substring(0, l.IndexOf("#menu:"));

                                if (item.Menu.Contains(";"))
                                    item.Menu = item.Menu.Substring(item.Menu.IndexOf(";") + 1).Trim();
                            }

                            item.Command = l.Trim();
                            if (item.Command == "")
                                continue;
                            if (item.Command.ToLower() == "ignore")
                                item.Command = "";
                            _InputItems.Add(item);
                        }
                    }
                }
                return _InputItems;
            }
        }
    }
}