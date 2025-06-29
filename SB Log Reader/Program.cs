using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using Timer = System.Windows.Forms.Timer;

namespace StreamerBotLogViewer
{
    public class LogViewer : Form
    {
        private readonly string logDir = "logs";
        private string logFilePath = string.Empty;
        private FileStream? logStream;
        private StreamReader? logReader;
        private readonly RichTextBox logBox;
        private readonly TextBox filterBox;
        private readonly CheckBox autoScrollBox;
        private readonly CheckBox errorFilterBox;
        private readonly CheckBox warningFilterBox;
        private readonly CheckBox regexFilterBox;
        private readonly Button exportButton;
        private readonly Timer pollTimer;
        private readonly NotifyIcon trayIcon;
        private readonly ContextMenuStrip trayMenu;
        private readonly DateTimePicker datePicker;
        private readonly TextBox alertBox;
        private readonly TextBox searchBox;
        private readonly Button searchButton;
        private readonly FlowLayoutPanel keywordPanel;
        private readonly Dictionary<string, CheckBox> keywordToggles = new();
        private readonly List<string> pinnedEntries = new();
        private readonly ComboBox profileBox;
        private readonly Button saveProfileButton;
        private readonly Button pinButton;
        private readonly ListBox pinnedListBox;
        private readonly Button exportPinsButton;
        private readonly Dictionary<string, List<string>> keywordGroupedLines = new();

        public LogViewer()
        {
            Text = "Streamer.bot Log Viewer";
            Size = new Size(800, 600);

            logBox = new RichTextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericMonospace, 9),
                BackColor = Color.Black,
                ForeColor = Color.LightGreen
            };

            filterBox = new TextBox { Dock = DockStyle.Top, PlaceholderText = "Filter..." };
            alertBox = new TextBox { Dock = DockStyle.Top, PlaceholderText = "Keyword Alerts (comma-separated)..." };
            autoScrollBox = new CheckBox { Text = "Auto Scroll", Dock = DockStyle.Bottom, Checked = true };
            errorFilterBox = new CheckBox { Text = "Error", Dock = DockStyle.Bottom };
            warningFilterBox = new CheckBox { Text = "Warning", Dock = DockStyle.Bottom };
            regexFilterBox = new CheckBox { Text = "Regex", Dock = DockStyle.Bottom };
            exportButton = new Button { Text = "Export Log", Dock = DockStyle.Bottom };
            exportPinsButton = new Button { Text = "Export Pins", Dock = DockStyle.Bottom };
            datePicker = new DateTimePicker { Dock = DockStyle.Top, Format = DateTimePickerFormat.Short };
            searchBox = new TextBox { Dock = DockStyle.Top, PlaceholderText = "Search keyword and jump..." };
            searchButton = new Button { Text = "Jump to Next", Dock = DockStyle.Top };
            keywordPanel = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 30, AutoScroll = true };
            profileBox = new ComboBox { Dock = DockStyle.Top, DropDownStyle = ComboBoxStyle.DropDownList };
            saveProfileButton = new Button { Text = "Save Profile", Dock = DockStyle.Top };
            pinButton = new Button { Text = "Pin Line", Dock = DockStyle.Top };
            pinnedListBox = new ListBox { Dock = DockStyle.Right, Width = 200 };

            Controls.Add(logBox);
            Controls.Add(pinButton);
            Controls.Add(searchButton);
            Controls.Add(searchBox);
            Controls.Add(profileBox);
            Controls.Add(saveProfileButton);
            Controls.Add(alertBox);
            Controls.Add(filterBox);
            Controls.Add(datePicker);
            Controls.Add(exportButton);
            Controls.Add(exportPinsButton);
            Controls.Add(regexFilterBox);
            Controls.Add(warningFilterBox);
            Controls.Add(errorFilterBox);
            Controls.Add(autoScrollBox);
            Controls.Add(keywordPanel);
            Controls.Add(pinnedListBox);

            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Restore", null, (s, e) => Show());
            trayMenu.Items.Add("Exit", null, (s, e) => Application.Exit());

            trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Visible = true,
                ContextMenuStrip = trayMenu,
                Text = "Streamer.bot Log Viewer"
            };
            trayIcon.DoubleClick += (s, e) => Show();

            Resize += (s, e) => { if (WindowState == FormWindowState.Minimized) Hide(); };
            datePicker.ValueChanged += (s, e) => ReloadLogFile();

            pollTimer = new Timer();
            pollTimer.Interval = 1000;
            pollTimer.Tick += (s, e) => ReadNewLines();
            pollTimer.Start();

            filterBox.TextChanged += (s, e) => ApplyFilter();
            alertBox.TextChanged += (s, e) => SaveKeywordsAndApply();
            errorFilterBox.CheckedChanged += (s, e) => ApplyFilter();
            warningFilterBox.CheckedChanged += (s, e) => ApplyFilter();
            regexFilterBox.CheckedChanged += (s, e) => ApplyFilter();
            exportButton.Click += (s, e) => ExportLog();
            exportPinsButton.Click += (s, e) => ExportPinned();
            searchButton.Click += (s, e) => SearchAndJump();
            saveProfileButton.Click += (s, e) => SaveProfile();
            profileBox.SelectedIndexChanged += (s, e) => LoadProfile(profileBox.SelectedItem?.ToString());
            pinButton.Click += (s, e) => PinCurrentLine();

            LoadProfiles();
            LoadLogFile();
        }

        private void ReloadLogFile()
        {
            logStream?.Dispose();
            logReader?.Dispose();
            LoadLogFile();
            ApplyFilter();
        }

        private void LoadLogFile()
        {
            string dateStr = datePicker.Value.ToString("yyyyMMdd");
            logFilePath = Path.Combine(logDir, $"log_{dateStr}.log");
            if (!File.Exists(logFilePath)) return;
            logStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            logReader = new StreamReader(logStream, Encoding.UTF8);
        }

        private DateTime ExtractTimestamp(string line)
        {
            var match = Regex.Match(line, @"\[(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})\]");
            if (match.Success && DateTime.TryParse(match.Groups[1].Value, out DateTime result))
                return result;
            return DateTime.MinValue;
        }

        private void AppendColoredLine(string line, Color? customColor = null)
        {
            Color color = customColor ?? Color.LightGreen;
            if (line.Contains("ERROR", StringComparison.OrdinalIgnoreCase)) color = Color.Red;
            else if (line.Contains("WARNING", StringComparison.OrdinalIgnoreCase)) color = Color.Yellow;
            else if (keywordToggles.Keys.Any(k => line.Contains(k, StringComparison.OrdinalIgnoreCase))) color = Color.Cyan;

            int start = logBox.TextLength;
            logBox.AppendText(line + Environment.NewLine);
            logBox.Select(start, line.Length);
            logBox.SelectionColor = color;

            if (autoScrollBox.Checked)
            {
                logBox.SelectionStart = logBox.Text.Length;
                logBox.ScrollToCaret();
            }
        }
        private bool ShouldDisplayFiltered(string line)
        {
            bool matchesFilter = string.IsNullOrWhiteSpace(filterBox.Text) ||
                (regexFilterBox.Checked ? SafeRegexMatch(line, filterBox.Text) : line.Contains(filterBox.Text, StringComparison.OrdinalIgnoreCase));

            bool matchesError = !errorFilterBox.Checked || line.Contains("ERROR", StringComparison.OrdinalIgnoreCase);
            bool matchesWarning = !warningFilterBox.Checked || line.Contains("WARNING", StringComparison.OrdinalIgnoreCase);

            return matchesFilter && matchesError && matchesWarning;
        }

        private bool SafeRegexMatch(string input, string pattern)
        {
            try
            {
                return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid regex: " + ex.Message);
                return false;
            }
        }

        private void ApplyFilter()
        {
            if (logStream == null || logReader == null) return;
            logStream.Seek(0, SeekOrigin.Begin);
            logReader.DiscardBufferedData();
            logBox.Clear();
            keywordGroupedLines.Clear();

            string? line;
            while ((line = logReader.ReadLine()) != null)
            {
                if (ShouldDisplayFiltered(line))
                {
                    string matchedKeyword = keywordToggles.FirstOrDefault(k => k.Value.Checked && line.Contains(k.Key, StringComparison.OrdinalIgnoreCase)).Key;
                    if (!string.IsNullOrEmpty(matchedKeyword))
                    {
                        if (!keywordGroupedLines.ContainsKey(matchedKeyword))
                            keywordGroupedLines[matchedKeyword] = new List<string>();
                        keywordGroupedLines[matchedKeyword].Add(line);
                    }
                    else
                    {
                        AppendColoredLine(line);
                    }
                }
            }

            foreach (var group in keywordGroupedLines.OrderBy(g => g.Key))
            {
                AppendColoredLine("[+] " + group.Key + " Matches (" + group.Value.Count + ")", Color.Magenta);
                foreach (string groupedLine in group.Value.OrderBy(l => ExtractTimestamp(l)))
                {
                    AppendColoredLine("    " + groupedLine);
                }
            }
        }

        private bool ShouldDisplayLine(string line)
        {
            bool matchesFilter = string.IsNullOrWhiteSpace(filterBox.Text) ||
                (regexFilterBox.Checked
                    ? Regex.IsMatch(line, filterBox.Text, RegexOptions.IgnoreCase)
                    : line.Contains(filterBox.Text, StringComparison.OrdinalIgnoreCase));

            bool matchesError = !errorFilterBox.Checked || line.Contains("ERROR", StringComparison.OrdinalIgnoreCase);
            bool matchesWarning = !warningFilterBox.Checked || line.Contains("WARNING", StringComparison.OrdinalIgnoreCase);

            return matchesFilter && matchesError && matchesWarning;
        }
        private void ReadNewLines()
        {
            if (logReader == null) return;

            string? line;
            while ((line = logReader.ReadLine()) != null)
            {
                if (ShouldDisplayLine(line))
                {
                    AppendColoredLine(line);
                    CheckAlerts(line);
                }
            }
        }

        private void AppendColoredLine(string line)
        {
            Color color = Color.LightGreen;
            if (line.Contains("ERROR", StringComparison.OrdinalIgnoreCase)) color = Color.Red;
            else if (line.Contains("WARNING", StringComparison.OrdinalIgnoreCase)) color = Color.Yellow;

            foreach (var kvp in keywordToggles)
            {
                if (kvp.Value.Checked && line.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase))
                {
                    color = Color.Cyan;
                    break;
                }
            }

            int start = logBox.TextLength;
            logBox.AppendText(line + Environment.NewLine);
            logBox.Select(start, line.Length);
            logBox.SelectionColor = color;

            if (autoScrollBox.Checked)
            {
                logBox.SelectionStart = logBox.Text.Length;
                logBox.ScrollToCaret();
            }
        }

        private void CheckAlerts(string line)
        {
            foreach (var kvp in keywordToggles)
            {
                if (kvp.Value.Checked && line.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase))
                {
                    trayIcon.BalloonTipTitle = "Alert Triggered!";
                    trayIcon.BalloonTipText = $"Keyword found: {kvp.Key}";
                    trayIcon.ShowBalloonTip(5000);
                    FlashWindow(this.Handle, true);
                    break;
                }
            }
        }

        private void SearchAndJump()
        {
            string search = searchBox.Text.Trim();
            if (string.IsNullOrEmpty(search)) return;

            int index = logBox.Text.IndexOf(search, logBox.SelectionStart + 1, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                logBox.SelectionStart = index;
                logBox.SelectionLength = search.Length;
                logBox.ScrollToCaret();
                logBox.Focus();
            }
            else
            {
                MessageBox.Show("No more matches found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        private void ExportLog()
        {
            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                FileName = "filtered_log.txt"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, logBox.Text);
                MessageBox.Show("Log exported successfully.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ExportPinned()
        {
            if (pinnedEntries.Count == 0)
            {
                MessageBox.Show("No pinned entries to export.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog { Filter = "Text Files|*.txt", Title = "Export Pinned Entries" };
            if (sfd.ShowDialog() == DialogResult.OK)
                File.WriteAllLines(sfd.FileName, pinnedEntries);
        }

        private void PinCurrentLine()
        {
            string? line = logBox.Lines.FirstOrDefault(l => logBox.GetLineFromCharIndex(logBox.SelectionStart) == Array.IndexOf(logBox.Lines, l));
            if (!string.IsNullOrWhiteSpace(line) && !pinnedEntries.Contains(line))
            {
                pinnedEntries.Add(line);
                pinnedListBox.Items.Add(line);
                MessageBox.Show("Line pinned.", "Pin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SaveKeywordsAndApply()
        {
            keywordPanel.Controls.Clear();
            keywordToggles.Clear();
            var keywords = alertBox.Text.Split(',').Select(k => k.Trim()).Where(k => !string.IsNullOrEmpty(k));
            foreach (var keyword in keywords)
            {
                var box = new CheckBox { Text = keyword, Checked = true, AutoSize = true };
                keywordPanel.Controls.Add(box);
                keywordToggles[keyword] = box;
            }
            ApplyFilter();
        }

        private void SaveProfile()
        {
            string profileName = Microsoft.VisualBasic.Interaction.InputBox("Enter profile name:", "Save Profile");
            if (string.IsNullOrWhiteSpace(profileName)) return;

            var keywords = keywordToggles.Keys.ToArray();
            File.WriteAllLines(Path.Combine(logDir, $"profile_{profileName}.txt"), keywords);
            LoadProfiles();
        }

        private void LoadProfiles()
        {
            profileBox.Items.Clear();
            foreach (var file in Directory.GetFiles(logDir, "profile_*.txt"))
                profileBox.Items.Add(Path.GetFileNameWithoutExtension(file).Replace("profile_", ""));
        }

        private void LoadProfile(string? profile)
        {
            if (string.IsNullOrWhiteSpace(profile)) return;
            string path = Path.Combine(logDir, $"profile_{profile}.txt");
            if (!File.Exists(path)) return;
            alertBox.Text = string.Join(", ", File.ReadAllLines(path));
        }
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogViewer());
        }
    }
}
