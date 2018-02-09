using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Fougerite;

namespace AntiBadWords
{
    public class AntiBadWords : Fougerite.Module
    {
        public override string Name { get { return "AntiBadWords"; } }
        public override string Author { get { return "Salva/Juli"; } }
        public override string Description { get { return "AntiBadWords"; } }
        public override Version Version { get { return new Version("1.1"); } }

        public string red = "[color #B40404]";
        public string blue = "[color #81F7F3]";
        public string green = "[color #82FA58]";
        public string yellow = "[color #F4FA58]";
        public string orange = "[color #FF8000]";
        public string pink = "[color #FA58F4]";
        public string white = "[color #FFFFFF]";
        public List<string> Words = new List<string>();
        public string pathfile = Directory.GetCurrentDirectory() + "\\save\\AntiBadWords\\Words.txt";

        public override void Initialize()
        {
            ReloadWords();
            Fougerite.Hooks.OnCommand += OnCommand;
            Fougerite.Hooks.OnChat += OnChat;
        }
        public override void DeInitialize()
        {
            Fougerite.Hooks.OnCommand -= OnCommand;
            Fougerite.Hooks.OnChat -= OnChat;
        }
        public void OnChat(Fougerite.Player Player, ref ChatString chatString)
        {
            string Text = chatString.OriginalMessage.ToLower();
            foreach (var xx in Words)
            {
                if (Text.Contains(xx))
                {
                    Player.MessageFrom("AntiBadWords", red + "The word you have written " + white + "(" + xx + ")" + red + " is not allowed.");
                    chatString.NewText = "";
                    break;
                }
            }
        }
        public void OnCommand(Fougerite.Player Player, string cmd, string[] args)
        {
            if (cmd == "bw")
            {
                if (!Player.Admin)
                {
                    Player.MessageFrom("AntiBadWords", red + "You are not an administrator to use this command.");
                    return;
                }
                else
                {
                    Player.MessageFrom("AntiBadWords", green + "Use " + white + "/bw" + green + " to see the HELP.");
                    Player.MessageFrom("AntiBadWords", green + "Use " + white + "/bwr" + green + " to reload the word list.");
                    Player.MessageFrom("AntiBadWords", green + "Use " + white + "/bwl" + green + " to see the list of forbidden words.");
                    Player.MessageFrom("AntiBadWords", green + "Use " + white + "/bwadd SomeWord" + green + " to add a word to the list.");
                }
            }
            else if (cmd == "bwr")
            {
                if (!Player.Admin)
                {
                    Player.MessageFrom("AntiBadWords", red + "You are not an administrator to use this command.");
                    return;
                }
                else
                {
                    ReloadWords();
                    Player.MessageFrom("AntiBadWords", green + "The word list has been updated.");
                }
            }
            else if (cmd == "bwl")
            {
                if (!Player.Admin)
                {
                    Player.MessageFrom("AntiBadWords", red + "You are not an administrator to use this command.");
                    return;
                }
                else
                {
                    Player.MessageFrom("AntiBadWords", orange + "List of forbidden words:");
                    foreach (var xx in Words)
                    {
                        Player.MessageFrom("AntiBadWords", xx.ToString());
                    }
                    Player.MessageFrom("AntiBadWords", orange + "End of the list.");
                }
            }
            else if (cmd == "bwadd")
            {
                if (!Player.Admin)
                {
                    Player.MessageFrom("AntiBadWords", red + "You are not an administrator to use this command.");
                    return;
                }
                else
                {
                    if (args.Length > 0)
                    {
                        if (Words.Contains(args[0]))
                        {
                            Player.MessageFrom("AntiBadWords", red + "The word you are trying to add  " + white + args[0] + red + " is already in the list.");
                            return;
                        }
                        else
                        {
                            Player.MessageFrom("AntiBadWords", green + "You added the Word " + white + args[0] + green + " to the list.");
                            StreamWriter WriteReportFile = File.AppendText(pathfile);
                            WriteReportFile.WriteLine(args[0]);
                            WriteReportFile.Close();
                            ReloadWords();
                        }
                    }
                    else
                    {
                        Player.MessageFrom("AntiBadWords", orange + "You need specify some word!!! " + white + " /addword someword.");
                    }
                }
            }
        }
        // METHODS
        public void ReloadWords()
        {
            
            if (!File.Exists(pathfile))
            {
                File.Create(pathfile).Dispose();
                StreamWriter WriteReportFile = File.AppendText(pathfile);
                WriteReportFile.WriteLine("retard");
                WriteReportFile.WriteLine("stupid");
                WriteReportFile.WriteLine("fuck");
                WriteReportFile.Close();
            }

            Words.Clear();
            foreach (string str in File.ReadAllLines(pathfile))
            {
                Words.Add(str.ToLower());
            }
            return;
        }
    }
}
