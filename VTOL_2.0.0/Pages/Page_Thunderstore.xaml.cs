﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VTOL;
using Timer = System.Timers.Timer;

using Downloader;
using Path = System.IO.Path;
using System.IO.Compression;
using Serilog;
using System.Globalization;
using System.Threading.Tasks.Dataflow;
using Threading;
using System.Windows.Threading;
using System.Diagnostics;
using ZipFile = Ionic.Zip.ZipFile;
using System.Reflection;
using System.Timers;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace VTOL.Pages
{
    class CustomStringComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return string.Equals(x, y);
        }
        public int GetHashCode(string s)
        {
            return string.IsNullOrEmpty(s) ? 0 :
                s.Length + 273133 * (int)s[0];
        }
        private CustomStringComparer() { }
        public static readonly CustomStringComparer Default
            = new CustomStringComparer();
    }
    public static class DependencyObjectExtensions
    {
        public static T FirstOrDefaultChild<T>(this DependencyObject parent, Func<T, bool> selector)
            where T : DependencyObject
        {
            T foundChild;
            return FirstOrDefaultVisualChildWhere(parent, selector, out foundChild) ? foundChild : default(T);
        }

        private static bool FirstOrDefaultVisualChildWhere<T>(DependencyObject parent, Func<T, bool> selector,
            out T foundChild) where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var tChild = child as T;
                if (tChild != null)
                {
                    if (!selector(tChild)) continue;
                    foundChild = tChild;
                    return true;
                }

                if (FirstOrDefaultVisualChildWhere(child, selector, out foundChild))
                {
                    return true;
                }
            }
            foundChild = default(T);
            return false;
        }
    }
        public class Skin_Processor_
    {

        private MainWindow Main = GetMainWindow();

        private List<string> Skin_List = new List<string>();
        private Wpf.Ui.Controls.Snackbar SnackBar;
        private List<string> names = new List<string>();
        User_Settings User_Settings_Vars = null;
        public static bool ZipHasFile(string Search, string zipFullPath)
        {
            ZipFile zipFile = new ZipFile(zipFullPath);


            foreach (var entry in zipFile.Entries)
            {
                if (entry.FileName.Contains(Search, StringComparison.OrdinalIgnoreCase))
                {

                    return true;
                }
            }

            return false;
        }

        private int ImageCheck(String ImageName)
        {
            int result = -1;
            int temp = ImageName.LastIndexOf("\\");
            ImageName = ImageName.Substring(0, temp);
            temp = ImageName.LastIndexOf("\\") + 1;
            ImageName = ImageName.Substring(temp, ImageName.Length - temp);
            switch (ImageName)
            {
                case "256x128":
                case "256x256":
                case "256":
                    //Big change,I don't want to do it:(
                    break;
                case "512x256":
                case "512x512":
                case "512":
                    result = 0;
                    break;
                case "1024x512":
                case "1024x1024":
                case "1024":
                    result = 1;
                    break;
                case "2048x1024":
                case "2048x2048":
                case "2048":
                    result = 2;
                    break;
                case "4096x2048":
                case "4096x4096":
                case "4096":
                    result = 3;
                    break;
                default:
                    result = -1;
                    break;
            }
            return result;
        }

        private bool IsPilot(string Name)
        {
            if (Name.Contains("Stim_") || Name.Contains("PhaseShift_") || Name.Contains("HoloPilot_") || Name.Contains("PulseBlade_") || Name.Contains("Grapple_") || Name.Contains("AWall_") || Name.Contains("Cloak_") || Name.Contains("Public_"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        string current_skin_folder = null;
        public bool TryDeleteDirectory(
 string directoryPath, bool overwrite = true,
 int maxRetries = 10,
 int millisecondsDelay = 300)
        {
            if (directoryPath == null)
                throw new ArgumentNullException(directoryPath);
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            if (millisecondsDelay < 1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {
                    if (Directory.Exists(directoryPath))
                    {
                        Directory.Delete(directoryPath, overwrite);
                    }

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
            }

            return false;
        }
        public bool TryCreateDirectory(
   string directoryPath,
   int maxRetries = 10,
   int millisecondsDelay = 200)
        {
            if (directoryPath == null)
                throw new ArgumentNullException(directoryPath);
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            if (millisecondsDelay < 1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {

                    Directory.CreateDirectory(directoryPath);

                    if (Directory.Exists(directoryPath))
                    {

                        return true;
                    }


                }
                catch (IOException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
            }

            return false;
        }
        public bool TryMoveFile(
   string Origin, string Destination, bool overwrite = true,
   int maxRetries = 10,
   int millisecondsDelay = 200)
        {
            if (Origin == null)
                throw new ArgumentNullException(Origin);
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            if (millisecondsDelay < 1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {
                    if (File.Exists(Origin))
                    {
                        File.Move(Origin, Destination, overwrite);
                    }

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
            }

            return false;
        }

        public bool TryCopyFile(
  string Origin, string Destination, bool overwrite = true,
  int maxRetries = 10,
  int millisecondsDelay = 300)
        {
            if (Origin == null)
                throw new ArgumentNullException(Origin);
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            if (millisecondsDelay < 1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {
                    if (File.Exists(Origin))
                    {
                        File.Copy(Origin, Destination, true);
                    }
                    Thread.Sleep(millisecondsDelay);

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
            }

            return false;
        }

        public bool TryUnzipFile(
string Zip_Path, string Destination, bool overwrite = true,
int maxRetries = 10,
int millisecondsDelay = 150)
        {
            if (Zip_Path == null)
                throw new ArgumentNullException(Zip_Path);
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            if (millisecondsDelay < 1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {
                    ZipFile zipFile = new ZipFile(Zip_Path);

                    zipFile.ExtractAll(Destination, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (Ionic.Zip.BadReadException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (Ionic.Zip.BadCrcException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (Ionic.Zip.BadStateException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (Ionic.Zip.ZipException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (Exception)
                {
                    Thread.Sleep(millisecondsDelay);
                }
            }

            return false;
        }
        public void Install_Skin_From_Path(string Zip_Path)
        {

            try
            {
                User_Settings_Vars = Main.User_Settings_Vars;
                SnackBar = Main.Snackbar;
                if (ZipHasFile(".dds", Zip_Path))
                {


                    if (Directory.Exists(User_Settings_Vars.NorthstarInstallLocation + @"Skins_Unpack_Mod_MNGR"))
                    {

                        current_skin_folder = User_Settings_Vars.NorthstarInstallLocation + @"Skins_Unpack_Mod_MNGR";

                        TryUnzipFile(Zip_Path, User_Settings_Vars.NorthstarInstallLocation + @"Skins_Unpack_Mod_MNGR");
                       
                        //Skin_Path = Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR";

                    }
                    else
                    {

                       TryCreateDirectory(User_Settings_Vars.NorthstarInstallLocation + @"Skins_Unpack_Mod_MNGR");
                        current_skin_folder = User_Settings_Vars.NorthstarInstallLocation + @"Skins_Unpack_Mod_MNGR";

                       
                        TryUnzipFile(Zip_Path, User_Settings_Vars.NorthstarInstallLocation + @"Skins_Unpack_Mod_MNGR");

                    }
                }
                else
                {
                    //Send_Error_Notif(GetTextResource("NOTIF_ERROR_SKIN_INCOMPATIBLE"));
                    Log.Error("Issue With Skin Install!");


                }




                //Block Taken From Skin Tool
                List<string> FileList = new List<string>();
                if (current_skin_folder != null)
                {
                    FindSkinFiles(current_skin_folder, FileList, ".dds");

                }
                else
                {
                    Log.Error("Issue With Skin Install!");

                    return;
                }



                var matchingvalues = FileList.FirstOrDefault(stringToCheck => stringToCheck.Contains(""));
                for (int i = 0; i < FileList.Count; i++)
                {
                    if (FileList[i].Contains("col")) // (you use the word "contains". either equals or indexof might be appropriate)
                    {

                        //Console.WriteLine(i);
                    }
                }
                int DDSFolderExist = 0;

                DDSFolderExist = FileList.Count;
                if (DDSFolderExist == 0)
                {
                    Log.Error("Could Not Find Skins in Zip??");
                }

                foreach (var i in FileList)
                {
                    int FolderLength = current_skin_folder.Length;
                    String FileString = i.Substring(FolderLength);
                    int imagecheck = ImageCheck(i);
                    //the following code is waiting for the custom model
                    Int64 toseek = 0;
                    int tolength = 0;
                    int totype = 0;
                    switch (GetTextureType(i))
                    {
                        case 1://Weapon
                               //Need to recode weapon part

                            VTOL.Titanfall2_Requisite.WeaponData.WeaponDataControl wdc = new VTOL.Titanfall2_Requisite.WeaponData.WeaponDataControl(i, imagecheck);
                            toseek = Convert.ToInt64(wdc.FilePath[0, 1]);
                            tolength = Convert.ToInt32(wdc.FilePath[0, 2]);
                            totype = Convert.ToInt32(wdc.FilePath[0, 3]);


                            break;
                        case 2://Pilot
                            VTOL.Titanfall2_Requisite.PilotDataControl.PilotDataControl pdc = new VTOL.Titanfall2_Requisite.PilotDataControl.PilotDataControl(i, imagecheck);
                            toseek = Convert.ToInt64(pdc.Seek);
                            tolength = Convert.ToInt32(pdc.Length);
                            totype = Convert.ToInt32(pdc.SeekLength);
                            break;
                        case 3://Titan
                            VTOL.Titanfall2_Requisite.TitanDataControl.TitanDataControl tdc = new VTOL.Titanfall2_Requisite.TitanDataControl.TitanDataControl(i, imagecheck);
                            toseek = Convert.ToInt64(tdc.Seek);
                            tolength = Convert.ToInt32(tdc.Length);
                            totype = Convert.ToInt32(tdc.SeekLength);
                            break;

                        default:
                            Log.Error("Issue With Skin Install!");

                            break;
                    }


                    StarpakControl sc = new StarpakControl(i, toseek, tolength, totype, User_Settings_Vars.NorthstarInstallLocation, "Titanfall2", imagecheck, "Replace");


                }

                FileList.Clear();
                DirectoryInfo di = new DirectoryInfo(current_skin_folder);
                FileInfo[] files = di.GetFiles();


                Main.Snackbar.Appearance = Wpf.Ui.Common.ControlAppearance.Success;
                Main.Snackbar.Show("SUCCESS!", VTOL.Resources.Languages.Language.Page_Skins_Install_Skin_From_Path_TheSkin + Path.GetFileNameWithoutExtension(Zip_Path) + VTOL.Resources.Languages.Language.Page_Skins_Install_Skin_From_Path_HasBeenInstalled);

                foreach (FileInfo file in files)
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir_ in di.GetDirectories())
                {
                    dir_.Delete(true);
                }
                TryDeleteDirectory(current_skin_folder);






            }
            catch (Exception ex)
            {
                Main.logger2.Open();
                 Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source +Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
Main.logger2.Close();

                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");

            }

        }

        private void FindSkinFiles(string FolderPath, List<string> FileList, string FileExtention)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(FolderPath);
                FileSystemInfo[] fi = di.GetFileSystemInfos();
                foreach (var i in fi)
                {
                    if (i is DirectoryInfo)
                    {
                        FindSkinFiles(i.FullName, FileList, FileExtention);
                    }
                    else
                    {
                        if (i.Extension == FileExtention)
                        {
                            FileList.Add(i.FullName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger2.Open();
                 Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source +Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
Main.logger2.Close();

                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");

            }
        }




        private static int GetTextureType(string Name)
        {
            if (Name != null && Name.Length == 0)
            {
                return 0;
            }
            if (Name.Contains("Stim_") || Name.Contains("PhaseShift_") || Name.Contains("HoloPilot_")
            || Name.Contains("PulseBlade_") || Name.Contains("Grapple_") || Name.Contains("AWall_")
            || Name.Contains("Cloak_") || Name.Contains("Pilot_"))
            {
                return 2;
            }
            else if (Name.Contains("Titan_"))
            {
                return 3;
            }
            else
            {
                return 1;
            }

        }

        private static MainWindow GetMainWindow()
        {
            MainWindow mainWindow = null;

            foreach (Window window in Application.Current.Windows)
            {
                Type type = typeof(MainWindow);
                if (window != null && window.DependencyObjectType.Name == type.Name)
                {
                    mainWindow = (MainWindow)window;
                    if (mainWindow != null)
                    {
                        break;
                    }
                }
            }


            return mainWindow;

        }


    }
    /// <summary>
    /// Interaction logic for Page_Thunderstore.xaml
    /// </summary>
    /// 
    public class NegatingConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                return -((double)value);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                return +(double)value;
            }
            return value;
        }
    }
    public partial class Page_Thunderstore : Page
    {
        public MainWindow Main = GetMainWindow();
        // Timer set to elapse after 750ms
        private Timer _timer = new Timer(100) { Enabled = false };
        private List<Grid_> itemsList = new List<Grid_>();
        private List<string> ListOfLink = new List<string>();
        Updater _updater;
        User_Settings User_Settings_Vars = null;
        private string DocumentsFolder = null;
        private bool init = false;
        private Wpf.Ui.Controls.Snackbar SnackBar;
        private ProgressBar Progress_Cur_Temp;
        private List<string> Current_Mod_Filter_Tags = null;
        private List<string> Options_List = new List<string>();
        bool do_not_overwrite_Ns_file = true;
        bool page_loaded = false;
        public bool Reverse_ = false;
        bool search_a_flag = false;

        public Page_Thunderstore()
        {
            InitializeComponent();
            Check_Reverse(false);
            User_Settings_Vars = Main.User_Settings_Vars;
            DocumentsFolder = Main.AppDataFolder;
            SnackBar = Main.Snackbar;
            Options_List.Add("Skins");
            Options_List.Add("Mods");
            Options_List.Add("Client-side");
            Options_List.Add("Server-side");
            Options_List.Add("Custom Menus");
            Options_List.Add("Language: EN");
            Options_List.Add("Language: CN");
            Options_List.Add("DDS");
            Options_List.Add("Maps");
            Options_List.Add("Models");

            Search_Filters.ItemsSource = Options_List;
            _timer.Elapsed += TextInput_OnKeyUpDone;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {


                Call_Mods_From_Folder_Lite();
                Call_Ts_Mods();


            };
            worker.RunWorkerCompleted += (sender, eventArgs) =>
            {

                Loading_Ring.Visibility = Visibility.Hidden;
                page_loaded = true;

            };
            worker.RunWorkerAsync();






            //Log.Logger = new LoggerConfiguration()
            //     .MinimumLevel.Debug()
            //     .WriteTo.Console()
            //     .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            //     .CreateLogger();

        }
        // Event handler
       
        public static MainWindow GetMainWindow()
        {
            MainWindow mainWindow = null;

            foreach (Window window in Application.Current.Windows)
            {
                Type type = typeof(MainWindow);
                if (window != null && window.DependencyObjectType.Name == type.Name)
                {
                    mainWindow = (MainWindow)window;
                    if (mainWindow != null)
                    {
                        break;
                    }
                }
            }


            return mainWindow;

        }
        private childItem FindVisualChild<childItem>(DependencyObject obj)
    where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        private void Ts_Image_GotFocus(object sender, RoutedEventArgs e)
        {


        }
      
        private void Ts_Image_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void Ts_Image_MouseEnter(object sender, MouseEventArgs e)
        {

            
        }
       
        private void mask_MouseEnter(object sender, MouseEventArgs e)
        {
            DispatchIfNecessary(async () =>
            {

            ContentPresenter myListBoxItem = (ContentPresenter)(Thunderstore_List.ItemContainerGenerator.ContainerFromItem(Thunderstore_List.Items.CurrentItem));
            Grid gridPanel = FindVisualChild<Grid>(myListBoxItem);

            DoubleAnimation animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.4)),
                AutoReverse = false
            };

            if (gridPanel.Opacity >= 1)
            {
                animation.From = gridPanel.Opacity;
                animation.To = 0;
                gridPanel.BeginAnimation(OpacityProperty, animation);
                gridPanel.IsEnabled = false;
            }
            else
            {
                animation.From = gridPanel.Opacity;
                animation.To = 1;
                gridPanel.BeginAnimation(OpacityProperty, animation);
                gridPanel.IsEnabled = true;
            }


            });
        }

        private void mask_MouseLeave(object sender, MouseEventArgs e)
        {
            DispatchIfNecessary(async () =>
            {
                ContentPresenter myListBoxItem = (ContentPresenter)(Thunderstore_List.ItemContainerGenerator.ContainerFromItem(Thunderstore_List.Items.CurrentItem));
            Grid GridPanel_ = FindVisualChild<Grid>(myListBoxItem);
            bool mouseIsDown = System.Windows.Input.Mouse.RightButton == MouseButtonState.Pressed;

            if (mouseIsDown)
            {
                DoubleAnimation da = new DoubleAnimation
                {
                    Duration = new Duration(TimeSpan.FromSeconds(0.4)),
                    AutoReverse = false
                };

                if (GridPanel_.Opacity >= 1)
                {
                    da.From = GridPanel_.Opacity;
                    da.To = 0;
                    GridPanel_.BeginAnimation(OpacityProperty, da);
                    GridPanel_.IsEnabled = false;
                }
                else
                {
                    da.From = GridPanel_.Opacity;
                    da.To = 1;
                    GridPanel_.BeginAnimation(OpacityProperty, da);
                    GridPanel_.IsEnabled = true;
                }
                }
            });
        }

        private void CardExpander_MouseLeftGrid_Down(object sender, MouseButton e)
        {
            //if (sender.GetType() == typeof(Wpf.Ui.Controls.CardExpander))
            //{
            //    Wpf.Ui.Controls.CardExpander ss = new Wpf.Ui.Controls.CardExpander();
            //    ss = sender as Wpf.Ui.Controls.CardExpander;
            //       var item = Final_List.FirstOrDefault(o => o == ss.Header);
            //    if (item != null)
            //        Thunderstore_List.ScrollIntoView(item.First());
            //}
        }



        private void StackPanel_GotFocus(object sender, MouseEventArgs e)
        {


            DispatchIfNecessary(async () =>
            {
                Grid Card;
                if (sender.GetType() == typeof(Grid))
                {
                    Card = sender as Grid;

                    HandyControl.Controls.SimplePanel GridPanel_ = FindVisualChild<HandyControl.Controls.SimplePanel>(Card);
                    Wpf.Ui.Controls.CardAction Card_Action = FindVisualChild<Wpf.Ui.Controls.CardAction>(Card);

                    if (Card != null && GridPanel_ != null && Card_Action != null)
                    {

                        string tooltip_string = Card_Action.ToolTip.ToString().Replace("northstar-Northstar", "").Replace("ebkr-r2modman-", "");

                        if (tooltip_string.Count() > 5 && tooltip_string.Length > 5)
                        {
                            Card_Action.IsEnabled = true;
                            Card_Action.Icon = Wpf.Ui.Common.SymbolRegular.BoxMultipleCheckmark20;
                            Card_Action.IconForeground = Brushes.LawnGreen;
                            SlowBlink(Card_Action, 0.3);
                        }
                        else
                        {
                            Card_Action.IsEnabled = false;
                            Card_Action.Icon = Wpf.Ui.Common.SymbolRegular.BoxMultiple20;

                        }

                    }



                }
            });
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
            {
                DispatchIfNecessary(async () =>
                {

                    Grid Card;
            if (sender.GetType() == typeof(Grid))
            {

                //                ContentPresenter myListBoxItem =
                //(ContentPresenter)(Thunderstore_List.ItemContainerGenerator.ContainerFromItem(Thunderstore_List.Items.CurrentItem));
                Card = sender as Grid;
                HandyControl.Controls.SimplePanel GridPanel_ = FindVisualChild<HandyControl.Controls.SimplePanel>(Card);
                Wpf.Ui.Controls.CardAction Card_Action = FindVisualChild<Wpf.Ui.Controls.CardAction>(Card);
                        if (Card_Action != null && GridPanel_ != null)
                        {
                            if (GridPanel_.Opacity < 0.2)
                             {
                   
                               
                                string tooltip_string = Card_Action.ToolTip.ToString().Replace("northstar-Northstar", "").Replace("ebkr-r2modman-","");

                        if (tooltip_string.Count() > 5 && tooltip_string.Length > 5)
                        {
                            Card_Action.IsEnabled = true;
                            Card_Action.Icon = Wpf.Ui.Common.SymbolRegular.BoxMultipleCheckmark20;
                            Card_Action.IconForeground = Brushes.LawnGreen;
                           // SlowBlink(Card_Action, 0.3);
                        }
                        else
                        {
                            Card_Action.IsEnabled = false;
                            Card_Action.Icon = Wpf.Ui.Common.SymbolRegular.BoxMultiple20;

                        }
                                //  Check_Update_Tag(GridPanel_);
                                DoubleAnimation da = new DoubleAnimation
                                {
                                    From = GridPanel_.Opacity,
                                    To = 1,
                                    Duration = new Duration(TimeSpan.FromSeconds(0.4)),
                                    AutoReverse = false
                                };
                                GridPanel_.BeginAnimation(OpacityProperty, da);
                                GridPanel_.IsEnabled = true;
                            }
                   

                }


                    }
                });
        }
        private void Search_Bar_Suggest_Mods_GotFocus(object sender, RoutedEventArgs e)
        {
            DispatchIfNecessary(async () =>
            {
                if (search_a_flag == true)
                {
                    clear_box();
                }
                Search_Bar_Suggest_Mods.IsReadOnly = false;

                Sort.SelectedIndex = -1;
                Search_Bar_Suggest_Mods.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFFFF");
                Search_Bar_Suggest_Mods.IconForeground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFFFFF");
                search_a_flag = false;
            });
        }

        private void Search_Bar_Suggest_Mods_LostFocus(object sender, RoutedEventArgs e)
        {
            DispatchIfNecessary(async () =>
            {
                Search_Bar_Suggest_Mods.IsReadOnly = true;
                search_a_flag = true;
                Search_Bar_Suggest_Mods.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#34FFFFFF");
                Search_Bar_Suggest_Mods.IconForeground = (SolidColorBrush)new BrushConverter().ConvertFrom("#34FFFFFF");
            });
        }
        async Task clear_box()
        {

            DispatchIfNecessary(async () =>
            {
                Search_Bar_Suggest_Mods.Text = "";

            });
        }
        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            DispatchIfNecessary(async () =>
            {
                Grid Card;
                if (sender.GetType() == typeof(Grid))
                {
                    Card = sender as Grid;

                    HandyControl.Controls.SimplePanel GridPanel_ = FindVisualChild<HandyControl.Controls.SimplePanel>(Card);

                    if (Card != null && GridPanel_ != null)
                    {
                        if (GridPanel_.Opacity > 0)
                        {

                            DoubleAnimation da = new DoubleAnimation
                            {
                                From = GridPanel_.Opacity,
                                To = 0,
                                Duration = new Duration(TimeSpan.FromSeconds(0.4)),
                                AutoReverse = false
                            };
                            GridPanel_.BeginAnimation(OpacityProperty, da);
                            GridPanel_.IsEnabled = false;



                        }
                    }



                }
            });
        }


        public async Task Call_Ts_Mods(bool hard_refresh = true, List<string> Filter_Type = null, bool Search_ = false, string SearchQuery = "#", bool tickle = false,bool clear = true)
        {



            try
            {

                if (clear == true)
                {

                    DispatchIfNecessary(() =>
                {
                    Loading_Ring.Visibility = Visibility.Visible;

                });
                }
                List<Grid_> List = null;
                _updater = new Updater("https://northstar.thunderstore.io/api/v1/package/");

                var NON_UI = new Thread(() =>
                {
                    _updater.Download_Cutom_JSON();
                    if (_updater.Thunderstore != null)
                    {
                        if (_updater.Thunderstore.Count() > 0)
                        {
                            if (Search_ == false)
                            {
                                if (tickle == false)
                                {
                                    DispatchIfNecessary(() =>
                                    {
                                        List = orderlist(LoadListViewData(Filter_Type, Search_, SearchQuery.Replace(" ", "_")));

                                    });
                                }
                                else
                                {
                                    DispatchIfNecessary(() =>
                                    {

                                        List = orderlist(LoadListViewData(Filter_Type, Search_, SearchQuery.Replace(" ", "_")));
                                    });


                                }



                            }
                            else
                            {

                                List = LoadListViewData(Filter_Type, Search_, SearchQuery.Replace(" ", "_"));

                            }



                        }
                    }
                    else //Dont Scream...i know it looks bad, but hey, now more crashing if you swap windows quick now :D.
                    {
                        _updater.Download_Cutom_JSON();
                        if (_updater.Thunderstore != null)
                        {
                            if (_updater.Thunderstore.Count() > 0)
                            {

                                if (Search_ == false)
                                {

                                    if (tickle == false)
                                    {
                                        DispatchIfNecessary(() =>
                                        {
                                            List = orderlist(LoadListViewData(Filter_Type, Search_, SearchQuery.Replace(" ", "_")));

                                        });
                                    }
                                    else
                                    {
                                        List = orderlist(LoadListViewData(Filter_Type, Search_, SearchQuery.Replace(" ", "_")));


                                    }
                                }
                                else
                                {
                                    List = orderlist(LoadListViewData(Filter_Type, Search_, SearchQuery.Replace(" ", "_")));


                                }


                            }
                        }
                    }
                });
                NON_UI.IsBackground = true;

                NON_UI.Start();
                NON_UI.Join();
                DispatchIfNecessary(() =>
                {

                    Thunderstore_List.ItemsSource = List;
                    Loading_Ring.Visibility = Visibility.Hidden;

                });






                init = true;





            }
            catch (Exception ex)
            {
                Main.logger2.Open();
                Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Main.logger2.Close();


                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");
            }
        }
        static int versionCompare(string v1, string v2)
        {
            // vnum stores each numeric

            // part of version

            int vnum1 = 0, vnum2 = 0;

            // loop until both string are
            // processed

            for (int i = 0, j = 0; (i < v1.Length || j < v2.Length);)

            {
                // storing numeric part of
                // version 1 in vnum1
                while (i < v1.Length && v1[i] != '.')
                {

                    vnum1 = vnum1 * 10 + (v1[i] - '0');

                    i++;
                }
                // storing numeric part of

                // version 2 in vnum2

                while (j < v2.Length && v2[j] != '.')
                {
                    vnum2 = vnum2 * 10 + (v2[j] - '0');
                    j++;
                }
                if (vnum1 > vnum2)
                    return 1;

                if (vnum2 > vnum1)
                    return -1;

                // if equal, reset variables and

                // go for next numeric part
                vnum1 = vnum2 = 0;
                i++;
                j++;
            }

            return 0;

        }
        private static Boolean findString(String baseString, String strinfToFind, String separator)
        {
            foreach (String str in baseString.Split(separator.ToCharArray()))
            {
                if (str.Equals(strinfToFind))
                {
                    return true;
                }
            }
            return false;
        }
        private void Compare_Mod_To_List(string modname, string Mod_version_current, HashSet<string> list, out string bg_color, out string label)
        {
            string res = "Install";
            string bg = "#FF005D42";
            
          
            try
            {


            
           

                if (list.Count() > 2)
                {
                

           

                    foreach (var item in list)
                    {
                
                        if (Regex.Replace(item, @"(\d+\.)(\d+\.)(\d)", "").TrimEnd('-') == modname)
                        {
                 

                            Regex pattern = new Regex(@"\d+(\.\d+)+");
                            Match m = pattern.Match(item);
                            string version = m.Value;
                            int result = versionCompare(version, Mod_version_current);
                            //if(version.Count() < 2)
                            //{
                            //    result = 2;
                            //}
                            switch (result)
                            {
                                
                                //fix versions
                                //case 2:
                                //    res = "Repair";
                                //    bg = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF005D42");
                                //    break;
                                case 1:
                                    res = "Re-Install";
                                    bg = "#FFAD7F1A";
                                    break;
                                case -1:
                                    res = "Update";
                                    bg = "#FF009817";

                                    break;
                                case 0:
                                    res = "Re-Install";
                                    bg = "#FFAD7F1A";

                                    break;
                                default:
                                    res = "Install";
                                    bg = "#FF005D42";

                                    break;
                            }
                            label = res;
                            bg_color = bg;
                        }












                    }

                }
        }
            catch (Exception ex)
            {
                Main.logger2.Open();
                Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine);
                Main.logger2.Close();

                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy-MM- dd-HH-mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");

                }
           

            label = res;
            bg_color = bg;


        }

        public static bool ContainsAny(string stringToTest, List<string> substrings)
        {
            if (string.IsNullOrEmpty(stringToTest) || substrings == null)
                return false;

            foreach (var substring in substrings)
            {
                if (stringToTest.Contains(substring, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        private void padd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Check_Reverse();


                if (Sort.SelectedItem != null)
                {
                    Thunderstore_List.ItemsSource = null;

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += (sender, e) =>
                    {



                        Call_Ts_Mods();


                    };
                    worker.RunWorkerCompleted += (sender, eventArgs) =>
                    {

                        Thunderstore_List.Refresh();


                    };
                    worker.RunWorkerAsync();
                }
            



            }
            catch (Exception ex)
            {
                Main.logger2.Open();
                Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine);
                Main.logger2.Close();

                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy-MM- dd-HH-mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");

            }
        }
        void Check_Reverse(bool Apply_Change = true)
        {
            try
            {
                DispatchIfNecessary(() =>
                {
                    if (Apply_Change == true)
                    {
                        if (Reverse_ == true)
                        {
                            Reverse_ = false;

                            padd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#07FFFFFF");

                        }
                        else
                        {

                            Reverse_ = true;
                            padd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF4CAF50");

                        }
                    }
                    else
                    {
                        if (Reverse_ == true)
                        {
                            padd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF4CAF50");


                        }
                        else
                        {
                            padd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#07FFFFFF");


                        }

                    }
                });
            }
            catch (Exception ex)
            {
                Main.logger2.Open();
                Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine);
                Main.logger2.Close();

                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");

            }
        }
        public bool TryDeleteDirectory(
string directoryPath, bool overwrite = true,
int maxRetries = 10,
int millisecondsDelay = 30)
        {
            if (directoryPath == null)
                throw new ArgumentNullException(directoryPath);
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            if (millisecondsDelay < 1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {
                    if (Directory.Exists(directoryPath))
                    {
                        Directory.Delete(directoryPath, overwrite);
                    }

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
            }

            return false;
        }
        public bool TryCreateDirectory(
   string directoryPath,
   int maxRetries = 10,
   int millisecondsDelay = 30)
        {
            if (directoryPath == null)
                throw new ArgumentNullException(directoryPath);
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            if (millisecondsDelay < 1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {
                    
                        Directory.CreateDirectory(directoryPath);
                    

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
            }

            return false;
        }
      
        public bool TryMoveFile(
    string Origin, string Destination, bool overwrite = true,
    int maxRetries = 10,
    int millisecondsDelay = 200)
        {
            if (Origin == null)
                throw new ArgumentNullException(Origin);
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            if (millisecondsDelay < 1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {
                    if (File.Exists(Origin))
                    {
                        File.Move(Origin, Destination, overwrite);
                    }

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
            }

            return false;
        }
        public bool TryMoveFolder(
   string Origin, string Destination, bool overwrite = true,
   int maxRetries = 10,
   int millisecondsDelay = 200)
        {
            if (Origin == null)
                throw new ArgumentNullException(Origin);
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            if (millisecondsDelay < 1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {
                    if (Directory.Exists(Origin))
                    {
                        Directory.Move(Origin, Destination);
                    }

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
            }

            return false;
        }
        public bool TryCopyFile(
  string Origin, string Destination, bool overwrite = true,
  int maxRetries = 10,
  int millisecondsDelay = 300)
        {
            if (Origin == null)
                throw new ArgumentNullException(Origin);
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            if (millisecondsDelay < 1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {
                    if (File.Exists(Origin))
                    {
                        File.Copy(Origin, Destination, true);
                    }
                    Thread.Sleep(2);

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
            }

            return false;
        }
        public List<Grid_> orderlist(List<Grid_> List)
        {

                if (Sort.SelectedItem != null) {
                    if (Reverse_ == false)
                    {
                        if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("Name"))
                        {

                            List = List.OrderBy(ob => ob.Name).ToList();

                        }
                        else if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("Rating"))
                        {
                            List = List.OrderBy(ob => ob.Rating).ToList();

                        }
                        else if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("Date"))
                        {
                            List = List.OrderBy(ob => Convert.ToDateTime(ob.date_created)).ToList();

                        }
                        else if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("File Size"))
                    {

                        List = List.OrderBy(ob =>Convert.ToInt32(ob.raw_size)).ToList();

                        }
                        else if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("Downloads"))
                        {
                            List = List.OrderBy(ob => Convert.ToInt32(ob.Downloads)).ToList();

                        }
                    else if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("Installed"))
                    {

                      //   List = List.OrderByDescending(x => x.Button_label.ToString().Contains("Re-Install")).ToList();
                       List = List.Where(item => item.Button_label.ToString().Contains("Re-Install")).OrderBy(ob => ob.Name).ToList();
                    }
                    else if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("Update"))
                    {
                        //List = List.OrderByDescending(x => x.Button_label.Contains("Update")).ToList();
                       List = List.Where(item => item.Button_label.ToString().Contains("Update")).OrderBy(ob => ob.Name).ToList();

                    }
                    else
                        {
                        return List;

                    }

                }
                    else
                    {

                        if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("Name"))
                        {

                            List = List.OrderByDescending(ob => ob.Name).ToList();

                        }
                        else if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("Rating"))
                        {
                            List = List.OrderByDescending(ob => ob.Rating).ToList();

                        }
                        else if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("Date"))
                        {
                            List = List.OrderByDescending(ob => Convert.ToDateTime(ob.date_created)).ToList();

                        }
                        else if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("File Size"))
                        {
                            List = List.OrderByDescending(ob => Convert.ToInt32(ob.raw_size)).ToList();

                        }
                        else if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("Downloads"))
                        {
                            List = List.OrderByDescending(ob => Convert.ToInt32(ob.Downloads)).ToList();

                    }
                    else if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("Installed"))
                    {

                       // List = List.OrderByDescending(x => x.Button_label.ToString().Contains("Re-Install")).ToList();
                       List = List.Where(item => item.Button_label.ToString().Contains("Re-Install")).OrderByDescending(ob => ob.Name).ToList();


                    }
                    else if (Sort.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "").Trim().Contains("Update"))
                    {
                      // List = List.OrderBy(x => x.Button_label.Contains("Update")).ToList();
                        List = List.Where(item => item.Button_label.ToString().Contains("Update")).OrderByDescending(ob => ob.Name).ToList();

                    }
                    else
                        {
                        return List;
                    }


                }
                }
                else
                {
                return List;

            }

            return List;
        }

        private List<Grid_> LoadListViewData(List<string> Filter_Type = null, bool Search_ = false, string SearchQuery = "#")
        {

            try
            {
             

                itemsList.Clear();
                string ICON = "";
                List<int> Downloads = new List<int> { };
                List<object> Temp = new List<object> { };
                List<string> Dependencies = new List<string> { };
                string Tags = "";
                string downloads = "";
                string download_url = "";
                string Descrtiption = "";
                string FileSize = "";
                string Exclude_String = "#";
                string Dependencies_ = "";
                string Update_data = "";
                string Button_label = "";

                if (Current_Mod_Filter_Tags != null)
                    {
                        Current_Mod_Filter_Tags = Current_Mod_Filter_Tags.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                    }
                    for (int i = 0; i < _updater.Thunderstore.Length; i++)
                    {

                        if ( _updater.Thunderstore[i].FullName.Contains("r2modman") )
                        {
                            continue;
                        }
                    if (_updater.Thunderstore[i].IsDeprecated == true)
                    {
                        continue;
                    }
                    int rating = _updater.Thunderstore[i].RatingScore;

                        Tags = String.Join(" , ", _updater.Thunderstore[i].Categories);


                        List<versions> versions = _updater.Thunderstore[i].versions;
                        if(Current_Mod_Filter_Tags != null && Current_Mod_Filter_Tags.Count > 0)
                        {


                            if (_updater.Thunderstore[i].Categories.Select(x => x).Intersect(Current_Mod_Filter_Tags).Any())
                            {
                                if (Search_ == true)
                                {





                                    if (_updater.Thunderstore[i].Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) || _updater.Thunderstore[i].Owner.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                                    {

                                        foreach (var items in versions)


                                        {


                                            Downloads.Add(Convert.ToInt32(items.Downloads));



                                        }



                                        downloads = (Downloads.Sum()).ToString();

                                        for (var x = 0; x < versions.First().Dependencies.Count; x++)
                                        {
                                            if (versions.First().Dependencies[x].Contains("northstar-Northstar")  || versions.First().Dependencies[x].Contains("ebkr-r2modman-"))
                                            {

                                                continue;
                                            }
                                            else
                                            {
                                                Dependencies.Add(versions.First().Dependencies[x]);

                                        }

                                    }

                                        Dependencies_ = String.Join(", ", Dependencies);

                                        download_url = versions.First().DownloadUrl;
                                        ICON = versions.First().Icon;
                                        FileSize = versions.First().FileSize.ToString();
                                        Descrtiption = versions.First().Description;
                                        Downloads.Clear();
                                        Dependencies.Clear();


                                    string raw_size = versions.First().FileSize.ToString();

                                    if (int.TryParse(FileSize, out int value))
                                        {
                                            FileSize = Convert_To_Size(value);
                                        }
                                    string bg_color;
                                    string label;
                                    Compare_Mod_To_List(_updater.Thunderstore[i].Name, versions.First().VersionNumber, Main.Current_Installed_Mods, out bg_color, out label);
                                    if (bg_color == null || label == null)
                                    {
                                        bg_color = "#FF005D42" ;
                                        label = "Install";



                                    }
                                    itemsList.Add(new Grid_ { Name = _updater.Thunderstore[i].Name.Replace("_", " ") + "-" + versions.First().VersionNumber, Icon = ICON, date_created = _updater.Thunderstore[i].DateCreated.ToString(), description = Descrtiption, owner = _updater.Thunderstore[i].Owner, Rating = rating, download_url = download_url + "|" + _updater.Thunderstore[i].Name + "-" + versions.First().VersionNumber + "|" + Tags + "|" + Dependencies_, Webpage = _updater.Thunderstore[i].PackageUrl, File_Size = FileSize, Tag = Tags, Downloads = downloads, Dependencies = Dependencies_, FullName = _updater.Thunderstore[i].FullName, raw_size = raw_size, Update_data = _updater.Thunderstore[i].Name+ "|" + versions.First().VersionNumber, Button_label = label, Button_Color = bg_color });




                                }


                                }
                                else
                                {




                                    foreach (var items in versions)


                                    {
                                        Downloads.Add(Convert.ToInt32(items.Downloads));



                                    }



                                    downloads = (Downloads.Sum()).ToString();
                                    for (var x = 0; x < versions.First().Dependencies.Count; x++)
                                    {
                                        if (versions.First().Dependencies[x].Contains("northstar-Northstar") || versions.First().Dependencies[x].Contains("ebkr-r2modman-"))
                                        {

                                            continue;
                                        }
                                        else
                                        {
                                            Dependencies.Add(versions.First().Dependencies[x]);

                                        }

                                    }


                                    download_url = versions.First().DownloadUrl;

                                    ICON = versions.First().Icon;
                                    FileSize = versions.First().FileSize.ToString();
                                    Descrtiption = versions.First().Description;
                                    Dependencies_ = String.Join(", ", Dependencies);

                                    Dependencies.Clear();

                                    Downloads.Clear();

                                string raw_size = versions.First().FileSize.ToString();

                                if (int.TryParse(FileSize, out int value))
                                    {
                                        FileSize = Convert_To_Size(value);
                                    }
                                string bg_color;
                                string label;


                                Compare_Mod_To_List(_updater.Thunderstore[i].Name, versions.First().VersionNumber, Main.Current_Installed_Mods, out bg_color, out label);
                                if (bg_color == null || label == null)
                                {
                                    bg_color = "#FF005D42";
                                    label = "Install";



                                }
                                itemsList.Add(new Grid_ { Name = _updater.Thunderstore[i].Name.Replace("_", " ") + "-" + versions.First().VersionNumber, Icon = ICON, date_created = _updater.Thunderstore[i].DateCreated.ToString(), description = Descrtiption, owner = _updater.Thunderstore[i].Owner, Rating = rating, download_url = download_url + "|" + _updater.Thunderstore[i].Name + "-" + versions.First().VersionNumber + "|" + Tags + "|" + Dependencies_, Webpage = _updater.Thunderstore[i].PackageUrl, File_Size = FileSize, Tag = Tags, Downloads = downloads, Dependencies = Dependencies_, FullName = _updater.Thunderstore[i].FullName, raw_size = raw_size, Update_data = _updater.Thunderstore[i].Name + "|" + versions.First().VersionNumber, Button_label = label, Button_Color = bg_color });
                            }
                        }
                        }
                        else
                        {

                            if (Search_ == true)
                            {





                                if (_updater.Thunderstore[i].Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) || _updater.Thunderstore[i].Owner.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                                {

                                    foreach (var items in versions)


                                    {


                                        Downloads.Add(Convert.ToInt32(items.Downloads));



                                    }



                                    downloads = (Downloads.Sum()).ToString();
                                    for (var x = 0; x < versions.First().Dependencies.Count; x++)
                                    {
                                        if (versions.First().Dependencies[x].Contains("northstar-Northstar") || versions.First().Dependencies[x].Contains("ebkr-r2modman-"))
                                        {

                                            continue;
                                        }
                                        else
                                        {
                                            Dependencies.Add(versions.First().Dependencies[x]);

                                        }

                                    }

                                    Dependencies_ = String.Join(", ", Dependencies);

                                    download_url = versions.First().DownloadUrl;
                                    ICON = versions.First().Icon;
                                    FileSize = versions.First().FileSize.ToString();
                                    Descrtiption = versions.First().Description;
                                    Downloads.Clear();
                                    Dependencies.Clear();

                                string raw_size = versions.First().FileSize.ToString();
                                    if (int.TryParse(FileSize, out int value))
                                    {
                                        FileSize = Convert_To_Size(value);
                                    }
                                string bg_color;
                                string label;
                                  Compare_Mod_To_List(_updater.Thunderstore[i].Name, versions.First().VersionNumber, Main.Current_Installed_Mods, out bg_color, out label);
                                if (bg_color == null || label == null)
                                {
                                    bg_color = "#FF005D42";
                                    label = "Install";



                                }

                                itemsList.Add(new Grid_ { Name = _updater.Thunderstore[i].Name.Replace("_", " ") + "-" + versions.First().VersionNumber, Icon = ICON, date_created = _updater.Thunderstore[i].DateCreated.ToString(), description = Descrtiption, owner = _updater.Thunderstore[i].Owner, Rating = rating, download_url = download_url + "|" + _updater.Thunderstore[i].Name + "-" + versions.First().VersionNumber + "|" + Tags + "|" + Dependencies_, Webpage = _updater.Thunderstore[i].PackageUrl, File_Size = FileSize, Tag = Tags, Downloads = downloads, Dependencies = Dependencies_, FullName = _updater.Thunderstore[i].FullName , raw_size = raw_size, Update_data = _updater.Thunderstore[i].Name + "|" + versions.First().VersionNumber, Button_label = label, Button_Color = bg_color });



                            }


                        }
                            else
                            {




                                foreach (var items in versions)


                                {
                                    Downloads.Add(Convert.ToInt32(items.Downloads));



                                }



                                downloads = (Downloads.Sum()).ToString();
                                for (var x = 0; x < versions.First().Dependencies.Count; x++)
                                {
                                    if (versions.First().Dependencies[x].Contains("northstar-Northstar")  || versions.First().Dependencies[x].Contains("ebkr-r2modman-"))
                                    {

                                        continue;
                                    }
                                    else
                                    {
                                        Dependencies.Add(versions.First().Dependencies[x]);

                                    }

                                }

                                download_url = versions.First().DownloadUrl;

                                ICON = versions.First().Icon;
                                FileSize = versions.First().FileSize.ToString();
                                Descrtiption = versions.First().Description;
                                Dependencies_ = String.Join(", ", Dependencies);

                                Dependencies.Clear();

                                Downloads.Clear();

                            string raw_size = versions.First().FileSize.ToString();

                            if (int.TryParse(FileSize, out int value))
                                {
                                    FileSize = Convert_To_Size(value);
                                }
                            string bg_color;
                            string label;
                            Compare_Mod_To_List(_updater.Thunderstore[i].Name, versions.First().VersionNumber, Main.Current_Installed_Mods,out bg_color,out label);
                            if (bg_color == null || label == null)
                            {
                                bg_color = "#FF005D42";
                                label = "Install";



                            }
                            itemsList.Add(new Grid_ { Name = _updater.Thunderstore[i].Name.Replace("_", " ") + "-" + versions.First().VersionNumber, Icon = ICON, date_created = _updater.Thunderstore[i].DateCreated.ToString(), description = Descrtiption, owner = _updater.Thunderstore[i].Owner, Rating = rating, download_url = download_url + "|" + _updater.Thunderstore[i].Name + "-" + versions.First().VersionNumber + "|" + Tags + "|" + Dependencies_, Webpage = _updater.Thunderstore[i].PackageUrl, File_Size = FileSize, Tag = Tags, Downloads = downloads, Dependencies = Dependencies_, FullName = _updater.Thunderstore[i].FullName, raw_size = raw_size, Update_data = _updater.Thunderstore[i].Name + "|" + versions.First().VersionNumber, Button_label = label, Button_Color = bg_color });

                        }


                    }



                       
                        


                            

                        }
                      

                    
                

                }
            catch (Exception ex)
            {
                Main.logger2.Open();
                 Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source +Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
Main.logger2.Close();

                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");

            }
            return  itemsList;

        }
        public string Search_For_Mod_Thunderstore(string SearchQuery = "None")

        {
            try { 
            _updater = new VTOL.Updater("https://northstar.thunderstore.io/api/v1/package/");

            _updater.Download_Cutom_JSON();

            for (int i = 0; i < _updater.Thunderstore.Length; i++)
            {
                List<versions> versions = _updater.Thunderstore[i].versions;

                string[] subs = SearchQuery.Split('-');
                    Console.WriteLine(subs[1]);
                if (_updater.Thunderstore[i].FullName.Contains(subs[1], StringComparison.OrdinalIgnoreCase) || _updater.Thunderstore[i].Owner.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                {

                    return versions.First().DownloadUrl + "|" + _updater.Thunderstore[i].Name + "-" + versions.First().VersionNumber;

                }
              


            }
            }
            catch (Exception ex)
            {
                Main.logger2.Open();
                 Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source +Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
Main.logger2.Close();

                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");

            }
            return null;
        }
        class LIST_JSON
        {
            public string Tag { get; set; }
            public string Name { get; set; }
            public string FullName { get; set; }
            public string Icon { get; set; }
            public string owner { get; set; }
            public string description { get; set; }
            public string download_url { get; set; }
            public string Webpage { get; set; }
            public string date_created { get; set; }
            public int Rating { get; set; }
            public string File_Size { get; set; }
            public string Downloads { get; set; }
            public string raw_size { get; set; }
            public string raw_date { get; set; }
            public string Update_data { get; set; }

            public string Button_label { get; set; }
            public string Button_Color { get; set; }

        }
        public class Grid_
        {
            public string Tag { get; set; }
            public string Name { get; set; }
            public string FullName { get; set; }
            public string Icon { get; set; }
            public string owner { get; set; }
            public string description { get; set; }
            public string download_url { get; set; }
            public string Webpage { get; set; }
            public string date_created { get; set; }
            public int Rating { get; set; }
            public string File_Size { get; set; }
            public string Downloads { get; set; }
            public string Dependencies { get; set; }

            public string raw_size { get; set; }
            public string raw_date { get; set; }
            public string Update_data { get; set; }

            public string Button_label { get; set; }
            public string Button_Color { get; set; }

        }


        string Convert_To_Size(int size)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double len = Convert.ToDouble(size);
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            string result = String.Format("{0:0.##} {1}", len, sizes[order]);                 //   ICON = items.Icon;
            return result;

        }
        private void TextBlock_MouseMove(object sender, MouseEventArgs e)
        {


        }

        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {

            }
        private void Dialog_ButtonRightClick(object sender, RoutedEventArgs e)
        {
            Dialog.Hide();

        }
        void downloader_ProgressChanged(object sender, Downloader.DownloadProgressChangedEventArgs e, ProgressBar Progress_Bar)
        {

            DispatchIfNecessary(() => {

                Progress_Bar.Value = (e.ProgressPercentage);
            });

           
        }
        
        void downloader_DownloadCompleted(object sender, AsyncCompletedEventArgs e, ProgressBar Progress_Bar, string Mod_Name, string Location, bool Skin_Install, bool NS_CANDIDATE_INSTALL)
        {
            Console.WriteLine(Location);
            if (NS_CANDIDATE_INSTALL == true)
            {

                Unpack_To_Location_Custom(Location, User_Settings_Vars.NorthstarInstallLocation + @"Northstar_TEMP_FILES\", Progress_Bar, true, false, Skin_Install, NS_CANDIDATE_INSTALL);

            }
            else
            {
               

                Unpack_To_Location_Custom(Location, User_Settings_Vars.NorthstarInstallLocation  + User_Settings_Vars.Profile_Path + @"\mods\" + Mod_Name, Progress_Bar, true, false, Skin_Install, NS_CANDIDATE_INSTALL,Mod_Name);
            }
          
        }
        async Task Download_Zip_To_Path(string url, string path, ProgressBar Progress_Bar = null, bool Skin_Install_ = false, bool NS_CANDIDATE_INSTALL = false)
        {
            await Task.Run(() =>
            {//Regex.Replace(item, @"(\d+\.)(\d+\.)(\d)", "").TrimEnd('-')
                DispatchIfNecessary(() => {
                if (Directory.Exists(User_Settings_Vars.NorthstarInstallLocation))
                {
                       
                    string[] words = url.Split("|");
                    IDownload downloader = DownloadBuilder.New()
    .WithUrl(words[0])
    .WithDirectory(path)
    .WithFileName(Regex.Replace(words[1], @"(\d+\.)(\d+\.)(\d)", "").TrimEnd('-') + ".zip")
    .WithConfiguration(new DownloadConfiguration())

    .Build();

                    if (Progress_Bar != null)
                    {
                        Console.WriteLine("Started_To_Donwload_The_Data_At_URL__" + url);



                    
                        downloader.DownloadProgressChanged += delegate (object sender2, Downloader.DownloadProgressChangedEventArgs e2)
                        {
                            
                            downloader_ProgressChanged(sender2, e2, Progress_Bar);
                    };
                    }
                    var Destinfo = new DirectoryInfo(User_Settings_Vars.NorthstarInstallLocation);

                        if (!Directory.Exists(Destinfo.FullName + @"NS_Downloaded_Mods\"))
                        {
                            Directory.CreateDirectory(Destinfo.FullName + @"NS_Downloaded_Mods\");

                        }
                        else
                        {

                            Clear_Folder(Destinfo.FullName + @"NS_Downloaded_Mods\");
                        }

                    downloader.DownloadFileCompleted += delegate (object sender4, AsyncCompletedEventArgs e4)
                    {
                       

                        downloader_DownloadCompleted(sender4, e4, Progress_Bar, words[1], Destinfo.FullName + @"NS_Downloaded_Mods\" + Regex.Replace(words[1], @"(\d+\.)(\d+\.)(\d)", "").TrimEnd('-') + ".zip",Skin_Install_,NS_CANDIDATE_INSTALL);
                    };

                    downloader.StartAsync();



                    }
                });
            });







        

    }
        bool Is_Valid_URl(string uriName)
        {

            Uri uriResult;
            bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
        public void DispatchIfNecessary(Action action)
        {
            if (!Dispatcher.CheckAccess())
                Dispatcher.Invoke(action);
            else
                action.Invoke();
        }
        void Dependency_Download( string Dependencies_To_Find_And_Download,ProgressBar Progress_Bar = null)
        {
            Loaded_Async_Handler = true;
            try
            {

                List<string> Mods = new List<string>();
                Mods = Dependencies_To_Find_And_Download?.Split("\n").ToList();
                List<string> Links = new List<string>();

                foreach (var x in Mods)
                {

                    string URL = Search_For_Mod_Thunderstore(x);

                    if (Is_Valid_URl(URL))
                    {
                        Links.Add(URL);
                        MessageBox.Show(URL);



                    }

                }

                var queue = new SerialQueue();





                queue.Enqueue(async () =>
                {
                    foreach (var y in Links)
                    {
                        Download_Zip_To_Path(y, User_Settings_Vars.NorthstarInstallLocation + @"NS_Downloaded_Mods", Progress_Bar);
                        Thread.Sleep(2500);
                    }

                });




            }
            catch (Exception ex)
            {
                Main.logger2.Open();
                 Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source +Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
Main.logger2.Close();

                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");

            }
            Dialog.Hide();

        }
        bool Loaded_Async_Handler = false;
        string pack = null;
      

        private void CardAction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressBar Progress_Bar = null;
                HandyControl.Controls.SimplePanel _Panel = (HandyControl.Controls.SimplePanel)((Wpf.Ui.Controls.CardAction)sender).Parent;
                Progress_Bar = FindVisualChild<ProgressBar>(_Panel);
                var Tag_Data_ = ((Wpf.Ui.Controls.CardAction)sender).ToolTip.ToString();
                var Name_Data = ((Wpf.Ui.Controls.CardAction)sender).Tag.ToString();
               
               string Tag_Data = "\n" + (Tag_Data_.Replace(",", "\n").Replace(" ", "") + "\n\n"+VTOL.Resources.Languages.Language.Page_Thunderstore_CardAction_Click_DoYouWantToInstallTheseAndTheMod+"\n" + Name_Data  +   "?").Trim();
                if (Tag_Data.Count() > 5)
            {
                Dialog.Title = Name_Data + " - Dependencies";
                
                Dialog.DialogHeight = 350;
                Dialog.Message = Tag_Data;
                Dialog.ButtonLeftName = "Yes";
                Dialog.ButtonRightName = "Cancel";
                Dialog.ButtonLeftAppearance = Wpf.Ui.Common.ControlAppearance.Success;
                Dialog.ButtonRightAppearance = Wpf.Ui.Common.ControlAppearance.Caution;
                    
                     
                        Dialog.Tag = ((Tag_Data_ + " ,"+Name_Data.Trim()).Replace(",", "\n").Replace(" ", "")) ;
                    
                        Progress_Cur_Temp = Progress_Bar;
                    
                        Dialog.Show();

            }

        }
            catch (Exception ex)
            {
                Main.logger2.Open();
                 Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source +Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
Main.logger2.Close();

                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");
            }
        }
        public static bool IsDirectoryEmpty(DirectoryInfo directory)
        {
           

            
            FileInfo[] files = directory.GetFiles();
            DirectoryInfo[] subdirs = directory.GetDirectories();

            return (files.Length == 0 && subdirs.Length == 0);

        
}

        private void Clear_Folder(string FolderName, bool overwrite = true, int maxRetries = 10, int millisecondsDelay = 30)
        {
            
                if (FolderName == null)
                    throw new ArgumentNullException(FolderName);
                if (maxRetries < 1)
                    throw new ArgumentOutOfRangeException(nameof(maxRetries));
                if (millisecondsDelay < 1)
                    throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

                for (int i = 0; i < maxRetries; ++i)
                {
                   
               
           
            try
            {
                DirectoryInfo dir = new DirectoryInfo(FolderName);

                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.Delete();
                }

                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    Clear_Folder(di.FullName);
                    di.Delete();
                }
            }
            catch (Exception ex)
            {
               
                    return;

                }
            }

                return;
            }
        private async Task CopyFilesRecursively(string sourcePath, string targetPath)
        {
            // Create the target directory if it doesn't exist
            if (!Directory.Exists(targetPath))
            {
                TryCreateDirectory(targetPath);
            }

            // Copy all the files & Replaces any files with the same name
            await Task.Run(() => Parallel.ForEach(Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories), file =>
            {
                string relativePath = file.Replace(sourcePath, "");
                string targetFilePath = Path.Combine(targetPath, relativePath);

                TryCopyFile(file, targetFilePath, true);
            }));
        }

        public bool TryUnzipFile(
string Zip_Path, string Destination, bool overwrite = true,
int maxRetries = 10,
int millisecondsDelay = 150)
        {
            if (Zip_Path == null)
                throw new ArgumentNullException(Zip_Path);
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            if (millisecondsDelay < 1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {
                    ZipFile zipFile = new ZipFile(Zip_Path);

                    zipFile.ExtractAll(Destination, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (Ionic.Zip.BadReadException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (Ionic.Zip.BadCrcException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (Ionic.Zip.BadStateException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (Ionic.Zip.ZipException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (Exception)
                {
                    Thread.Sleep(millisecondsDelay);
                }
            }

            return false;

        }
        public bool TryDeleteFile(
string Origin,
int maxRetries = 10,
int millisecondsDelay = 300)
        {
            if (Origin == null)
                throw new ArgumentNullException(Origin);
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries));
            if (millisecondsDelay < 1)
                throw new ArgumentOutOfRangeException(nameof(millisecondsDelay));

            for (int i = 0; i < maxRetries; ++i)
            {
                try
                {
                    if (File.Exists(Origin))
                    {
                        File.Delete(Origin);
                    }
                    Thread.Sleep(millisecondsDelay);

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(millisecondsDelay);
                }
            }

            return false;
        }
        public bool IsValidPath(string path, bool allowRelativePaths = false)
        {
            bool isValid = true;

            try
            {
                string fullPath = Path.GetFullPath(path);

                if (allowRelativePaths)
                {
                    isValid = Path.IsPathRooted(path);
                }
                else
                {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");
                Main.logger2.Open();
                Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Main.logger2.Close();
                isValid = false;
            }

            return isValid;
        }
        async void Call_Mods_From_Folder_Lite()
        {

            try
            {

                if (User_Settings_Vars.NorthstarInstallLocation != null || User_Settings_Vars.NorthstarInstallLocation != "" || Directory.Exists(User_Settings_Vars.NorthstarInstallLocation))
                {

                    if (Directory.Exists(User_Settings_Vars.NorthstarInstallLocation))
                    {


                        Main.Current_Installed_Mods.Clear();

                        string NS_Mod_Dir = User_Settings_Vars.NorthstarInstallLocation + @"R2Northstar\mods";

                        System.IO.DirectoryInfo rootDirs = new DirectoryInfo(@NS_Mod_Dir);
                        if (IsValidPath(NS_Mod_Dir) == true)
                        {

                            System.IO.DirectoryInfo[] subDirs = null;
                            subDirs = rootDirs.GetDirectories();


                            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                            {


                                Main.Current_Installed_Mods.Add(dirInfo.Name.Trim());

                            }
                            
                        }



                    }


                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");
                Main.logger2.Open();
                Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Main.logger2.Close();
            }

        }  public void SlowBlink(Control control, double minimumOpacity)
        {
            DispatchIfNecessary(() =>
            {
                // Create a DoubleAnimation to animate the control's Opacity property
                var animation = new DoubleAnimation
                {
                    From = 1.0,
                    To = minimumOpacity,
                    Duration = new Duration(TimeSpan.FromSeconds(0.6)),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };

                // Apply the animation to the control's Opacity property
                control.BeginAnimation(UIElement.OpacityProperty, animation);
            });
        }
      
        public async Task Unpack_To_Location_Custom(string Target_Zip, string Destination, ProgressBar Progress_Bar, bool Clean_Thunderstore = false, bool clean_normal = false, bool Skin_Install = false,bool NS_CANDIDATE_INSTALL = false ,string mod_name ="~")
        {
            //ToDo Check if url or zip location
            //add drag and drop

            try
            {
               
                string Dir_Final = null;

                if (File.Exists(Target_Zip))
                {
                    if (NS_CANDIDATE_INSTALL == false && Skin_Install == false && Destination.Contains(@"\mods"))
                    {
                        if (Main.Current_Installed_Mods.Count() > 1)
                        {

                            foreach (var item in Main.Current_Installed_Mods)
                            {

                                if (Regex.Replace(item, @"(\d+\.)(\d+\.)(\d)", "").TrimEnd('-') == Regex.Replace(mod_name, @"(\d+\.)(\d+\.)(\d)", "").TrimEnd('-'))
                                {
                                    string mod = User_Settings_Vars.NorthstarInstallLocation + User_Settings_Vars.Profile_Path + @"\mods\" + item;
                                    if (Directory.Exists(mod))
                                    {
                                        Clear_Folder(mod);

                                       TryDeleteDirectory(mod, true);


                                    }
                                }
                            }
                        }
                    }


                            
                    if (!Directory.Exists(Destination))
                    {
                       TryCreateDirectory(Destination);
                    }
                    if (Directory.Exists(Destination))
                    {
                        if (NS_CANDIDATE_INSTALL == false)
                        {
                            Clear_Folder(Destination);
                        }
                        string fileExt = System.IO.Path.GetExtension(Target_Zip);

                        if (fileExt == ".zip")
                        {
                           
                            TryUnzipFile(Target_Zip, Destination);

                            if (Clean_Thunderstore == true)
                            {


                                // Check if file exists with its full path    
                                if (File.Exists(Path.Combine(Destination, "icon.png")))
                                {
                                    // If file found, delete it    
                                    TryDeleteFile(Path.Combine(Destination, "icon.png"));
                                }
                               

                                    if (File.Exists(Path.Combine(Destination, "README.md")))
                                {
                                    // If file found, delete it    
                                    TryDeleteFile(Path.Combine(Destination, "README.md"));
                                }
                               


                                //if (File.Exists(Path.Combine(Destination, "manifest.json")) && mod_name != "~")
                                //{
                                //    var myJsonString = File.ReadAllText(Path.Combine(Destination, "manifest.json"));
                                //    string Destinationnew =  Destination.Replace(mod_name, " ");

                                //    dynamic myJObject = JObject.Parse(myJsonString);
                                //    string name = myJObject.name;
                                //    string mod_version = "-" + myJObject.version_number;
                                //    while(mod_version.Count() < 2)
                                //    {

                                //        mod_version = "-" + myJObject.version;
                                //        if (mod_version.Count() < 2)
                                //        {
                                //            continue;


                                //        }
                                //        else
                                //        {
                                //            mod_version = mod_version.TrimEnd('-');
                                //            Destinationnew = Destinationnew.Trim() + name + mod_version;



                                //            Destination = Destinationnew;

                                //        }

                                //    }
                                //    mod_version = mod_version.TrimEnd('-');
                                //    Destinationnew = Destinationnew.Trim() + name + mod_version;
                             


                                //     Destination = Destinationnew;
                                //}

                                if (File.Exists(Path.Combine(Destination, "manifest.json")))
                                {
                                    // If file found, delete it    
                                    TryDeleteFile(Path.Combine(Destination, "manifest.json"));
                                }



                                if (Skin_Install == false)
                                {
                                    string searchQuery3 = "*" + "mod.json" + "*";

                                  
                                    var Destinfo = new DirectoryInfo(Destination);


                                    var Script = Destinfo.GetFiles(searchQuery3, SearchOption.AllDirectories);
                                    Destinfo.Attributes &= ~FileAttributes.ReadOnly;
                                    Console.WriteLine(Script.Length.ToString());
                                    if (Script.Length != 0 && Script.Length <= 1)
                                    {
                                        var File_ = Script.FirstOrDefault();


                                        FileInfo FolderTemp = new FileInfo(File_.FullName);
                                        DirectoryInfo di = new DirectoryInfo(Directory.GetParent(File_.FullName).ToString());
                                        string firstFolder = di.FullName;

                                        if (Directory.Exists(Destination))
                                        {




                                           TryCreateDirectory(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder");
                                            if (Directory.Exists(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder"))
                                            {
                                                await CopyFilesRecursively(firstFolder, Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder");




                                                Clear_Folder(Destination);
                                               await CopyFilesRecursively(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder", Destination);
                                                TryDeleteDirectory(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder", true);

                                            }
                                            Console.WriteLine("Unpacked - " + Destination);


                                        }

                                        Call_Mods_From_Folder_Lite();
                                        DispatchIfNecessary(() => {
                                            if (Progress_Bar != null)
                                            {
                                                Progress_Bar.Value = 0;
                                            }

                                            SnackBar.Title = "SUCCESS";
                                            SnackBar.Appearance = Wpf.Ui.Common.ControlAppearance.Success;
                                            SnackBar.Message = "The Mod " + Path.GetFileNameWithoutExtension(Target_Zip).Replace("_", " ") + VTOL.Resources.Languages.Language.Page_Thunderstore_Unpack_To_Location_Custom_HasBeenDownloadedAndInstalled;
                                            SnackBar.Show();


                                        });

                                    }
                                    else if (Script.Length > 1)
                                    {

                                      

                                           


                                            foreach (var File_ in Script)
                                            {
                                                FileInfo FolderTemp = new FileInfo(File_.FullName);

                                                DirectoryInfo di = new DirectoryInfo(Directory.GetParent(File_.FullName).ToString());
                                             if (Directory.Exists(Destination))
                                        {
                                                TryCreateDirectory(Destination + @"\" + "Multipack");

                                                TryMoveFolder(di.FullName, Directory.GetParent(Destination).ToString() + @"\" + di.Name);



                                            }


                                        }
                                        Call_Mods_From_Folder_Lite();
                                        DispatchIfNecessary(() => {
                                            if (Progress_Bar != null)
                                            {
                                                Progress_Bar.Value = 0;
                                            }

                                            SnackBar.Title = "SUCCESS";
                                        SnackBar.Appearance = Wpf.Ui.Common.ControlAppearance.Success;
                                        SnackBar.Message = "The the multiple Mods in - " + mod_name + " - have been installed Succesfully";
                                        SnackBar.Show();


                                        });


                                    }
                                    
                                  
                                }

                                else if(NS_CANDIDATE_INSTALL == true)
                                {
                                    DispatchIfNecessary(() => {

                                        if (Progress_Bar != null)
                                    {
                                        Progress_Bar.IsIndeterminate = true;
                                    }

                                });
                                if (File.Exists(Path.Combine(Destination, "manifest.json")))
                                    {
                                        // If file found, delete it    
                                        TryDeleteFile(Path.Combine(Destination, "manifest.json"));
                                    }


                                    if (Directory.Exists(User_Settings_Vars.NorthstarInstallLocation  + User_Settings_Vars.Profile_Path + @"\mods\Northstar.Client\Locked_Folder"))
                                    {
                                        TryDeleteDirectory(User_Settings_Vars.NorthstarInstallLocation  + User_Settings_Vars.Profile_Path + @"\mods\Northstar.Client\Locked_Folder", true);

                                    }
                                    if (Directory.Exists(User_Settings_Vars.NorthstarInstallLocation  + User_Settings_Vars.Profile_Path + @"\mods\Northstar.Custom\Locked_Folder"))
                                    {
                                        TryDeleteDirectory(User_Settings_Vars.NorthstarInstallLocation  + User_Settings_Vars.Profile_Path + @"\mods\Northstar.Custom\Locked_Folder", true);


                                    }
                                    if (Directory.Exists(User_Settings_Vars.NorthstarInstallLocation  + User_Settings_Vars.Profile_Path + @"\mods\Northstar.CustomServers\Locked_Folder"))
                                    {
                                        TryDeleteDirectory(User_Settings_Vars.NorthstarInstallLocation  + User_Settings_Vars.Profile_Path + @"\mods\Northstar.CustomServers\Locked_Folder", true);


                                    }
                                    if (!Directory.Exists(User_Settings_Vars.NorthstarInstallLocation + @"TempCopyFolder"))
                                    {
                                       TryCreateDirectory(User_Settings_Vars.NorthstarInstallLocation + @"TempCopyFolder");
                                    }

                                    if (do_not_overwrite_Ns_file == true)
                                    {
                                      
                                            if (File.Exists(User_Settings_Vars.NorthstarInstallLocation + @"ns_startup_args.txt"))
                                            {
                                               TryCopyFile(User_Settings_Vars.NorthstarInstallLocation + @"ns_startup_args.txt",User_Settings_Vars.NorthstarInstallLocation + @"TempCopyFolder\ns_startup_args.txt", true);
                                            }

                                        


                                    
                                        if (File.Exists(User_Settings_Vars.NorthstarInstallLocation +  User_Settings_Vars.Profile_Path + @"\mods\Northstar.CustomServers\mod\cfg\autoexec_ns_server.cfg"))
                                        {

                                            TryCopyFile(User_Settings_Vars.NorthstarInstallLocation +  User_Settings_Vars.Profile_Path + @"\mods\Northstar.CustomServers\mod\cfg\autoexec_ns_server.cfg", User_Settings_Vars.NorthstarInstallLocation + @"TempCopyFolder\autoexec_ns_server.cfg", true);



                                        }


                                    
                                        if (File.Exists(User_Settings_Vars.NorthstarInstallLocation + @"ns_startup_args_dedi.txt"))
                                        {


                                          TryCopyFile(User_Settings_Vars.NorthstarInstallLocation + @"ns_startup_args_dedi.txt", User_Settings_Vars.NorthstarInstallLocation + @"TempCopyFolder\ns_startup_args_dedi.txt", true);


                                        }

                                    }
                                    string searchQuery3 = "*" + "Northstar.dll" + "*";

                                    var Destinfo = new DirectoryInfo(Destination);
                                    var Script_ = Directory.GetFiles(Destination);


                                    var Script = Destinfo.GetFiles(searchQuery3, SearchOption.AllDirectories);
                                    Destinfo.Attributes &= ~FileAttributes.ReadOnly;


                                    if (Script.Length != 0)
                                    {
                                           var File_ = Script.FirstOrDefault();


                                        FileInfo FolderTemp = new FileInfo(File_.FullName);
                                        DirectoryInfo di = new DirectoryInfo(Directory.GetParent(File_.FullName).ToString());
                                        string firstFolder = di.FullName;
                                        Console.WriteLine(firstFolder);
                                        string Northstar_VEr_Temp = null;
                                        if (Directory.Exists(Destination))
                                        {
                                            await CopyFilesRecursively(firstFolder, User_Settings_Vars.NorthstarInstallLocation);

                                            }
                                          

                                        if (do_not_overwrite_Ns_file == true)
                                        {
                                          
                                                if (File.Exists(User_Settings_Vars.NorthstarInstallLocation + @"TempCopyFolder\ns_startup_args.txt"))
                                                {
                                                   TryCopyFile(User_Settings_Vars.NorthstarInstallLocation + @"TempCopyFolder\ns_startup_args.txt", User_Settings_Vars.NorthstarInstallLocation + @"ns_startup_args.txt", true);
                                                }

                                            


                                       
                                            if (File.Exists(User_Settings_Vars.NorthstarInstallLocation + @"TempCopyFolder\autoexec_ns_server.cfg"))
                                            {

                                               TryCopyFile (User_Settings_Vars.NorthstarInstallLocation + @"TempCopyFolder\autoexec_ns_server.cfg",User_Settings_Vars.NorthstarInstallLocation + User_Settings_Vars.Profile_Path + @"\mods\Northstar.CustomServers\mod\cfg\autoexec_ns_server.cfg",true);



                                            }


                                        
                                            if (File.Exists(User_Settings_Vars.NorthstarInstallLocation + @"TempCopyFolder\ns_startup_args_dedi.txt"))
                                            {


                                                TryCopyFile(User_Settings_Vars.NorthstarInstallLocation + @"TempCopyFolder\ns_startup_args_dedi.txt", User_Settings_Vars.NorthstarInstallLocation + @"ns_startup_args_dedi.txt", true);


                                            }

                                        }

                                        DispatchIfNecessary(() => {
                                            if (Progress_Bar != null)
                                            {
                                                Progress_Bar.IsIndeterminate = false;

                                                Progress_Bar.Value = 0;
                                            }
                                        });
                                        if (File.Exists(User_Settings_Vars.NorthstarInstallLocation + @"NorthstarLauncher.exe"))
                                            {

                                                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(User_Settings_Vars.NorthstarInstallLocation + @"NorthstarLauncher.exe");
                                            string Current_Ver_ = myFileVersionInfo.FileVersion;

                                                User_Settings_Vars.CurrentVersion = Current_Ver_;
                                                Properties.Settings.Default.Version = Current_Ver_;
                                                Properties.Settings.Default.Save();
                                            Northstar_VEr_Temp = Current_Ver_;
                                                DispatchIfNecessary(() =>
                                                {

                                                    Main.VERSION_TEXT.Text = "VTOL - " + ProductVersion + " | Northstar Version - " + Current_Ver_.Remove(0, 1);
                                                    Main.VERSION_TEXT.Refresh();
                                                });
                                        }

                                        DispatchIfNecessary(() =>
                                        {
                                            SnackBar.Title = "SUCCESS";
                                            SnackBar.Appearance = Wpf.Ui.Common.ControlAppearance.Success;
                                            string temp_;
                                            if(Northstar_VEr_Temp != null && Northstar_VEr_Temp.Length > 2)
                                            {
                                                temp_ = "The Build " + Path.GetFileNameWithoutExtension(Target_Zip).Replace("_", " ") + Northstar_VEr_Temp.Remove(0, 1) + VTOL.Resources.Languages.Language.Page_Thunderstore_Unpack_To_Location_Custom_HasBeenDownloadedAndInstalled;

                                            }
                                            else
                                            {
                                                temp_ = Path.GetFileNameWithoutExtension(Target_Zip).Replace("_", " ")+ VTOL.Resources.Languages.Language.Page_Thunderstore_Unpack_To_Location_Custom_HasBeenDownloadedAndInstalled;
                                            }
                                            SnackBar.Message = temp_;
                                            SnackBar.Show();


                                        });

                                    }

                                }
                                else if(Skin_Install == true)
                                {
                                    var ext = new List<string> { "zip" };
                                    var myFiles = Directory.EnumerateFiles(Destination, "*.*", SearchOption.AllDirectories).Where(s => ext.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));

                                    Install_Skin_Async_Starter(myFiles, Destination);
                                    DispatchIfNecessary(() =>
                                    {
                                        if (Progress_Bar != null)
                                        {
                                            Progress_Bar.Value = 0;
                                        }
                                    });
                                    }




                            }
                            else
                            {

                                string fileExts = System.IO.Path.GetExtension(Target_Zip);

                                if (fileExts == ".zip")
                                {
                                    string searchQuery3 = "*" + "mod.json" + "*";

                                   
                                    var Destinfo = new DirectoryInfo(Destination);


                                    var Script = Destinfo.GetFiles(searchQuery3, SearchOption.AllDirectories);
                                    Destinfo.Attributes &= ~FileAttributes.ReadOnly;
                                    Console.WriteLine(Script.Length.ToString());
                                    if (Script.Length != 0)
                                    {
                                        var File_ = Script.FirstOrDefault();

                                       

                                        FileInfo FolderTemp = new FileInfo(File_.FullName);
                                        DirectoryInfo di = new DirectoryInfo(Directory.GetParent(File_.FullName).ToString());
                                        string firstFolder = di.FullName;


                                        if (Directory.Exists(Destination))
                                        {




                                           TryCreateDirectory(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder");
                                            if (Directory.Exists(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder"))
                                            {
                                                await CopyFilesRecursively(firstFolder, Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder");




                                                Clear_Folder(Destination);
                                                await CopyFilesRecursively(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder", Destination);
                                                TryDeleteDirectory(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder", true);

                                            }
                                            Console.WriteLine("Unpacked - " + Destination);


                                        }
                                    }
                                    else if (Script.Length > 1)
                                    {



                                        foreach (var File_ in Script)
                                        {
                                            FileInfo FolderTemp = new FileInfo(File_.FullName);

                                            DirectoryInfo di = new DirectoryInfo(Directory.GetParent(File_.FullName).ToString());
                                            if (Directory.Exists(Destination))
                                            {
                                                TryCreateDirectory(Destination + @"\" + "Multipack");

                                                TryMoveFolder(di.FullName, Directory.GetParent(Destination).ToString() + @"\" + di.Name);

                                                // string firstFolder = di.FullName;


                                            }


                                        }
                                        Call_Mods_From_Folder_Lite();
                                        DispatchIfNecessary(() => {
                                            if (Progress_Bar != null)
                                            {
                                                Progress_Bar.Value = 0;
                                            }

                                            SnackBar.Title = "SUCCESS";
                                            SnackBar.Appearance = Wpf.Ui.Common.ControlAppearance.Success;
                                            SnackBar.Message = "The the multiple Mods in - " + mod_name + " - have been installed Succesfully";
                                            SnackBar.Show();


                                        });
                                    }
                                   

                                }
                                else
                                {
                                    Log.Warning("The File" + Target_Zip + "Is not a zip!!");
                                    SnackBar.Appearance = Wpf.Ui.Common.ControlAppearance.Caution;
                                    SnackBar.Content = "The File " + Target_Zip + " Is noT a zip!!";


                                }

                                DispatchIfNecessary(() => {
                                    if (Progress_Bar != null)
                                    {
                                        Progress_Bar.Value = 0;
                                    }
                                   
                                        SnackBar.Title = "SUCCESS";
                                        SnackBar.Appearance = Wpf.Ui.Common.ControlAppearance.Success;
                                        SnackBar.Message = "The Mod " + Path.GetFileNameWithoutExtension(Target_Zip).Replace("_", " ") + VTOL.Resources.Languages.Language.Page_Thunderstore_Unpack_To_Location_Custom_HasBeenDownloadedAndInstalled;
                                        SnackBar.Show();

                                    

                                });
                            }

                            
                        }
                        else
                        {

                            Log.Warning("The File" + Target_Zip + "Is noT a zip!!");
                            SnackBar.Appearance = Wpf.Ui.Common.ControlAppearance.Caution;
                            SnackBar.Content = "The File " + Target_Zip + " Is noT a zip!!";
                            DispatchIfNecessary(() =>
                            {
                                if (Progress_Bar != null)
                                {
                                    Progress_Bar.Value = 0;
                                }
                            });
                        }



                    }

                    else
                    {

                        if (!File.Exists(Target_Zip))
                        {
                            DispatchIfNecessary(() =>
                            {
                                if (Progress_Bar != null)
                                {
                                    Progress_Bar.Value = 0;
                                }
                            });
                            Log.Error("The Zip File" + Target_Zip + " was not found or does not exist?");


                        }
                        if (!Directory.Exists(Destination))
                        {
                            DispatchIfNecessary(() =>
                            {
                                if (Progress_Bar != null)
                                {
                                    Progress_Bar.Value = 0;
                                }
                            });
                            Log.Error("The Destination" + Destination + " is not accessible or does not exist?");


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger2.Open();
                 Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source +Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
Main.logger2.Close();

                DispatchIfNecessary(() =>
                {
                    if (Progress_Bar != null)
                    {
                        Progress_Bar.Value = 0;
                    }
                });
                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");


            }
        }
        public string ProductVersion
        {
            get
            {
                try
                {
                    string file = (FileVersionInfo.GetVersionInfo(Assembly.GetCallingAssembly().Location).ProductVersion).ToString();
                    return file.Substring(0, file.IndexOf("+") + 1).Replace("+", "");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");
                    Main.logger2.Open();
                    Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
                    Main.logger2.Close();

                }
                return "0.0.0";

            }
        }

        private string Find_Folder(string searchQuery, string folderPath)
        {
            searchQuery = "*" + searchQuery + "*";

            var directory = new DirectoryInfo(folderPath);

            var directories = directory.GetDirectories(searchQuery, SearchOption.AllDirectories);
            return directories[0].ToString();
        }
        async Task Install_Skin_Async_Starter(IEnumerable<string> in_, string Destination = "")
        {
          
            await Task.Run(async () => //Task.Run automatically unwraps nested Task types!
            {


                foreach (string i in in_)
                {
                    DispatchIfNecessary(() =>
                    {
                        Skin_Processor_ Skinp = new Skin_Processor_();


                         Skinp.Install_Skin_From_Path(i);

                      
                    });
                    await Task.Delay(1500);


                }

                if (Directory.Exists(Destination) && Destination != "")
                {
                    TryDeleteDirectory(Destination, true);
                }
            });
        }
        private void ScrollViewer_MouseEnter(object sender, MouseEventArgs e)
        {

          

            }

      


    
        private void Install_Bttn_Thunderstore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button Button;
                Button = sender as Button;
                ProgressBar Progress_Bar = null;
                HandyControl.Controls.SimplePanel _Panel = (HandyControl.Controls.SimplePanel)((Button)sender).Parent;
                Progress_Bar = FindVisualChild<ProgressBar>(_Panel);
                string tags = Button.ToolTip.ToString();
                if (tags.Count() < 2)
                {
                    tags = "Mods";
                }

                if (Button.Tag.ToString().Contains("http")|| Button.Tag.ToString().Contains("https"))
                {

                    if (tags.Contains("DDS"))
                    {


                            Download_Zip_To_Path(Button.Tag.ToString(), User_Settings_Vars.NorthstarInstallLocation + @"NS_Downloaded_Mods", Progress_Bar, true);

                    }
                    else if(Button.Tag.ToString().Contains("Northstar Release Candidate") || Button.Tag.ToString().Contains("NorthstarReleaseCandidate") || (Button.Tag.ToString().Contains("Northstar") && Button.ToolTip.ToString().Count() < 5))
                    {
                        Download_Zip_To_Path(Button.Tag.ToString(), User_Settings_Vars.NorthstarInstallLocation + @"NS_Downloaded_Mods", Progress_Bar, true,true);


                    }
                    else
                    {
                        Download_Zip_To_Path(Button.Tag.ToString(), User_Settings_Vars.NorthstarInstallLocation + @"NS_Downloaded_Mods", Progress_Bar, false);

                    }



                }
               



               
            }
            catch (Exception ex)
            {
                Main.logger2.Open();
                 Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source +Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name);
Main.logger2.Close();



                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");
            }

        }
        async void Reload_Search()
        {






        }
        private void TextInput_OnKeyUpDone(object sender, ElapsedEventArgs e)
        {
            // If we don't stop the timer, it will keep elapsing on repeat.
            try
            {
                _timer.Stop();


                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (sender, e) =>
                {

                    DispatchIfNecessary(() =>
                    {
                        if (init == true)
                        {
                            if (Search_Bar_Suggest_Mods.Text.Trim() != "" && Search_Bar_Suggest_Mods.Text != null && Search_Bar_Suggest_Mods.Text.Length != 0)
                            {
                                Thunderstore_List.ItemsSource = null;





                                Call_Ts_Mods(true, Search_: true, SearchQuery: Search_Bar_Suggest_Mods.Text);








                            }
                            else
                            {

                                Thunderstore_List.ItemsSource = null;





                                Call_Ts_Mods(true, Search_: true, SearchQuery: Search_Bar_Suggest_Mods.Text);

                            }



                        }


                    });


                };

                worker.RunWorkerAsync();


              
            }
            catch (Exception ex)
            {

                Main.logger2.Open();
                Main.logger2.Log($"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.TargetSite + Environment.NewLine + "From VERSION - " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine);
                Main.logger2.Close();


                Log.Error(ex, $"A crash happened at {DateTime.Now.ToString("yyyy - MM - dd HH - mm - ss.ff", CultureInfo.InvariantCulture)}{Environment.NewLine}");
            }
        }
            private void Search_Bar_Suggest_Mods_TextChanged(object sender, TextChangedEventArgs e)
        {
            _timer.Stop();
            _timer.Start();

        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void Dialog_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
          
            Dependency_Download( Dialog.Tag.ToString(), Progress_Cur_Temp);

        }
        
        private void Search_Filters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (page_loaded == true)
            {
                if (sender.GetType() == typeof(HandyControl.Controls.CheckComboBox))
                {
                    HandyControl.Controls.CheckComboBox comboBox = (HandyControl.Controls.CheckComboBox)sender;
                    Current_Mod_Filter_Tags = String.Join(",", comboBox.SelectedItems.Cast<String>()).Split(',').ToList();
                    Thunderstore_List.ItemsSource = null;
                    if (Search_Filters.SelectedItems != null && Search_Filters.SelectedItems.Count > 0) /*|| Options_List.Contains(Search_Filters.SelectedItem))*/
                    {
                        Category_Label.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        Category_Label.Visibility = Visibility.Visible;

                    }
                    Console.WriteLine(String.Join(",", Current_Mod_Filter_Tags));

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += (sender, e) =>
                    {



                        Call_Ts_Mods();


                    };
                    worker.RunWorkerCompleted += (sender, eventArgs) =>
                    {

                        Thunderstore_List.Refresh();


                    };
                    worker.RunWorkerAsync();












                }
            }

        }
        async Task OPEN_WEBPAGE(string URL)
        {
            await Task.Run(() =>
            {
                DispatchIfNecessary(() => {
                    SnackBar.Message = "Opening the Following URL - " + URL;
            SnackBar.Title = "INFO";
            SnackBar.Appearance = Wpf.Ui.Common.ControlAppearance.Info;
            SnackBar.Show();
                });

                Thread.Sleep(2000);
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = URL,
                UseShellExecute = true
            });
            });
        }
        private void Auto_Scroll_Description(Canvas canMain, TextBlock tbmarquee)
        {
            double width = canMain.ActualWidth - tbmarquee.ActualWidth;
            double H = tbmarquee.ActualHeight - canMain.ActualHeight;
            Console.WriteLine(H);
            tbmarquee.Margin = new Thickness(width / 2, 0, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = tbmarquee.ActualHeight;
            doubleAnimation.To = tbmarquee.ActualHeight + H;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.AutoReverse = true;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
            tbmarquee.BeginAnimation(Canvas.TopProperty, doubleAnimation);
        }

        private void Open_Webpage_Click(object sender, RoutedEventArgs e)
        {
           

               
            var URL = ((System.Windows.Controls.Button)sender).Tag.ToString();
            OPEN_WEBPAGE(URL);
           
        }

        private void ScrollViewer_MouseMove_1(object sender, MouseEventArgs e)
        {

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            e2.RoutedEvent = UIElement.MouseWheelEvent;
            Thunderstore_List.RaiseEvent(e2);
            e.Handled = true;
        }
        public static Size MeasureText(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
            GlyphTypeface glyphTypeface;

            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
            {
                return MeasureTextSize(text, fontFamily, fontStyle, fontWeight, fontStretch, fontSize);
            }

            double totalWidth = 0;
            double height = 0;

            for (int n = 0; n < text.Length; n++)
            {
                ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];

                double width = glyphTypeface.AdvanceWidths[glyphIndex] * fontSize;

                double glyphHeight = glyphTypeface.AdvanceHeights[glyphIndex] * fontSize;

                if (glyphHeight > height)
                {
                    height = glyphHeight;
                }

                totalWidth += width;
            }

            return new Size(totalWidth, height);
        }

        /// <summary>
        /// Get the required height and width of the specified text. Uses FortammedText
        /// </summary>
        public static Size MeasureTextSize(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            FormattedText ft = new FormattedText(text,
                                                    CultureInfo.CurrentCulture,
                                                    FlowDirection.LeftToRight,
                                                    new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                                                    fontSize,
                                                    Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }
       
        private void canMain_IsVisibleChanged(object sender, RoutedEventArgs e)
        {
          
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Sort.SelectedItem != null && Sort.SelectedItem.ToString().Length > 1)
            { 
                Sort_Label.Visibility = Visibility.Hidden;
            }
            else
            {
                Sort_Label.Visibility = Visibility.Visible;

            }
            if (page_loaded == true)
            {
                Thunderstore_List.ItemsSource = null;


                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (sender, e) =>
                {
                    

                    Call_Ts_Mods();


                    


                };
                worker.RunWorkerCompleted += (sender, eventArgs) =>
                {

                    Thunderstore_List.Refresh();


                };
                worker.RunWorkerAsync();

            }
        }

        private void Search_Bar_Suggest_Mods_KeyUp(object sender, KeyEventArgs e)
        {
           
               
        }

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Sort_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (page_loaded == true)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (sender, e) =>
                {


                    Call_Mods_From_Folder_Lite();
                    Call_Ts_Mods(clear:false);





                };
                worker.RunWorkerCompleted += (sender, eventArgs) =>
                {

                    Thunderstore_List.Refresh();


                };
             
          
           
            worker.RunWorkerAsync();
            }

        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {

            
        }
    }
        
}
