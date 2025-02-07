using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BendyCheatsInstaller
{
    public partial class main : Form
    {
        string dllUrl = "https://raw.githubusercontent.com/AndrewCromar/Bendy-Cheats/main/dll/Assembly-CSharp.dll";
        string libraryFolders = "C:\\Program Files (x86)\\Steam\\steamapps\\libraryfolders.vdf";
        int gameId = 622650;  // Steam App ID for "Bendy and the Ink Machine"
        string installLocation;

        public main()
        {
            InitializeComponent();
            Output("Starting...");
            Output("NOTE: Make sure the game is not currently running.");
            Output("Finding install location...");
            installLocation = Path.Combine(GetGamePath(), "Bendy and the Ink Machine_Data", "Managed", "Assembly-CSharp.dll");
            Output("- Found!");
            btn_install.Enabled = true;
            Output("- Awaiting install command.");
        }

        private string GetGamePath()
        {
            var steamLibraries = GetSteamLibraryPaths(libraryFolders);
            foreach (var library in steamLibraries)
            {
                string gameFolder = Path.Combine(library, "steamapps", "common", "Bendy and the Ink Machine");
                if (Directory.Exists(gameFolder))
                {
                    return gameFolder;
                }
            }
            return null;
        }

        private List<string> GetSteamLibraryPaths(string libraryFile)
        {
            List<string> libraries = new List<string>();

            if (!File.Exists(libraryFile))
            {
                MessageBox.Show("Steam library file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return libraries;
            }

            string[] lines = File.ReadAllLines(libraryFile);

            foreach (var line in lines)
            {
                if (line.Contains("\"path\""))
                {
                    var match = Regex.Match(line, "\"path\"[\\s]*\"([^\"]+)\"");
                    if (match.Success)
                    {
                        libraries.Add(match.Groups[1].Value);
                    }
                }
            }

            return libraries;
        }

        private async void btn_install_Click(object sender, EventArgs e)
        {
            btn_install.Enabled = false;
            Output("Installing...");

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += (s, ev) =>
                    {
                        pgb_installProgress.Value = ev.ProgressPercentage;
                    };

                    await client.DownloadFileTaskAsync(new Uri(dllUrl), installLocation);

                    Output("- Install complete. (" + installLocation + ")");
                    Output("- You can safly close the installer.");
                    Output("To uninstall the cheats make steam verify the integrity of the game files.");
                    MessageBox.Show($"Download completed!\nFile saved to: {installLocation}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Output("- Install failed.");
            }
        }

        private void Output(string _output)
        {
            rtb_output.AppendText(_output + "\n");
            rtb_output.SelectionStart = rtb_output.Text.Length;
            rtb_output.ScrollToCaret();
        }
    }
}