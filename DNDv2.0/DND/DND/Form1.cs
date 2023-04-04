 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DND
{
    public partial class Form1 : Form
    {
        public string[] files;
        private String heslo = "";
        public int KeySize = 256;
        public int BlockSize = 256;
        private const int SaltSize = 8;
        public Boolean deletePovodne = false;
        public static PerformanceCounter theCPUCounter =  new PerformanceCounter("Process", "% Processor Time",Process.GetCurrentProcess().ProcessName);
        public static PerformanceCounter theMemCounter = new PerformanceCounter("Process", "Working Set", Process.GetCurrentProcess().ProcessName);
        PerformanceCounter total_cpu = new PerformanceCounter("Process", "% Processor Time", "_Total");

        public static void getCpuUsage()
        {
            
            Console.WriteLine('\n' + "CPU usage: " + theCPUCounter.NextValue() + "%");
        }

        //prints the value of available memory
        public static void getMemory()
        {

            Console.WriteLine('\n' + "mem:" + theMemCounter.NextValue() + "MB");
        }
        public Form1()
        {
            InitializeComponent();
            panelBackground.BackColor = Color.FromArgb(100, 88, 44, 55);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void resetCounters()
        {
            progressBar1.Value = 1;
            cpuLabel.Text = "CPU usage: " + 0 + " %";
            Thread.Sleep(80);
            memLabel.Text = "RAM usage: " + 0 + " MB";
        }
        private void listBoxData_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;

        }

        private void listBoxData_DragDrop(object sender, DragEventArgs e)
        {
            heslo = passwdBox.Text;
            // MessageBox.Show(heslo);
            files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            execute().Start();
        }
        Boolean IsRunning = true;
        private async Task execute()
        {
            const string keepAliveMessage = "{\"message\": {\"type\": \"keepalive\"}}";
            await Task.Run(() => {
                var seconds = 0;
                while (IsRunning)
                {
                    DOTHEWORK();
                    break;
                }
            });
        }

        public void DOTHEWORK()
        {
            errorLbl.ForeColor = Color.PaleVioletRed;
            errorLbl.Text = "";
            foreach (var file in files)
            {
                dragDropBox.Items.Add(file);
                heslo = passwdBox.Text;
                //last tri znaky uwu
                byte[] fileAsBytes = File.ReadAllBytes(file);
                printBytes(fileAsBytes);
                // byte[] data = null;

                //data = File.ReadAllBytes(file);

                if (fileAsBytes[0] == 82 && fileAsBytes[1] == 97 && fileAsBytes[2] == 114 && fileAsBytes[3] == 33)
                {
           
                    byte[] tmp = pokazRar(fileAsBytes);
                    File.WriteAllBytes(Path.GetTempPath() + "uwu.tmp", tmp);

                    printBytes(fileAsBytes);
                    FileEncrypt(Path.GetTempPath() + "uwu.tmp", heslo);
                    if (deletePovodne)
                    {
                        System.IO.File.Delete(file);

                    }
                    System.IO.File.Move(Path.GetTempPath() + "uwu.uwu", file.Substring(0, file.Length - 4) + ".uwu");
                    Console.WriteLine('\n' + KeySize);
      
                    resetCounters();
                    playDone();
                    errorLbl.ForeColor = Color.GreenYellow;
                    errorLbl.Text = "Done!";

                }
                else if (fileAsBytes[0] == 80 && fileAsBytes[1] == 75 && fileAsBytes[2] == 3 && fileAsBytes[3] == 4)
                {
                    byte[] tmp = pokazRar(fileAsBytes);
                    //fileAsBytes = tmp;
                    File.WriteAllBytes(Path.GetTempPath() + "uwu.tmp", tmp);

                    printBytes(fileAsBytes);
                    FileEncrypt(Path.GetTempPath() + "uwu.tmp", heslo);
                    if (deletePovodne)
                    {
                        System.IO.File.Delete(file);

                    }
                    System.IO.File.Move(Path.GetTempPath() + "uwu.uwu", file.Substring(0, file.Length - 4) + ".uwu");

                    playDone();
                    resetCounters();
                    errorLbl.ForeColor = Color.GreenYellow;
                    errorLbl.Text = "Done!";
                }
                else
                {
                    try
                    {

                        FileDecrypt(file, Path.GetTempPath() + "uwu.uwu", heslo);
                        fileAsBytes = File.ReadAllBytes(Path.GetTempPath() + "uwu.uwu");

                        if (fileAsBytes[0] == 11 && fileAsBytes[1] == 22 && fileAsBytes[2] == 33 && fileAsBytes[3] == 44)
                        {
                            byte[] tmp = opravRar(fileAsBytes);


                            File.WriteAllBytes(file.Substring(0, file.Length - 4) + ".rar", tmp);
                            printBytes(tmp);
                            if (deletePovodne)
                            {
                                System.IO.File.Delete(file);

                            }
                            playDone();
                            System.IO.File.Delete(file.Substring(0, file.Length - 4));
                            // File.WriteAllBytes(file, data);
                            Console.WriteLine('\n' + KeySize);
                            resetCounters();
                            errorLbl.ForeColor = Color.GreenYellow;
                            errorLbl.Text = "Done!";
                        }
                        else if (fileAsBytes[0] == 11 && fileAsBytes[1] == 22 && fileAsBytes[2] == 33 && fileAsBytes[3] == 45)
                        {
                            byte[] tmp = opravZip(fileAsBytes);

                            File.WriteAllBytes(file.Substring(0, file.Length - 4) + ".zip", tmp);
                            printBytes(tmp);
                            if (deletePovodne)
                            {
                                System.IO.File.Delete(file);

                            }
                            playDone();
                            System.IO.File.Delete(file.Substring(0, file.Length - 4));
                            //File.WriteAllBytes(file, data);
                            Console.WriteLine('\n' + KeySize);
                            resetCounters();
                            errorLbl.ForeColor = Color.GreenYellow;
                            errorLbl.Text = "Done!";
                        }
                        else
                        {

                            System.IO.File.Delete(file.Substring(0, file.Length - 4));
                            //vrati povodny subor....
                            //MessageBox.Show("Wrong Password....");
                            errorLbl.ForeColor = Color.PaleVioletRed;
                            errorLbl.Text = "Error: " + "Wrong Password....";
                            // File.WriteAllBytes(file, data);
                            resetCounters();
                        }
                    }
                    catch (Exception ed)
                    {
                        playError();
                        if (ed.Message == "Exception of type 'System.OutOfMemoryException' was thrown.")
                        {
                            System.IO.File.Delete(file.Substring(0, file.Length - 4));
                            // MessageBox.Show("Out of memory...");
                            errorLbl.ForeColor = Color.PaleVioletRed;
                            errorLbl.Text = "Error: " + "Out of memory....";
                            //File.WriteAllBytes(file, data);
                            resetCounters();
                        }
                        else
                        {
                            Console.Out.WriteLine(ed.Message);
                            errorLbl.ForeColor = Color.PaleVioletRed;
                            errorLbl.Text = "Error: " + "Wrong password or encrypted in other bit standard...";

                           // MessageBox.Show("Wrong password or encrypted in other bit standard...");
                            System.IO.File.Delete(file.Substring(0, file.Length - 4));
                            //File.WriteAllBytes(file, data);
                            resetCounters();
                        }

                    }
                }
            }
            dragDropBox.Items.Clear();
        }

        private void playDone() // defining the function
        {
            SoundPlayer audio = new SoundPlayer(DND.Properties.Resources.done); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
            audio.Play();
        }

        private void playError() // defining the function
        {
            SoundPlayer audio = new SoundPlayer(DND.Properties.Resources.error); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
            audio.Play();
        }

        private void printBytes(byte[] file)
        {
            byte[] done;
            Console.Out.Write('\n' + "bytes:" + file[0] + " " + file[1] + " " + file[2] + " " + file[3]);
            //return done;
        }

        private byte[] readBytes(String file)
        {
            return File.ReadAllBytes(file);
        }

        private byte[] pokazRar(byte[] file)
        {
            file[0] = 11;
            file[1] = 22;
            file[2] = 33;
            file[3] = 44;

            return file;
        }

        private byte[] pokazZip(byte[] file)
        {
            file[0] = 11;
            file[1] = 22;
            file[2] = 33;
            file[3] = 45;

            return file;
        }
        private byte[] opravRar(byte[] file)
        {
            file[0] = 82;
            file[1] = 97;
            file[2] = 114;
            file[3] = 33;

            return file;
        }
        private byte[] opravZip(byte[] file)
        {
            file[0] = 80;
            file[1] = 75;
            file[2] = 3;
            file[3] = 4;

            return file;
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
            byte[] buffer = new byte[1048576];
            long length = new System.IO.FileInfo(inputFile).Length;

           try
           {
                progressBar1.Maximum = ((int)length);
                progressBar1.Minimum = 1;
                progressBar1.Step = 1048576;
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Application.DoEvents();
                    fsOut.Write(buffer, 0, read);
                    progressBar1.PerformStep(); cpuLabel.Text = "CPU usage: " + Math.Round(((theCPUCounter.NextValue() / ((Environment.ProcessorCount) * total_cpu.NextValue())) * 1000), 2) + " %";
                    Thread.Sleep(80);
                    memLabel.Text = "RAM usage: " + Math.Round((theMemCounter.NextValue() / 1000000F),0) + " MB";
                    //getCpuUsage();
                    //getMemory();
                }
                progressBar1.Value = 1;
            }
            catch (CryptographicException ex_CryptographicException)
            {
                Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
                resetCounters();
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
            byte[] buffer = new byte[1048576];
            int read;

            long length = new System.IO.FileInfo(inputFile).Length;

            try
            {
                progressBar1.Maximum = ((int)length);
                progressBar1.Minimum = 1;
                progressBar1.Step = 1048576;
                while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Application.DoEvents(); // -> for responsive GUI, using Task will be better!
                    cs.Write(buffer, 0, read);
                    progressBar1.PerformStep();
                    //getCpuUsage();
                    //getMemory();
                   
                    cpuLabel.Text = "CPU usage: " + Math.Round(((theCPUCounter.NextValue() / ((Environment.ProcessorCount) * total_cpu.NextValue())) * 1000),2) + " %";
                    Thread.Sleep(80);
                    memLabel.Text = "RAM usage: " + Math.Round((theMemCounter.NextValue() / 1000000F), 0) + " MB";
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                deletePovodne = true;
            }
            else
            {
                deletePovodne = false;
            }
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
                BlockSize = 256;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panelBackground_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
