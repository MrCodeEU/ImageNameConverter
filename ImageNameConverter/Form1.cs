﻿using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.QuickTime;
using MetadataExtractor.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImageNameConverter
{
    public partial class Form1 : Form
    {
        private string filePath = string.Empty;
        private List<string> files = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Öffnet mit Filedialog alle ausgewählten Fotos uns zeigt diese in der Liste da
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                files = openFileDialog.FileNames.ToList<string>();

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
                lstOld.Items.AddRange(files.ToArray());

                //Generate new File Names and show them in listbox
                lstNew.Items.AddRange(Umbennen(files).ToArray());

                //Progressbar size setzen
                PrbUmbennen.Maximum = files.Count;
            }
        }

        /// <summary>
        /// Ändert die Rotation des Bildes aufgrund der ExIF Daten
        /// </summary>
        /// <param name="image">Das zu rotierende Bild</param>
        /// <returns></returns>
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

        /// <summary>
        /// Stellt sicher das immer die gleichen Bilder in beiden Listen ausgewählt sind
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Bennent alle Bilder die in der Liste sind um
        /// </summary>
        /// <param name="Dateien">Liste aller Fotos</param>
        /// <returns>Liste aller Fotos umbennant</returns>
        private List<string> Umbennen(List<string> Dateien)
        {
            List<string> newDateien = new List<string>(Dateien);
            List<string> errorList = new List<string>();

            //alle Dateien umbennen (Fehlt noch für Whatsapp)
            for (int i = 0; i < Dateien.Count; i++)
            {
                FileType filetype = FileTypeDetector.DetectFileType(new FileStream(Dateien[i], FileMode.Open, FileAccess.Read));
                switch (filetype)
                {
                    case FileType.Bmp:
                    case FileType.Heif:
                    case FileType.Jpeg:
                    case FileType.Nef:
                    case FileType.Png:
                        if (Dateien[i].ToLower().Contains("screenshot"))
                        {
                            string oldName = Dateien[i].Replace(filePath, "");
                            string newName = oldName.ToLower().Replace("screenshot_", "").Replace("-", " ");
                            newName = newName.Remove(newName.IndexOf("."), 4);
                            newDateien[i] = filePath + "umbennant\\" + newName + "." + filetype.ToString().ToLower();
                        }
                        else if(Dateien[i].ToLower().Contains("-wa"))
                        {
                            string oldName = Dateien[i].Replace(filePath, "");
                            string newName = oldName.ToLower().Replace("img-", "");
                            newName = newName.Remove(newName.IndexOf("-"), newName.Length - newName.IndexOf("-"));
                            newName += " 000000";
                            newDateien[i] = filePath + "umbennant\\" + newName + "." + filetype.ToString().ToLower();
                        }
                        else
                        {
                            var directories = ImageMetadataReader.ReadMetadata(new FileStream(Dateien[i], FileMode.Open, FileAccess.Read));

                            // obtain the Exif SubIFD directory
                            var directory = new ExifSubIfdDirectory();
                            try
                            {
                                directory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                            }
                            catch (Exception)
                            {
                                newDateien[i] = filePath + "umbennant\\" + Dateien[i].Replace(filePath, "");
                                return new List<string>();
                            }

                            if (directory == null)
                            {
                                newDateien[i] = Dateien[i];
                                newDateien[i] = filePath + "umbennant\\" + Dateien[i].Replace(filePath, "");
                                errorList.Add("• Unbekannter Fehler: " + Dateien[i]);
                                break;
                            }
                            // query the tag's value
                            if (directory.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out var dateTime))
                            {
                                newDateien[i] = filePath + "umbennant\\" + dateTime.ToString("s").Replace("-", "").Replace("T", " ").Replace(":", "") + "." + filetype.ToString().ToLower();
                            }
                            else
                            {
                                newDateien[i] = filePath + "umbennant\\" + Dateien[i].Replace(filePath,"");
                                errorList.Add("• Kein Aufnahmedatum: " + Dateien[i]);
                            }
                        }
                        break;

                    case FileType.QuickTime:
                        var directoriesQuick = QuickTimeMetadataReader.ReadMetadata(new FileStream(Dateien[i], FileMode.Open, FileAccess.Read));
                        // obtain the Exif SubIFD directory
                        var directoryQuick = directoriesQuick.OfType<QuickTimeMetadataHeaderDirectory>().FirstOrDefault();

                        if (directoryQuick == null)
                        {
                            newDateien[i] = filePath + "umbennant\\" + Dateien[i].Replace(filePath, "");
                            errorList.Add("• Unbekannter Fehler: " + Dateien[i]);
                            break;
                        }

                        // query the tag's value
                        if (directoryQuick.TryGetDateTime(QuickTimeMetadataHeaderDirectory.TagCreationDate, out var dateTimeQuick))
                        {
                            //Convert Name captured.Year + "_" + captured.Month + "_" + captured.day + " " + captured.Hour + "-" + captured.Minute + "-" + captured.Second;
                            newDateien[i] = filePath + "umbennant\\" + dateTimeQuick.AddHours(1).ToString("s").Replace("-", "").Replace("T", " ").Replace(":", "") + "." + Dateien[i].Split('.')[1];
                        }
                        else
                        {
                            newDateien[i] = filePath + "umbennant\\" + Dateien[i].Replace(filePath, "");
                            errorList.Add("• Kein Aufnahmedatum: " + Dateien[i]);
                        }
                        break;
                    default:
                        newDateien[i] = filePath + "umbennant\\" + Dateien[i].Replace(filePath, "");
                        errorList.Add("• Unbekannter Fehler: " + Dateien[i]);
                        break;
                }
            }

            //alle Dateien auf Doppelbennenungen überprüfen
            for (int i = 0; i < newDateien.Count; i++)
            {
                int DuplikatCounter = 0;
                
                //Create helper list for replacement!!

                for (int j = 0; j < newDateien.Count; j++)
                {
                    if (newDateien[i] == newDateien[j] && i!=j && !newDateien[i].Contains("("))
                    {
                        DuplikatCounter++;
                        newDateien[j] = newDateien[i].Insert(newDateien[i].IndexOf("."), " (" + DuplikatCounter.ToString() + ")");
                    }
                }
            }

            if (errorList.Count > 0)
            {
                string messageDone = "Folgende Dateien konnten nicht umbenannt werden:\n";
                string captionDone = "Nichtkonvertierbare Dateien";

                for (int i = 0; i < errorList.Count; i++)
                {
                    messageDone += errorList[i].Replace(filePath, "") + "\n";
                }
                MessageBox.Show(messageDone, captionDone, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return newDateien;
        }

        /// <summary>
        /// Stellt sicher das immer das gleiche Foto in beiden Listen ausgewählt ist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LstNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstOld.SelectedIndex = lstNew.SelectedIndex;
        }

        /// <summary>
        /// Kopiert alle Bilder in einen neuen Ordner mit einem neuen Namen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRename_Click(object sender, EventArgs e)
        {
            const string message = "Wollen sie die ausgewählten Dateien sicher umbennen?";
            const string caption = "Dateien Umbennen";
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            picSelected.Image.Dispose();

            if (System.IO.Directory.Exists(filePath + "umbennant\\"))
            {
                System.IO.Directory.Delete(filePath + "umbennant\\", true);
            }

            System.IO.Directory.CreateDirectory(filePath + "umbennant\\");

            // Dateien umbennen
            if (result == DialogResult.Yes)
            {
                try
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        File.Copy(files[i], (string)lstNew.Items[i]);
                        PrbUmbennen.Value++;
                    }

                    const string messageDone = "Alle Dateien wurden umbennant. \n Die Dateien befinden sich in einem neuen Unterordner mit dem Namen \"umbennant\".";
                    const string captionDone = "Dateien umbennen erfolgreich";
                    MessageBox.Show(messageDone, captionDone, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Clear everything

                }
                catch (Exception)
                {
                    const string messageDone = "Unbekannter Fehler ist aufgetretten!!";
                    const string captionDone = "Help!?!";
                    MessageBox.Show(messageDone, captionDone, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}
