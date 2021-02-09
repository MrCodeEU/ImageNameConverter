using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ImageNameConverter
{
    public partial class Form1 : Form
    {
        string filePath = string.Empty;
        string[] files = new string[1];

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            //Initialize openFiledialog
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Bild Dateien (*.BMP; *.JPG; *.JPEG; *.NEF; *.HEIC; *.MOV)|*.BMP;*.JPG;*.JPEG;*.NEF;*.HEIC;*.MOV";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;

            //See if something was selected
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Delete previos Files
                lstOld.Items.Clear();

                //Get the path of specified file
                filePath = openFileDialog.FileNames[0].Replace(openFileDialog.SafeFileNames[0], "");
                Array.Resize<string>(ref files, openFileDialog.FileNames.Length);
                files = openFileDialog.FileNames;

                //Set selected Image as Image of PictureBox
                try
                {
                    //Set selected Image as Image of PictureBox
                    picSelected.Image = ChangeRotation(Image.FromFile(files[0]));
                }
                catch (Exception)
                {
                    picSelected.Image = picSelected.ErrorImage;
                }
                lstNew.SelectedIndex = lstOld.SelectedIndex;

                //Shpow filepaths in listbox
                lstOld.Items.AddRange(files);

                //Generate new File Names and show them in listbox
                lstNew.Items.AddRange(Umbennen(files));


            }
        }

        //Ändert die Rotation der Bilder aufgrund der EXIF Daten.
        public Image ChangeRotation(Image image)
        {

            if (Array.IndexOf(image.PropertyIdList, 0x0112) > -1)
            {
                int orientation;

                orientation = image.GetPropertyItem(0x0112).Value[0];

                if (orientation >= 1 && orientation <= 8)
                {
                    switch (orientation)
                    {
                        case 2:
                            image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            break;
                        case 3:
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case 4:
                            image.RotateFlip(RotateFlipType.Rotate180FlipX);
                            break;
                        case 5:
                            image.RotateFlip(RotateFlipType.Rotate90FlipX);
                            break;
                        case 6:
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 7:
                            image.RotateFlip(RotateFlipType.Rotate270FlipX);
                            break;
                        case 8:
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }

                }
            }
            return image;
        }

        private void LstOld_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Set selected Image as Image of PictureBox
                picSelected.Image = ChangeRotation(Image.FromFile(files[lstOld.SelectedIndex]));
            }
            catch (Exception)
            {
                picSelected.Image = picSelected.ErrorImage;
            }
            lstNew.SelectedIndex = lstOld.SelectedIndex;
        }

        private string[] Umbennen(string[] Dateien)
        {
            string[] newDateien = new string[Dateien.Length];

            for (int i = 0; i < Dateien.Length; i++)
            {
                
                if (!Dateien[i].Contains("Screenshot"))
                {
                    if (!Dateien[i].Contains("HEIC") && !Dateien[i].Contains("heic") && !Dateien[i].Contains("MOV") && !Dateien[i].Contains("mov"))
                    {
                        FileStream fs = new FileStream(Dateien[i], FileMode.Open, FileAccess.Read);
                        Image myImage = Image.FromStream(fs, false, false);

                        PropertyItem propItem = myImage.PropertyItems[0];

                        try
                        {
                            propItem = myImage.GetPropertyItem(36867);
                        }
                        catch (Exception)
                        {
                            propItem = null;
                        }

                        myImage.Dispose();

                        if (propItem != null)
                        {
                            string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);

                            //Convert Name captured.Year + "_" + captured.Month + "_" + captured.day + " " + captured.Hour + "-" + captured.Minute + "-" + captured.Second;
                            newDateien[i] = filePath + DateTime.Parse(dateTaken).ToString("s").Replace("-", "_").Replace("T", " ").Replace(":", "-") + "." + Dateien[i].Split('.')[1];
                        }
                        else
                        {
                            //Kann Datei nicht umbennen, da sie kein Aufnahmnedatum in EXIF gespeichert hat
                            newDateien[i] = Dateien[i];
                        }
                    }
                    else
                    {
                        //HEIC und MOV Dateien umbennen herausfinden
                        newDateien[i] = Dateien[i];
                    }
                }
                else
                {
                    //Screenshot umbennen
                    newDateien[i] = Dateien[i];
                }
            }

            return newDateien;
        }

        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static readonly Regex r = new Regex(":");

        private void LstNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstOld.SelectedIndex = lstNew.SelectedIndex;
        }

        private void BtnRename_Click(object sender, EventArgs e)
        {
            const string message = "Wollen sie die ausgewählten Dateien sicher umbennen?";
            const string caption = "Dateien Umbennen";
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            picSelected.Image.Dispose();

            Dispose();

            // Dateien umbennen
            if (result == DialogResult.Yes)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    File.Move(files[i], (string)lstNew.Items[i]);
                }
            }
        }
    }
}
