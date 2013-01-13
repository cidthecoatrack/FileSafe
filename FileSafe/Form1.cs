using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Ionic.Zip;
using Ionic.Zlib;

namespace FileSafe
{
    public partial class Form1 : Form
    {
        LinkedList<string> originals;
        Queue<string> Errors;
        Options configuration;
        Thread TimerThread;
        Thread WorkThread;
        bool PauseTimer;

        // This delegate enables asynchronous calls for setting
        // various aspects of the form
        delegate void SetTextCallback(Label label, string text);
        delegate void RefreshGroupCallback();
        delegate void SetProgressBarMaxCallback(int max);
        delegate void SetProgressBarValueCallback(int value);
        delegate void EnableButtonCallback(Button button);
        delegate void SetGroupCallback(GroupBox groupBox, bool enabled);
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FileStream input;
            FileStream output;
            BinaryFormatter binary = new BinaryFormatter();
            PauseTimer = false;
            ElapsedTimeLabel.Text = "";
            TaskLabel.Text = "";

            //attempt to locate the saved back-up directory
            try
            {
                input = new FileStream("Configuration", FileMode.Open, FileAccess.Read);
                configuration = (Options)binary.Deserialize(input);
                input.Close();

                if (!configuration.BackUpDirectory.EndsWith("\\"))
                {
                    configuration.BackUpDirectory += "\\";
                }
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException)
                {
                    
                    MessageBox.Show("No saved configuration file.  Please choose a back-up directory.", "Error 01: No Config file");
                    FolderBrowserDialog open = new FolderBrowserDialog();
                    DialogResult answer = open.ShowDialog();
                    if (answer == DialogResult.Cancel)
                    {
                        this.Close();
                    }
                    
                    configuration = new Options(open.SelectedPath);
                    if (!configuration.BackUpDirectory.EndsWith("\\"))
                    {
                        configuration.BackUpDirectory += "\\";
                    }

                    output = new FileStream("Configuration", FileMode.OpenOrCreate, FileAccess.Write);
                    binary.Serialize(output, configuration);
                    output.Close();
                }
                else
                {
                    MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }

            if (!Directory.Exists(configuration.BackUpDirectory))
            {
                DialogResult answer = MessageBox.Show("Back-up directory could not be found.  Choose a different back-up directory (No quits the program)", String.Format("Error 02: {0} Not Found", configuration.BackUpDirectory), MessageBoxButtons.YesNo);
                if (answer == DialogResult.Yes)
                {
                    FolderBrowserDialog open = new FolderBrowserDialog();
                    answer = open.ShowDialog();
                    if (answer == DialogResult.Cancel)
                    {
                        return;
                    }
                    configuration.BackUpDirectory = open.SelectedPath;
                    if (!configuration.BackUpDirectory.EndsWith("\\"))
                    {
                        configuration.BackUpDirectory += "\\";
                    }

                    //save the new backup directory
                    output = new FileStream("Configuration", FileMode.OpenOrCreate, FileAccess.Write);
                    binary.Serialize(output, configuration);
                    output.Close();
                }
                else
                {
                    this.Close();
                }
            }

            //create backup zip file if one does not already exist

            if (!File.Exists(configuration.BackUpDirectory + "UserBackup.zip"))
            {
                using (ZipFile zip = new ZipFile(configuration.BackUpDirectory + "UserBackup.zip"))
                {
                    zip.Save();
                }
            }

            //utilize the configuration of the program
            BackUpDirectoryText.Text = configuration.BackUpDirectory;
            AllDirLoadCheckBox.Checked = configuration.CheckOriginalsOnLoad;
            AllExcpLoadCheckBox.Checked = configuration.CheckExceptionsOnLoad;

            foreach (string item in configuration.OriginalDirectories)
            {
                Application.DoEvents();
                DirChecklist.Items.Add(item);
            }

            foreach (string item in configuration.Exceptions)
            {
                Application.DoEvents();
                ExceptChecklist.Items.Add(item);
            }

            if (configuration.CheckOriginalsOnLoad)
            {
                for (int i = 0; i < DirChecklist.Items.Count; i++)
                {
                    Application.DoEvents();
                    DirChecklist.SetItemChecked(i, true);
                }
            }

            if (configuration.CheckExceptionsOnLoad)
            {
                for (int i = 0; i < ExceptChecklist.Items.Count; i++)
                {
                    Application.DoEvents(); 
                    ExceptChecklist.SetItemChecked(i, true);
                }
            }
        }

        private void NestedAccessCheck(string directory)
        {
            string[] tempdoc;
            bool ExceptionFile;

            Update("Checking " + directory + " (Program may not respond, but it is still running correctly.  Just be patient.)", false);

            try
            {
                tempdoc = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
                SetMax(tempdoc.Length);
                foreach (string file in tempdoc)
                {
                    Application.DoEvents();
                    ExceptionFile = false;

                    if (configuration.Exceptions.Contains(file) && ExceptChecklist.GetItemChecked(ExceptChecklist.FindStringExact(file)))
                    {
                        ExceptionFile = true;
                    }
                    else
                    {
                        foreach (string exception in configuration.Exceptions)
                        {
                            Application.DoEvents();
                            if (file.StartsWith(exception) && ExceptChecklist.GetItemChecked(ExceptChecklist.FindStringExact(exception)))
                            {
                                ExceptionFile = true;
                                break;
                            }
                        }
                    }

                    if (!originals.Contains(file) && !file.EndsWith(".tmp") && !ExceptionFile)
                    {
                        originals.AddLast(file);
                        Update("Collecting " + file, true);
                    }
                    else
                    {
                        Update("Skipping " + file, true);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                try
                {
                    tempdoc = Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly);
                    SetMax(tempdoc.Length);
                    foreach (string file in tempdoc)
                    {
                        Application.DoEvents();
                        ExceptionFile = false;

                        if (configuration.Exceptions.Contains(file) && ExceptChecklist.GetItemChecked(ExceptChecklist.FindStringExact(file)))
                        {
                            ExceptionFile = true;
                        }
                        else
                        {
                            foreach (string exception in configuration.Exceptions)
                            {
                                Application.DoEvents();
                                if (file.StartsWith(exception) && ExceptChecklist.GetItemChecked(ExceptChecklist.FindStringExact(exception)))
                                {
                                    ExceptionFile = true;
                                    break;
                                }
                            }
                        }

                        if (!originals.Contains(file) && !file.EndsWith(".tmp") && !ExceptionFile)
                        {
                            originals.AddLast(file);
                            Update("Collecting " + file, true);
                        }
                        else
                        {
                            Update("Skipping " + file, true);
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    Errors.Enqueue(directory);
                    return;
                }

                tempdoc = Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly);
                foreach (string dir in tempdoc)
                {
                    Application.DoEvents();
                    NestedAccessCheck(dir);
                }
            }
        }

        private int[] BackUp()
        {            
            int[] count = new int[2];
            
            Update("Getting original files...", false);
            originals = new LinkedList<string>();
            Errors = new Queue<string>();
            Queue<string> ToRemove = new Queue<string>();

            foreach (string Dir in DirChecklist.CheckedItems)
            {
                Application.DoEvents();
                Reset();
                NestedAccessCheck(Dir);
            }

            if (Errors.Count != 0)
            {
                string output = "FileSafe does not have rights to the following directories, and cannot verify what files are within these folders:\n\n";
                while (Errors.Count != 0)
                {
                    Application.DoEvents();
                    output += String.Format("\t{0}\n", Errors.Dequeue());
                }
                output += "\nContinue with backup?";

                InterruptTimer();
                DialogResult result = MessageBox.Show(output, "Error: Insufficient Access Rights", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return count;
                }
                InterruptTimer();
            }

            Reset();

            Update("Getting available space for backup...", false);
            long backupSize = 0;
            string root = configuration.BackUpDirectory; int j = 0;
            while (root[j] != '\\')
            {
                Application.DoEvents();
                j++;
            }
            if (j != root.Length - 1)
            {
                root = root.Remove(j + 1);
            }

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                Application.DoEvents();
                if (drive.Name == root)
                {
                    backupSize += drive.TotalFreeSpace;
                    break;
                }
            }

            if (!File.Exists(configuration.BackUpDirectory + "UserBackup.zip"))
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.Save();
                }
            }

            using (ZipFile zip = ZipFile.Read(configuration.BackUpDirectory + "UserBackup.zip"))
            {
                backupSize += zip.NumberOfSegmentsForMostRecentSave * zip.MaxOutputSegmentSize;

                Reset();
                Update("Estimating compression ratio...", false);
                
                double CompressionRatio = 1;
                long Compressed = 1;
                long Uncompressed = 1;

                SetMax(zip.Count);
                foreach (ZipEntry e in zip)
                {
                    Application.DoEvents();
                    Compressed += e.CompressedSize;
                    Uncompressed += e.UncompressedSize;
                    Update(String.Format("Estimated compression ratio is {0:0.00}%", (double)Compressed / (double)Uncompressed * 100), true);
                }

                CompressionRatio = (double)Compressed / (double)Uncompressed;

                Reset();

                Update("Computing size requirements...", false);
                SetMax(originals.Count);
                long originalSize = 0;
                foreach (string file in originals)
                {
                    Application.DoEvents();
                    
                    try { originalSize += new FileInfo(file).Length; }
                    catch (FileNotFoundException error)
                    {
                        Errors.Enqueue(error.FileName);
                        ToRemove.Enqueue(error.FileName);
                    }

                    if ((double)originalSize * CompressionRatio > backupSize)
                    {
                        InterruptTimer();
                        MessageBox.Show("Information to back up is too large for the storage location.  Please select fewer items to back up, or choose more exceptions.", "Back up too large", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        InterruptTimer();
                        return count;
                    }
                    else
                    {
                        Update(String.Format("Computing projected compressed backup size: {0} [{1:0.00}% Complete] out of {2} available", SizeString(originalSize, CompressionRatio), (double)progressBar.Value / (double)progressBar.Maximum * 100, SizeString(backupSize)), true);
                    }
                }

                while (ToRemove.Count != 0)
                {
                    Application.DoEvents();
                    originals.Remove(ToRemove.Dequeue());
                }

                SetMax(zip.Count + originals.Count);
                Queue<ZipEntry> Remove = new Queue<ZipEntry>();

                //Purge files with no corresponding original file
                foreach (ZipEntry e in zip)
                {
                    Application.DoEvents();
                    Update("Scanning for files to purge...", true);
                    string name = "C:\\" + e.FileName;
                    name = name.Replace('/', '\\');
                    if (!originals.Contains(name))
                    {
                        Remove.Enqueue(e);
                    }
                }

                while (Remove.Count != 0)
                {
                    Application.DoEvents();
                    Update("Purging " + Remove.Peek().FileName, false);
                    count[0]++;
                    zip.RemoveEntry(Remove.Dequeue());
                }

                //Looking for files that need to be backed up
                foreach (string file in originals)
                {
                    Application.DoEvents();

                    //if there isn't a backup of a file
                    if (!zip.ContainsEntry(file.Remove(0, 3).Replace('\\', '/')))
                    {
                        Update("Adding " + file, true);
                        zip.AddFile(file);
                        count[1]++;
                    }
                    //if the file is there, check to see if the original is newer than the backup
                    else
                    {
                        int compare = new FileInfo(file).LastWriteTime.CompareTo(zip[file].LastModified);
                        if (compare > 0 && compare != 1)
                        {
                            Update("Adding " + file, true);
                            zip.UpdateFile(file);
                            count[1]++;
                        }
                        else
                        {
                            Update("Scanning for files to back up...", true);
                        }
                    }
                }

                Update("Saving .zip file...", false);
                bool donesaving = false;
                zip.UseZip64WhenSaving = Zip64Option.Always;
                //if (zip.MaxOutputSegmentSize == 0)
                //{
                //    zip.MaxOutputSegmentSize = 2 * (int)Math.Pow(1024, 2);
                //}
                zip.SaveProgress += SaveProgress;

                while (!donesaving)
                {
                    Application.DoEvents();
                    try
                    {
                        if (count[0] != 0 || count[1] != 0)
                        {
                            SetMax(zip.Count);
                            zip.Save();
                        }
                        donesaving = true;
                    }
                    catch (FileNotFoundException)
                    {
                        Update("Error: a file was not found.  Cleaning and restarting the .zip save", false);

                        //remove all files that may have been deleted and/or moved since the entry was added to the zip
                        foreach (ZipEntry e in zip)
                        {
                            Application.DoEvents();
                            if (!File.Exists("c:\\" + e.FileName))
                            {
                                Errors.Enqueue(e.FileName);
                                ToRemove.Enqueue(e.FileName);
                            }
                        }

                        while (ToRemove.Count != 0)
                        {
                            Application.DoEvents();
                            zip.RemoveEntry(ToRemove.Dequeue());
                            count[1]--;
                        }

                        //delete the temporary file that was created
                        if (zip.TempFileFolder != null)
                        {
                            string[] tempFiles = Directory.GetFiles(zip.TempFileFolder, ".tmp", SearchOption.TopDirectoryOnly);
                            foreach (string file in tempFiles)
                            {
                                Application.DoEvents();
                                File.Delete(file);
                            }
                        }
                    }
                    catch (IOException error)
                    {
                        Update("Error: IO.  Cleaning and canceling the .zip save", false);

                        if (zip.MaxOutputSegmentSize != 0)
                        {
                            zip.MaxOutputSegmentSize = 0;
                            Errors.Enqueue("Split zip save failed.  Saving as single file.");

                            //delete the temporary file that was created
                            string[] tempFiles = Directory.GetFiles(configuration.BackUpDirectory, ".tmp", SearchOption.TopDirectoryOnly);
                            foreach (string file in tempFiles)
                            {
                                Application.DoEvents();
                                File.Delete(file);
                            }
                        }
                        else
                        {
                            Errors.Enqueue(error.Message);
                            count[0] = count[1] = 0;

                            //delete the temporary file that was created
                            string[] tempFiles = Directory.GetFiles(configuration.BackUpDirectory, ".tmp", SearchOption.TopDirectoryOnly);
                            foreach (string file in tempFiles)
                            {
                                Application.DoEvents();
                                File.Delete(file);
                            }

                            donesaving = true;
                        }
                    }
                    catch (OutOfMemoryException error)
                    {
                        Update("Error: Out of Memory.  Cleaning and canceling the .zip save", false);

                        Errors.Enqueue(error.Message);
                        count[0] = count[1] = 0;

                        //delete the temporary file that was created
                        string[] tempFiles = Directory.GetFiles(configuration.BackUpDirectory, ".tmp", SearchOption.TopDirectoryOnly);
                        foreach (string file in tempFiles)
                        {
                            Application.DoEvents();
                            File.Delete(file);
                        }

                        donesaving = true;
                    }
                }
            }

            //output errors to a messagebox
            if (Errors.Count != 0)
            {
                string output = "Errors backing up " + Errors.Count + " files:\n";
                while (Errors.Count != 0)
                {
                    Application.DoEvents();
                    output += "\t" + Errors.Dequeue() + "\n";
                }

                InterruptTimer();
                MessageBox.Show(output, "Errors backing up files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                InterruptTimer();
            }

            return count;
        }

        //Returns a string that shows a size in memory in the most relevant size category, adjusted by a compression ratio
        private string SizeString(long originalSize, double CompressionRatio)
        {
            if ((double)originalSize * CompressionRatio < 1024.0)
            {
                return String.Format("{0} bytes", originalSize * CompressionRatio);
            }
            else if ((double)originalSize * CompressionRatio < Math.Pow(1024, 2))
            {
                return String.Format("{0:.00} KiB", (double)originalSize * CompressionRatio / 1024.0);
            }
            else if ((double)originalSize * CompressionRatio < Math.Pow(1024, 3))
            {
                return String.Format("{0:.00} MiB", (double)originalSize * CompressionRatio / Math.Pow(1024, 2));
            }

            return String.Format("{0:.00} GiB", (double)originalSize * CompressionRatio / Math.Pow(1024, 3));
        }

        //Returns a string that shows a size in memory in the most relevant size category
        private string SizeString(long originalSize)
        {
            if (originalSize < 1024)
            {
                return String.Format("{0} bytes", originalSize);
            }
            else if (originalSize < Math.Pow(1024, 2))
            {
                return String.Format("{0:.00} KiB", (double)originalSize / 1024);
            }
            else if (originalSize < Math.Pow(1024, 3))
            {
                return String.Format("{0:.00} MiB", (double)originalSize / Math.Pow(1024, 2));
            }

            return String.Format("{0:.00} GiB", (double)originalSize / Math.Pow(1024, 3));
        }

        //To be added to the zip file, so the program may display the progress
        public void SaveProgress(object sender, SaveProgressEventArgs e)
        {
            if (e.EventType == ZipProgressEventType.Saving_BeforeWriteEntry)
            {
                Application.DoEvents();
                Update(String.Format("[{0:0.00}%] {1}", (double)progressBar.Value / (double)progressBar.Maximum * 100, e.CurrentEntry.FileName), true);
            }
            else if (e.EventType == ZipProgressEventType.Saving_EntryBytesRead)
            {
                Application.DoEvents();
                Update(String.Format("[{0:0.00}%] {1}", (double)progressBar.Value / (double)progressBar.Maximum * 100, e.CurrentEntry.FileName), false, e.BytesTransferred / (0.01 * e.TotalBytesToTransfer));
            }
        }


        private void BrowseDirectoryButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.ShowDialog();
            configuration.BackUpDirectory = browser.SelectedPath;
            if (!configuration.BackUpDirectory.EndsWith("\\"))
            {
                configuration.BackUpDirectory += "\\";
            }
            BackUpDirectoryText.Text = configuration.BackUpDirectory;
            if (!Directory.Exists(configuration.BackUpDirectory))
            {
                Directory.CreateDirectory(configuration.BackUpDirectory);
            }
            Save();
        }

        private void BackUpButton_Click(object sender, EventArgs e)
        {
            if (DirChecklist.CheckedItems.Count == 0)
            {
                MessageBox.Show("No original directories selected to back up.", "No Original Directories", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            PrepforWork();
            WorkThread = new Thread(BackupHelper);
            WorkThread.Name = "BackUp";
            WorkThread.Start();
        }

        private void BackupHelper()
        {
            int[] count = BackUp();
            InterruptTimer();
            MessageBox.Show(String.Format("Back-up complete. {0} file(s) purged, {1} file(s) backed up.", count[0], count[1]));
            InterruptTimer();
            DonewithWork();
        }

        private void PrepforWork()
        {
            OperationsGroup.Enabled = true;
            Reset();
            SettingsGroup.Enabled = false;
            OptionsGroup.Enabled = false;
            BackUpButton.Enabled = false;
            RestoreButton.Enabled = false;
            TimerThread = new Thread(Timer);
            TimerThread.Name = "Timer";
            TimerThread.Start();
        }

        private void DonewithWork()
        {
            while (TimerThread.ThreadState != ThreadState.Aborted && TimerThread.ThreadState != ThreadState.Stopped)
            {
                TimerThread.Abort();
            }
            SetText(ElapsedTimeLabel, "");
            EnableButton(BackUpButton);
            EnableButton(RestoreButton);
            SetGroup(SettingsGroup, true);
            SetGroup(OptionsGroup, true);
            Reset();
            SetGroup(OperationsGroup, false);
        }

        private void NewDirButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog open = new FolderBrowserDialog();
            DialogResult answer = open.ShowDialog();
            if (answer == DialogResult.Cancel)
            {
                return;
            }
            configuration.OriginalDirectories.AddLast(open.SelectedPath);
            DirChecklist.Items.Add(open.SelectedPath);
            Save();
        }

        private void DelDirButton_Click(object sender, EventArgs e)
        {
            if (DirChecklist.Items.Count != 0)
            {
                configuration.OriginalDirectories.Remove(DirChecklist.SelectedItem.ToString());
                DirChecklist.Items.Remove(DirChecklist.SelectedItem);
                Save();
            }
        }

        private void Save()
        {
            BinaryFormatter binary = new BinaryFormatter();
            FileStream output = new FileStream("Configuration", FileMode.Create, FileAccess.Write);
            binary.Serialize(output, configuration);
            output.Close();
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            if (DirChecklist.CheckedItems.Count == 0)
            {
                MessageBox.Show("No original directories selected to restore.", "No Original Directories", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            PrepforWork();
            WorkThread = new Thread(RestoreHelper);
            WorkThread.Name = "Restore";
            WorkThread.Start();
        }

        private void RestoreHelper()
        {
            int result = Restore();
            InterruptTimer();
            MessageBox.Show(String.Format("Restoration complete. {0} files restored.", result));
            InterruptTimer();
            DonewithWork();
        }

        private int Restore()
        {
            int count = 0;
            Update("Getting original files...", false);
            
            originals = new LinkedList<string>();
            Errors = new Queue<string>();

            foreach (string Dir in DirChecklist.CheckedItems)
            {
                Application.DoEvents();
                Reset();
                NestedAccessCheck(Dir);
            }

            if (Errors.Count != 0)
            {
                string output = "FileSafe does not have rights to the following directories, and cannot verify what files are within these folders:\n\n";
                while (Errors.Count != 0)
                {
                    Application.DoEvents();
                    output += String.Format("\t{0}\n", Errors.Dequeue());
                }
                output += "\nContinue with backup?";

                InterruptTimer();
                DialogResult result = MessageBox.Show(output, "Error: Insufficient Access Rights", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return count;
                }
                InterruptTimer();
            }

            Reset();

            using (ZipFile zip = ZipFile.Read(configuration.BackUpDirectory + "UserBackup.zip"))
            {
                SetMax(zip.Count);

                //Restore missing original files
                foreach (ZipEntry e in zip)
                {
                    Application.DoEvents();
                    string name = "C:\\" + e.FileName;
                    name = name.Replace('/', '\\');
                    if (!originals.Contains(name))
                    {
                        Update("Restoring " + e.FileName, true);
                        string target = "c:\\" + e.FileName; int split = target.Length - 1;
                        while (target[split] != '\\')
                        {
                            Application.DoEvents();
                            split--;
                        }
                        target = target.Remove(split + 1);

                        e.Extract(target, ExtractExistingFileAction.OverwriteSilently);
                        count++;
                    }
                    else
                    {
                        int compare = e.LastModified.CompareTo(new FileInfo("C:\\" + e.FileName).LastWriteTime);
                        if (compare > 0 && compare != 1)
                        {
                            Update("Restoring " + e.FileName, true);
                            string target = "c:\\" + e.FileName; int split = target.Length - 1;
                            while (target[split] != '\\')
                            {
                                Application.DoEvents();
                                split--;
                            }
                            target = target.Remove(split + 1);

                            e.Extract(target, ExtractExistingFileAction.OverwriteSilently);
                            count++;
                        }
                        else
                        {
                            Update("Scanning for files to restore...", true);
                        }
                    }
                }

                if (Errors.Count != 0)
                {
                    string output = String.Format("Errors copying {0} files:\n", Errors.Count);
                    while (Errors.Count != 0)
                    {
                        Application.DoEvents();
                        output += "\t" + Errors.Dequeue() + "\n";
                    }

                    InterruptTimer();
                    MessageBox.Show(output, "Errors copying files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    InterruptTimer();
                }
            }

            return count;
        }

        private void ExceptAddButton_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("File or Directory?  \"Yes\" for file, \"No\" for directory.", "File or directory?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (answer == DialogResult.Cancel)
            {
                return;
            }
            else if (answer == DialogResult.Yes)
            {
                OpenFileDialog open = new OpenFileDialog();
                answer = open.ShowDialog();
                if (answer == DialogResult.Cancel)
                {
                    return;
                }
                configuration.Exceptions.AddLast(open.FileName);
                ExceptChecklist.Items.Add(open.FileName);
                Save();
            }
            else
            {
                FolderBrowserDialog open = new FolderBrowserDialog();
                answer = open.ShowDialog();
                if (answer == DialogResult.Cancel)
                {
                    return;
                }
                if (configuration.OriginalDirectories.Contains(open.SelectedPath))
                {
                    answer = MessageBox.Show("This path is a backup directory.  Do you wish to delete the backup directory instead?", "Delete backup directory?", MessageBoxButtons.YesNo);
                    if (answer == DialogResult.Yes)
                    {
                        configuration.OriginalDirectories.Remove(open.SelectedPath);
                        DirChecklist.Items.Remove(open.SelectedPath);
                        Save();
                    }
                }
                else
                {
                    configuration.Exceptions.AddLast(open.SelectedPath);
                    ExceptChecklist.Items.Add(open.SelectedPath);
                    Save();
                }
            }
        }

        private void ExceptDeleteButton_Click(object sender, EventArgs e)
        {
            if (ExceptChecklist.Items.Count != 0)
            {
                configuration.Exceptions.Remove(ExceptChecklist.SelectedItem.ToString());
                ExceptChecklist.Items.Remove(ExceptChecklist.SelectedItem);
                Save();
            }
        }

        private void Update(string action, bool IncreaseProgBar)
        {
            if (action.Length > 125)
            {
                action = action.Remove(123);
                action += "...";
            }
            SetText(TaskLabel, action);

            if (IncreaseProgBar)
            {
                SetProgressBarValue(progressBar.Value + 1);
            }

            RefreshGroup();
        }

        private void Update(string action, bool IncreaseProgBar, double Percentage)
        {
            if (action.Length > 120)
            {
                action = action.Remove(118);
                action += "...";
            }
            SetText(TaskLabel, String.Format("{0} {1:N0}%", action, Percentage));

            if (IncreaseProgBar)
            {
                SetProgressBarValue(progressBar.Value + 1);
            }

            RefreshGroup();
        }

        private void Reset()
        {
            SetText(TaskLabel, "");
            SetProgressBarValue(0);
            RefreshGroup();
        }

        private void SetProgressBarValue(int value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (progressBar.InvokeRequired)
            {
                SetProgressBarValueCallback d = new SetProgressBarValueCallback(SetProgressBarValue);
                this.Invoke(d, new object[] { value });
            }
            else
            {
                progressBar.Value = value;
            }
        }

        private void SetMax(int max)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (progressBar.InvokeRequired)
            {
                SetProgressBarMaxCallback d = new SetProgressBarMaxCallback(SetMax);
                this.Invoke(d, new object[] { max });
            }
            else
            {
                progressBar.Maximum = max;
                Reset();
            }
        }

        private void AllDirLoadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            configuration.CheckOriginalsOnLoad = AllDirLoadCheckBox.Checked;
            Save();
        }

        private void AllExcpLoadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            configuration.CheckExceptionsOnLoad = AllExcpLoadCheckBox.Checked;
            Save();
        }

        private void Timer()
        {
            TimeSpan ElapsedTime = new TimeSpan();

            while (true)
            {
                if (!PauseTimer)
                {
                    SetText(ElapsedTimeLabel, String.Format("Elapsed time: {0:00}:{1:00}:{2:00}", ElapsedTime.Hours, ElapsedTime.Minutes, ElapsedTime.Seconds));
                    RefreshGroup();
                    Thread.Sleep(1000);
                    ElapsedTime = ElapsedTime.Add(new TimeSpan(0, 0, 1));
                }
            }
        }

        private void AbortButton_Click(object sender, EventArgs e)
        {
            AbortButton.Enabled = false;
            AbortButton.Text = "Cancelling";

            SetText(TaskLabel, "Aborting timer thread");
            TimerThread.Abort();
            while (TimerThread.ThreadState == ThreadState.AbortRequested)
            {
                Application.DoEvents();
                SetText(TaskLabel, "Aborting timer thread");
            }

            SetText(TaskLabel, "Aborting work thread");
            WorkThread.Abort();
            while (WorkThread.ThreadState == ThreadState.AbortRequested || WorkThread.ThreadState != ThreadState.Stopped)
            {
                Application.DoEvents();
                SetText(TaskLabel, "Aborting work thread");
            }

            //delete the temporary file that was created
            string[] tempFiles = Directory.GetFiles(configuration.BackUpDirectory, ".tmp", SearchOption.TopDirectoryOnly);
            foreach (string file in tempFiles)
            {
                Application.DoEvents();
                File.Delete(file);
            }

            InterruptTimer();
            MessageBox.Show(String.Format("{0} has been cancelled.", WorkThread.Name));
            InterruptTimer();

            AbortButton.Text = "Cancel";
            AbortButton.Enabled = true;
            DonewithWork();
        }

        private void SetText(Label label, string text)
        {
            if (label != null)
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (label.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    this.Invoke(d, new object[] { label, text });
                }
                else
                {
                    label.Text = text;
                }
            }
        }

        private void EnableButton(Button button)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (button.InvokeRequired)
            {
                EnableButtonCallback d = new EnableButtonCallback(EnableButton);
                this.Invoke(d, new object[] { button });
            }
            else
            {
                button.Enabled = true;
            }
        }

        private void SetGroup(GroupBox groupBox, bool enabled)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (groupBox.InvokeRequired)
            {
                SetGroupCallback d = new SetGroupCallback(SetGroup);
                this.Invoke(d, new object[] { groupBox , enabled });
            }
            else
            {
                groupBox.Enabled = enabled;
            }
        }

        private void RefreshGroup()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (OperationsGroup.InvokeRequired)
            {
                RefreshGroupCallback d = new RefreshGroupCallback(RefreshGroup);
                this.Invoke(d, new object[] { });
            }
            else
            {
                OperationsGroup.Refresh();
            }
        }

        private void InterruptTimer()
        {
            PauseTimer = !PauseTimer;
        }
    }
}