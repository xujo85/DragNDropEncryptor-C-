using Cysharp.Collections;
using System.Diagnostics;
using System.Media;
using System.Net;
using System.Security.Cryptography;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;
using Aspose.Zip;
using SharpCompress;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection.Metadata;

namespace GUI
{
    public partial class Form1 : Form
    {
        public string[] files;
        private String heslo = "";
        public int KeySize = 256;
        public int BlockSize = 128;
        private const int SaltSize = 8;
        public Boolean deletePovodne = false;
        public static PerformanceCounter theCPUCounter = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
        public static PerformanceCounter theMemCounter = new PerformanceCounter("Process", "Working Set", Process.GetCurrentProcess().ProcessName);
        PerformanceCounter total_cpu = new PerformanceCounter("Process", "% Processor Time", "_Total");
        Stopwatch stopwatch = new Stopwatch();
        NativeMemoryArray<byte> fileAsBytes;
        public async Task<NativeMemoryArray<byte>> ReadFileAsync(String file)
        {
            using var handle = File.OpenHandle(file, FileMode.Open, FileAccess.Read, options: FileOptions.Asynchronous);
            var size = RandomAccess.GetLength(handle);
            fileAsBytes = new NativeMemoryArray<byte>(size);
            await RandomAccess.ReadAsync(handle, fileAsBytes.AsMemoryList(), 0);
            handle.Close();
            return fileAsBytes;
        }
        public static async Task<NativeMemoryArray<byte>> WriteFile(String file, NativeMemoryArray<byte> data)
        {
            using var handle = File.OpenHandle(file, FileMode.OpenOrCreate, FileAccess.Write, options: FileOptions.Asynchronous);

            await RandomAccess.WriteAsync(handle, data.AsReadOnlyMemoryList(), 0);
            handle.Close();
            return data;
        }
        public static void rar(String file)
        {
            string SourceFile = file;
            string DestinationPath = file.Substring(0, file.LastIndexOf(("\\")));
            MessageBox.Show(SourceFile);
            MessageBox.Show(DestinationPath);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = @"C:\Program Files\WinRAR\rar.exe";
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.EnableRaisingEvents = false;
            process.StartInfo.Arguments = "r " + SourceFile + " " + DestinationPath;
            process.Start();
        }

        public Form1()
        {
            InitializeComponent();
            panelBackground.BackColor = Color.FromArgb(100, 88, 44, 55);
        }

        private void dragDropBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }
        private NativeMemoryArray<byte> pokazRar(NativeMemoryArray<byte> file)
        {
            file[0] = 11;
            file[1] = 22;
            file[2] = 33;
            file[3] = 44;

            return file;
        }
        private NativeMemoryArray<byte> pokazZip(NativeMemoryArray<byte> file)
        {
            file[0] = 11;
            file[1] = 22;
            file[2] = 33;
            file[3] = 45;

            return file;
        }
        private NativeMemoryArray<byte> opravRar(NativeMemoryArray<byte> file)
        {
            file[0] = 82;
            file[1] = 97;
            file[2] = 114;
            file[3] = 33;

            return file;
        }
        private NativeMemoryArray<byte> opravZip(NativeMemoryArray<byte> file)
        {
            file[0] = 80;
            file[1] = 75;
            file[2] = 3;
            file[3] = 4;

            return file;

        }
        private async Task execute()
        {
            const string keepAliveMessage = "{\"message\": {\"type\": \"keepalive\"}}";
            await Task.Run(() =>
            {
                processAsync();
            });
        }
        private void resetCounters()
        {
            Thread.Sleep(2500);
            progressBar1.Value = 1;
            cpuLabel.Text = "CPU usage: " + 0 + " %";
            memLabel.Text = "RAM usage: " + 0 + " MB";
            errorLbl.Text = "";
            stopwatch.Reset();
        }
        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(data);
                }
            }
            return data;
        }
        private void FileDecrypt(string inputFile, string outputFile, string password)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] salt = new byte[32];

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            fsCrypt.Read(salt, 0, salt.Length);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = KeySize;
            AES.BlockSize = BlockSize;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CFB;

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int read;
            byte[] buffer = new byte[10485760];
            long length = new System.IO.FileInfo(inputFile).Length;

            try
            {
                progressBar1.Maximum = (int)(length / 4);
                progressBar1.Minimum = 1;
                progressBar1.Step = (10485760 / 4);
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Application.DoEvents();
                    fsOut.Write(buffer, 0, read);
                    progressBar1.PerformStep();
                    cpuLabel.Text = "CPU usage: " + Math.Round(((theCPUCounter.NextValue() / ((Environment.ProcessorCount) * total_cpu.NextValue())) * 1000), 2) + " %";
                    Thread.Sleep(35);
                    memLabel.Text = "RAM usage: " + Math.Round((theMemCounter.NextValue() / 1000000F), 0) + " MB";
                    timeLbl.Text = "Time elapsed: " + (stopwatch.ElapsedMilliseconds / 1000) + " Sec";
                }
                progressBar1.Value = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                resetCounters();
            }
            try
            {
                cs.Close();
                resetCounters();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error by closing CryptoStream: " + ex.Message);
                resetCounters();
            }
            finally
            {
                fsOut.Close();
                fsCrypt.Close();
                resetCounters();
            }
        }


        private void FileEncrypt(string inputFile, string password)
        {
            //generate random salt
            byte[] salt = GenerateRandomSalt();

            //create output file name
            FileStream fsCrypt = new FileStream(inputFile.Substring(0, inputFile.Length - 4) + ".uwu", FileMode.Create);

            //convert password string to byte arrray
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            //Set Rijndael symmetric encryption algorithm
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = KeySize;
            AES.BlockSize = BlockSize;
            AES.Padding = PaddingMode.PKCS7;

            //http://stackoverflow.com/questions/2659214/why-do-i-need-to-use-the-rfc2898derivebytes-class-in-net-instead-of-directly
            //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);

            //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
            AES.Mode = CipherMode.CFB;

            // write salt to the begining of the output file, so in this case can be random every time
            fsCrypt.Write(salt, 0, salt.Length);

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            //create a buffer (1mb) so only this amount will allocate in the memory and not the whole file
            byte[] buffer = new byte[10485760];
            int read;

            long length = new System.IO.FileInfo(inputFile).Length;

            try
            {
                progressBar1.Maximum = (int)(length / 4);
                progressBar1.Minimum = 1;
                progressBar1.Step = (10485760 / 4);
                while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Application.DoEvents(); // -> for responsive GUI, using Task will be better!
                    cs.Write(buffer, 0, read);
                    progressBar1.PerformStep();
                    cpuLabel.Text = "CPU usage: " + Math.Round(((theCPUCounter.NextValue() / ((Environment.ProcessorCount) * total_cpu.NextValue())) * 1000), 2) + " %";
                    Thread.Sleep(35);
                    memLabel.Text = "RAM usage: " + Math.Round((theMemCounter.NextValue() / 1000000F), 0) + " MB";
                    timeLbl.Text = "Time elapsed: " + (stopwatch.ElapsedMilliseconds / 1000) + " Sec";
                }
                // Close up
                fsIn.Close();
                resetCounters();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                cs.Close();
                fsCrypt.Close();
                resetCounters();
            }
            finally
            {
                cs.Close();
                fsCrypt.Close();
                resetCounters();
            }
        }
        private void printBytes(NativeMemoryArray<byte> file)
        {
            MessageBox.Show('\n' + "bytes:" + file[0] + " " + file[1] + " " + file[2] + " " + file[3]);
        }
        private void playDone() // defining the function
        {
            SoundPlayer audio = new SoundPlayer(Properties.Resources.done1); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
            audio.Play();
        }
        private void playError() // defining the function
        {
            SoundPlayer audio = new SoundPlayer(Properties.Resources.error); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
            audio.Play();
        }
        public async Task processAsync()
        {
            heslo = passwdBox.Text;
            foreach (var file in files)
            {
                dragDropBox.Items.Add(file);
            }
            foreach (string file in dragDropBox.Items)
            {
                System.GC.Collect();
                timeLbl.Text = "Time elapsed: " + 0 + " Sec";
                stopwatch.Start();
                await ReadFileAsync(file);

                if (fileAsBytes[0] == 82 && fileAsBytes[1] == 97 && fileAsBytes[2] == 114 && fileAsBytes[3] == 33)
                {
                    fileAsBytes = pokazRar(fileAsBytes);
        
                    await WriteFile(Path.GetTempPath() + "uwu.tmp", fileAsBytes);

                    FileEncrypt(Path.GetTempPath() + "uwu.tmp", heslo);
                    if (deletePovodne)
                    {
                        System.IO.File.Delete(file);

                    }
                    System.IO.File.Move(Path.GetTempPath() + "uwu.uwu", file.Substring(0, file.Length - 4) + ".uwu");
                    System.IO.File.Delete(Path.GetTempPath() + "uwu.tmp");
                    System.IO.File.Delete(Path.GetTempPath() + "uwu.uwu");
                    Console.WriteLine('\n' + KeySize);
                    errorLbl.ForeColor = Color.GreenYellow;
                    errorLbl.Text = "Done!";
                    playDone();
                    resetCounters();
                }
                else if (fileAsBytes[0] == 80 && fileAsBytes[1] == 75 && fileAsBytes[2] == 3 && fileAsBytes[3] == 4)
                {
                    fileAsBytes = pokazZip(fileAsBytes);
                    await WriteFile(Path.GetTempPath() + "uwu.tmp", fileAsBytes);
                    FileEncrypt(Path.GetTempPath() + "uwu.tmp", heslo);
                    if (deletePovodne)
                    {
                        System.IO.File.Delete(file);

                    }
                    System.IO.File.Move(Path.GetTempPath() + "uwu.uwu", file.Substring(0, file.Length - 4) + ".uwu");
                    System.IO.File.Delete(Path.GetTempPath() + "uwu.tmp");
                    System.IO.File.Delete(Path.GetTempPath() + "uwu.uwu");
                    errorLbl.ForeColor = Color.GreenYellow;
                    errorLbl.Text = "Done!";
                    playDone();
                    resetCounters();               
                }
                else
                {
                    try
                    {
                        FileDecrypt(file, Path.GetTempPath() + "uwu.tmp", heslo);

                        await ReadFileAsync(Path.GetTempPath() + "uwu.tmp");
          
                        if (fileAsBytes[0] == 11 && fileAsBytes[1] == 22 && fileAsBytes[2] == 33 && fileAsBytes[3] == 44)
                        {
                            fileAsBytes = opravRar(fileAsBytes);
                            await WriteFile(file.Substring(0, file.Length - 4) + ".rar", fileAsBytes);

                            if (deletePovodne)
                            {
                                System.IO.File.Delete(file);

                            }
                            Console.WriteLine('\n' + KeySize);
                            System.IO.File.Delete(Path.GetTempPath() + "uwu.tmp");
                            System.IO.File.Delete(Path.GetTempPath() + "uwu.uwu");
                            errorLbl.ForeColor = Color.GreenYellow;
                            errorLbl.Text = "Done!";
                            playDone();
                            resetCounters();
                        }
                        else if (fileAsBytes[0] == 11 && fileAsBytes[1] == 22 && fileAsBytes[2] == 33 && fileAsBytes[3] == 45)
                        {
                            fileAsBytes = opravZip(fileAsBytes);
                            await WriteFile(file.Substring(0, file.Length - 4) + ".zip", fileAsBytes);
                            if (deletePovodne)
                            {
                                System.IO.File.Delete(file);

                            }
                            Console.WriteLine('\n' + KeySize);
                            System.IO.File.Delete(Path.GetTempPath() + "uwu.tmp");
                            System.IO.File.Delete(Path.GetTempPath() + "uwu.uwu");
                            errorLbl.ForeColor = Color.GreenYellow;
                            errorLbl.Text = "Done!";
                            playDone();
                            resetCounters();
                        }
                        else
                        {
                            System.IO.File.Delete(file.Substring(0, file.Length - 4));
                            System.IO.File.Delete(Path.GetTempPath() + "uwu.tmp");
                            System.IO.File.Delete(Path.GetTempPath() + "uwu.uwu");
                            errorLbl.ForeColor = Color.PaleVioletRed;
                            errorLbl.Text = "Error: " + "Wrong password or encrypted in other bit standard...";
                            playError();
                            resetCounters();
                        }
                    }
                    catch (Exception ed)
                    {
                        //playError();
                        if (ed.Message == "Exception of type 'System.OutOfMemoryException' was thrown.")
                        {
                            System.IO.File.Delete(file.Substring(0, file.Length - 4));
                            System.IO.File.Delete(Path.GetTempPath() + "uwu.tmp");
                            System.IO.File.Delete(Path.GetTempPath() + "uwu.uwu");
                            errorLbl.ForeColor = Color.PaleVioletRed;
                            errorLbl.Text = "Error: " + "Out of memory....";
                            playError();
                            resetCounters();
                        }
                        else
                        {
                            Console.Out.WriteLine(ed.Message);
                            System.IO.File.Delete(file.Substring(0, file.Length - 4));
                             System.IO.File.Delete(Path.GetTempPath() + "uwu.tmp");
                            System.IO.File.Delete(Path.GetTempPath() + "uwu.uwu");
                            errorLbl.ForeColor = Color.PaleVioletRed;
                            errorLbl.Text = "Error: " + "Wrong password or encrypted in other bit standard...";
                            playError();
                            resetCounters();
                        }
                    }
                }
                //dragDropBox.Items.Remove(file);
                stopwatch.Stop();
                System.GC.Collect();
            }

        }

        private void dragDropBox_DragDrop_1(object sender, DragEventArgs e)
        {
            dragDropBox.Items.Clear();

            heslo = passwdBox.Text;
            files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            try
            {
                execute();
            }
            catch (Exception eed)
            {
                if (eed.Message == "Start may not be called on a promise-style task.")
                {
                    errorLbl.ForeColor = Color.GreenYellow;
                    errorLbl.Text = "Processing... if progress bar is not moving, file is too big.";
                }
                else
                {
                    MessageBox.Show(eed.Message);
                }
            }
        }

        private void dragDropBox_DragEnter_1(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            MessageBox.Show(Path.GetTempPath());
            File.Create(Path.GetTempPath() + "a.data");

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            rar(passwdBox.Text);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                KeySize = 128;
                BlockSize = 128;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                KeySize = 256;
                BlockSize = 128;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                deletePovodne = true;
            }
            else
            {
                deletePovodne = false;
            }
        }
    }
}