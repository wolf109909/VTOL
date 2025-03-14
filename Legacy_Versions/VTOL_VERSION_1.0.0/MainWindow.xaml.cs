﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HandyControl;
using System.IO.Compression;
using System.IO;
using Path = System.IO.Path;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Microsoft.Win32;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;
using Utils.Extensions;
using static VTOL.MainWindow;
using Microsoft.Xaml.Behaviors;
using System.Threading;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Windows.Threading;
using Zipi = Ionic.Zip;
using System.Net.NetworkInformation;
using System.Timers;
using HandyControl.Data;
using System.Security.Principal;
using Walterlv.Windows.Interop;
using Newtonsoft.Json;
using System.Windows.Media.Effects;
using Aspose.Zip;
using System.Runtime.CompilerServices;

//****TODO*****//

//Migrate Release Parse to the New Updater Sys
//Migrate all the json code to the new wrapped Updater System.

namespace System.Windows.Media
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
    internal static class ColorExtensions
    {
        public static float GetBrightness(this Color color)
        {
            float num = ((float)color.R) / 255f;
            float num2 = ((float)color.G) / 255f;
            float num3 = ((float)color.B) / 255f;
            float num4 = num;
            float num5 = num;
            if (num2 > num4)
                num4 = num2;
            if (num3 > num4)
                num4 = num3;
            if (num2 < num5)
                num5 = num2;
            if (num3 < num5)
                num5 = num3;
            return ((num4 + num5) / 2f);
        }

        public static float GetHue(this Color color)
        {
            if ((color.R == color.G) && (color.G == color.B))
                return 0f;
            float num = ((float)color.R) / 255f;
            float num2 = ((float)color.G) / 255f;
            float num3 = ((float)color.B) / 255f;
            float num7 = 0f;
            float num4 = num;
            float num5 = num;
            if (num2 > num4)
                num4 = num2;
            if (num3 > num4)
                num4 = num3;
            if (num2 < num5)
                num5 = num2;
            if (num3 < num5)
                num5 = num3;
            float num6 = num4 - num5;
            if (num == num4)
                num7 = (num2 - num3) / num6;
            else if (num2 == num4)
                num7 = 2f + ((num3 - num) / num6);
            else if (num3 == num4)
                num7 = 4f + ((num - num2) / num6);
            num7 *= 60f;
            if (num7 < 0f)
                num7 += 360f;
            return num7;
        }

        public static float GetSaturation(this Color color)
        {
            float num = ((float)color.R) / 255f;
            float num2 = ((float)color.G) / 255f;
            float num3 = ((float)color.B) / 255f;
            float num7 = 0f;
            float num4 = num;
            float num5 = num;
            if (num2 > num4)
                num4 = num2;
            if (num3 > num4)
                num4 = num3;
            if (num2 < num5)
                num5 = num2;
            if (num3 < num5)
                num5 = num3;
            if (num4 == num5)
                return num7;
            float num6 = (num4 + num5) / 2f;
            if (num6 <= 0.5)
                return ((num4 - num5) / (num4 + num5));
            return ((num4 - num5) / ((2f - num4) - num5));
        }
    }
}

//**************//
namespace VTOL
{
    public enum IPStatus
    {
        Unknown = -1, // 0xFFFFFFFF
        Success = 0,
        DestinationNetworkUnreachable = 11002, // 0x00002AFA
        DestinationHostUnreachable = 11003, // 0x00002AFB
        DestinationProhibited = 11004, // 0x00002AFC
        DestinationProtocolUnreachable = 11004, // 0x00002AFC
        DestinationPortUnreachable = 11005, // 0x00002AFD
        NoResources = 11006, // 0x00002AFE
        BadOption = 11007, // 0x00002AFF
        HardwareError = 11008, // 0x00002B00
        PacketTooBig = 11009, // 0x00002B01
        TimedOut = 11010, // 0x00002B02
        BadRoute = 11012, // 0x00002B04
        TtlExpired = 11013, // 0x00002B05
        TtlReassemblyTimeExceeded = 11014, // 0x00002B06
        ParameterProblem = 11015, // 0x00002B07
        SourceQuench = 11016, // 0x00002B08
        BadDestination = 11018, // 0x00002B0A
        DestinationUnreachable = 11040, // 0x00002B20
        TimeExceeded = 11041, // 0x00002B21
        BadHeader = 11042, // 0x00002B22
        UnrecognizedNextHeader = 11043, // 0x00002B23
        IcmpError = 11044, // 0x00002B24
        DestinationScopeMismatch = 11045, // 0x00002B25 A?
    }

    public static class ExtensionMethods
    {
        private static readonly Action EmptyDelegate = delegate { };
        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class Button : INotifyPropertyChanged
    {
        public string Tag;
        public string Name;
        public string Icon;
        public string owner;
        public string description;
        public string download_url;
        public string Webpage;
        public string date_created;
        public int Rating;
        public string File_Size;
        public string Downloads;

        public string Tag_
        {

            get { return Tag; }
            set { Tag = value; NotifyPropertyChanged("Tag"); }
        }
        public string Name_
        {

            get { return Name; }
            set { Name = value; NotifyPropertyChanged("Name"); }
        }
        public string Icon_
        {

            get { return Icon; }
            set { Icon = value; NotifyPropertyChanged("Icon"); }
        }
        public string owner_
        {

            get { return owner; }
            set { owner = value; NotifyPropertyChanged("owner"); }
        }
        public string description_
        {

            get { return description; }
            set { description = value; NotifyPropertyChanged("description"); }
        }
        public string download_url_
        {

            get { return download_url; }
            set { download_url = value; NotifyPropertyChanged("download_url"); }
        }
        public string Webpage_
        {

            get { return Webpage; }
            set { Webpage = value; NotifyPropertyChanged("Webpage"); }
        }

        public string date_created_
        {

            get { return date_created; }
            set { date_created = value; NotifyPropertyChanged("date_created"); }
        }
        public int Rating_
        {

            get { return Rating; }
            set { Rating = value; NotifyPropertyChanged("Rating"); }
        }
        public string File_Size_
        {

            get { return File_Size; }
            set { File_Size = value; NotifyPropertyChanged("Name"); }
        }
        public string Downloads_
        {

            get { return Downloads; }
            set { Downloads = value; NotifyPropertyChanged("Downloads"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string Property)
        {

            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(Property));
                PropertyChanged(this, new PropertyChangedEventArgs("DisplayMember"));
            }
        }

    }
    public class Server_Template_Selector : DataTemplateSelector
    {

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            var Item = item as Arg_Set;
            if (Item == null) { return null; }

            if (Item.Type == "PORT" || Item.Type == "STRING" || Item.Type == "STRINGQ")
                return
                    element.FindResource("NormalBox")
                    as DataTemplate;
            else if (Item.Type == "INT")
                return
                    element.FindResource("IntBox")
                    as DataTemplate;
            else if (Item.Type == "FLOAT")
                return
                    element.FindResource("FloatBox")
                    as DataTemplate;
            else if (Item.Type == "BOOL")
                return
                    element.FindResource("BOOLBox")
                    as DataTemplate;
            else if (Item.Type == "ONE_SELECT")
                return
                    element.FindResource("One_Select_Combo")
                    as DataTemplate;
            else
                return
                    element.FindResource("ComboBox")
                    as DataTemplate;


        }





    }


    public partial class MainWindow : Window
    {
        public bool Found_Install_Folder = false;


        BitmapImage Vanilla = new BitmapImage(new Uri(@"/Resources/TF2_Vanilla_promo.gif", UriKind.Relative));
        BitmapImage Northstar = new BitmapImage(new Uri(@"/Resources/Northstar_Smurfson.gif", UriKind.Relative));
        static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();
        string LAST_INSTALLED_MOD = "";
        List<string> Mod_Directory_List_Active = new List<string>();
        List<string> Mod_Directory_List_InActive = new List<string>();
        private static String updaterModulePath;
        private readonly CollectionViewSource viewSource = new CollectionViewSource();
        public Server_Setup Server_Json;
        public string Current_Install_Folder = "";
        private string NSExe;
        private bool NS_Installed;
        private WebClient webClient = null;
        string current_Northstar_version_Url;
        int failed_search_counter = 0;
        bool deep_Chk = false;
        List<string> Mod_List = new List<string>();
        bool do_not_overwrite_Ns_file = true;
        bool do_not_overwrite_Ns_file_Dedi = true;
        List<object> itemsList = new List<object>();
        public IEnumerable<string> Phrases { get; private set; }
        public List<string> Game_Modes_List = new List<string>();
        public List<string> Game_MAP_List = new List<string>();
        public List<string> Game_WEAPON_List = new List<string>();
        private static readonly Action EmptyDelegate = delegate { };
        public string Current_REPO_URL;
        public string Author_Used;
        public string Repo_Used;
        public string MasterServer_URL;
        public string MasterServer_URL_CN = "nscn.wolf109909.top";
        public string Current_REPO_URL_CN = "https://nscn.wolf109909.top/version/query";
        public bool Loaded_ = false;
        public string Current_Ver_ = "NODATA";
        public int pid;
        string Skin_Path = "";
        string Skin_Temp_Loc = "";
        bool Finished_Init = false;
        public Thunderstore_V1 Thunderstore_;
        Updater Update;
        public bool Animation_Start_Northstar { get; set; }
        public bool Completed_Operation = false; 

        public bool Animation_Start_Vanilla { get; set; }
        public string Ns_dedi_File = "";
        public string Convar_File = "";
        bool Started_Selection = false;
        public bool Sort_Lists;
        bool Origin_Client_Running = false;
        List<object> Temp = new List<object> { };
        bool Warn_Close_EA;
        SolidColorBrush Accent_Color = (SolidColorBrush)new BrushConverter().ConvertFrom("#2C4C4C");
        SolidColorBrush Border_Color = (SolidColorBrush)new BrushConverter().ConvertFrom("#2C4C4C");
        ObservableCollection<object> OjectList = new ObservableCollection<object>();
        int completed_flag;
        bool PackasSkin = false;
        public string Thunderstore_Template_Text = @"#PLACEHOLDER_SKIN_NAME";
        User_Settings User_Settings_Vars = null;
        public string DocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public class Colors_Set
        {
            public SolidColorBrush Accent_Color;

            public SolidColorBrush Accent_Color_
            {

                get { return Accent_Color; }
                set { Accent_Color = value; }
            }
        }
        
        public MainWindow()
        {
            Startup splash = new Startup();

            splash.Show();
            Application.Current.MainWindow.Loaded += VTOL_Loaded;


            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);


            InitializeComponent();
            BeginInit();



            do_not_overwrite_Ns_file = Properties.Settings.Default.Ns_Startup;
            do_not_overwrite_Ns_file_Dedi = Properties.Settings.Default.Ns_Dedi;
            Sort_Lists = Properties.Settings.Default.Sort_Mods;
            Warn_Close_EA = Properties.Settings.Default.Warning_Close_EA;
            //Author_Used = Properties.Settings.Default.Author;
            //Repo_Used = Properties.Settings.Default.Repo;
            //MasterServer_URL = Properties.Settings.Default.MasterServer_URL;
            //Current_REPO_URL = Properties.Settings.Default.Current_REPO_URL;
            PackasSkin = Properties.Settings.Default.PackageAsSkin;
            Skin_Mod_Pack_Check.IsChecked = Properties.Settings.Default.PackageAsSkin;
            //  Test_List.ItemsSource = itemsList;

            try
            {
                // InstalledApplications.GetInstalledApps();
                //      MessageBox.Show(InstalledApplications.GetApplictionInstallPath("Titanfall2 "));
                //  _Item_Filter = CollectionViewSource.GetDefaultView(itemsList);

                /*Ref Code To see The ISO name for Lang
                //Console.WriteLine("Default Language Info:");
                //Console.WriteLine("* Name: {0}", ci.Name);
                //Console.WriteLine("* Display Name: {0}", ci.DisplayName);
                //Console.WriteLine("* English Name: {0}", ci.EnglishName);
                //Console.WriteLine("* 2-letter ISO Name: {0}", ci.TwoLetterISOLanguageName);
                //Console.WriteLine("* 3-letter ISO Name: {0}", ci.ThreeLetterISOLanguageName);
                //Console.WriteLine("* 3-letter Win32 API Name: {0}", ci.ThreeLetterWindowsLanguageName);
                */


                Mod_dependencies.Text = "northstar-Northstar-" + Properties.Settings.Default.Version.Remove(0, 1);
                // Web_Browser.JavascriptMessageReceived += Web_Browser_JavascriptMessageReceived();
                DataContext = this;
                Animation_Start_Northstar = false;
                Animation_Start_Vanilla = false;
                Write_To_Log("", true);
                LOG_BOX.IsReadOnly = true;
                Loading_Panel.Visibility = Visibility.Collapsed;
                Mod_Panel.Visibility = Visibility.Collapsed;
                //  Load_Line.Visibility = Visibility.Collapsed;
                skins_Panel.Visibility = Visibility.Collapsed;
                Main_Panel.Visibility = Visibility.Visible;
                About_Panel.Visibility = Visibility.Collapsed;
                Mod_Browse_Panel.Visibility = Visibility.Collapsed;
                About_Panel.Visibility = Visibility.Collapsed;
                Dedicated_Server_Panel.Visibility = Visibility.Collapsed;
                Log_Panel.Visibility = Visibility.Collapsed;
                Updates_Panel.Visibility = Visibility.Collapsed;
                Drag_Drop_Overlay.Visibility = Visibility.Collapsed;
                Drag_Drop_Overlay_Skins.Visibility = Visibility.Collapsed;
                Tools_Panel.Visibility = Visibility.Collapsed;

                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                Install_Skin_Bttn.IsEnabled = false;
                Badge.Visibility = Visibility.Collapsed;
                Server_Indicator.Fill = Brushes.Red;
                Log_Indicator.Background = Brushes.Transparent;

                string Header = Path.GetFullPath(Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, @"../"));
                updaterModulePath = Path.Combine(Header, "VTOL_Updater.exe");


                if (Directory.Exists(DocumentsFolder))
                {

                    if (!File.Exists(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json"))
                    {
                        Directory.CreateDirectory(DocumentsFolder + @"\VTOL_DATA\Settings");
                        dynamic User_Settings_Json = new JObject();
                        User_Settings_Json.Current_Version = "NODATA";
                        User_Settings_Json.Theme = "NODATA";
                        User_Settings_Json.Master_Server_Url = "Northstar.tf";
                        User_Settings_Json.Repo = "Northstar";
                        User_Settings_Json.Language = "NODATA";
                        User_Settings_Json.Repo_Url = "NODATA";
                        User_Settings_Json.Northstar_Install_Location = "NODATA";
                        User_Settings_Json.MasterServer_URL_CN = "nscn.wolf109909.top";
                        User_Settings_Json.Current_REPO_URL_CN = "https://nscn.wolf109909.top/version/query";
                        User_Settings_Json.Author = "R2Northstar";

                        var User_Settings_Json_String = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Json);

                        using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", true))
                        {
                            StreamWriter.WriteLine(User_Settings_Json_String.ToString());
                            StreamWriter.Close();
                        }
                        User_Settings_Vars = User_Settings.FromJson(User_Settings_Json_String);
                    }
                    else
                    {
                        string User_Settings_String = System.IO.File.ReadAllText(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json");
                        
                         User_Settings_Vars = User_Settings.FromJson(User_Settings_String);

                        

                    }

                }
                else
                {
                    Application.Current.Shutdown();

                }


                



                Task.WaitAll(Set_About(), Select_Main(), getProcessorInfo());

                if (User_Settings_Vars != null)
                {

                    if (User_Settings_Vars.Language != "NODATA")
                    {
                        ChangeLanguageTo(User_Settings_Vars.Language);
                    }
                    else
                    {
                        CultureInfo ci = CultureInfo.InstalledUICulture;
                        if (ci.TwoLetterISOLanguageName == "zh")
                        {
                            ChangeLanguageTo("cn");
                        }
                        else // this is due to we misused cn and zh. zh is the actual languageName and cn is what we have in file.
                        {
                            ChangeLanguageTo(ci.TwoLetterISOLanguageName);
                        }

                        Write_To_Log("\nLanguage Detected was - " + ci.TwoLetterISOLanguageName);
                    }
                    Author_Used = User_Settings_Vars.Author;
                                      
                        Repo_Used = User_Settings_Vars.Repo;
                                      
                        Current_REPO_URL = User_Settings_Vars.RepoUrl;
                                                           
                        Current_Ver_ = User_Settings_Vars.CurrentVersion;

                        MasterServer_URL = User_Settings_Vars.MasterServerUrl;
                        
                        Current_Install_Folder = User_Settings_Vars.NorthstarInstallLocation;


                    if (isValidHexaCode(User_Settings_Vars.Theme))
                    {

                        Accent_Color = (SolidColorBrush)new BrushConverter().ConvertFrom(User_Settings_Vars.Theme);
                        ColorPicker_Accent.SelectedBrush = (SolidColorBrush)new BrushConverter().ConvertFrom(User_Settings_Vars.Theme);

                        Colors_Set Colors_Set = new Colors_Set { Accent_Color = Accent_Color };
                        this.DataContext = Colors_Set;
                        this.Resources["Button_BG"] = (SolidColorBrush)new BrushConverter().ConvertFrom(ColorPicker_Accent.SelectedBrush.Color.ToString());

                    }
                    else
                    {
                        this.Resources["Button_BG"] = Accent_Color;

                    }
                }

               


                if (Directory.Exists(Current_Install_Folder))
                    {
                        //if (File.Exists(Current_Install_Folder + @"\ns_version.txt") )
                        //{
                        //    Current_Ver_ = File.ReadAllText(Current_Install_Folder + @"\ns_version.txt");
                        //    User_Settings_Vars.CurrentVersion = Current_Ver_;
                        //    Properties.Settings.Default.Version = Current_Ver_;
                        //    Properties.Settings.Default.Save();
                        //}
                        
                        Titanfall2_Directory_TextBox.Text = Current_Install_Folder;
                        Send_Info_Notif(GetTextResource("NOTIF_INFO_FOUND_INSTALL_PATH") +" "+ Current_Install_Folder + "\n");

                        
                    // Install_Textbox.BackColor = Color.White;
                   

                        NSExe = Get_And_Set_Filepaths(Current_Install_Folder, "NorthstarLauncher.exe");
                        Check_Integrity_Of_NSINSTALL();
                        


                        if (Titanfall2_Directory_TextBox.Text == null || Titanfall2_Directory_TextBox.Text == "")
                        {

                            Titanfall2_Directory_TextBox.Background = Brushes.Red;
                        }
                        else
                        {
                            Titanfall2_Directory_TextBox.Background = Brushes.White;


                        }


                    }
                    else
                    {

                    //Current_Install_Folder = InstalledApplications.GetApplictionInstallPath("Titanfall2");
                    string Path = Auto_Find_And_verify();
                    if (IsValidPath(Path))
                    {
                        Current_Install_Folder = Path;
                        User_Settings_Vars.NorthstarInstallLocation = Path;
                        string User_Settings_Json_Strings_ = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
                        Titanfall2_Directory_TextBox.Text = Current_Install_Folder;
                        NSExe = Get_And_Set_Filepaths(Current_Install_Folder, "NorthstarLauncher.exe");

                        using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
                        {
                            StreamWriter.WriteLine(User_Settings_Json_Strings_);
                            StreamWriter.Close();
                        }
                        Check_Integrity_Of_NSINSTALL();
                    }
                    else
                    {
                        Send_Warning_Notif(GetTextResource("NOTIF_ERROR_INVALID_INSTALL_PATH"));
                        Send_Warning_Notif(GetTextResource("NOTIF_ERROR_NS_BAD_INTEGRITY"));

                    }

                }
                
                // string[] arguments = Environment.GetCommandLineArgs();

                //Console.WriteLine("GetCommandLineArgs: {0}", string.Join(", ", arguments));


                if (do_not_overwrite_Ns_file == true)
                {
                    Ns_Args.IsChecked = true;

                }
                else
                {

                    Ns_Args.IsChecked = false;

                }
                if (do_not_overwrite_Ns_file_Dedi == true)
                {
                    Ns_Args_Dedi.IsChecked = true;

                }
                else
                {

                    Ns_Args_Dedi.IsChecked = false;

                }

                Check_For_New_Northstar_Install();








                // HandyControl.Controls.Growl.InfoGlobal(Header);
                // Send_Info_Notif(Properties.Settings.Default.Version);

                if (Directory.Exists(Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR"))
                {
                    try
                    {

                        Directory.Delete(Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR", true);
                        GC.Collect();
                    }
                    catch (Exception ef)
                    {
                        Write_To_Log(ErrorManager(ef));

                        Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_CONTACT"));
                    }




                }
                if (Directory.Exists(Current_Install_Folder + @"\Thumbnails"))
                {
                    try
                    {
                        Directory.Delete(Current_Install_Folder + @"\Thumbnails", true);
                    }
                    catch (Exception ef)
                    {
                        Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_CONTACT"));
                        Write_To_Log(ErrorManager(ef));

                    }

                }


                if (Directory.Exists(@"C:\ProgramData\VTOL_DATA"))
                {

                    System.Windows.MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo { Message = "Detected Legacy Files For VTOL\n WOULD YOU LIKE TO MIGRATE AND DELETE?", Caption = "INFO", Button = MessageBoxButton.YesNo, IconBrushKey = ResourceToken.AccentBrush, IconKey = ResourceToken.AskGeometry, StyleKey = "MessageBoxCustom" });
                    if (result == System.Windows.MessageBoxResult.Yes)
                    {


                        if (File.Exists(@"C:\ProgramData\VTOL_DATA\VARS\AUTHOR.txt"))
                        {
                            User_Settings_Vars.Author= Read_From_TextFile_OneLine(@"C:\ProgramData\VTOL_DATA\VARS\AUTHOR.txt").Trim();
                            File.Delete(@"C:\ProgramData\VTOL_DATA\VARS\AUTHOR.txt");     
                        }
                        if (File.Exists(@"C:\ProgramData\VTOL_DATA\VARS\REPO.txt"))
                        {
                            User_Settings_Vars.Repo  = Read_From_TextFile_OneLine(@"C:\ProgramData\VTOL_DATA\VARS\REPO.txt").Trim();
                            File.Delete(@"C:\ProgramData\VTOL_DATA\VARS\REPO.txt");
                        }
                        if (File.Exists(@"C:\ProgramData\VTOL_DATA\VARS\REPO_URL.txt"))
                        {
                            User_Settings_Vars.RepoUrl= Read_From_TextFile_OneLine(@"C:\ProgramData\VTOL_DATA\VARS\REPO_URL.txt").Trim();
                            File.Delete(@"C:\ProgramData\VTOL_DATA\VARS\REPO_URL.txt");
                        }
                        if (File.Exists(@"C:\ProgramData\VTOL_DATA\VARS\MASTER_SERVERURL.txt"))
                        {
                            User_Settings_Vars.MasterServerUrl  = Read_From_TextFile_OneLine(@"C:\ProgramData\VTOL_DATA\VARS\MASTER_SERVERURL.txt").Trim();
                            File.Delete(@"C:\ProgramData\VTOL_DATA\VARS\MASTER_SERVERURL.txt");

                        }
                        if (File.Exists(@"C:\ProgramData\VTOL_DATA\VARS\Current_Ver.txt"))
                        {
                            User_Settings_Vars.CurrentVersion  = Read_From_TextFile_OneLine(@"C:\ProgramData\VTOL_DATA\VARS\Current_Ver.txt").Trim();
                            File.Delete(@"C:\ProgramData\VTOL_DATA\VARS\Current_Ver.txt");

                        }
                        if (File.Exists(@"C:\ProgramData\VTOL_DATA\VARS\INSTALL.txt"))
                        {
                            User_Settings_Vars.NorthstarInstallLocation = Read_From_TextFile_OneLine(@"C:\ProgramData\VTOL_DATA\VARS\INSTALL.txt");
                            File.Delete(@"C:\ProgramData\VTOL_DATA\VARS\INSTALL.txt");

                        }
                        if (File.Exists(@"C:\ProgramData\VTOL_DATA\VARS\Theme.txt"))
                        {
                            User_Settings_Vars.Theme = Read_From_TextFile_OneLine(@"C:\ProgramData\VTOL_DATA\VARS\Theme.txt");
                            File.Delete(@"C:\ProgramData\VTOL_DATA\VARS\Theme.txt");

                        }
                        if (File.Exists(@"C:\ProgramData\VTOL_DATA\VARS\Language.txt"))
                        {
                            User_Settings_Vars.Language = Read_From_TextFile_OneLine(@"C:\ProgramData\VTOL_DATA\VARS\Language.txt");
                            File.Delete(@"C:\ProgramData\VTOL_DATA\VARS\Language.txt");

                        }
                        Directory.Delete(@"C:\ProgramData\VTOL_DATA", true);
                    }
                }


              

                string User_Settings_Json_Strings = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
                using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
                {
                    StreamWriter.WriteLine(User_Settings_Json_Strings);
                    StreamWriter.Close();
                }


                
                Origin_Client_Running = Check_Process_Running("OriginClientService");

                PingHost(MasterServer_URL);
                 System.Timers.Timer aTimer = new System.Timers.Timer(TimeSpan.FromSeconds(300).TotalMilliseconds); 
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                 aTimer.Start();
                Log_Indicator.Background = Brushes.Transparent;
                if (Properties.Settings.Default.Version != "NODATA" || Properties.Settings.Default.Version != "")
                {
                    this.VTOL.Title = String.Format("VTOL {0}", version + "  |  Northstar Version - " + Properties.Settings.Default.Version.Remove(0, 1));


                }
                else
                {
                    this.VTOL.Title = String.Format("VTOL {0}", version);



                }

                if (NS_Installed == true)
                {


                    Install_NS.Content = GetTextResource("UPDATE_REPAIR_NS");
                }
                else
                {

                    Install_NS.Content = GetTextResource("INSTALL_NS");



                }
            }
            catch (Exception ef)
            {
                Send_Warning_Notif(GetTextResource("NOTIF_ERROR_NS_BAD_INTEGRITY"));
                Write_To_Log(ErrorManager(ef));
                splash.Close();

            }
            EndInit();
            splash.Close();


        }
        void pencereac<T>(int Ops) where T : Window, new()
        {
            if (!Application.Current.Windows.OfType<T>().Any()) // Check is Not Open, Open it.
            {
                var wind = new T();
                switch (Ops)
                {
                    case 1:
                        wind.Close();
                        break;
                    case 0:
                        wind.Show();
                        break;
                }
            }
        }
        static bool isValidHexaCode(string str)
        {
            if (str[0] != '#')
                return false;

            if (!(str.Length < 4 || str.Length >= 7))
                return false;

            for (int i = 1; i < str.Length; i++)
                if (!((str[i] >= '0' && str[i] <= 9)
                    || (str[i] >= 'a' && str[i] <= 'f')
                    || (str[i] >= 'A' || str[i] <= 'F')))
                    return false;

            return true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Finished_Init = false;
            PingHost(MasterServer_URL);
            Origin_Client_Running = Check_Process_Running("OriginClientService");
            Indicator_Panel.Refresh();
            GC.Collect();

        }
        public bool Check_Process_Running(string ProcessName, bool generic = false)
        {

            Process[] pname = Process.GetProcessesByName(ProcessName);
            if (pname.Length == 0)
            {
                if (generic == false)
                {
                    Indicator_Origin_Client.Visibility = Visibility.Visible;
                    Origin_Client_Status.Fill = Brushes.Red;
                }
                return false;
            }
            else
            {
                if (generic == false)
                {
                    Indicator_Origin_Client.Visibility = Visibility.Hidden;
                    Origin_Client_Status.Fill = Brushes.LimeGreen;
                }
                return true;
            }

        }
        public void call_TS_MODS()
        {

            try
            {
                
                    if (Finished_Init == false)
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                        // Thread thread = new Thread(delegate ()
                        // {
                        Thread.Sleep(50);
                        Check_Tabs(true);

                        // });
                        //thread.IsBackground = true;
                        // thread.Start();
                    }

                    else
                    {


                        Check_Tabs(false);

                    }
              


            }
            catch (Exception ex)
            {
                Send_Fatal_Notif("Fatal error\n");
                Write_To_Log(ErrorManager(ex));
            }
        }
        //This function is used to get string content from Resource file.
        public string GetTextResource(string ResourceName)
        {
            string TextResource;

            try
            {
                TextResource = VTOL.FindResource(ResourceName).ToString();
            }
            catch (ResourceReferenceKeyNotFoundException ex)
            {
                Send_Fatal_Notif("a FATAL error has occured while requesting dynamic Text resource : " + ResourceName + " detail:" + ex);
                return "undefined";
            }
            return TextResource;

        }
        private async Task ChangeLanguageTo(string LanguageCode)

        {
            try
            {

                ResourceDictionary dict = new ResourceDictionary();
                MasterServer_URL = Properties.Settings.Default.MasterServer_URL;
                Current_REPO_URL = Properties.Settings.Default.Current_REPO_URL;
                Master_ServerBox.Text = MasterServer_URL;
                Repo_URl.Text = Current_REPO_URL;
                //  this.Resources.MergedDictionaries[0] = dict;

                switch (LanguageCode)
                {

                    case "en":
                        Language_Selection.SelectedIndex = 0;
                        dict.Source = new Uri(@"Resources\Languages\" + LanguageCode + ".xaml", UriKind.Relative);
                        this.Resources["Northstar_Icon"] = new BitmapImage(new Uri(@"\Resources\NS_ICON.png", UriKind.Relative));
                        this.Resources.MergedDictionaries.Add(dict);
                    
                        
                      
                       
                        break;
                    case "fr":
                        Language_Selection.SelectedIndex = 1;
                        dict.Source = new Uri(@"Resources\Languages\" + LanguageCode + ".xaml", UriKind.Relative);
                        this.Resources["Northstar_Icon"] = new BitmapImage(new Uri(@"\Resources\NS_ICON.png", UriKind.Relative));
                       
                        Master_ServerBox.Text = MasterServer_URL;
                        Repo_URl.Text = Current_REPO_URL;
                        this.Resources.MergedDictionaries.Add(dict);
                      
                        break;
                    case "de":
                        Language_Selection.SelectedIndex = 2;
                        dict.Source = new Uri(@"Resources\Languages\" + LanguageCode + ".xaml", UriKind.Relative);
                        this.Resources["Northstar_Icon"] = new BitmapImage(new Uri(@"\Resources\NS_ICON.png", UriKind.Relative));
                  
                        Master_ServerBox.Text = MasterServer_URL;
                        Repo_URl.Text = Current_REPO_URL;
                        this.Resources.MergedDictionaries.Add(dict);
                    
                        break;
                    case "it":
                        Language_Selection.SelectedIndex = 3;
                        dict.Source = new Uri(@"Resources\Languages\" + LanguageCode + ".xaml", UriKind.Relative);
                        this.Resources["Northstar_Icon"] = new BitmapImage(new Uri(@"\Resources\NS_ICON.png", UriKind.Relative));
                  
                        Master_ServerBox.Text = MasterServer_URL;
                        Repo_URl.Text = Current_REPO_URL;
                        this.Resources.MergedDictionaries.Add(dict);
                       
                        break;
                    case "cn":
                        Language_Selection.SelectedIndex = 4;
                        dict.Source = new Uri(@"Resources\Languages\" + LanguageCode + ".xaml", UriKind.Relative);
                        this.Resources["Northstar_Icon"] = new BitmapImage(new Uri(@"\Resources\NSCN_ICON.png", UriKind.Relative));
                        this.Resources.MergedDictionaries.Add(dict);
                        MasterServer_URL = MasterServer_URL_CN;
                        Current_REPO_URL = Current_REPO_URL_CN;
                        Master_ServerBox.Text = MasterServer_URL_CN;
                        Repo_URl.Text = Current_REPO_URL_CN;
                        User_Settings_Vars.MasterServerUrl = MasterServer_URL_CN;
                        User_Settings_Vars.RepoUrl = Current_REPO_URL_CN;


                        break;
                    case "kr":
                        Language_Selection.SelectedIndex = 5;
                        dict.Source = new Uri(@"Resources\Languages\" + LanguageCode + ".xaml", UriKind.Relative);
                        this.Resources["Northstar_Icon"] = new BitmapImage(new Uri(@"\Resources\NS_ICON.png", UriKind.Relative));
               
                        this.Resources.MergedDictionaries.Add(dict);
                        Master_ServerBox.Text = MasterServer_URL;
                        Repo_URl.Text = Current_REPO_URL;
                      
                        break;
                    default:
                        LanguageCode = "en";
                        Language_Selection.SelectedIndex = 0;
                        dict.Source = new Uri(@"Resources\Languages\" + LanguageCode + ".xaml", UriKind.Relative);
                        this.Resources["Northstar_Icon"] = new BitmapImage(new Uri(@"\Resources\NS_ICON.png", UriKind.Relative));
                  
                        this.Resources.MergedDictionaries.Add(dict);
                        Master_ServerBox.Text = MasterServer_URL;
                        Repo_URl.Text = Current_REPO_URL;
                    
                        break;

                }
                if (NS_Installed == true)
                {


                    Install_NS.Content = GetTextResource("UPDATE_REPAIR_NS");
                }
                else
                {

                    Install_NS.Content = GetTextResource("INSTALL_NS");


                }
                Badge.Text = GetTextResource("BADGE_NEW_UPDATE_AVAILABLE");
                User_Settings_Vars.MasterServerUrl = MasterServer_URL;
                User_Settings_Vars.RepoUrl = Current_REPO_URL;
                User_Settings_Vars.Language = LanguageCode;
                string User_Settings_Json_Strings = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
                using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
                {
                    StreamWriter.WriteLine(User_Settings_Json_Strings);
                    StreamWriter.Close();
                }

            }
            catch (Exception ex)
            {
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_ERROR_OCCURRED"));
                Write_To_Log(ErrorManager(ex));
            }
        }


        public class PropertyGridDemoModel
        {
            [Category("Repository Settings")]
            public string Repository_URL { get; set; }
            [Category("Launch Settings")]
            public bool Start_On_Top { get; set; }
            [Category("App Settings")]
            public Times Duration_of_popups { get; set; }

            public string GetRepo()
            {
                return Repository_URL;

            }
            public void SetRepo(string newurl)
            {
                Repository_URL = newurl;
            }
        }


        public enum Times
        {
            Short,
            Long
        }




        public void LIST_CLICK(object sender, RoutedEventArgs e)
        {

            Button btn = ((Button)sender);


        }
        private void Download_Install_Dynamic_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            //  Send_Info_Notif(button.Name);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ask Question?");
        }
        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
           // save_Log();
            HandyControl.Controls.Growl.ClearGlobal();


        }
        protected void MainWindow_Closed(object sender, EventArgs args)
        {
            App.Current.Shutdown();
            

        }
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public static bool CheckForInternetConnection()
        {
            try
            {
                int Desc;
                return InternetGetConnectedState(out Desc, 0);
            }
            catch
            {
                return false;
            }
        }
        public bool PingHost(string nameOrAddress)
        {
            // Server_Indicator.Fill = Brushes.Red;
            //  Indicator_Server_Conn.Visibility = Visibility.Visible;
            bool pingable = false;
            Ping pinger = null;

            try
            {
                if (CheckForInternetConnection() == true)
                {
                    pinger = new Ping();
                    PingReply reply = pinger.Send(nameOrAddress);
                    pingable = reply.Status == System.Net.NetworkInformation.IPStatus.Success;
                    Server_Indicator.Fill = Brushes.LimeGreen;
                    Indicator_Server_Conn.Visibility = Visibility.Hidden;
                    Server_poptip.Visibility = Visibility.Hidden;
                }
                else
                {
                    Server_Indicator.Fill = Brushes.Red;
                    Indicator_Server_Conn.Visibility = Visibility.Visible;
                    Server_poptip.Content = "The Device Is offline!";
                    return false;


                }
            }
            catch (PingException)
            {
                Server_Indicator.Fill = Brushes.Red;
                Indicator_Server_Conn.Visibility = Visibility.Visible;
                Server_poptip.Content = "The Northstar Servers are offline!";

                return false;
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }
            return pingable;
        }
        private static void Delete_empty_Folders(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                Delete_empty_Folders(directory);
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }
        async Task Thunderstore_Parse(bool hard_refresh = true, string Filter_Type = "None", bool Search_ = false, string SearchQuery = "#")
        {

            try
            {



                if (hard_refresh == true)
                {
                    Update = new Updater("https://northstar.thunderstore.io/api/v1/package/");
                    Update.Download_Cutom_JSON();
                    //  LoadListViewData(Filter_Type);


                                  //  MessageBox.Show("FD");

                    // Test_List.ItemsSource = null;

                    // ICollectionView view = CollectionViewSource.GetDefaultView(LoadListViewData(Filter_Type));
                    //  view.GroupDescriptions.Add(new PropertyGroupDescription("Pinned"));
                    // view.SortDescriptions.Add(new SortDescription("Country", ListSortDirection.Ascending));
                    //   Test_List.ItemsSource = view;

                    List<object> List = null;
                    var NON_UI = new Thread(() => { List = LoadListViewData(Filter_Type, Search_, SearchQuery); });
                    NON_UI.IsBackground = true;

                    NON_UI.Start();
                    NON_UI.Join();
                    Test_List.ItemsSource = List;

                    Finished_Init = true;



                    Test_List.Items.Refresh();

                }
                else
                {

                    List<object> List = null;
                    var NON_UI = new Thread(() => { List = LoadListViewData(Filter_Type, Search_, SearchQuery); });
                    NON_UI.IsBackground = true;

                    NON_UI.Start();
                    NON_UI.Join();

                    Test_List.ItemsSource = List;


                    Test_List.Items.Refresh();

                }

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;



            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_ERROR_OCCURRED"));
                Write_To_Log(ErrorManager(ex));
            }

        }

        private void Test_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        public T FindDescendant<T>(DependencyObject obj) where T : DependencyObject
        {
            // Check if this object is the specified type
            if (obj is T)
                return obj as T;

            // Check for children
            int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
            if (childrenCount < 1)
                return null;

            // First check all the children
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is T)
                    return child as T;
            }

            // Then check the childrens children
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = FindDescendant<T>(VisualTreeHelper.GetChild(obj, i));
                if (child != null && child is T)
                    return child as T;
            }

            return null;
        }
        class LIST_JSON
        {
            public string Tag { get; set; }
            public string Name { get; set; }
            public string Icon { get; set; }
            public string owner { get; set; }
            public string description { get; set; }
            public string download_url { get; set; }
            public string Webpage { get; set; }
            public string date_created { get; set; }
            public int Rating { get; set; }
            public string File_Size { get; set; }
            public string Downloads { get; set; }

        }
        public class Button
        {
            public string Tag { get; set; }
            public string Name { get; set; }
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


        }
        void Download_Install(object sender, RoutedEventArgs e)
        {
            try
            {

                Send_Info_Notif(GetTextResource("NOTIF_INFO_DOWNLOAD_START"));
                var objname = ((System.Windows.Controls.Button)sender).Tag.ToString();
                string[] words = objname.Split("|");
                LAST_INSTALLED_MOD = (words[1]);
                if (words[2].Contains("DDS"))
                {

                    parse_git_to_zip(words[0], true);
                }
                else
                {
                    parse_git_to_zip(words[0]);
                }
                if (words[3].Trim() != null && words[3].Trim() != "" && words[3].Count() > 2)
                {
                    System.Windows.MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo { Message = "Required Dependency Called -\n" + words[3] + "\nIs required, would you like to Add and download?", Caption = "INFO", Button = MessageBoxButton.YesNo, IconBrushKey = ResourceToken.AccentBrush, IconKey = ResourceToken.AskGeometry, StyleKey = "MessageBoxCustom" });
                    if (result == System.Windows.MessageBoxResult.Yes)
                    {

                        if (words[3].Contains(","))
                        {

                            string[] Downloads = words[3].Split(",");

                            foreach (var it in Downloads)
                            {
                              //   while(webClient. == true)
                              //  { 
                              //   Console.WriteLine("Download_InProgress_Wait");
                              //  Thread.Sleep(200);
                              //}
                                string[] Word_Arr = it.Split("-");

                                LAST_INSTALLED_MOD = Word_Arr[0] + "-" + Word_Arr[1];
                                parse_git_to_zip(Search_For_Mod_Thunderstore(Word_Arr[0] + "-" + Word_Arr[1]));
                            }
                            Send_Info_Notif("Dependencies Download Completed");


                        }
                        else
                        {
                            if (Completed_Operation == false)
                            {
                                Send_Warning_Notif("Download in progress Detected, added Mod to qeue");


                                string[] Word_Arr = Search_For_Mod_Thunderstore(words[3]).Split("`");
                                LAST_INSTALLED_MOD = Word_Arr[1];


                                parse_git_to_zip(Word_Arr[0]);



                                Send_Info_Notif("Dependencies Download Completed");

                            }
                            else
                            {
                                string[] Word_Arr = Search_For_Mod_Thunderstore(words[3]).Split("`");
                                LAST_INSTALLED_MOD = Word_Arr[1];


                                parse_git_to_zip(Word_Arr[0]);



                                Send_Info_Notif("Dependencies Download Completed");

                            }


                            //}



                        }

                    }
                    else
                    {

                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                Mod_Progress_BAR.Value = 0;
                Mod_Progress_BAR.ShowText = false;
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_ERROR_OCCURRED"));
                Write_To_Log(ErrorManager(ex));
                Completed_Operation = true;

            }
        }
        void Open_Package_Webpage(object sender, RoutedEventArgs e)
        {
            var val = ((System.Windows.Controls.Button)sender).Tag.ToString();

            Send_Info_Notif(GetTextResource("NOTIF_INFO_OPENING") + val);
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = val,
                UseShellExecute = true
            });
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
        public string Search_For_Mod_Thunderstore(string SearchQuery = "None")

        {
            Update = new Updater("https://northstar.thunderstore.io/api/v1/package/");

            Update.Download_Cutom_JSON();

            for (int i = 0; i < Update.Thunderstore.Length; i++)
            {
                List<versions> versions = Update.Thunderstore[i].versions;

                string[] subs = SearchQuery.Split('-');
              

                if (Update.Thunderstore[i].FullName.Contains(subs[1], StringComparison.OrdinalIgnoreCase) || Update.Thunderstore[i].Owner.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                {
                   

                    return versions.First().DownloadUrl + "`" + Update.Thunderstore[i].Name + "-" + versions.First().VersionNumber;

                }


            }
            return null; 
        }
    

        private List<object> LoadListViewData(string Filter_Type = "None", bool Search_ = false, string SearchQuery = "#")
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
                string Exclude_String = "";
                string Dependencies_ = "";
                switch (Filter_Type)
                {
                    case "All":
                        Exclude_String = "#";
                        break;
                    case "Skins":
                        Exclude_String = "Server-side";

                        break;
                    case "Menu Mods":
                        Exclude_String = "Server-side";

                        break;
                    case "Server Mods":
                        Exclude_String = "Client Mods";

                        break;
                    case "DDS-Skins":
                        Exclude_String = "Server-side";
                        Filter_Type = "DDS";
                        break;
                    case "Client Mods":
                        Exclude_String = "Server-side";

                        break;
                    default:
                        Exclude_String = "#";

                        break;
                }
                if (Update.Thunderstore.Length > 0)
                {


                    for (int i = 0; i < Update.Thunderstore.Length; i++)
                    {

                        if (Update.Thunderstore[i].FullName == "northstar-Northstar" || Update.Thunderstore[i].FullName.Contains("r2modman"))
                        {
                            continue;
                        }

                        int rating = Update.Thunderstore[i].RatingScore;

                        Tags = String.Join(" , ", Update.Thunderstore[i].Categories);



                        List<versions> versions = Update.Thunderstore[i].versions;

                        if (Search_ == true)
                        {

                            if (Filter_Type != "All" && Filter_Type != "None")
                            {

                                if (Tags.Contains(Filter_Type) && !Tags.Contains(Exclude_String))
                                {



                                    if (Update.Thunderstore[i].Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) || Update.Thunderstore[i].Owner.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                                    {

                                        foreach (var items in versions)


                                        {


                                            Downloads.Add(Convert.ToInt32(items.Downloads));



                                        }



                                        downloads = (Downloads.Sum()).ToString();
                                        for (var x = 0; x < versions.First().Dependencies.Count; x++)
                                        {
                                            if (versions.First().Dependencies[x].Contains("northstar-Northstar") || versions.First().Dependencies[x].Contains("r2modman"))
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
                                       


                                        if (int.TryParse(FileSize, out int value))
                                        {
                                            FileSize = Convert_To_Size(value);
                                        }

                                        itemsList.Add(new Button { Name = Update.Thunderstore[i].Name + "-" + versions.First().VersionNumber, Icon = ICON, date_created = Update.Thunderstore[i].DateCreated.ToString(), description = Descrtiption, owner = Update.Thunderstore[i].Owner, Rating = rating, download_url = download_url + "|" + Update.Thunderstore[i].Name + "-" + versions.First().VersionNumber + "|" + Tags + "|" + Dependencies_, Webpage = Update.Thunderstore[i].PackageUrl, File_Size = FileSize, Tag = Tags, Downloads = downloads, Dependencies = Dependencies_ });


                                    }
                                }
                            }
                            else
                            {
                                if (Update.Thunderstore[i].Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) || Update.Thunderstore[i].Owner.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                                {

                                    foreach (var items in versions)


                                    {
                                        Downloads.Add(Convert.ToInt32(items.Downloads));
                                      


                                    }



                                    downloads = (Downloads.Sum()).ToString();
                                    for (var x = 0; x < versions.First().Dependencies.Count; x++)
                                    {
                                        if (versions.First().Dependencies[x].Contains("northstar-Northstar") || versions.First().Dependencies[x].Contains("r2modman"))
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


                                    if (int.TryParse(FileSize, out int value))
                                    {
                                        FileSize = Convert_To_Size(value);
                                    }
                                    itemsList.Add(new Button { Name = Update.Thunderstore[i].Name + "-" + versions.First().VersionNumber, Icon = ICON, date_created = Update.Thunderstore[i].DateCreated.ToString(), description = Descrtiption, owner = Update.Thunderstore[i].Owner, Rating = rating, download_url = download_url + "|" + Update.Thunderstore[i].Name + "-" + versions.First().VersionNumber + "|" + Tags + "|" + Dependencies_, Webpage = Update.Thunderstore[i].PackageUrl, File_Size = FileSize, Tag = Tags, Downloads = downloads, Dependencies = Dependencies_ });


                                }


                            }

                        }
                        else if (Filter_Type == "None")
                        {
                            foreach (var items in versions)


                            {

                                Downloads.Add(Convert.ToInt32(items.Downloads));
                            }

                            downloads = (Downloads.Sum()).ToString();
                            for (var x = 0; x < versions.First().Dependencies.Count; x++)
                            {
                                if (versions.First().Dependencies[x].Contains("northstar-Northstar") || versions.First().Dependencies[x].Contains("r2modman"))
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



                            if (int.TryParse(FileSize, out int value))
                            {
                                FileSize = Convert_To_Size(value);
                            }


                            itemsList.Add(new Button { Name = Update.Thunderstore[i].Name + "-" + versions.First().VersionNumber, Icon = ICON, date_created = Update.Thunderstore[i].DateCreated.ToString(), description = Descrtiption, owner = Update.Thunderstore[i].Owner, Rating = rating, download_url = download_url + "|" + Update.Thunderstore[i].Name + "-" + versions.First().VersionNumber + "|" + Tags + "|" + Dependencies_, Webpage = Update.Thunderstore[i].PackageUrl, File_Size = FileSize, Tag = Tags, Downloads = downloads, Dependencies = Dependencies_ });


                        }

                        else if (Tags.Contains(Filter_Type) && !Tags.Contains(Exclude_String))
                        {

                            foreach (var items in versions)


                            {

                                Downloads.Add(Convert.ToInt32(items.Downloads));
                                


                            }


                          
                            downloads = (Downloads.Sum()).ToString();
                        

                           

                            download_url = versions.First().DownloadUrl;
                            for (var x = 0; x < versions.First().Dependencies.Count; x++)
                            {
                                if (versions.First().Dependencies[x].Contains("northstar-Northstar") || versions.First().Dependencies[x].Contains("r2modman"))
                                {

                                    continue;
                                }
                                else
                                {
                                    Dependencies.Add(versions.First().Dependencies[x]);

                                }

                            }
                            ICON = versions.First().Icon;
                            FileSize = versions.First().FileSize.ToString();
                            Descrtiption = versions.First().Description;
                            Dependencies_ = String.Join(", ", Dependencies);
                            Dependencies.Clear();

                            Downloads.Clear();


                            if (int.TryParse(FileSize, out int value))
                            {
                                FileSize = Convert_To_Size(value);
                            }
                          
                            itemsList.Add(new Button { Name = Update.Thunderstore[i].Name + "-" + versions.First().VersionNumber, Icon = ICON, date_created = Update.Thunderstore[i].DateCreated.ToString(), description = Descrtiption, owner = Update.Thunderstore[i].Owner, Rating = rating, download_url = download_url + "|" + Update.Thunderstore[i].Name + "-" + versions.First().VersionNumber + "|" + Tags + "|" + Dependencies_, Webpage = Update.Thunderstore[i].PackageUrl, File_Size = FileSize, Tag = Tags, Downloads = downloads , Dependencies = Dependencies_ });

                        }

                    }
                }

            }
            catch (Exception ex)
            {

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_ERROR_OCCURRED"));
                Write_To_Log(ErrorManager(ex));

                //Console.WriteLine(ex.ToString());
            }
            return itemsList;

        }
        async Task Set_About()
        {
            About_BOX.IsReadOnly = true;
            Paragraph paragraph = new Paragraph();


            string Text = @"-This Application Installs The Northstar Launcher Created by BobTheBob and can install the countless Mods Authored by the many Titanfall2 Modders.
Current Features:
Easily install, update and launch Northstar

Manage installed Northstar mods

Downloading and installing Northstar mods from a GitHub/GitLab repository

Installing downloaded Northstar mods (.zip files)

Easily install custom Weapon/Pilot Skins

Easily start a Northstar Dedicated Server

Thunderstore Mod Browser

Create custom servers using this application to fine tune setups.

Manage dedicated Northstar servers.

Use the Tools To pack Thunderstore Compatible Mod Packages

Install Skins From the Thunderstore Mod Browser


-Please do suggest any new features and/or improvements through the Git issue tracker, or by sending me a personal message.
Thank you again to all you Pilots, Hope we wreak havoc on the Frontier for years to come.
More Instructions at this link: https://github.com/BigSpice/VTOL/blob/master/README.md

Gif image used in Northstar is by @Smurfson.

Big Thanks to - 
@Ralley111
@MysteriousRSA
@emma-miler
@wolf109909
@laundmo
@SamLam140330
@ConnorDoesDev
@ScureX
@rrrfffrrr
@themoonisacheese
@xamionex
Every cent counts towards feeding my baby Ticks - https://www.buymeacoffee.com/Ju1cy ";
            About_BOX.Document.Blocks.Clear();
            Run run = new Run(Text);
            paragraph.Inlines.Add(run);
            About_BOX.Document.Blocks.Add(paragraph);








        }

        private void SideMenuItem_Selected(object sender, RoutedEventArgs e)
        {

        }
        async Task Select_Main()
        {


            Mod_Panel.Visibility = Visibility.Hidden;
            skins_Panel.Visibility = Visibility.Hidden;
            Main_Panel.Visibility = Visibility.Visible;
            About_Panel.Visibility = Visibility.Hidden;
            Mod_Browse_Panel.Visibility = Visibility.Hidden;
            Dedicated_Server_Panel.Visibility = Visibility.Hidden;
            Log_Panel.Visibility = Visibility.Hidden;
            Updates_Panel.Visibility = Visibility.Hidden;
            Theme_Panel.Visibility = Visibility.Hidden;
            Tools_Panel.Visibility = Visibility.Hidden;


            Skins.IsSelected = false;
            Main.IsSelected = true;
            About.IsSelected = false;
            Mods.IsSelected = false;
            Server_Configuration.IsSelected = false;
            Mod_Browse.IsSelected = false;
            Log.IsSelected = false;
            Update_Tab.IsSelected = false;

            Origin_Client_Running = Check_Process_Running("OriginClientService");
            Tools.IsSelected = false;
            Themes.IsSelected = false;

            // PingHost("Northstar.tf");

        }
        void Select_Mods()
        {
            Mod_Panel.Visibility = Visibility.Visible;
            skins_Panel.Visibility = Visibility.Hidden;
            Main_Panel.Visibility = Visibility.Hidden;
            About_Panel.Visibility = Visibility.Hidden;
            Mod_Browse_Panel.Visibility = Visibility.Hidden;
            Dedicated_Server_Panel.Visibility = Visibility.Hidden;
            Log_Panel.Visibility = Visibility.Hidden;
            Updates_Panel.Visibility = Visibility.Hidden;
            Theme_Panel.Visibility = Visibility.Hidden;
            Tools_Panel.Visibility = Visibility.Hidden;


            Skins.IsSelected = false;
            Main.IsSelected = false;
            About.IsSelected = false;
            Mods.IsSelected = true;
            Server_Configuration.IsSelected = false;
            Mod_Browse.IsSelected = false;
            Log.IsSelected = false;
            Update_Tab.IsSelected = false;
            Tools.IsSelected = false;
            Themes.IsSelected = false;

            try
            {
                Call_Mods_From_Folder();

            }
            catch (Exception ex)
            {

                Send_Error_Notif(ex.Message);

                if (ex.Message == "Sequence contains no elements")
                {
                    //   Send_Info_Notif(GetTextResource("NOTIF_INFO_NO_MODS_FOUND"));

                }
            }
        }
        void Select_Skins()
        {
            Mod_Panel.Visibility = Visibility.Hidden;
            skins_Panel.Visibility = Visibility.Visible;
            Main_Panel.Visibility = Visibility.Hidden;
            About_Panel.Visibility = Visibility.Hidden;
            Mod_Browse_Panel.Visibility = Visibility.Hidden;
            Dedicated_Server_Panel.Visibility = Visibility.Hidden;
            Log_Panel.Visibility = Visibility.Hidden;
            Updates_Panel.Visibility = Visibility.Hidden;
            Theme_Panel.Visibility = Visibility.Hidden;
            Tools_Panel.Visibility = Visibility.Hidden;


            Skins.IsSelected = true;
            Main.IsSelected = false;
            About.IsSelected = false;
            Mods.IsSelected = false;
            Server_Configuration.IsSelected = false;
            Mod_Browse.IsSelected = false;
            Log.IsSelected = false;
            Update_Tab.IsSelected = false;
            Tools.IsSelected = false;
            Themes.IsSelected = false;

        }
        void Select_About()
        {

            Mod_Panel.Visibility = Visibility.Hidden;
            skins_Panel.Visibility = Visibility.Hidden;
            Main_Panel.Visibility = Visibility.Hidden;
            About_Panel.Visibility = Visibility.Visible;
            Mod_Browse_Panel.Visibility = Visibility.Hidden;
            Dedicated_Server_Panel.Visibility = Visibility.Hidden;
            Log_Panel.Visibility = Visibility.Hidden;
            Updates_Panel.Visibility = Visibility.Hidden;
            Theme_Panel.Visibility = Visibility.Hidden;
            Tools_Panel.Visibility = Visibility.Hidden;


            Skins.IsSelected = false;
            Main.IsSelected = false;
            About.IsSelected = true;
            Mods.IsSelected = false;
            Server_Configuration.IsSelected = false;
            Mod_Browse.IsSelected = false;
            Log.IsSelected = false;
            Update_Tab.IsSelected = false;
            Tools.IsSelected = false;
            Themes.IsSelected = false;


        }
        async Task Select_Mod_Browse()
        {

            Mod_Panel.Visibility = Visibility.Hidden;
            skins_Panel.Visibility = Visibility.Hidden;
            Main_Panel.Visibility = Visibility.Hidden;
            About_Panel.Visibility = Visibility.Hidden;
            Mod_Browse_Panel.Visibility = Visibility.Visible;
            Dedicated_Server_Panel.Visibility = Visibility.Hidden;
            Log_Panel.Visibility = Visibility.Hidden;
            Updates_Panel.Visibility = Visibility.Hidden;
            Theme_Panel.Visibility = Visibility.Hidden;
            Tools_Panel.Visibility = Visibility.Hidden;

            Skins.IsSelected = false;
            Main.IsSelected = false;
            About.IsSelected = false;
            Mods.IsSelected = false;
            Server_Configuration.IsSelected = false;
            Mod_Browse.IsSelected = true;
            Update_Tab.IsSelected = false;
            Log.IsSelected = false;



            Tools.IsSelected = false;
            Themes.IsSelected = false;
            call_TS_MODS();


        }
        void Select_Server()
        {
            Mod_Panel.Visibility = Visibility.Hidden;
            skins_Panel.Visibility = Visibility.Hidden;
            Main_Panel.Visibility = Visibility.Hidden;
            About_Panel.Visibility = Visibility.Hidden;
            Mod_Browse_Panel.Visibility = Visibility.Hidden;
            Dedicated_Server_Panel.Visibility = Visibility.Visible;
            Log_Panel.Visibility = Visibility.Hidden;
            Updates_Panel.Visibility = Visibility.Hidden;
            Theme_Panel.Visibility = Visibility.Hidden;
            Tools_Panel.Visibility = Visibility.Hidden;

            Skins.IsSelected = false;
            Main.IsSelected = false;
            About.IsSelected = false;
            Mods.IsSelected = false;
            Server_Configuration.IsSelected = true;
            Mod_Browse.IsSelected = false;
            Update_Tab.IsSelected = false;
            Log.IsSelected = false;
            Tools.IsSelected = false;
            Themes.IsSelected = false;


        }
        void Select_Log()
        {
            Mod_Panel.Visibility = Visibility.Hidden;
            skins_Panel.Visibility = Visibility.Hidden;
            Main_Panel.Visibility = Visibility.Hidden;
            About_Panel.Visibility = Visibility.Hidden;
            Mod_Browse_Panel.Visibility = Visibility.Hidden;
            Dedicated_Server_Panel.Visibility = Visibility.Hidden;
            Log_Panel.Visibility = Visibility.Visible;
            Updates_Panel.Visibility = Visibility.Hidden;
            Theme_Panel.Visibility = Visibility.Hidden;
            Tools_Panel.Visibility = Visibility.Hidden;

            Skins.IsSelected = false;
            Main.IsSelected = false;
            About.IsSelected = false;
            Mods.IsSelected = false;
            Server_Configuration.IsSelected = false;
            Mod_Browse.IsSelected = false;
            Log.IsSelected = true;
            Update_Tab.IsSelected = false;
            Log_Indicator.Background = Brushes.Transparent;
            LOG_BOX.ScrollToEnd();
            Tools.IsSelected = false;
            Themes.IsSelected = false;


        }
        void Select_Update()
        {

            AuthorBox.Text = Author_Used;
            RepoBox.Text = Repo_Used;
            Master_ServerBox.Text = MasterServer_URL;
            Repo_URl.Text = Current_REPO_URL;

            Mod_Panel.Visibility = Visibility.Hidden;
            skins_Panel.Visibility = Visibility.Hidden;
            Main_Panel.Visibility = Visibility.Hidden;
            About_Panel.Visibility = Visibility.Hidden;
            Mod_Browse_Panel.Visibility = Visibility.Hidden;
            Dedicated_Server_Panel.Visibility = Visibility.Hidden;
            Log_Panel.Visibility = Visibility.Hidden;
            Updates_Panel.Visibility = Visibility.Visible;
            Theme_Panel.Visibility = Visibility.Hidden;
            Tools_Panel.Visibility = Visibility.Hidden;

            Skins.IsSelected = false;
            Main.IsSelected = false;
            About.IsSelected = false;
            Mods.IsSelected = false;
            Server_Configuration.IsSelected = false;
            Mod_Browse.IsSelected = false;
            Log.IsSelected = false;
            Update_Tab.IsSelected = true;
            Tools.IsSelected = false;
            Themes.IsSelected = false;

        }
        void Select_Themes()
        {
            ;
            Mod_Panel.Visibility = Visibility.Hidden;
            skins_Panel.Visibility = Visibility.Hidden;
            Main_Panel.Visibility = Visibility.Hidden;
            About_Panel.Visibility = Visibility.Hidden;
            Mod_Browse_Panel.Visibility = Visibility.Hidden;
            Dedicated_Server_Panel.Visibility = Visibility.Hidden;
            Log_Panel.Visibility = Visibility.Hidden;
            Updates_Panel.Visibility = Visibility.Hidden;
            Theme_Panel.Visibility = Visibility.Visible;
            Tools_Panel.Visibility = Visibility.Hidden;

            Skins.IsSelected = false;
            Main.IsSelected = false;
            About.IsSelected = false;
            Mods.IsSelected = false;
            Server_Configuration.IsSelected = false;
            Mod_Browse.IsSelected = false;
            Log.IsSelected = false;
            Update_Tab.IsSelected = false;
            Themes.IsSelected = true;
            Tools.IsSelected = false;

        }
        void Select_Tools()
        {

            Mod_Panel.Visibility = Visibility.Hidden;
            skins_Panel.Visibility = Visibility.Hidden;
            Main_Panel.Visibility = Visibility.Hidden;
            About_Panel.Visibility = Visibility.Hidden;
            Mod_Browse_Panel.Visibility = Visibility.Hidden;
            Dedicated_Server_Panel.Visibility = Visibility.Hidden;
            Log_Panel.Visibility = Visibility.Hidden;
            Updates_Panel.Visibility = Visibility.Hidden;
            Theme_Panel.Visibility = Visibility.Hidden;
            Tools_Panel.Visibility = Visibility.Visible;

            Skins.IsSelected = false;
            Main.IsSelected = false;
            About.IsSelected = false;
            Mods.IsSelected = false;
            Server_Configuration.IsSelected = false;
            Mod_Browse.IsSelected = false;
            Log.IsSelected = false;
            Update_Tab.IsSelected = false;
            Themes.IsSelected = false;
            Tools.IsSelected = true;

            Paragraph paragraph = new Paragraph();
            Description_Box.Document.Blocks.Clear();
            Run run = new Run(Thunderstore_Template_Text);
            paragraph.Inlines.Add(run);
            Description_Box.Document.Blocks.Add(paragraph);
        }
        private void SideMenu_SelectionChanged(object sender, HandyControl.Data.FunctionEventArgs<object> e)
        {

            if (Main.IsSelected == true)
            {

                Select_Main();
            }
            if (Mods.IsSelected == true)
            {

                Select_Mods();

            }
            if (Skins.IsSelected == true)
            {

                Select_Skins();

            }
            if (About.IsSelected == true)
            {
                Select_About();

            }
            if (Mod_Browse.IsSelected == true)
            {

                Select_Mod_Browse();

            }
            if (Server_Configuration.IsSelected == true)
            {
                Select_Server();

            }
            if (Log.IsSelected == true)
            {
                Select_Log();

            }
            if (Update_Tab.IsSelected == true)
            {
                Select_Update();

            }

            if (Themes.IsSelected == true)
            {
                Select_Themes();

            }
            if (Tools.IsSelected == true)
            {
                Select_Tools();

            }
        }


        private void Mods_Selected(object sender, RoutedEventArgs e)
        {
        }
        void Send_Success_Notif(string Input_Message)
        {

            HandyControl.Controls.Growl.SuccessGlobal(Input_Message);
            Write_To_Log("\n" + Input_Message + '\n' + DateTime.Now.ToString());
        }

        void Send_Error_Notif(string Input_Message, [CallerMemberName] string memberName = "")
        {
            HandyControl.Controls.Growl.ErrorGlobal(Input_Message);
            Write_To_Log("\n" + memberName + '\n' + DateTime.Now.ToString());

        }

        void Send_Info_Notif(string Input_Message)
        {
            HandyControl.Controls.Growl.InfoGlobal(Input_Message);
            Write_To_Log("\n" + Input_Message + '\n' + DateTime.Now.ToString());


        }
        void Send_Warning_Notif(string Input_Message)
        {
            HandyControl.Controls.Growl.WarningGlobal(Input_Message);
            Write_To_Log("\n" + Input_Message + '\n' + DateTime.Now.ToString());

        }
        void Send_Fatal_Notif(string Input_Message)
        {


            HandyControl.Controls.Growl.FatalGlobal(Input_Message);
            Write_To_Log("\n" + Input_Message + '\n' + DateTime.Now.ToString());
        }


        private string Get_And_Set_Filepaths(string rootDir, string Filename)
        {
            try
            {
                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(@rootDir);
                FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + Filename + "*.*");
                // //Console.WriteLine(rootDir);
                // //Console.WriteLine(Filename);

                foreach (FileInfo foundFile in filesInDir)
                {
                    if (foundFile.Name.Equals(Filename))
                    {
                        ////Console.WriteLine("Found");

                        string fullName = foundFile.FullName;
                        //////Console.WriteLine(fullName);
                        return fullName;


                    }
                    else
                    {

                        return "\nCould Not Find" + Filename + "\n";

                    }

                }
            }

            catch (Exception e)
            {
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

                Write_To_Log("The process failed: " + e.ToString());
                Write_To_Log(ErrorManager(e));

            }


            return "Exited with No Due to Missing Or Inaccurate Path";


        }
        private void FindNSInstall(string Search, string FolderDir)
        {
            System.IO.DirectoryInfo rootDirs = new DirectoryInfo(@FolderDir);


            if (Directory.Exists(FolderDir))
            {
                if (!IsDirectoryEmpty(rootDirs))
                {
                    WalkDirectoryTree(rootDirs, Search);

                    ////Console.WriteLine("Files with restricted access:");
                    
                }
                else
                {

                    Send_Error_Notif(GetTextResource("NOTIF_ERROR_DIRECTORY_EMPTY"));
                    return;

                }


            }

            else

            {

                Send_Error_Notif(GetTextResource("NOTIF_ERROR_INVALID_PATH_FED"));
                failed_search_counter++;
                
            }



        }
        public static bool IsDirectoryEmpty(DirectoryInfo directory)
        {
            FileInfo[] files = directory.GetFiles();
            DirectoryInfo[] subdirs = directory.GetDirectories();

            return (files.Length == 0 && subdirs.Length == 0);
        }
        private static bool ListCheck<T>(IEnumerable<T> l1, IEnumerable<T> l2)
        {
            // TODO: Null parm checks 
            if (l1.Intersect(l2).Any())
            {
                ////Console.WriteLine("matched");
                return true;
            }
            else
            {
                ////Console.WriteLine("not matched");
                return false;
            }
        }
       
        private async Task Check_Integrity_Of_NSINSTALL()
        {


            if (File.Exists(NSExe))
            {
                System.IO.DirectoryInfo[] FolderDir = null;
                System.IO.DirectoryInfo rootDirs = new DirectoryInfo(Current_Install_Folder);
                FolderDir = rootDirs.GetDirectories();
                // List<string> Baseline = Read_From_Text_File(@"C:\ProgramData\NorthstarModManager\NormalFolderStructure.txt");
                List<string> Baseline = new List<string>()
                {
                    @"Titanfall2\bin",
                    @"Titanfall2\Core",
@"Titanfall2\platform",
@"Titanfall2\r2",
@"Titanfall2\R2Northstar",
@"Titanfall2\ShaderCache",
@"Titanfall2\Support",
@"Titanfall2\vpk",
@"Titanfall2\__Installer"

                };
                List<string> current = new List<string>();
                ////Console.WriteLine("Baseline");

                foreach (var Folder in FolderDir)
                {
                    string s = Folder.ToString().Substring(Folder.ToString().LastIndexOf("Titanfall2"));

                    current.Add(s);

                    //saveAsyncFile(s, @"C:\temp\NormalFolderStructure");

                }
                ////Console.WriteLine("current");

                foreach (var Folder in current)
                {

                    ////Console.WriteLine(Folder.ToString());

                }
                ////Console.WriteLine(Baseline.SequenceEqual(current));

                if (ListCheck(Baseline, current) == true)
                {
                    NS_Installed = true;


                }
                else
                {
                    Send_Error_Notif(GetTextResource("NOTIF_ERROR_DIRECTORY_CHECK_FAILED"));
                    NS_Installed = false;


                }




            }
            else
            {

                NS_Installed = false;
            }

            if (NS_Installed == false)
            {

                Send_Error_Notif(GetTextResource("NOTIF_ERROR_NS_BAD_INTEGRITY"));

                Install_NS_EXE_Textbox.Foreground = Brushes.Red;

            }
            else
            {

                Install_NS_EXE_Textbox.Foreground = Brushes.Green;
                Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_INTEGRITY_VERIFIED"));


            }
            Install_NS_EXE_Textbox.Text = NSExe;




        }
        public string Read_From_TextFile_OneLine(string Filepath)
        {
            string line = "";
            try
            {
                using (var sr = new StreamReader(Filepath))
                {
                    line = sr.ReadLine();
                    return line;
                }

            }
            catch (System.IO.FileNotFoundException e)
            {
                Send_Error_Notif(GetTextResource("NOTIF_ERROR_CANNOT_FIND") + Filepath);
                Write_To_Log(ErrorManager(e));


            }

            return line;

        }
        public List<string> Read_From_Text_File(string Filepath)
        {
            List<string> lines = new List<string>();

            try
            {
                using (var sr = new StreamReader(Filepath))
                {
                    while (sr.Peek() >= 0)
                        lines.Add(sr.ReadLine());
                }
                foreach (string line in lines)
                {
                    // Use a tab to indent each line of the file.
                    ////Console.WriteLine("\t" + line);
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                Send_Error_Notif(GetTextResource("NOTIF_ERROR_CANNOT_FIND") + Filepath);
                Write_To_Log(ErrorManager(e));


            }
            return lines;
        }
        public async Task saveAsyncFile(string Text, string Filename, bool ForceTxt = true, bool append = true)
        {
            if (ForceTxt == true)
            {
                if (!Filename.Contains(".txt"))
                {
                    Filename = Filename + ".txt";

                }
            }
            if (append == true)
            {
                if (File.Exists(Filename))
                {

                    using StreamWriter file = new(Filename, append: true);
                    await file.WriteLineAsync(Text);
                    file.Close();
                }
                else
                {

                    await File.WriteAllTextAsync(Filename, string.Empty);

                    await File.WriteAllTextAsync(Filename, Text);

                }
            }
            else
            {
                await File.WriteAllTextAsync(Filename, string.Empty);

                await File.WriteAllTextAsync(Filename, Text);

            }
        }
        private async Task Read_Latest_Release(string address, string json_name = "temp.json", bool Parse = true, bool Log_Msgs = true)
        {
            if (address != null)
            {
                if (Log_Msgs == true)
                {
                    // Send_Info_Notif("\nJson Download Started!");

                }
                WebClient client = new WebClient();
                Uri uri1 = new Uri(address);
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                Stream data = client.OpenRead(address);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();



                s = s.Replace("[", "");
                s = s.Replace("]", ""); 
                if (Directory.Exists(DocumentsFolder + @"\VTOL_DATA\temp"))
                {
                    saveAsyncFile(s, DocumentsFolder + @"\VTOL_DATA\temp\" + json_name, false, false);
                    if (Log_Msgs == true)
                    {
                        //  Send_Info_Notif("\nJson Download completed!");
                        //  Send_Info_Notif("\nParsing Latest Release........");
                    }
                    if (Parse == true)
                    {
                        Thread.Sleep(100);
                        Parse_Release();
                    }

                }
                else
                {
                    Directory.CreateDirectory(DocumentsFolder + @"\VTOL_DATA\temp\");
                    saveAsyncFile(s, DocumentsFolder + @"\VTOL_DATA\temp\" + json_name, false, false);
                    if (Log_Msgs == true)
                    {
                        // Send_Info_Notif("\nJson Download completed!");
                        //    Send_Info_Notif("\nParsing Latest Release........");
                    }
                    if (Parse == true)
                    {
                        Parse_Release();
                    }
                }

            }
            else
            {


                Send_Error_Notif(GetTextResource("NOTIF_ERROR_INVALID_URL"));
            }

        }
        private string Parse_Custom_Release(string json_name = "temp.json")
        {
            try
            {
                if (File.Exists(DocumentsFolder + @"\VTOL_DATA\temp\" + json_name))
                {
                    var myJsonString = File.ReadAllText(DocumentsFolder + @"\VTOL_DATA\temp\" + json_name);
                    var myJObject = JObject.Parse(myJsonString);

                    string out_ = myJObject.SelectToken("assets.browser_download_url").Value<string>();
                    Properties.Settings.Default.Version = myJObject.SelectToken("tag_name").Value<string>();
                    Properties.Settings.Default.Save();
                    User_Settings_Vars.CurrentVersion = Properties.Settings.Default.Version;
                    string User_Settings_Json_Strings = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
                    using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
                    {
                        StreamWriter.WriteLine(User_Settings_Json_Strings);
                        StreamWriter.Close();
                    }
                    Send_Info_Notif(GetTextResource("NOTIF_INFO_RELEASE_PARSED") + out_);
                    if (Directory.Exists(DocumentsFolder + @"\VTOL_DATA\temp\" + json_name))
                    {
                        if (File.Exists(DocumentsFolder + @"\VTOL_DATA\temp\" + json_name))
                        {
                         File.Delete(DocumentsFolder + @"\VTOL_DATA\temp\" + json_name);
                        }
                    }
                    return out_;

                }
                else
                {
                    Send_Error_Notif(GetTextResource("NOTIF_ERROR_RELEASE_NOT_FOUND"));

                    return null;
                }
            }
            catch (Exception e)
            {
                Write_To_Log(ErrorManager(e));
                return null;

            }

        }
        private async void Parse_Release(string json_name = "temp.json")
        {
            try
            {
                if (File.Exists(DocumentsFolder + @"\VTOL_DATA\temp\" + json_name))
                {

                    var myJsonString = File.ReadAllText(DocumentsFolder + @"\VTOL_DATA\temp\" + json_name);
                    var myJObject = JObject.Parse(myJsonString);


                    current_Northstar_version_Url = myJObject.SelectToken("assets.browser_download_url").Value<string>();
                    Properties.Settings.Default.Version = myJObject.SelectToken("tag_name").Value<string>();
                    Properties.Settings.Default.Save();
                    User_Settings_Vars.CurrentVersion = Properties.Settings.Default.Version;
                    string User_Settings_Json_Strings = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
                    using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
                    {
                        StreamWriter.WriteLine(User_Settings_Json_Strings);
                        StreamWriter.Close();
                    }
                    Send_Info_Notif(GetTextResource("NOTIF_INFO_RELEASE_PARSED") + current_Northstar_version_Url);

                }
                else
                {
                    Send_Error_Notif(GetTextResource("NOTIF_ERROR_RELEASE_NOT_FOUND"));


                }

                if (Directory.Exists(DocumentsFolder + @"\VTOL_DATA\temp\" + json_name))
                {
                    if (File.Exists(DocumentsFolder + @"\VTOL_DATA\temp\" + json_name))
                    {
                       File.Delete(DocumentsFolder + @"\VTOL_DATA\temp\" + json_name);
                    }
                }
            }
            catch (Exception e)
            {
                Write_To_Log(ErrorManager(e));
                return;

            }
        }
        private void DirectoryCopy(
              string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();
            copySubDirs = true;
            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                Write_To_Log("\nDirectory - " + dir.FullName + "Does not Exist. Logging For possible Errors.\n");

            }
            else
            {

                // If the destination directory does not exist, create it.
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }


                // Get the file contents of the directory to copy.
                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo file in files)
                {
                    // Create the path to the new copy of the file.
                    string temppath = Path.Combine(destDirName, file.Name);

                    // Copy the file.
                    file.CopyTo(temppath, true);
                }

                // If copySubDirs is true, copy the subdirectories.
                if (copySubDirs)
                {

                    foreach (DirectoryInfo subdir in dirs)
                    {
                        // Create the subdirectory.
                        string temppath = Path.Combine(destDirName, subdir.Name);

                        // Copy the subdirectories.
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                    }
                }
            }
        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                if (!File.Exists(targetPath))
                {
                    File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);

                }
            }
        }

        private void zipProgress(object sender, Zipi.ExtractProgressEventArgs e)
        {
            if (e.TotalBytesToTransfer > 0)
            {
                if (e.EventType == Zipi.ZipProgressEventType.Extracting_EntryBytesWritten)
                {
                    Progress_Bar_Window.Value = (int)((e.BytesTransferred * 100) / e.TotalBytesToTransfer);
                }
            }
            if (e.EventType == Zipi.ZipProgressEventType.Extracting_AfterExtractAll)
            {
                Current_File_Label.Content = "";

            }
            //  if (e.EventType == Zipi.ZipProgressEventType.Saving_EntryBytesRead)
            // this.progressbar1.Value = (int)((e.BytesTransferred * 100) / e.TotalBytesToTransfer);

            //   else if (e.EventType == ZipProgressEventType.Saving_Completed)
            // this.progressbar1.Value = 100;
        }


        private void Unpack_To_Location_Custom(string Target_Zip, string Destination, bool Clean_Thunderstore = false, bool clean_normal = false, bool Skin_Install = false)
        {
            //ToDo Check if url or zip location
            //add drag and drop

            try
            {
                //    Loading_Panel.Visibility = Visibility.Visible;
                
                string Dir_Final = null;
                if (File.Exists(Target_Zip))
                {
                    if (!Directory.Exists(Destination))
                    {
                        Directory.CreateDirectory(Destination);
                    }
                    if (Directory.Exists(Destination))
                    {


                        string fileExt = System.IO.Path.GetExtension(Target_Zip);
                        ////Console.WriteLine("It only works if i have this line :(");

                        if (fileExt == ".zip")
                        {
                            //   Current_File_Label.Content = Target_Zip;

                            //     using (Zipi.ZipFile zip = Zipi.ZipFile.Read(Target_Zip))
                            //    {
                            // initial setup before extraction
                            //        zip.ExtractProgress += zipProgress;
                            // actual extraction process
                            //      zip.ExtractAll(Destination, Zipi.ExtractExistingFileAction.OverwriteSilently);
                            // since the boolean below is in the same "thread" the extraction must 
                            // complete for the boolean to be set to true
                            //   }
                            //  Loading_Panel.Visibility = Visibility.Hidden;
                            if(IsDirectoryEmpty(new DirectoryInfo(Destination)) == false)
                            {
                                Clear_Folder(Destination);
                            }
                            ZipFile.ExtractToDirectory(Target_Zip, Destination, true);
                            //dialog.Close();
                            Send_Success_Notif("\nUnpacking Complete!\n");

                            if (Clean_Thunderstore == true)
                            {

                               

                                    // Check if file exists with its full path    
                                    if (File.Exists(Path.Combine(Destination, "icon.png")))
                                    {
                                        // If file found, delete it    
                                        File.Delete(Path.Combine(Destination, "icon.png"));
                                    }
                                    else { Send_Warning_Notif(GetTextResource("NOTIF_WARN_CLEANUP_FILES_NOT_FOUND")); }
                                    if (File.Exists(Path.Combine(Destination, "manifest.json")))
                                    {
                                        // If file found, delete it    
                                        File.Delete(Path.Combine(Destination, "manifest.json"));
                                    }
                                    else { Send_Warning_Notif(GetTextResource("NOTIF_WARN_CLEANUP_FILES_NOT_FOUND")); }

                                    if (File.Exists(Path.Combine(Destination, "README.md")))
                                    {
                                        // If file found, delete it    
                                        File.Delete(Path.Combine(Destination, "README.md"));
                                    }
                                    else { Send_Warning_Notif(GetTextResource("NOTIF_WARN_CLEANUP_FILES_NOT_FOUND")); }






                                    if (Skin_Install == false)
                                    {
                                        string searchQuery3 = "*" + "mod.json" + "*";

                                        //  string[] list = Directory.GetFiles(Destination, "*mod.json*",
                                        //      SearchOption.AllDirectories);
                                        var Destinfo = new DirectoryInfo(Destination);


                                        var Script = Destinfo.GetFiles(searchQuery3, SearchOption.AllDirectories);
                                        Destinfo.Attributes &= ~FileAttributes.ReadOnly;
                                        //foreach (var file in Script)
                                        // {
                                        //       MessageBox.Show(file.FullName);
                                        // }


                                        if (Script.Length != 0)
                                        {
                                        var File_ = Script.FirstOrDefault();

                                        if (Script.Length > 1)
                                        {
                                           
                                            if (Directory.Exists(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder"))
                                            {
                                                Directory.Delete(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder", true);

                                            }
                                            if (Directory.Exists(Current_Install_Folder + @"\NS_Downloaded_Mods"))
                                            {
                                                Directory.Delete(Current_Install_Folder + @"\NS_Downloaded_Mods", true);
                                            }
                                            Send_Error_Notif("This Mod is a Multipack, Please Manually Install this Mod!");
                                            Send_Info_Notif("You can Find the Mod in your mod Folder for Configuration.");
                                            return;
                                            
                                        }

                                        FileInfo FolderTemp = new FileInfo(File_.FullName);
                                        DirectoryInfo di = new DirectoryInfo(Directory.GetParent(File_.FullName).ToString());
                                        string firstFolder = di.FullName;

                                        if (Directory.Exists(Destination))
                                        {
                                            if (Destination.Contains("materials"))
                                            {
                                                // Send_Error_Notif(GetTextResource("NOTIF_ERROR_MOD_INCOMPATIBLE"));
                                                // Send_Warning_Notif(GetTextResource("NOTIF_WARN_SUGGEST_DISABLE_MOD"));
                                                Send_Error_Notif("This Mod Requires More Steps By a User To install! \n Please Manually Install this Mod By Clicking Go to Webpge.");
                                                if (Directory.Exists(Current_Install_Folder + @"\NS_Downloaded_Mods"))
                                                {
                                                    Directory.Delete(Current_Install_Folder + @"\NS_Downloaded_Mods", true);
                                                }
                                                return;
                                            }
                                            else
                                            {



                                                Directory.CreateDirectory(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder");
                                                if (Directory.Exists(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder"))
                                                {
                                                    CopyFilesRecursively(firstFolder, Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder");




                                                    Clear_Folder(Destination);
                                                    CopyFilesRecursively(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder", Destination);
                                                    Directory.Delete(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder", true);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                       
                                            if (Directory.Exists(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder"))
                                            {
                                                Directory.Delete(Destinfo.Parent.FullName + @"\" + "Temp_Working_Folder", true);

                                            }
                                            if (Directory.Exists(Current_Install_Folder + @"\NS_Downloaded_Mods"))
                                            {
                                                Directory.Delete(Current_Install_Folder + @"\NS_Downloaded_Mods", true);
                                            }
                                            Directory.Delete(Destination, true);
                                        Completed_Operation = true;
                                            Send_Error_Notif("This Mod Requires More Steps By a User To install! \n Please Manually Install this Mod By Clicking Go to Webpge.");
                                            return;

                                       


                                    }


                                    }
                                    else
                                    {
                                        var ext = new List<string> { "zip" };
                                       var myFiles = Directory.EnumerateFiles(Destination + @"\", "*.*", SearchOption.AllDirectories).Where(s => ext.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));
                                        foreach (string x in myFiles)
                                        {


                                            Skin_Install_Tree(true, x);
                                            Thread.Sleep(100);
                                            Install_Skin();
                                            Thread.Sleep(100);

                                    }
                                }


                                
                                    if (Destination == null)
                                    {
                                        if (Skin_Install == false)
                                        {
                                            Send_Error_Notif(GetTextResource("NOTIF_ERROR_MOD_INCOMPATIBLE"));
                                            Send_Warning_Notif(GetTextResource("NOTIF_WARN_SUGGEST_DISABLE_MOD"));
                                        }
                                        return;
                                    }
                                    else
                                    {
                                        if (Skin_Install == false)
                                        {
                                            Send_Info_Notif(GetTextResource("NOTIF_INFO_GROUP_UNPACK_UNPACKED") + "  " + Path.GetFileName(Target_Zip) + "  " + GetTextResource("NOTIF_INFO_GROUP_UNPACK_TO") + "  " + Destination);
                                            Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_INSTALLED_DASH") + LAST_INSTALLED_MOD);
                                            Completed_Operation = true;

                                        }
                                    }
                                    if (Directory.Exists(Current_Install_Folder + @"\NS_Downloaded_Mods"))
                                    {
                                        Directory.Delete(Current_Install_Folder + @"\NS_Downloaded_Mods", true);
                                    }

                                Completed_Operation = true;


                            }
                            else
                            {

                                string fileExts = System.IO.Path.GetExtension(Target_Zip);

                                if (fileExts == ".zip")
                                {
                                    if (clean_normal == true)
                                    {
                                        if (!Directory.Exists(Destination))
                                        {
                                            Directory.CreateDirectory(Destination);
                                        }
                                        //TODO
                                        //    string folderName = Destination;

                                        //    var directory = new DirectoryInfo(folderName);
                                        //   var Destinfo = new DirectoryInfo(Destination);
                                        ZipFile.ExtractToDirectory(Target_Zip, Destination, true);
                                        Send_Success_Notif("\nUnpacking Complete!\n");

                                    }
                                    else
                                    {
                                        ZipFile.ExtractToDirectory(Target_Zip, Destination, true);
                                        Send_Success_Notif("\nUnpacking Complete!\n");
                                        Completed_Operation = true;

                                    }
                                }
                                else
                                {
                                    //Main_Window.SelectedTab = Main;
                                    Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_OBJ_NOT_ZIP"));
                                    Completed_Operation = true;


                                }
                            }
                        }
                        else
                        {
                            //Main_Window.SelectedTab = Main;

                            Send_Warning_Notif(GetTextResource("NOTIF_ERROR_OBJ_NOT_ZIP"));

                            Completed_Operation = true;

                        }



                    }

                    else
                    {

                        if (!File.Exists(Target_Zip))
                        {
                            Send_Error_Notif(GetTextResource("NOTIF_ERROR_ZIP_NOT_EXIST"));
                            Completed_Operation = true;


                        }
                        if (!Directory.Exists(Destination))
                        {
                            Completed_Operation = true;

                            Send_Error_Notif(GetTextResource("NOTIF_ERROR_ZIP_NOT_EXIST_CHECK_PATH"));

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Completed_Operation = true;

                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));
               

            }
        }
        private void Clear_Folder(string FolderName)
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
        private void Unpack_To_Location(string Target_Zip, string Destination_Zip)
        {
            if (Directory.Exists(Current_Install_Folder + @"\R2Northstar\mods\Northstar.Client\Locked_Folder"))
            {
                Directory.Delete(Current_Install_Folder + @"\R2Northstar\mods\Northstar.Client\Locked_Folder", true);

            }
            if (Directory.Exists(Current_Install_Folder + @"\R2Northstar\mods\Northstar.Custom\Locked_Folder"))
            {
                Directory.Delete(Current_Install_Folder + @"\R2Northstar\mods\Northstar.Custom\Locked_Folder", true);


            }
            if (Directory.Exists(Current_Install_Folder + @"\R2Northstar\mods\Northstar.CustomServers\Locked_Folder"))
            {
                Directory.Delete(Current_Install_Folder + @"\R2Northstar\mods\Northstar.CustomServers\Locked_Folder", true);


            }



            Send_Info_Notif(GetTextResource("NOTIF_INFO_GROUP_UNPACK_UNPACKING") + Path.GetFileName(Target_Zip) + GetTextResource("NOTIF_INFO_GROUP_UNPACK_TO") + Destination_Zip);
            if (File.Exists(Target_Zip) && Directory.Exists(Destination_Zip))
            {
                string fileExt = System.IO.Path.GetExtension(Target_Zip);

                if (fileExt == ".zip")
                {
                    ZipFile.ExtractToDirectory(Target_Zip, Destination_Zip, true);
                    Send_Info_Notif(GetTextResource("NOTIF_INFO_UNPACK_COMPLETE"));
                    if (File.Exists(Current_Install_Folder + @"\ns_startup_args_dedi.txt") && File.Exists(Current_Install_Folder + @"\ns_startup_args.txt"))
                    {
                        if (do_not_overwrite_Ns_file == true)
                        {
                            Send_Info_Notif(GetTextResource("NOTIF_INFO_RESTORING_FILES"));
                            if (Directory.Exists(Current_Install_Folder + @"\TempCopyFolder\"))
                            {
                                if (Directory.Exists(Current_Install_Folder + @"\TempCopyFolder\ns_startup_args.txt"))
                                {
                                    System.IO.File.Copy(Current_Install_Folder + @"\TempCopyFolder\ns_startup_args.txt", Current_Install_Folder + @"\ns_startup_args.txt", true);
                                }
                                Send_Info_Notif(GetTextResource("NOTIF_INFO_CLEANING_RESIDUAL"));

                            }


                        }
                        if (do_not_overwrite_Ns_file_Dedi == true)
                        {
                            Send_Info_Notif(GetTextResource("NOTIF_INFO_RESTORING_FILES"));
                            if (Directory.Exists(Current_Install_Folder + @"\TempCopyFolder\"))
                            {
                                if (Directory.Exists(Current_Install_Folder + @"\TempCopyFolder\autoexec_ns_server.cfg"))
                                {
                                    System.IO.File.Copy(Current_Install_Folder + @"\TempCopyFolder\autoexec_ns_server.cfg", Current_Install_Folder + @"\R2Northstar\mods\Northstar.CustomServers\mod\cfg\autoexec_ns_server.cfg", true);
                                }
                                if (Directory.Exists(Current_Install_Folder + @"\TempCopyFolder\ns_startup_args_dedi.txt"))
                                {
                                    System.IO.File.Copy(Current_Install_Folder + @"\TempCopyFolder\ns_startup_args_dedi.txt", Current_Install_Folder + @"\ns_startup_args_dedi.txt", true);
                                }
                                Send_Info_Notif(GetTextResource("NOTIF_INFO_CLEANING_RESIDUAL"));

                            }


                        }
                        if (Directory.Exists(Current_Install_Folder + @"\TempCopyFolder"))
                        {
                            Directory.Delete(Current_Install_Folder + @"\TempCopyFolder", true);
                        }
                        Send_Info_Notif(GetTextResource("NOTIF_INFO_INSTALL_COMPLETE"));

                    }

                }
                else
                {
                    if (!File.Exists(Target_Zip))
                    {
                        Send_Error_Notif(GetTextResource("NOTIF_ERROR_ZIP_NOT_EXIST"));


                    }
                    if (!Directory.Exists(Destination_Zip))
                    {
                        Send_Error_Notif(GetTextResource("NOTIF_ERROR_ZIP_NOT_EXIST_CHECK_PATH"));

                    }
                }
            }
            else
            {

                // Main_Window.SelectedTab = Main;
                Send_Error_Notif(GetTextResource("NOTIF_ERROR_OBJ_NOT_ZIP"));

            }
        }
        private bool Template_traverse(System.IO.DirectoryInfo root, String Search)
        {

            string outt = "";
            try
            {
                System.IO.DirectoryInfo[] subDirs = null;
                subDirs = root.GetDirectories();
                var last = subDirs.Last();
                //Log_Box.AppendText(last.FullName + "sdsdsdsd");
                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    outt = dirInfo.FullName;
                    if (dirInfo.Name.Contains(Search))
                    {
                        // ////Console.WriteLine("Found Folder");
                        ////Console.WriteLine(dirInfo.FullName);
                        return true;

                    }
                    else if (last.Equals(dirInfo))
                    {
                        return false;
                    }
                    else
                    {

                        ////Console.WriteLine("Trying again at " + dirInfo);

                    }
                    if (dirInfo == null)
                    {
                        ////Console.WriteLine(dirInfo.FullName + "This is not a valid Folder????!");
                        continue;

                    }
                    // Resursive call for each subdirectory.
                }

                ////Console.WriteLine("\nCould not Find the Install at " + root + " - Continuing Traversal");

            }
            catch (Exception e)
            {

                if (e.Message == "Sequence contains no elements")
                {
                    System.IO.DirectoryInfo Dir = new DirectoryInfo(outt);

                    ////Console.WriteLine("Empty Folder at - "+ outt);
                    if (IsDirectoryEmpty(Dir))
                    {
                        Directory.Delete(outt, true);
                    }
                    //   Delete_empty_Folders(outt);
                }
                else
                {
                    System.IO.DirectoryInfo Dir = new DirectoryInfo(outt);

                    if (IsDirectoryEmpty(Dir))
                    {
                        Directory.Delete(outt, true);
                    }
                    Write_To_Log(ErrorManager(e));

                    Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));
                }
                // Log_Box.AppendText("\nCould not Find the Install at " +root+ " - Continuing Traversal");
            }


            return false;

        }
        void WalkDirectoryTree(System.IO.DirectoryInfo root, String Search)
        {
           
            System.IO.DirectoryInfo[] subDirs = null;

            try
            {
               
                subDirs = root.GetDirectories();


                var last = subDirs.Last();
                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    if (dirInfo.Name.Contains(Search))
                    {
                     
                        Current_Install_Folder = (dirInfo.FullName);
                        break;

                    }
                    else if (last.Equals(dirInfo) && NS_Installed == false)
                    {
                        failed_search_counter++;
                        return;
                    }
                    else
                    {

                        if (deep_Chk == true)
                        {
                            WalkDirectoryTree(dirInfo, Search);

                        }
                    }
                    if (dirInfo == null)
                    {
                        continue;

                    }
                }

                Send_Error_Notif(GetTextResource("NOTIF_ERROR_GROUP_DIRECTORY_TREE_CANNOT_FIND_INSTALL_AT") + root + GetTextResource("NOTIF_ERROR_GROUP_DIRECTORY_TREE_CONTINUE_TRAVERSE"));

            }
            catch (NullReferenceException e)
            {
                Write_To_Log(ErrorManager(e));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));


            }

        }
        private void parse_git_to_zip(string address, bool skin = false)
        {

            Mod_Progress_BAR.Value = 0;
            Mod_Progress_BAR.ShowText = true;

            if (webClient != null)
                return;
            webClient = new WebClient();
            if (skin == true)
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed_Mod_Browser_Skins);


            }
            else
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed_Mod_Browser);
            }
            if (Directory.Exists(Current_Install_Folder + @"\NS_Downloaded_Mods"))
            {

                webClient.DownloadFileAsync(new Uri(address), Current_Install_Folder + @"\NS_Downloaded_Mods\" + LAST_INSTALLED_MOD + ".zip");
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback4);

            }
            else
            {
                Directory.CreateDirectory(Current_Install_Folder + @"\NS_Downloaded_Mods");
                webClient.DownloadFileAsync(new Uri(address), Current_Install_Folder + @"\NS_Downloaded_Mods\" + LAST_INSTALLED_MOD + ".zip");
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback4);


            }







            //   Active_ListBox.Refresh();
            //   Inactive_ListBox.Refresh();


        }
        private void DownloadProgressCallback4(object sender, DownloadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.
            //////Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...", (string)e.UserState, e.BytesReceived,e.TotalBytesToReceive,e.ProgressPercentage);

            Mod_Progress_BAR.Value = e.ProgressPercentage;
        }
        private void DownloadProgressCallback_Progress_Window(object sender, DownloadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.

            Progress_Bar_Window.Value = e.ProgressPercentage;
        }
        private string Auto_Find_And_verify()
        {
           
                string path = "C:/ProgramData/Microsoft/Windows/Start Menu/Programs/Steam";

                if (!Directory.Exists(path) || !File.Exists(Path.Combine(path, "Steam.lnk")))
                {
                    if (Directory.Exists("C:/Program Files (x86)/Origin Games/Titanfall2") && File.Exists("C:/Program Files (x86)/Origin Games/Titanfall2/Titanfall2.exe"))
                        return "C:/Program Files (x86)/Origin Games/Titanfall2";

                    try
                    {
                        RegistryKey originReg = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Respawn").OpenSubKey("Titanfall2");
                        if (originReg.GetValue("Install Dir") != null) return (string)originReg.GetValue("Install Dir");
                    }
                    catch
                    {

                    }

                Titanfall2_Directory_TextBox.Background = Brushes.Red;
                       Install_NS_EXE_Textbox.Background = Brushes.Red;
                       Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_GAME_INSTALL_NOT_FOUND"));
                       return null;
            }

            // AUTOMATIC AQCUISITION
            // THE COOL SHIT:TM:

            string target = GetShortcutTarget(Path.Combine(path, "Steam.lnk"));
                string steamDir = Path.GetDirectoryName(target);

                //Console.WriteLine(target);

                List<string> folderPaths = new List<string>();

                // probably stupid, but meh
                string[] libraryFolders = File.ReadAllText(Path.Combine(steamDir, "config/libraryfolders.vdf")).Split('\"');
                for (int i = 0; i < libraryFolders.Length; i++)
                {
                    string val = libraryFolders[i];
                    if (val == "path")
                    {
                        Console.WriteLine(libraryFolders[i + 2]);
                        folderPaths.Add(libraryFolders[i + 2]);
                    }
                }

                foreach (string folder in folderPaths)
                {
                    //Console.WriteLine(folder);
                    Thread.Sleep(1000);
                    foreach (string dir in Directory.GetDirectories(Path.Combine(folder, "steamapps/common")))
                    {
                        //Console.WriteLine(dir);
                        if (dir.EndsWith("Titanfall2") && File.Exists(Path.Combine(dir, "Titanfall2.exe")))
                        {
                            return dir;
                        }
                    }
                }

                try
                {
                    RegistryKey originReg = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Respawn").OpenSubKey("Titanfall2");
                    if (originReg.GetValue("Install Dir") != null) return (string)originReg.GetValue("Install Dir");
                }
                catch
                {

                }

                if (Directory.Exists("C:/Program Files (x86)/Origin Games/Titanfall2") && File.Exists("C:/Program Files (x86)/Origin Games/Titanfall2/Titanfall2.exe"))
                    return "C:/Program Files (x86)/Origin Games/Titanfall2";
            Titanfall2_Directory_TextBox.Background = Brushes.Red;
                   Install_NS_EXE_Textbox.Background = Brushes.Red;
                   Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_GAME_INSTALL_NOT_FOUND"));
            return null;

            //failed_search_counter = 0;
            //Send_Info_Notif(GetTextResource("NOTIF_INFO_FINDING_GAME"));
            //while (NS_Installed == false && failed_search_counter < 1)
            //{

            //    Send_Info_Notif(GetTextResource("NOTIF_INFO_LOOKING_FOR_INSTALL_SMILEYFACE"));
            //    FindNSInstall("Titanfall2", @"C:\Program Files (x86)\Steam");

            //    FindNSInstall("Titanfall2", @"C:\Program Files (x86)\Origin Games");

            //    FindNSInstall("Titanfall2", @"C:\Program Files\EA Games");
            //    if (NS_Installed == false && failed_search_counter >= 1)
            //    {
            //        Titanfall2_Directory_TextBox.Background = Brushes.Red;
            //        Install_NS_EXE_Textbox.Background = Brushes.Red;
            //        Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_GAME_INSTALL_NOT_FOUND"));
            //        break;


            //    }

            //}
            //if (NS_Installed == true)
            //{
               
            //    User_Settings_Vars.NorthstarInstallLocation = Current_Install_Folder;
            //    string User_Settings_Json_Strings = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
            //    using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
            //    {
            //        StreamWriter.WriteLine(User_Settings_Json_Strings);
            //        StreamWriter.Close();
            //    }
            //    NSExe = Get_And_Set_Filepaths(Current_Install_Folder, "NorthstarLauncher.exe");
            //    Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_FOUND_INSTALL"));
            //    Check_Integrity_Of_NSINSTALL();
            //}

        }
        // stolen from the internet
        //stolen from EladNLG with permission
        private static string GetShortcutTarget(string file)
        {
            try
            {
                if (System.IO.Path.GetExtension(file).ToLower() != ".lnk")
                {
                    throw new Exception("Supplied file must be a .LNK file");
                }

                FileStream fileStream = File.Open(file, FileMode.Open, FileAccess.Read);
                using (System.IO.BinaryReader fileReader = new BinaryReader(fileStream))
                {
                    fileStream.Seek(0x14, SeekOrigin.Begin);     // Seek to flags
                    uint flags = fileReader.ReadUInt32();        // Read flags
                    if ((flags & 1) == 1)
                    {                      // Bit 1 set means we have to
                                           // skip the shell item ID list
                        fileStream.Seek(0x4c, SeekOrigin.Begin); // Seek to the end of the header
                        uint offset = fileReader.ReadUInt16();   // Read the length of the Shell item ID list
                        fileStream.Seek(offset, SeekOrigin.Current); // Seek past it (to the file locator info)
                    }

                    long fileInfoStartsAt = fileStream.Position; // Store the offset where the file info
                                                                 // structure begins
                    uint totalStructLength = fileReader.ReadUInt32(); // read the length of the whole struct
                    fileStream.Seek(0xc, SeekOrigin.Current); // seek to offset to base pathname
                    uint fileOffset = fileReader.ReadUInt32(); // read offset to base pathname
                                                               // the offset is from the beginning of the file info struct (fileInfoStartsAt)
                    fileStream.Seek((fileInfoStartsAt + fileOffset), SeekOrigin.Begin); // Seek to beginning of
                                                                                        // base pathname (target)
                    long pathLength = (totalStructLength + fileInfoStartsAt) - fileStream.Position - 2; // read
                                                                                                        // the base pathname. I don't need the 2 terminating nulls.
                    char[] linkTarget = fileReader.ReadChars((int)pathLength); // should be unicode safe
                    var link = new string(linkTarget);

                    int begin = link.IndexOf("\0\0");
                    if (begin > -1)
                    {
                        int end = link.IndexOf("\\\\", begin + 2) + 2;
                        end = link.IndexOf('\0', end) + 1;

                        string firstPart = link.Substring(0, begin);
                        string secondPart = link.Substring(end);

                        return firstPart + secondPart;
                    }
                    else
                    {
                        return link;
                    }
                }
            }
            catch
            {
                return "";
            }
        }
    
    private bool IsValidPath(string path, bool allowRelativePaths = false)
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
                isValid = false;
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));
                Write_To_Log(ErrorManager(ex));
            }

            return isValid;
        }

        private string ErrorManager(Exception ex, bool CrashProgram = false)
        {
            string stack = ex.StackTrace;
            string source = ex.Source;
            string message = ex.Message;
            string LF = Environment.NewLine;

            string Error = $"A crash happened at {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.ff", CultureInfo.InvariantCulture)}{LF}" +
                           $"This is what crashed: {source}{LF}" +
                           $"This is the error message : {LF}{message}{LF + LF}" +
                           $"These are the last instructions executed before the crash : {LF}{stack}{LF + LF}";

            if (ex.InnerException != null)
            {
                Error += $"Inner Exception : {LF}{ex.InnerException.Message}";
            }

            if (CrashProgram)
            {
                throw new Exception(Error);
            }
            Log_Indicator.Background = Brushes.Red;

            return Error.Trim();
        }
        void Call_Mods_From_Folder()
        {
            bool install_Prompt = false;
            try
            {

                Enabled_ListBox.ItemsSource = null;
                Disabled_ListBox.ItemsSource = null;
                Mod_Directory_List_Active.Clear();
                Mod_Directory_List_InActive.Clear();





                if (Current_Install_Folder == null || Current_Install_Folder == "" || !Directory.Exists(Current_Install_Folder))
                {
                    HandyControl.Controls.Growl.AskGlobal("Could Not find That Install Location !!!, please renavigate to the Correct Install Path!", isConfirmed =>
                     {
                         install_Prompt = isConfirmed;
                         return true;
                     });

                    if (install_Prompt == true)
                    {
                        Select_Main();
                    }



                }
                else
                {
                    if (Directory.Exists(Current_Install_Folder))
                    {
                        if (NS_Installed == true)
                        {
                   
                            string NS_Mod_Dir = Current_Install_Folder + @"\R2Northstar\mods";
                            System.IO.DirectoryInfo rootDirs = new DirectoryInfo(@NS_Mod_Dir);
                            if (!Directory.Exists(NS_Mod_Dir))
                            {
                                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_MOD_DIRECTORY_EMPTY"));
                                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_NS_NOT_INSTALLED_PROPERLY"));

                            }
                            else if (IsValidPath(NS_Mod_Dir) == true)
                            {

                                System.IO.DirectoryInfo[] subDirs = null;
                                subDirs = rootDirs.GetDirectories();

                                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                                {
                                    if (IsDirectoryEmpty(dirInfo))
                                    {
                                        if(IsDirectoryEmpty(new DirectoryInfo(dirInfo.FullName)) == true)
                                        {
                                            Directory.Delete(dirInfo.FullName, true);


                                        }

                                    }
                                    else if (Template_traverse(dirInfo, "Locked_Folder") == true)
                                    {
                                     
                                        Mod_Directory_List_InActive.Add(dirInfo.Name);

                                        if (Directory.Exists(dirInfo + @"\Locked_Folder") && IsDirectoryEmpty(new DirectoryInfo(dirInfo + @"\Locked_Folder")))
                                        {

                                       

                                            Directory.Delete(dirInfo + @"\Locked_Folder");

                                     
                                        }
                                    }
                                    else
                                    {

                                        Mod_Directory_List_Active.Add(dirInfo.Name);


                                    }
                                }

                                ApplyDataBinding();
                            }
                            else
                            {

                                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_NS_NOT_INSTALLED_PROPERLY"));
                            }
                        }
                        else
                        {

                            Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_NS_NOT_INSTALLED"));


                        }
                    }

                    else
                    {

                        Send_Error_Notif(GetTextResource("NOTIF_FATALL_GAME_PATH_INVALID"));

                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Sequence contains no elements")
                {
                    Send_Info_Notif(GetTextResource("NOTIF_INFO_NO_MODS_FOUND"));

                }
                else
                {
                    Write_To_Log(ErrorManager(ex));
                    Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

                }



            }


        }
        async Task Install_NS_METHOD()
        {
            try
            {

                Loading_Panel.Visibility = Visibility.Visible;
                Progress_Bar_Window.Value = 0;
                if (Directory.Exists(Current_Install_Folder + @"\TempCopyFolder"))
                {
                    try
                    {
                        Directory.Delete(Current_Install_Folder + @"\TempCopyFolder", true);
                    }
                    catch (Exception e)
                    {
                        Write_To_Log("Err on temp folder delete.");
                        Write_To_Log(ErrorManager(e));

                    }
                }
                completed_flag = 0;
                PropertyGridDemoModel Git = new PropertyGridDemoModel();
                Read_Latest_Release(Current_REPO_URL).Wait();
                Current_File_Label.Content = GetTextResource("DOWNLOADING_NORTHSTAR_LATEST_RELEAST_TEXT") + "-" + Properties.Settings.Default.Version;
                Status_Label.Content = GetTextResource("CURRENTLY_DOWNLOADING");
                Wait_Text.Text = GetTextResource("PLEASE_WAIT");
                //  Is file downloading yet?
                if (webClient != null)
                    return;
                webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                string x = "";
                if (Current_Install_Folder != null || Current_Install_Folder != "")
                {
                    if (File.Exists(Current_Install_Folder + @"\ns_startup_args_dedi.txt") && File.Exists(Current_Install_Folder + @"\ns_startup_args.txt") && File.Exists(GetFile(Current_Install_Folder, "autoexec_ns_server.cfg").First()))
                    {
                        x = GetFile(Current_Install_Folder, "autoexec_ns_server.cfg").First();
                       
                        if (do_not_overwrite_Ns_file == true)
                        {
                            if (Directory.Exists(Current_Install_Folder + @"\TempCopyFolder"))
                            {
                                Send_Info_Notif(GetTextResource("NOTIF_INFO_BACKING_UP_ARG_FILES"));
                                if (Directory.Exists(Current_Install_Folder + @"\ns_startup_args.txt"))
                                {
                                    System.IO.File.Copy(Current_Install_Folder + @"\ns_startup_args.txt", Current_Install_Folder + @"\TempCopyFolder\ns_startup_args.txt", true);
                                }
                                if (Directory.Exists(Current_Install_Folder + @"\ns_startup_args_dedi.txt"))
                                {
                                    System.IO.File.Copy(Current_Install_Folder + @"\ns_startup_args_dedi.txt", Current_Install_Folder + @"\TempCopyFolder\ns_startup_args_dedi.txt", true);
                                }
                            }
                            else
                            {

                                Send_Info_Notif(GetTextResource("NOTIF_INFO_CREATING_DIRECTORY_AND_BACKUP_ARGS"));
                                System.IO.Directory.CreateDirectory(Current_Install_Folder + @"\TempCopyFolder");
                                if (Directory.Exists(Current_Install_Folder + @"\ns_startup_args.txt"))
                                {
                                    System.IO.File.Copy(Current_Install_Folder + @"\ns_startup_args.txt", Current_Install_Folder + @"\TempCopyFolder\ns_startup_args.txt", true);
                                }
                                if (Directory.Exists(Current_Install_Folder + @"\ns_startup_args_dedi.txt"))
                                {
                                    System.IO.File.Copy(Current_Install_Folder + @"\ns_startup_args_dedi.txt", Current_Install_Folder + @"\TempCopyFolder\ns_startup_args_dedi.txt", true);
                                }
                            }
                            Directory.CreateDirectory(DocumentsFolder + @"\VTOL_DATA\Releases\");
                            webClient.DownloadFileAsync(new Uri(current_Northstar_version_Url), DocumentsFolder + @"\VTOL_DATA\Releases\Northstar_Release.zip");
                            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback_Progress_Window);

                            Send_Warning_Notif(GetTextResource("NOTIF_WARN_INSTALL_START"));



                        }
                        else
                        {

                            Directory.CreateDirectory(DocumentsFolder + @"\VTOL_DATA\Releases\");
                            webClient.DownloadFileAsync(new Uri(current_Northstar_version_Url), DocumentsFolder + @"\VTOL_DATA\Releases\Northstar_Release.zip");
                            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback_Progress_Window);

                            Send_Warning_Notif(GetTextResource("NOTIF_WARN_INSTALL_START"));



                        }



                    }
                    else
                    {
                        Send_Error_Notif(GetTextResource("NOTIF_ERROR_CANNOT_FIND_NS_AND_DEDI_ARG"));

                        Directory.CreateDirectory(DocumentsFolder + @"\VTOL_DATA\Releases\");
                        webClient.DownloadFileAsync(new Uri(current_Northstar_version_Url), DocumentsFolder + @"\VTOL_DATA\Releases\Northstar_Release.zip");
                        webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback_Progress_Window);

                        Send_Warning_Notif(GetTextResource("NOTIF_WARN_INSTALL_START"));

                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == "Sequence contains no elements")
                {
                    //   Directory.CreateDirectory(@"C:\ProgramData\VTOL_DATA\Releases\");
                    //   webClient.DownloadFileAsync(new Uri(current_Northstar_version_Url), @"C:\ProgramData\VTOL_DATA\Releases\Northstar_Release.zip");
                    //   webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback_Progress_Window);

                    //    Send_Warning_Notif(GetTextResource("NOTIF_WARN_INSTALL_START"));
                }
                else
                {
                    Write_To_Log(ErrorManager(ex));
                    Install_NS.IsEnabled = true;
                    Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));
                    Loading_Panel.Visibility = Visibility.Hidden;



                }

                return;
            }
        }
      
        private void Check_Args()
        {


            if (Directory.Exists(Current_Install_Folder))
            {


                if (File.Exists(Ns_dedi_File))
                {
                    Dedicated_Server_Startup_ARGS.Text = Ns_dedi_File;
                    Dedicated_Server_Startup_ARGS.Background = Brushes.White;
                    Arg_Box_Dedi.Text = Read_From_TextFile_OneLine(Ns_dedi_File);
                    GC.Collect();


                }
                else
                {
                    Send_Error_Notif(GetTextResource("NOTIF_ERROR_CANNOT_FIND_FILE_SETPATH"));
                    Arg_Box_Dedi.Text = "Err, File not found - ns_startup_args_dedi.txt";
                    Dedicated_Server_Startup_ARGS.Background = Brushes.Red;

                    GC.Collect();

                }


                if (File.Exists(GetFile(Current_Install_Folder, "ns_startup_args.txt").First()))
                {
                    Arg_Box.Text = Read_From_TextFile_OneLine(Current_Install_Folder + @"\ns_startup_args.txt");

                    GC.Collect();

                }
                else
                {
                    Send_Error_Notif(GetTextResource("NOTIF_ERROR_CANNOT_FIND_FILE_CREATE"));
                    Arg_Box.Text = "Err, File not found - ns_startup_args.txt";
                    GC.Collect();
                }

                if (File.Exists(Convar_File))
                {

                    Dedicated_Convar_ARGS.Text = Convar_File;
                    Dedicated_Convar_ARGS.Background = Brushes.White;

                    GC.Collect();

                }
                else
                {
                    Send_Error_Notif(GetTextResource("NOTIF_ERROR_CANNOT_FIND_FILE_SETPATH"));
                    Dedicated_Convar_ARGS.Background = Brushes.Red;

                    Dedicated_Convar_ARGS.Text = "Err, File not found - autoexec_ns_server.cfg";
                    GC.Collect();
                }
            }


            else
            {

                Send_Error_Notif(GetTextResource("NOTIF_ERROR_CANNOT_FIND_FOLDER"));
                GC.Collect();


            }
        }

        private void ApplyDataBinding()
        {
            Disabled_ListBox.ItemsSource = null;
            Disabled_ListBox.ItemsSource = Mod_Directory_List_InActive.ToArray();

            Enabled_ListBox.ItemsSource = null;

            Enabled_ListBox.ItemsSource = Mod_Directory_List_Active.ToArray();
            Enabled_ListBox.Refresh();
            Disabled_ListBox.Refresh();

        }
        void Move_List_box_Inactive_To_Active(ListBox Inactive)
        {
            try
            {
                string Modname = "";

                string selected = null;
                if (Inactive.Items.Count != 0)
                {

                    if (Inactive.SelectedValue != null || Inactive.SelectedValue != "")
                    {
                        Modname = Inactive.SelectedValue.ToString();
                        selected = Inactive.SelectedValue.ToString();
                        int index = Inactive.SelectedIndex;
                        if (Mod_Directory_List_Active != null && Mod_Directory_List_InActive != null)
                        {
                            Mod_Directory_List_InActive.RemoveAt(index);
                            Mod_Directory_List_Active.Add(selected);
                        }
                        ApplyDataBinding();
                        //  from.Items.RemoveAt(from.Items.IndexOf(from.SelectedItem));
                        //  To.Items.Add(selected);
                        if (index >= Inactive.Items.Count)
                        {
                            index--;

                        }
                        Inactive.SelectedIndex = index;
                    }
                    else
                    {
                        Inactive.SelectedIndex = 0;

                    }
                }
                else
                {
                    Send_Warning_Notif(GetTextResource("NOTIF_WARN_CANNOT_MOVE_MOD_FROM_EMPTY"));
                    return;

                }
            }
            catch (NullReferenceException ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Warning_Notif(GetTextResource("NOTIF_WARN_CANNOT_MOVE_MOD"));
                return;
            }





        }
        void Move_List_box_Active_To_Inactive(ListBox Active)
        {
            try
            {
                string selected = null;


                if (Active.Items.Count != 0)
                {

                    if (Active.SelectedValue != null || Active.SelectedValue != "")
                    {
                        selected = Active.SelectedValue.ToString();

                        int index = Active.SelectedIndex;
                        if (Mod_Directory_List_Active != null && Mod_Directory_List_InActive != null)
                        {
                            Mod_Directory_List_Active.RemoveAt(index);
                            Mod_Directory_List_InActive.Add(selected);
                        }
                        ApplyDataBinding();
                        //  from.Items.RemoveAt(from.Items.IndexOf(from.SelectedItem));
                        //  To.Items.Add(selected);
                        if (index >= Active.Items.Count)
                        {
                            index--;

                        }
                        Active.SelectedIndex = index;
                    }
                    else
                    {
                        Active.SelectedIndex = 0;

                    }
                }
                else
                {
                    Send_Warning_Notif(GetTextResource("NOTIF_WARN_CANNOT_MOVE_MOD_FROM_EMPTY"));
                    return;

                }
            }
            catch (NullReferenceException ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Warning_Notif(GetTextResource("NOTIF_WARN_CANNOT_MOVE_MOD"));
                return;
            }


        }
        private static void MoveFiles(string sourceDir, string targetDir)
        {
            System.IO.DirectoryInfo Targ = new DirectoryInfo(targetDir);
            System.IO.DirectoryInfo src = new DirectoryInfo(sourceDir);

            if (!IsDirectoryEmpty(Targ))
            {
                if (!IsDirectoryEmpty(src))
                {
                    IEnumerable<FileInfo> files = Directory.GetFiles(sourceDir).Select(f => new FileInfo(f));
                    foreach (var file in files)
                    {
                        File.Move(file.FullName, Path.Combine(targetDir, file.Name), true);
                    }

                }

            }
        }

        public void Move_Mods()
        {

            try
            {

                if (Directory.Exists(Current_Install_Folder + @"\R2Northstar\mods\"))
                {
                    List<string> Inactive = Disabled_ListBox.Items.OfType<string>().ToList();
                    List<string> Active = Enabled_ListBox.Items.OfType<string>().ToList();

                    foreach (var val in Inactive)
                    {
                        if (val != null)
                        {
                            //////Console.WriteLine(val);
                            System.IO.DirectoryInfo rootDirs = new DirectoryInfo(Current_Install_Folder + @"\R2Northstar\mods\" + val);

                            if (!IsDirectoryEmpty(rootDirs))
                            {
                                if (Directory.Exists(Current_Install_Folder + @"\R2Northstar\mods\" + val + @"\Locked_Folder"))
                                {

                                    // MoveFiles(Current_Install_Folder + @"\R2Northstar\mods\" + val, Current_Install_Folder + @"\R2Northstar\mods\" + val + @"\Locked_Folder");
                                    if (File.Exists(Current_Install_Folder + @"\R2Northstar\mods\" + val + @"\mod.json"))
                                    {
                                        File.Move(Current_Install_Folder + @"\R2Northstar\mods\" + val + @"\mod.json", Current_Install_Folder + @"\R2Northstar\mods\" + val + @"\Locked_Folder" + @"\mod.json", true);


                                    }
                                    else
                                    {
                                        MoveFiles(Current_Install_Folder + @"\R2Northstar\mods\" + val, Current_Install_Folder + @"\R2Northstar\mods\" + val + @"\Locked_Folder");

                                    }

                                }
                                else
                                {

                                    Directory.CreateDirectory(Current_Install_Folder + @"\R2Northstar\mods\" + val + @"\Locked_Folder");
                                    if (File.Exists(Current_Install_Folder + @"\R2Northstar\mods\" + val + @"\mod.json"))
                                    {
                                        File.Move(Current_Install_Folder + @"\R2Northstar\mods\" + val + @"\mod.json", Current_Install_Folder + @"\R2Northstar\mods\" + val + @"\Locked_Folder" + @"\mod.json", true);


                                    }
                                    else
                                    {
                                        MoveFiles(Current_Install_Folder + @"\R2Northstar\mods\" + val, Current_Install_Folder + @"\R2Northstar\mods\" + val + @"\Locked_Folder");

                                    }

                                }
                            }
                        }

                    }
                    foreach (var val in Active)
                    {
                        if (val != null)
                        {
                            //  ////Console.WriteLine(Current_Install_Folder + @"\R2Northstar\mods\" + val);
                            System.IO.DirectoryInfo rootDirs = new DirectoryInfo(Current_Install_Folder + @"\R2Northstar\mods\" + val);
                            System.IO.DirectoryInfo Locked = new DirectoryInfo(Current_Install_Folder + @"\R2Northstar\mods\" + val + @"\Locked_Folder");

                            if (!IsDirectoryEmpty(rootDirs))
                            {
                                if (Directory.Exists(Locked.FullName))
                                {
                                    if (IsDirectoryEmpty(Locked))
                                    {

                                        Directory.Delete(Locked.FullName);
                                        Call_Mods_From_Folder();

                                    }
                                    MoveFiles(Locked.FullName, rootDirs.FullName);
                                    Directory.Delete(Locked.FullName);

                                }

                            }
                        }

                    }

                    Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_MODS_MOVED_SUCCESS"));
                }
                else
                {
                    Send_Error_Notif(GetTextResource("NOTIF_ERROR_NS_BAD_INTEGRITY"));
                }
            }
            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));

                Send_Warning_Notif(GetTextResource("NOTIF_WARN_CHECK_MOD_AT") + Current_Install_Folder + @"\R2Northstar\mods\");
                // Log_Box.AppendText(ex.StackTrace);

            }


        }
        IEnumerable<string> SearchAccessibleFiles(string root, string searchTerm)
        {
            var files = new List<string>();


            foreach (var file in Directory.EnumerateFiles(root).Where(m => m.Contains(searchTerm)))
            {
                // string FolderName = file.Split(Path.DirectorySeparatorChar).Last();
                string lastFolderName = new DirectoryInfo(System.IO.Path.GetDirectoryName(file)).FullName;

                //////Console.WriteLine(lastFolderName);
                files.Add(file);
            }
            foreach (var subDir in Directory.EnumerateDirectories(root))
            {
                try
                {
                    files.AddRange(SearchAccessibleFiles(subDir, searchTerm));
                }
                catch (UnauthorizedAccessException ex)
                {
                    Send_Warning_Notif(GetTextResource("NOTIF_WARN_NO_ACCESS_TO_DIRECTORY"));
                    Write_To_Log(ErrorManager(ex));

                }
            }

            return files;
        }
        public static string FindFirstFile(string path, string searchPattern)
        {
            string[] files;

            try
            {
                // Exception could occur due to insufficient permission.
                files = Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {


                return string.Empty;
            }

            // If matching files have been found, return the first one.
            if (files.Length > 0)
            {
                return files[0];
            }
            else
            {
                // Otherwise find all directories.
                string[] directories;

                try
                {
                    // Exception could occur due to insufficient permission.
                    directories = Directory.GetDirectories(path);
                }
                catch (Exception)
                {
                    return string.Empty;
                }

                // Iterate through each directory and call the method recursivly.
                foreach (string directory in directories)
                {
                    string file = FindFirstFile(directory, searchPattern);

                    // If we found a file, return it (and break the recursion).
                    if (file != string.Empty)
                    {
                        return file;
                    }
                }
            }

            // If no file was found (neither in this directory nor in the child directories)
            // simply return string.Empty.
            return string.Empty;
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
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

                Write_To_Log(ErrorManager(ex));
            }
        }
        public static bool ZipHasFile(string Search, string zipFullPath)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipFullPath))  //safer than accepted answer
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.Contains(Search, StringComparison.OrdinalIgnoreCase))
                    {

                        return true;
                    }
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
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                webClient = null;
                Send_Info_Notif(GetTextResource("NOTIF_INFO_DOWNLOAD_COMPLETE"));
                if (File.Exists(DocumentsFolder + @"\VTOL_DATA\Releases\NorthStar_Release.zip"))
                {
                    Unpack_To_Location(DocumentsFolder + @"\VTOL_DATA\Releases\NorthStar_Release.zip", Current_Install_Folder);
                }
                Install_NS.IsEnabled = true;
                Loading_Panel.Visibility = Visibility.Hidden;
                Wait_Text.Text = GetTextResource("PLEASE_WAIT");
                Status_Label.Content = (GetTextResource("CURRENTLY_DOWNLOADING"));
                Progress_Bar_Window.Value = 0;
                Current_File_Label.Content = "";
            }

            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }
        }
        private void Completed_t(object sender, AsyncCompletedEventArgs e)
        {
            try
            {

                webClient = null;
                Send_Info_Notif(GetTextResource("NOTIF_INFO_DOWNLOAD_COMPLETE"));
                Unpack_To_Location_Custom(Current_Install_Folder + @"\NS_Downloaded_Mods\MOD.zip", Current_Install_Folder + @"\R2Northstar\mods");
                Loading_Panel.Visibility = Visibility.Hidden;
                Status_Label.Content = (GetTextResource("CURRENTLY_INSTALLING"));
                Wait_Text.Text = GetTextResource("PLEASE_WAIT");

                Progress_Bar_Window.Value = 0;
                Current_File_Label.Content = "";
            }

            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }
        }

        private void Completed_Mod_Browser(object sender, AsyncCompletedEventArgs e)
        {
            try
            {

                webClient = null;
                Send_Info_Notif(GetTextResource("NOTIF_INFO_DOWNLOAD_COMPLETE"));
                Mod_Progress_BAR.Value = 0;
                Mod_Progress_BAR.ShowText = false;


                Unpack_To_Location_Custom(Current_Install_Folder + @"\NS_Downloaded_Mods\" + LAST_INSTALLED_MOD + ".zip", Current_Install_Folder + @"\R2Northstar\mods\" + LAST_INSTALLED_MOD, true, false, false);
                Completed_Operation = true;
            }

            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }
        }
        private void Completed_Mod_Browser_Skins(object sender, AsyncCompletedEventArgs e)
        {
            try
            {

                webClient = null;
                Send_Info_Notif(GetTextResource("NOTIF_INFO_DOWNLOAD_COMPLETE"));
                Mod_Progress_BAR.Value = 0;
                Mod_Progress_BAR.ShowText = false;


                Unpack_To_Location_Custom(Current_Install_Folder + @"\NS_Downloaded_Mods\" + LAST_INSTALLED_MOD + ".zip", Current_Install_Folder + @"\R2Northstar\mods\" + LAST_INSTALLED_MOD, true, false, true);
                Completed_Operation = true;
            }

            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }
        }
        private void Check_Btn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!IsValidPath(Current_Install_Folder))
                {
                    MessageBox.Show("o");
                    string Path = Auto_Find_And_verify();
                    if (IsValidPath(Path))
                    {
                        Current_Install_Folder = Path;
                        User_Settings_Vars.NorthstarInstallLocation = Path;
                        string User_Settings_Json_Strings = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
                        using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
                        {
                            StreamWriter.WriteLine(User_Settings_Json_Strings);
                            StreamWriter.Close();
                        }

                    }
                }
                Check_Integrity_Of_NSINSTALL();

            }


            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }
        }

        private void Titanfall_2_Btn_MouseEnter(object sender, MouseEventArgs e)
        {

            //Button Bt = (Button)sender;
            // DoubleAnimation Anim = new DoubleAnimation(0, TimeSpan.FromSeconds(2));

            Banner_Image.Visibility = Visibility.Collapsed;

            Gif_Image_Northstar.Visibility = Visibility.Collapsed;
            Animation_Start_Vanilla = true;
            Gif_Image_Vanilla.Visibility = Visibility.Visible;
        }

        private void Northstar_Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            Gif_Image_Vanilla.Visibility = Visibility.Collapsed;

            Banner_Image.Visibility = Visibility.Collapsed;
            Animation_Start_Northstar = true;
            Gif_Image_Northstar.Visibility = Visibility.Visible;


        }
        void Check_For_New_Northstar_Install()
        {
            try
            {
                Updater Update = new Updater(Author_Used, Repo_Used);
                Update.Force_Version = Properties.Settings.Default.Version.Remove(0,1);
                Update.Force_Version_ = true;
                if (Update.CheckForUpdate())
                {
                    Badge.Visibility = Visibility.Visible;
                    Badge.Text = GetTextResource("BADGE_NEW_UPDATE_AVAILABLE");
                }
                else
                {

                    Badge.Visibility = Visibility.Collapsed;


                }

            }
            catch (Exception ex)
            {
                Send_Warning_Notif(GetTextResource("NOTIF_WARN_SUGGEST_REINSTALL_NS"));
                Write_To_Log(ErrorManager(ex));
            }



        }

        private void Titanfall_2_Btn_Click(object sender, RoutedEventArgs e)
        {

            if (Directory.Exists(Current_Install_Folder))
            {

                if (File.Exists(Current_Install_Folder + @"\" + "Titanfall2.exe"))
                {
                    ProcessStartInfo procStartInfo = new ProcessStartInfo();
                    Process process = new Process();
                    procStartInfo.FileName = Current_Install_Folder + @"\" + "Titanfall2.exe";
                    procStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Current_Install_Folder + @"\" + "Titanfall2.exe");
                    ;

                    // procStartInfo.Arguments = args;

                    process.StartInfo = procStartInfo;

                    process.Start();
                    int id = process.Id;
                    pid = id;
                    Process tempProc = Process.GetProcessById(id);
                    // this.Visible = false;
                    // Thread.Sleep(5000);
                    // tempProc.WaitForExit();
                    // this.Visible = true;

                    // Process process = Process.Start(NSExe, Arg_Box.Text);
                    process.Close();


                }
                else
                {

                    MessageBox.Show("Could Not Find Northstar.exe!");


                }
            }
            else
            {

                ////Console.WriteLine("Err, File not found");


            }

        }
        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
        public void Retry(Action action, int retryCount)
        {
            int retries = 0;

            while (retries < retryCount)
            {
                try
                {
                    Thread.Sleep(1000);
                    action();
                    return;
                }
                catch (Exception e)
                {
                    // Try Again
                    retries++;
                }
            }
        }

        public void Skin_Install_Tree(bool external_Skin = false, string external_Skin_Path = "", bool isTitan = false)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (Directory.Exists(Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR"))
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                    bitmap.EndInit();
                    Diffuse_IMG.Source = bitmap;


                    Glow_IMG.Source = bitmap;

                    Retry(() => Directory.Delete(Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR", true), 3);

                    GC.Collect();
                }
                catch (Exception ef)
                {
                    Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

                    Write_To_Log(ErrorManager(ef));
                }




            }
            if (Directory.Exists(Current_Install_Folder + @"\Thumbnails"))
            {
                try
                {
                    Diffuse_IMG.Source = null;
                    Glow_IMG.Source = null;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                    bitmap.EndInit();
                    Diffuse_IMG.Source = bitmap;


                    Glow_IMG.Source = bitmap;
                    Retry(() => Directory.Delete(Current_Install_Folder + @"\Thumbnails", true), 3);
                    GC.Collect();
                }
                catch (Exception ef)
                {
                    MessageBox.Show(ef.Message);


                    Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

                    Write_To_Log(ErrorManager(ef));
                }

            }

            if (external_Skin == false)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == true)
                {
                    Skin_Temp_Loc = openFileDialog.FileName;
                    if (Skin_Temp_Loc == null || !File.Exists(Skin_Temp_Loc))
                    {

                        Send_Error_Notif(GetTextResource("NOTIF_ERROR_INVALID_ZIP_PATH"));
                        return;

                    }
                    else
                    {
                        Skin_Path_Box.Text = Skin_Temp_Loc;
                        // Send_Success_Notif("\nSkin Found!");
                        if (ZipHasFile(".dds", Skin_Temp_Loc))
                        {
                            Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_COMPATIBLE_SKIN_FOUND"));
                            Compat_Indicator.Fill = Brushes.LimeGreen;
                            Install_Skin_Bttn.IsEnabled = true;
                            //   var directory = new DirectoryInfo(root);
                            // var myFile = (from f in directory.GetFiles()orderby f.LastWriteTime descending select f).First();
                            if (Directory.Exists(Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR"))
                            {
                                Skin_Path = Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR";
                                ZipFile.ExtractToDirectory(Skin_Temp_Loc, Skin_Path, Encoding.GetEncoding("GBK"), true);

                            }
                            else
                            {

                                Directory.CreateDirectory(Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR");
                                Skin_Path = Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR";

                                ZipFile.ExtractToDirectory(Skin_Temp_Loc, Skin_Path, Encoding.GetEncoding("GBK"),true);

                            }
                        }
                        else
                        {
                            Send_Error_Notif(GetTextResource("NOTIF_ERROR_SKIN_INCOMPATIBLE"));
                            Compat_Indicator.Fill = Brushes.Red;
                            Install_Skin_Bttn.IsEnabled = false;

                        }

                        try
                        {
                            //  ////Console.WriteLine(Skin_Temp_Loc);
                            String Thumbnail = Current_Install_Folder + @"\Thumbnails\";
                            if (Directory.Exists(Thumbnail))
                            {
                                //DirectoryInfo dir = new DirectoryInfo(Thumbnail);
                                var Serached = SearchAccessibleFiles(Skin_Path, "col");
                                var firstOrDefault_Col = Serached.FirstOrDefault();
                                if (!Serached.Any())
                                {
                                    throw new InvalidOperationException();
                                }
                                else
                                {
                                    if (File.Exists(firstOrDefault_Col))
                                    {
                                        String col = Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png";
                                        //  ////Console.WriteLine(firstOrDefault_Col);
                                        if (File.Exists(col))
                                        {

                                            DDSImage img_1 = new DDSImage(firstOrDefault_Col);
                                            img_1.Save(Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png");
                                            BitmapImage bitmap = new BitmapImage();
                                            bitmap.BeginInit();
                                            bitmap.UriSource = new Uri(col);
                                            bitmap.EndInit();
                                            Diffuse_IMG.Source = bitmap;
                                        }
                                        else
                                        {
                                            //////Console.WriteLine(col);
                                            DDSImage img_1 = new DDSImage(firstOrDefault_Col);

                                            img_1.Save(col);

                                            BitmapImage bitmap = new BitmapImage();
                                            bitmap.BeginInit();
                                            bitmap.UriSource = new Uri(col);
                                            bitmap.EndInit();
                                            Diffuse_IMG.Source = bitmap;

                                        }

                                    }
                                    else
                                    {
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                                        bitmap.EndInit();
                                        Diffuse_IMG.Source = bitmap;

                                    }


                                }

                                var Serached_ = SearchAccessibleFiles(Skin_Path, "ilm");
                                var firstOrDefault_ilm = Serached_.FirstOrDefault();
                                if (!Serached.Any())
                                {
                                    throw new InvalidOperationException();
                                }
                                else
                                {
                                    if (File.Exists(firstOrDefault_ilm))
                                    {
                                        if (File.Exists(firstOrDefault_ilm + ".png"))
                                        {

                                            ////Console.WriteLine(firstOrDefault_ilm);
                                            // Image Image_2 = new Bitmap(Thumbnail+Path.GetFileName(firstOrDefault_ilm)+".png");
                                            BitmapImage bitmap = new BitmapImage();
                                            bitmap.BeginInit();
                                            bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                            bitmap.EndInit();
                                            Glow_IMG.Source = bitmap;
                                        }
                                        else
                                        {

                                            DDSImage img_2 = new DDSImage(firstOrDefault_ilm);
                                            img_2.Save(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");

                                            //Image Image_2 = new Bitmap(Thumbnail+Path.GetFileName(firstOrDefault_ilm)+".png");
                                            BitmapImage bitmap = new BitmapImage();
                                            bitmap.BeginInit();
                                            bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                            bitmap.EndInit();
                                            Glow_IMG.Source = bitmap;
                                        }
                                    }
                                    else
                                    {
                                        // Image Image_1 = new Bitmap(Directory.GetCurrentDirectory()+@"\No_Texture.jpg");
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                                        bitmap.EndInit();
                                        Glow_IMG.Source = bitmap;
                                    }

                                }




                            }

                            else
                            {

                                Directory.CreateDirectory(Thumbnail);


                                //DirectoryInfo dir = new DirectoryInfo(Thumbnail);
                                var Serached = SearchAccessibleFiles(Skin_Path, "col");
                                var firstOrDefault_Col = Serached.FirstOrDefault();
                                if (!Serached.Any())
                                {
                                    throw new InvalidOperationException();
                                }
                                else
                                {
                                    if (File.Exists(firstOrDefault_Col))
                                    {
                                        //  ////Console.WriteLine(firstOrDefault_Col);
                                        if (File.Exists(firstOrDefault_Col + ".png"))
                                        {

                                            //   Image Image_1 = new Bitmap(Thumbnail+Path.GetFileName(firstOrDefault_Col)+".png");
                                            BitmapImage bitmap = new BitmapImage();
                                            bitmap.BeginInit();
                                            bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png");
                                            bitmap.EndInit();
                                            Diffuse_IMG.Source = bitmap;
                                        }
                                        else
                                        {
                                            DDSImage img_1 = new DDSImage(firstOrDefault_Col);
                                            img_1.Save(Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png");
                                            // Image Image_1 = new Bitmap(Thumbnail+Path.GetFileName(firstOrDefault_Col)+".png");
                                            BitmapImage bitmap = new BitmapImage();
                                            bitmap.BeginInit();
                                            bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png");
                                            bitmap.EndInit();
                                            Diffuse_IMG.Source = bitmap;

                                        }

                                    }
                                    else
                                    {
                                        // Image Image_1 = new Bitmap(Directory.GetCurrentDirectory()+@"\No_Texture.jpg");
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                                        bitmap.EndInit();
                                        Diffuse_IMG.Source = bitmap;

                                    }


                                }

                                var Serached_ = SearchAccessibleFiles(Skin_Path, "ilm");
                                var firstOrDefault_ilm = Serached_.FirstOrDefault();
                                if (!Serached.Any())
                                {
                                    throw new InvalidOperationException();
                                }
                                else
                                {
                                    if (File.Exists(firstOrDefault_ilm))
                                    {
                                        if (File.Exists(firstOrDefault_ilm + ".png"))
                                        {

                                            //   ////Console.WriteLine(firstOrDefault_ilm);
                                            //    Image Image_2 = new Bitmap(Thumbnail+Path.GetFileName(firstOrDefault_ilm)+".png");
                                            BitmapImage bitmap = new BitmapImage();
                                            bitmap.BeginInit();
                                            bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                            bitmap.EndInit();
                                            Glow_IMG.Source = bitmap;
                                        }
                                        else
                                        {

                                            DDSImage img_2 = new DDSImage(firstOrDefault_ilm);
                                            img_2.Save(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                            // Image Image_2 = new Bitmap(Thumbnail+Path.GetFileName(firstOrDefault_ilm)+".png");
                                            BitmapImage bitmap = new BitmapImage();
                                            bitmap.BeginInit();
                                            bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                            bitmap.EndInit();
                                            Glow_IMG.Source = bitmap;
                                        }
                                    }
                                    else
                                    {
                                        //  Image Image_1 = new Bitmap(Directory.GetCurrentDirectory()+@"\No_Texture.jpg");
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                                        bitmap.EndInit();
                                        Glow_IMG.Source = bitmap;

                                    }

                                }





                            }

                            //   Import_Skin_Bttn.Enabled=false;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("Specified"))
                            {
                                BitmapImage bitmap2 = new BitmapImage();
                                bitmap2.BeginInit();
                                bitmap2.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                                bitmap2.EndInit();
                                Diffuse_IMG.Source = bitmap2;

                                Glow_IMG.Source = bitmap2;
                                Send_Info_Notif(GetTextResource("THUMBNAIL_FAILED"));

                            }
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                            bitmap.EndInit();
                            Diffuse_IMG.Source = bitmap;

                            Glow_IMG.Source = bitmap;
                            Write_To_Log(ErrorManager(ex));
                            Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

                        }

                    }

                }
            }
            else if (external_Skin == true)
            {

                Skin_Temp_Loc = external_Skin_Path;
                if (Skin_Temp_Loc == null || !File.Exists(Skin_Temp_Loc))
                {

                    Send_Error_Notif(GetTextResource("NOTIF_ERROR_INVALID_ZIP_PATH"));
                    return;

                }
                else
                {
                    Skin_Path_Box.Text = Skin_Temp_Loc;
                    if (ZipHasFile(".dds", Skin_Temp_Loc))
                    {
                        Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_COMPATIBLE_SKIN_FOUND"));
                        Compat_Indicator.Fill = Brushes.LimeGreen;
                        Install_Skin_Bttn.IsEnabled = true;
                        if (Directory.Exists(Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR"))
                        {
                            Skin_Path = Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR";
                            ZipFile.ExtractToDirectory(Skin_Temp_Loc, Skin_Path, Encoding.GetEncoding("GBK"), true);

                        }
                        else
                        {

                            Directory.CreateDirectory(Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR");
                            Skin_Path = Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR";

                            ZipFile.ExtractToDirectory(Skin_Temp_Loc, Skin_Path, Encoding.GetEncoding("GBK"),true);

                        }
                    }
                    else
                    {
                        Send_Error_Notif(GetTextResource("NOTIF_ERROR_SKIN_INCOMPATIBLE"));
                        Compat_Indicator.Fill = Brushes.Red;
                        Install_Skin_Bttn.IsEnabled = false;

                    }

                    try
                    {
                        String Thumbnail = Current_Install_Folder + @"\Thumbnails\";
                        if (Directory.Exists(Thumbnail))
                        {
                            var Serached = SearchAccessibleFiles(Skin_Path, "col");
                            var firstOrDefault_Col = Serached.FirstOrDefault();
                            if (!Serached.Any())
                            {
                                throw new InvalidOperationException();
                            }
                            else
                            {
                                if (File.Exists(firstOrDefault_Col))
                                {
                                    String col = Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png";
                                    if (File.Exists(col))
                                    {

                                        DDSImage img_1 = new DDSImage(firstOrDefault_Col);
                                        img_1.Save(Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png");
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(col);
                                        bitmap.EndInit();
                                        Diffuse_IMG.Source = bitmap;
                                    }
                                    else
                                    {
                                        DDSImage img_1 = new DDSImage(firstOrDefault_Col);

                                        img_1.Save(col);

                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(col);
                                        bitmap.EndInit();
                                        Diffuse_IMG.Source = bitmap;

                                    }

                                }
                                else
                                {
                                    BitmapImage bitmap = new BitmapImage();
                                    bitmap.BeginInit();
                                    bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                                    bitmap.EndInit();
                                    Diffuse_IMG.Source = bitmap;

                                }


                            }

                            var Serached_ = SearchAccessibleFiles(Skin_Path, "ilm");
                            var firstOrDefault_ilm = Serached_.FirstOrDefault();
                            if (!Serached.Any())
                            {
                                throw new InvalidOperationException();
                            }
                            else
                            {
                                if (File.Exists(firstOrDefault_ilm))
                                {
                                    if (File.Exists(firstOrDefault_ilm + ".png"))
                                    {

                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                        bitmap.EndInit();
                                        Glow_IMG.Source = bitmap;
                                    }
                                    else
                                    {

                                        DDSImage img_2 = new DDSImage(firstOrDefault_ilm);
                                        img_2.Save(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");

                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                        bitmap.EndInit();
                                        Glow_IMG.Source = bitmap;
                                    }
                                }
                                else
                                {
                                    BitmapImage bitmap = new BitmapImage();
                                    bitmap.BeginInit();
                                    bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                                    bitmap.EndInit();
                                    Glow_IMG.Source = bitmap;
                                }

                            }




                        }

                        else
                        {

                            Directory.CreateDirectory(Thumbnail);

                            var Serached = SearchAccessibleFiles(Skin_Path, "col");
                            var firstOrDefault_Col = Serached.FirstOrDefault();
                            if (!Serached.Any())
                            {
                                throw new InvalidOperationException();
                            }
                            else
                            {
                                if (File.Exists(firstOrDefault_Col))
                                {
                                    if (File.Exists(firstOrDefault_Col + ".png"))
                                    {

                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png");
                                        bitmap.EndInit();
                                        Diffuse_IMG.Source = bitmap;
                                    }
                                    else
                                    {
                                        DDSImage img_1 = new DDSImage(firstOrDefault_Col);
                                        img_1.Save(Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png");
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png");
                                        bitmap.EndInit();
                                        Diffuse_IMG.Source = bitmap;

                                    }

                                }
                                else
                                {
                                    BitmapImage bitmap = new BitmapImage();
                                    bitmap.BeginInit();
                                    bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                                    bitmap.EndInit();
                                    Diffuse_IMG.Source = bitmap;

                                }


                            }

                            var Serached_ = SearchAccessibleFiles(Skin_Path, "ilm");
                            var firstOrDefault_ilm = Serached_.FirstOrDefault();
                            if (!Serached.Any())
                            {
                                throw new InvalidOperationException();
                            }
                            else
                            {
                                if (File.Exists(firstOrDefault_ilm))
                                {
                                    if (File.Exists(firstOrDefault_ilm + ".png"))
                                    {

                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                        bitmap.EndInit();
                                        Glow_IMG.Source = bitmap;
                                    }
                                    else
                                    {

                                        DDSImage img_2 = new DDSImage(firstOrDefault_ilm);
                                        img_2.Save(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                        bitmap.EndInit();
                                        Glow_IMG.Source = bitmap;
                                    }
                                }
                                else
                                {
                                    BitmapImage bitmap = new BitmapImage();
                                    bitmap.BeginInit();
                                    bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                                    bitmap.EndInit();
                                    Glow_IMG.Source = bitmap;

                                }

                            }





                        }

                    }
                    catch (Exception ex)
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                        bitmap.EndInit();
                        Diffuse_IMG.Source = bitmap;

                        Glow_IMG.Source = bitmap;
                        Write_To_Log(ErrorManager(ex));
                        Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

                    }

                }


            }

        }
        private void Browse_For_Skin_Click(object sender, RoutedEventArgs e)
        {

            Skin_Install_Tree(false, "");
        }

        private void Browse_Titanfall_2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var folderDlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
                folderDlg.ShowNewFolderButton = true;
                // Show the FolderBrowserDialog.  
                var result = folderDlg.ShowDialog();
                if (result == true)
                {
                    string path = folderDlg.SelectedPath;
                    if (path == null || !Directory.Exists(path))
                    {
                        Send_Error_Notif(GetTextResource("NOTIF_ERROR_INVALID_INSTALL_PATH"));


                    }
                    else
                    {
                        Current_Install_Folder = path;
                        Found_Install_Folder = true;
                        Titanfall2_Directory_TextBox.Background = Brushes.White;

                        Titanfall2_Directory_TextBox.Text = Current_Install_Folder;
                        
                        User_Settings_Vars.NorthstarInstallLocation = Current_Install_Folder;
                        string User_Settings_Json_Strings = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
                        using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
                        {
                            StreamWriter.WriteLine(User_Settings_Json_Strings);
                            StreamWriter.Close();
                        }
                        NSExe = Get_And_Set_Filepaths(Current_Install_Folder, "NorthstarLauncher.exe");
                        Check_Integrity_Of_NSINSTALL();

                    }

                }
            }
            catch (Exception ex)
            {

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_FILE_PATH_ISSUE_REBROWSE"));
                Write_To_Log(ErrorManager(ex));
            }
        }

        private void Install_NS_Click(object sender, RoutedEventArgs e)
        {
            Badge.Visibility = Visibility.Collapsed;

            Install_NS.IsEnabled = false;




            do_not_overwrite_Ns_file = Properties.Settings.Default.Ns_Startup;
            do_not_overwrite_Ns_file_Dedi = Properties.Settings.Default.Ns_Dedi;
            Install_NS_METHOD();


        }

        private void Northstar_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Current_Install_Folder))
            {

                if (File.Exists(NSExe))
                {
                    ProcessStartInfo procStartInfo = new ProcessStartInfo();
                    Process process = new Process();
                    procStartInfo.FileName = NSExe;
                    procStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(NSExe);



                    process.StartInfo = procStartInfo;

                    process.Start();
                    int id = process.Id;
                    pid = id;
                    Process tempProc = Process.GetProcessById(id);
                    WindowState = WindowState.Minimized;
                
                    process.Close();


                }
                else
                {

                    MessageBox.Show("Could Not Find Northstar.exe!");


                }
            }
            else
            {



            }
        }

        private void Enable_Mod_Click(object sender, RoutedEventArgs e)
        {

            Move_List_box_Inactive_To_Active(Disabled_ListBox);
            Move_Mods();
            Call_Mods_From_Folder();
        }

        private void Disable_Mod_Click(object sender, RoutedEventArgs e)
        {

            Move_List_box_Active_To_Inactive(Enabled_ListBox);
            Move_Mods();
            Call_Mods_From_Folder();

        }

        private void Apply_Btn_Click(object sender, RoutedEventArgs e)
        {
            Move_Mods();
            Call_Mods_From_Folder();
        }

        private void Browse_For_Mod_zip_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var File = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();

                // Show the FolderBrowserDialog.  
                var result = File.ShowDialog();
                if (result == true)
                {
                    string path = File.FileName;
                    if (path == null || !File.CheckFileExists)
                    {

                        Send_Error_Notif(GetTextResource("NOTIF_ERROR_INVALID_MOD_ZIP_PATH"));


                    }
                    else
                    {
                        string FolderName = path.Split(Path.DirectorySeparatorChar).Last();
                        Browse_For_MOD.Text = path;
                    
                        Unpack_To_Location_Custom(path, Current_Install_Folder + @"\R2Northstar\mods");
                        Call_Mods_From_Folder();

                        ApplyDataBinding();
                    }

                }
            }
            catch (Exception ex)
            {

                if (ex.Message == "Sequence contains no elements")
                {
                    Send_Info_Notif(GetTextResource("NOTIF_INFO_NO_MODS_FOUND"));

                }
                Send_Error_Notif(GetTextResource("NOTIF_ERROR_FILE_PATH_ISSUE_REBROWSE"));
                Write_To_Log(ErrorManager(ex));
            }
        }
        public static int GetTextureType(string Name)
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
        public static string[] GetTextureName(string name)
        {
            string[] s = new string[3];
            int toname = name.LastIndexOf("\\") + 1;
            string str = name.Substring(toname, name.Length - toname);
            toname = str.IndexOf("_");
            string temp = str.Substring(toname, str.Length - toname);
            s[0] = str;
            s[1] = str.Replace(temp, "");
            s[2] = temp;
            return s;
        }
       public void Install_Skin()
        {

            try
            {
                Skin_Path_Box.Text = "";
                Compat_Indicator.Fill = Brushes.Gray;



                //Block Taken From Skin Tool
                List<string> FileList = new List<string>();
                FindSkinFiles(Skin_Path, FileList, ".dds");



                var matchingvalues = FileList.FirstOrDefault(stringToCheck => stringToCheck.Contains(""));
                // for (int i = 0; i < FileList.Count; i++)
                //   {
                //       if (FileList[i].Contains("col")) // (you use the word "contains". either equals or indexof might be appropriate)
                //       {
                //  //Console.WriteLine(i);
                //      }
                //    }
                int DDSFolderExist = 0;

                DDSFolderExist = FileList.Count;
                if (DDSFolderExist == 0)
                {
                    MessageBox.Show("Could Not Find Skins in Zip??");
                    //   throw new Exception(rm.GetString("FindSkinFailed"));
                }

                foreach (var i in FileList)
                {
                    int FolderLength = Skin_Path.Length;
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

                            Titanfall2_SkinTool.Titanfall2.WeaponData.WeaponDataControl wdc = new Titanfall2_SkinTool.Titanfall2.WeaponData.WeaponDataControl(i, imagecheck);
                            toseek = Convert.ToInt64(wdc.FilePath[0, 1]);
                            tolength = Convert.ToInt32(wdc.FilePath[0, 2]);
                            totype = Convert.ToInt32(wdc.FilePath[0, 3]);


                            break;
                        case 2://Pilot
                            Titanfall2_SkinTool.Titanfall2.PilotData.PilotDataControl pdc = new Titanfall2_SkinTool.Titanfall2.PilotData.PilotDataControl(i, imagecheck);
                            toseek = Convert.ToInt64(pdc.Seek);
                            tolength = Convert.ToInt32(pdc.Length);
                            totype = Convert.ToInt32(pdc.SeekLength);
                            break;
                        case 3://Titan
                            Titanfall2_SkinTool.Titanfall2.TitanData.TitanDataControl tdc = new Titanfall2_SkinTool.Titanfall2.TitanData.TitanDataControl(i, imagecheck);
                            toseek = Convert.ToInt64(tdc.Seek);
                            tolength = Convert.ToInt32(tdc.Length);
                            totype = Convert.ToInt32(tdc.SeekLength);
                            break;

                        default:
                            Write_To_Log("Issue With Skin Install!");

                            Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));
                            break;
                    }
                    

                    StarpakControl sc = new StarpakControl(i, toseek, tolength, totype, Current_Install_Folder, "Titanfall2", imagecheck, "Replace");


                }

                FileList.Clear();
                DirectoryInfo di = new DirectoryInfo(Skin_Path);
                FileInfo[] files = di.GetFiles();
                Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_INSTALLED"));

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                bitmap.EndInit();
                Glow_IMG.Source = bitmap;

                Diffuse_IMG.Source = bitmap;

                foreach (FileInfo file in files)
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir_ in di.GetDirectories())
                {
                    dir_.Delete(true);
                }
                Directory.Delete(Skin_Path);



                //Console.WriteLine("Files deleted successfully");
                GC.Collect();
                Install_Skin_Bttn.IsEnabled = false;



            }
            catch (Exception ef)
            {
                Write_To_Log(ErrorManager(ef));

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));
            }
        

    }
        private void Install_Skin_Bttn_Click(object sender, RoutedEventArgs e)
        {
            Install_Skin();
        }
           /*
        public void getOperatingSystemInfo()
        {
            Write_To_Log("Displaying operating system info....");
            //Create an object of ManagementObjectSearcher class and pass query as parameter.
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            string x = "";
            foreach (ManagementObject managementObject in mos.Get())
            {
                if (managementObject["Caption"] != null)
                {
                    x = x +"\n"+ "Operating System Name  :  " + managementObject["Caption"].ToString();   //Display operating system caption
                }
                if (managementObject["OSArchitecture"] != null)
                {
                    x = x +"\n"+ "Operating System Architecture  :  " + managementObject["OSArchitecture"].ToString();   //Display operating system architecture.
                }
                if (managementObject["CSDVersion"] != null)
                {
                    x = x +"\n"+ "Operating System Service Pack   :  " + managementObject["CSDVersion"].ToString();     //Display operating system version.
                }
            }
            Write_To_Log(x);
        }*/
        public string HKLM_GetString(string path, string key)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(path);
                if (rk == null) return "";
                return (string)rk.GetValue(key);
            }
            catch { return ""; }
        }

        public string FriendlyName()
        {
            string ProductName = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
            string CSDVersion = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");
            if (ProductName != "")
            {
                return (ProductName.StartsWith("Microsoft") ? "" : "Microsoft ") + ProductName +
                            (CSDVersion != "" ? " " + CSDVersion : "");
            }
            return "";
        }
        public async Task getProcessorInfo()
        {
            Write_To_Log("\nDisplaying Processor Name And System Info....");
            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   //This registry entry contains entry for processor info.
            string x = "";

            if (processor_name != null)
            {
                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    x = x + processor_name.GetValue("ProcessorNameString");   //Display processor ingo.
                }
            }

            Write_To_Log(x);
            Write_To_Log(FriendlyName() + "\n");

        }
        void Write_To_Log(string Text, bool clear_First = false)
        {
            Paragraph paragraph = new Paragraph();

            if (clear_First == true)
            {
                LOG_BOX.Document.Blocks.Clear();

                Run run = new Run(Text);
                paragraph.Inlines.Add(run);
                LOG_BOX.Document.Blocks.Add(paragraph);

            }
            else
            {


                Run run = new Run(Text);
                paragraph.Inlines.Add(run);
                LOG_BOX.Document.Blocks.Add(paragraph);
            }
        }
        void save_Log()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            TextRange Log_Box = new TextRange(
        // TextPointer to the start of content in the RichTextBox.
        LOG_BOX.Document.ContentStart,
        // TextPointer to the end of content in the RichTextBox.
        LOG_BOX.Document.ContentEnd
    );
            if (Directory.Exists(DocumentsFolder + @"\VTOL_DATA\Logs"))
            {
                if (File.Exists(DocumentsFolder + @"\VTOL_DATA\Logs\" + date + "-LOG_MODMANAGER V-" + version + ".txt"))
                {
                    string Accurate_Date = DateTime.Now.ToString("yyyy/MM/dd/(HH:mm:ss)");

                    saveAsyncFile("\n--------------------------------------" + "\n\n" + Accurate_Date + "\n\n" + "--------------------------------------\n", @"C:\ProgramData\VTOL_DATA\Logs\" + date + "-LOG_MODMANAGER V-" + version, true, true);
                    saveAsyncFile(Log_Box.Text, DocumentsFolder + @"\VTOL_DATA\Logs\" + date + "-LOG_MODMANAGER V-" + version, true, true);

                    Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_SAVED_TO") + DocumentsFolder + @"\VTOL_DATA\Logs");
                }
                else
                {

                    saveAsyncFile(Log_Box.Text, DocumentsFolder + @"\VTOL_DATA\Logs\" + date + "-LOG_MODMANAGER V-" + version, true, false);
                    Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_SAVED_TO") + DocumentsFolder + @"\VTOL_DATA\Logs");


                }



            }
            else
            {
                Directory.CreateDirectory(DocumentsFolder + @"\VTOL_DATA\Logs");
                saveAsyncFile(Log_Box.Text, DocumentsFolder + @"\VTOL_DATA\Logs\" + date + " -LOG_MODMANAGER" + version, true, false);
                Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_SAVED_TO") + DocumentsFolder + @"\VTOL_DATA\Logs");

            }


        }
        private void Save_LOG_Click(object sender, RoutedEventArgs e)
        {
            save_Log();
        }

        private void Clear_LOG_Click(object sender, RoutedEventArgs e)
        {
            Write_To_Log("", true);
        }


        private async Task Load_Click(object sender, RoutedEventArgs e)
        {




            /*
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Thunderstore_Parse();
            }), DispatcherPriority.Background);

           */







        }

        private void Test_List_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;

        }

        private void Browse_For_MOD_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Dedicated_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Current_Install_Folder))
            {

                if (File.Exists(Current_Install_Folder + @"\ns_startup_args_dedi.txt"))
                {
                    saveAsyncFile(Arg_Box_Dedi.Text, Current_Install_Folder + @"\ns_startup_args_dedi.txt", false, false);


                }
                else
                {
                    //Console.WriteLine("Err, File not found ns_startup_args_dedi");

                }

                if (File.Exists(NSExe))
                {
                    ProcessStartInfo procStartInfo = new ProcessStartInfo();
                    Process process = new Process();
                    procStartInfo.FileName = Current_Install_Folder + @"\r2ds.bat";
                    procStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(NSExe);


                    // procStartInfo.Arguments = "-dedicated -multiple";

                    process.StartInfo = procStartInfo;

                    process.Start();
                    // int id = process.Id;
                    //  pid = id;
                    // Process tempProc = Process.GetProcessById(id);
                    // this.Visible = false;
                    // Thread.Sleep(5000);
                    // tempProc.WaitForExit();
                    // this.Visible = true;

                    // Process process = Process.Start(NSExe, Arg_Box.Text);
                    process.Close();



                }
                else
                {

                    MessageBox.Show("Could Not Find Dedicated bat!");


                }
            }
            else
            {

                MessageBox.Show("Could Not Find Dedicated bat!");


            }

        }


        private void Launch_Northstar_Advanced_Click(object sender, RoutedEventArgs e)
        {
                if (File.Exists(Current_Install_Folder + @"\ns_startup_args.txt"))
                {
                    saveAsyncFile(Arg_Box.Text, Current_Install_Folder + @"\ns_startup_args.txt", false, false);


                }
            else
            {
                //Console.WriteLine("Err, File not found");

            }

            if (Directory.Exists(Current_Install_Folder))
            {

                if (File.Exists(NSExe))
                {
                    ProcessStartInfo procStartInfo = new ProcessStartInfo();
                    Process process = new Process();
                    procStartInfo.FileName = NSExe;
                    procStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(NSExe);
                    ;

                    // procStartInfo.Arguments = args;

                    process.StartInfo = procStartInfo;

                    process.Start();
                    int id = process.Id;
                    pid = id;
                    Process tempProc = Process.GetProcessById(id);
                    // this.Visible = false;
                    // Thread.Sleep(5000);
                    // tempProc.WaitForExit();
                    // this.Visible = true;

                    // Process process = Process.Start(NSExe, Arg_Box.Text);
                    process.Close();


                }
                else
                {

                    MessageBox.Show("Could Not Find Northstar.exe!");


                }
            }
            else
            {

                //Console.WriteLine("Err, File not found");


            }
        }

        private void Ns_Args_Unchecked(object sender, RoutedEventArgs e)
        {
            Write_To_Log("\nOVERWRITE ns_startup_args.txt ENABLED!");
            Properties.Settings.Default.Ns_Startup = false;
            Properties.Settings.Default.Save();
            do_not_overwrite_Ns_file = Properties.Settings.Default.Ns_Dedi;

        }

        private void Ns_Args_Checked(object sender, RoutedEventArgs e)
        {

            Write_To_Log("\nDo not overwrite ns_startup_args.txt ENABLED! - this will backup and restore the original ns_startup_args and from the folder");
            Properties.Settings.Default.Ns_Startup = true;
            Properties.Settings.Default.Save();
            do_not_overwrite_Ns_file = Properties.Settings.Default.Ns_Dedi;

        }

        private void Ns_Args_Dedi_Checked(object sender, RoutedEventArgs e)
        {
            Write_To_Log("\nDo not overwrite ns_startup_args_Dedi.txt ENABLED! - this will backup and restore the original ns_startup_args_dedi from the folder");
            Properties.Settings.Default.Ns_Dedi = true;
            Properties.Settings.Default.Save();
            do_not_overwrite_Ns_file_Dedi = Properties.Settings.Default.Ns_Dedi;

        }

        private void Ns_Args_Dedi_Unchecked(object sender, RoutedEventArgs e)
        {
            Write_To_Log("\nOVERWRITE ns_startup_args_Dedi.txt ENABLED!");
            Properties.Settings.Default.Ns_Dedi = false;
            Properties.Settings.Default.Save();
            do_not_overwrite_Ns_file_Dedi = Properties.Settings.Default.Ns_Dedi;
        }



        private void Check_For_Updates_Click(object sender, RoutedEventArgs e)
        {

            if (File.Exists(updaterModulePath))
            {
                //   StartSilent();
                Process process = Process.Start(updaterModulePath, "/checknow ");
                // process.Close();
            }
            else
            {
                Send_Error_Notif(GetTextResource("NOTIF_ERROR_UPDATER_NOT_FOUND"));

            }
        }

        private void Config_Updates_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(updaterModulePath))
            {
                try
                {


                    Process[] processes = Process.GetProcessesByName(updaterModulePath);
                    if (processes.Length > 0)
                        processes[0].CloseMainWindow();
                }
                catch (Exception ex)
                {
                    Write_To_Log(ErrorManager(ex));

                }
                Process process = Process.Start(updaterModulePath, "/configure");
                process.Close();

            }
            else
            {
                Send_Error_Notif(GetTextResource("NOTIF_ERROR_UPDATER_NOT_FOUND"));

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


        }

        private void OnKeyDownHandler_Dedi_Arg(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    if (File.Exists(Current_Install_Folder + @"\ns_startup_args_dedi.txt"))
                    {
                        saveAsyncFile(Arg_Box_Dedi.Text, Current_Install_Folder + @"\ns_startup_args_dedi.txt", false, false);

                        Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_SAVED_TO_DASH_NS_ARGS_DEDI"));
                    }
                    else
                    {
                        Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_AUTO_SAVE_FAILED"));

                        Write_To_Log("File Location Not Found!");

                    }
                }
            }
            catch (Exception ex)
            {

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_AUTO_SAVE_FAILED"));
                Write_To_Log(ErrorManager(ex));
            }
        }

        private void OnKeyDownHandler_Nrml_Arg(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {

                    if (File.Exists(Current_Install_Folder + @"\ns_startup_args.txt"))
                    {
                        saveAsyncFile(Arg_Box.Text, Current_Install_Folder + @"\ns_startup_args.txt", false, false);
                        Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_SAVED_TO_DASH_NS_ARGS"));


                    }
                    else
                    {
                        Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_AUTO_SAVE_FAILED"));
                        Write_To_Log("File Location Not Found!");

                    }
                }
            }
            catch (Exception ex)
            {

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_AUTO_SAVE_FAILED"));
                Write_To_Log(ErrorManager(ex));
            }
        }
        private void Validate_Combo_Box(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Started_Selection == true)
                {

                    if (sender.GetType() == typeof(HandyControl.Controls.CheckComboBox))
                    {
                        HandyControl.Controls.CheckComboBox comboBox = (HandyControl.Controls.CheckComboBox)sender;
                        var Var = ((HandyControl.Controls.CheckComboBox)sender).Tag.ToString();
                        var Description = ((HandyControl.Controls.CheckComboBox)sender).ToolTip.ToString();

                        string[] Split = Var.Split("|");
                        string type = Split[0];
                        string name = Split[1];
                        string ARG = Split[2];
                        if (ARG != null && ARG != "" && ARG == "CONVAR")
                        {



                            string list = String.Join(",", comboBox.SelectedItems.Cast<String>().ToArray());
                            Write_convar_To_File(name, list, Description, true, Convar_File);
                            if (list == "" || list == null)
                            {

                                Send_Error_Notif(GetTextResource("NOTIF_ERROR_NO_SELECTION"));
                                Write_convar_To_File(name, "REMOVE", Description, true, Convar_File);

                            }
                            comboBox.Foreground = Brushes.White;


                        }

                        else if (ARG != null && ARG != "" && ARG == "STARTUP")

                        {
                            string list = String.Join(" ", comboBox.SelectedItems.Cast<String>().ToArray());

                            Write_Startup_Arg_To_File(name, list, false, true, Ns_dedi_File);
                            comboBox.Foreground = Brushes.White;

                        }
                        else
                        {
                            Send_Error_Notif(GetTextResource("NOTIF_ERROR_CANNOT_READ_INPUT"));
                        }
                    }
                    else
                    {

                        if (sender.GetType() == typeof(ComboBox))
                        {
                            ComboBox comboBox = (ComboBox)sender;
                            var Var = comboBox.Tag.ToString();
                            var Description = ((ComboBox)sender).ToolTip.ToString();

                            string[] Split = Var.Split("|");
                            string type = Split[0];
                            string name = Split[1];
                            string ARG = Split[2];
                            if (ARG == "CONVAR")
                            {

                                if (type == "BOOL")
                                {
                                    if (comboBox.SelectedIndex != -1)
                                    {
                                        if (comboBox.SelectedIndex == 1)
                                        {
                                            Write_convar_To_File(name, "1", Description, false, Convar_File);
                                            comboBox.Foreground = Brushes.White;


                                        }
                                        else
                                        {


                                            Write_convar_To_File(name, "0", Description, false, Convar_File);
                                            comboBox.Foreground = Brushes.White;

                                        }
                                    }

                                }
                                if (type == "ONE_SELECT")
                                {
                                    if (comboBox.SelectedIndex != -1)
                                    {

                                        Write_convar_To_File(name, comboBox.SelectedValue.ToString(), Description, false, Convar_File);
                                        comboBox.Foreground = Brushes.White;
                                    }


                                }



                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Error_Notif(GetTextResource("NOTIF_ERROR_FATAL"));


            }
        }

        private void ValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            try
            {
                var Var = ((TextBox)sender).Tag.ToString();
                string[] Split = Var.Split("|");
                string type = Split[0];
                Regex regex = new Regex("[^0-9]+");

                switch (type)
                {

                    case "STRING":
                        // Send_Success_Notif("Found type String!");
                        break;
                    case "PORT":
                        e.Handled = regex.IsMatch(e.Text);


                        break;
                    case "INT":
                        e.Handled = regex.IsMatch(e.Text);
                        break;
                    case "FLOAT":
                        Regex Floaty = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$ or ^[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?$");
                        e.Handled = Floaty.IsMatch(e.Text);
                        break;
                }
            }
            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));


            }
        }
        public bool IsPort(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return false;

                Regex numeric = new Regex(@"^[0-9]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (numeric.IsMatch(value))
                {
                    try
                    {
                        if (Convert.ToInt32(value) < 65535)
                            return true;
                    }
                    catch (OverflowException)
                    {
                    }
                }
            }
            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));


            }
            return false;

        }
        void validate(object sender, RoutedEventArgs e)
        {





        }
        public IEnumerable<string> GetFile(string directory, string Search)
        {
            List<string> files = new List<string>();

            try
            {
                if (Directory.Exists(directory))
                {
                    files.AddRange(Directory.GetFiles(directory, Search, SearchOption.AllDirectories));
                    if (files.Count >= 1)
                    {
                        return files;

                    }
                    else
                    {
                        return null;
                    }
                }


            }
            catch (Exception ex)
            {
                Write_To_Log(ErrorManager(ex));
            }
            return null;

        }
        void Write_convar_To_File(string Convar_Name, string Convar_Value, string Description, bool add_quotation = false, string File_Root = null)
        {
            try
            {

                // string val = Convar_Name.Trim(new Char[] { '-', '+' });


                Convar_Value = Convar_Value.Trim();
                string RootFolder = "";
                if (File_Root != null)
                {
                    if (File.Exists(File_Root))
                    {
                        RootFolder = File_Root;
                    }
                    else
                    {
                        Send_Warning_Notif(GetTextResource("NOTIF_WARN_CANNOT_SET_PATH"));
                        RootFolder = GetFile(Current_Install_Folder, "autoexec_ns_server.cfg").First();

                    }
                }
                else
                {


                    RootFolder = GetFile(Current_Install_Folder, "autoexec_ns_server.cfg").First();
                }
                string[] intake = File.ReadAllLines(RootFolder);

                string[] intermid = intake;



                if (Array.Exists(intermid, element => element.StartsWith(Convar_Name)))
                {


                    int index_of_var = Array.FindIndex(intermid, element => element.StartsWith(Convar_Name));
                    if (add_quotation == true)
                    {
                        var desc = intermid[index_of_var];
                        desc = desc.Substring(desc.LastIndexOf('/') + 1);
                        if (desc != null && desc != "")
                        {
                            intermid[index_of_var] = Convar_Name + " " + '\u0022' + Convar_Value + '\u0022' + " " + "//" + desc;


                        }
                        else
                        {

                            intermid[index_of_var] = Convar_Name + " " + '\u0022' + Convar_Value + '\u0022' + " " + "//" + desc;

                        }

                    }
                    else
                    {

                        var desc = intermid[index_of_var];
                        desc = desc.Substring(desc.LastIndexOf('/') + 1);
                        intermid[index_of_var] = Convar_Name + " " + Convar_Value + " " + "//" + desc;
                        if (desc != null && desc != "")
                        {
                            intermid[index_of_var] = Convar_Name + " " + Convar_Value + " " + "//" + desc;


                        }
                        else
                        {

                            intermid[index_of_var] = Convar_Name + " " + Convar_Value + " " + "//" + Description;

                        }
                    }
                    if (Convar_Value == "REMOVE")
                    {
                        intermid = intermid.Where((source, index) => index != index_of_var).ToArray();

                    }


                    String x = String.Join("\r\n", intermid.ToArray());
                    //Send_Warning_Notif(x);
                    // x = x.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
                    //ClearFile(Convar_File);
                    using (StreamWriter sw = new StreamWriter(Convar_File, false, Encoding.UTF8, 65536))
                    {

                        sw.WriteLine(x);
                    }
                    Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_GROUP_CVAR_THE_VARIABLE") + Convar_Name + GetTextResource("NOTIF_SUCCESS_GROUP_CVAR_HAS_BEEN_FOUND_VALUE") + Convar_Value + "]");


                }
                else
                {


                    string[] intake_ = File.ReadAllLines(Convar_File);

                    string[] intermid_ = intake_;

                    intermid_ = AddElementToArray(intermid_, Convar_Name + " " + Convar_Value + " " + "//" + Description);

                    String x = String.Join("\r\n", intermid_.ToArray());

                    using (StreamWriter sw = new StreamWriter(Convar_File, false, Encoding.UTF8, 65536))
                    {
                        sw.WriteLine(x);
                    }
                    Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_GROUP_CVAR_THE_VARIABLE") + Convar_Name + GetTextResource("NOTIF_SUCCESS_GROUP_CVAR_NOT_FOUND_VALUE") + Convar_Value + "]");

                }
            }
            catch (Exception ex)
            {
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_USING_SERVER_SETUP_SYS"));
                Write_To_Log(ErrorManager(ex));


            }
        }
        private T[] AddElementToArray<T>(T[] array, T element)
        {

            T[] newArray = new T[array.Length + 1];
            int i;
            for (i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i];
            }
            newArray[i] = element;
            return newArray;
        }
        private void ClearFile(string path)
        {
            if (!File.Exists(path))
                File.Create(path);

            StreamWriter tw = new StreamWriter(path, false);
            tw.Flush();

            tw.Close();
        }
        string Read_Convar_args(string Convar_Name, string File_Root = null)
        {
            try
            {

                string RootFolder = "";
                if (File_Root != null)
                {
                    if (File.Exists(File_Root))
                    {
                        RootFolder = File_Root;
                    }
                    else
                    {
                        Send_Warning_Notif(GetTextResource("NOTIF_WARN_CANNOT_SET_PATH"));

                    }

                }
                else
                {


                    RootFolder = GetFile(Current_Install_Folder, "autoexec_ns_server.cfg").First();
                }
                string[] intake = File.ReadAllLines(RootFolder);


                for (int i = 0; i < intake.Length; i++)
                {
                    //Console.WriteLine(String.Format(" array[{0}] = {1}", i, intake[i]));
                }
                //Console.WriteLine("\n\n\n");
                //   Send_Warning_Notif(intake[Array.FindIndex(intake, element => element.Contains(Convar_Name))].ToString());

                if (Array.Exists(intake, element => element.StartsWith(Convar_Name)))
                {


                    int index_of_var = Array.FindIndex(intake, element => element.StartsWith(Convar_Name));

                    return intake[index_of_var].ToString();


                }
                else
                {
                    //  Send_Error_Notif("CONVAR NOT FOUND-"+ Convar_Name);

                    return null;
                }
            }
            catch (Exception ex)
            {
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_USING_SERVER_SETUP_SYS"));
                Write_To_Log(ErrorManager(ex));


            }
            return null;
        }
        string Read_Startup_args(string Convar_Name, string File_Root = null)
        {
            try
            {
                var pattern = @"(?=[+-])";

                string RootFolder = "";
                if (File_Root != null)
                {
                    if (File.Exists(File_Root))
                    {
                        RootFolder = File_Root;
                    }
                    else
                    {
                        Send_Warning_Notif(GetTextResource("NOTIF_WARN_CANNOT_SET_PATH"));
                        RootFolder = GetFile(Current_Install_Folder, "ns_startup_args_dedi.txt").First();

                    }
                }
                else
                {


                    RootFolder = GetFile(Current_Install_Folder, "ns_startup_args_dedi.txt").First();
                }
                string[] intake = File.ReadAllLines(Ns_dedi_File);

                string[] intermid = null;

                foreach (string line in intake)
                {
                    intermid = Regex.Split(line.Trim(' '), pattern);

                }

                if (Array.Exists(intermid, element => element.StartsWith(Convar_Name)))
                {


                    int index_of_var = Array.FindIndex(intermid, element => element.StartsWith(Convar_Name));
                    return intermid[index_of_var].ToString();


                }
                else
                {
                    // Send_Error_Notif("Did Not Find-"+ Convar_Name);

                    return null;
                }
            }
            catch (Exception ex)
            {
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_USING_SERVER_SETUP_SYS"));
                Write_To_Log(ErrorManager(ex));


            }
            return null;
        }

        void Write_Startup_Arg_To_File(string var_Name, string var_Value, bool add_quotation = false, bool Kill_If_empty = false, string File_Root = null)
        {

            try
            {

                string val = var_Name.Trim(new Char[] { '-', '+' });
                var pattern = @"(?=[+-])";
                var_Value = var_Value.Trim();
                string RootFolder = "";
                if (File_Root != null)
                {
                    if (File.Exists(File_Root))
                    {
                        RootFolder = File_Root;
                    }
                    else
                    {
                        Send_Warning_Notif(GetTextResource("NOTIF_WARN_CANNOT_SET_PATH"));
                        RootFolder = GetFile(Current_Install_Folder, "ns_startup_args_dedi.txt").First();

                    }
                }
                else
                {


                    RootFolder = GetFile(Current_Install_Folder, "ns_startup_args_dedi.txt").First();
                }
                string[] intake = File.ReadAllLines(RootFolder);

                string[] intermid = null;

                foreach (string line in intake)
                {
                    intermid = Regex.Split(line.Trim(' '), pattern);


                }
                for (int j = 0; j < intermid.Length; j++)
                {
                    // //Console.WriteLine("array[{0}] = {1}", j, intermid[j]);

                }
                if (Array.Exists(intermid, element => element.StartsWith(var_Name)))
                {


                    int index_of_var = Array.FindIndex(intermid, element => element.StartsWith(var_Name));
                    if (add_quotation == true)
                    {
                        intermid[index_of_var] = var_Name + " " + '\u0022' + var_Value + '\u0022';


                    }
                    else
                    {
                        intermid[index_of_var] = var_Name + " " + var_Value;

                    }

                    if (Kill_If_empty == true)
                    {
                        if (var_Value == "" || var_Value == null)
                        {
                            intermid = intermid.Where((source, index) => index != index_of_var).ToArray();
                        }

                    }

                    String x = String.Join(" ", intermid.ToArray());
                    //  ClearFile(RootFolder +@"\" + "ns_startup_args_dedi.txt");

                    using (StreamWriter sw = new StreamWriter(RootFolder, false, Encoding.UTF8, 65536))
                    {
                        sw.WriteLine(Regex.Replace(x, @"\s+", " ").Replace("+ ", "+"));
                    }
                    Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_GROUP_VAR_THE_VARIABLE") + var_Name + GetTextResource("NOTIF_SUCCESS_GROUP_VAR_HAS_BEEN_FOUND_VALUE") + var_Value);


                }
                else
                {

                    string[] intake_ = File.ReadAllLines(Ns_dedi_File);

                    string[] intermid_ = null;
                    foreach (string line in intake_)
                    {
                        //  intermid_ = line.Split('+');
                        intermid_ = Regex.Split(line, pattern);

                    }

                    intermid_ = AddElementToArray(intermid_, var_Name + " " + var_Value);


                    String x = String.Join(" ", intermid_.ToArray());
                    //x.Replace(System.Environment.NewLine, "replacement text");
                    //  File.WriteAllText(RootFolder, String.Empty);
                    // ClearFile(RootFolder +@"\" + "ns_startup_args_dedi.txt");
                    using (StreamWriter sw = new StreamWriter(RootFolder, false, Encoding.UTF8, 65536))
                    {
                        sw.WriteLine(Regex.Replace(x, @"\s+", " "));
                    }
                    // File.WriteAllText(GetFile(RootFolder, "ns_startup_args_dedi.txt").First(), x);
                    Send_Warning_Notif(GetTextResource("NOTIF_WARN_GROUP_VAR_NOT_FOUND_INFILE") + var_Name + GetTextResource("NOTIF_WARN_GROUP_VAR_SAVED_VALUE") + var_Value + "]");

                }
            }
            catch (Exception ex)
            {
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_USING_SERVER_SETUP_SYS"));
                Write_To_Log(ErrorManager(ex));


            }

        }

        int cntr = 0;
        void Clear_Box(object sender, KeyEventArgs e)
        {
            cntr++;



            TextBox Text_Box = (TextBox)sender;
            if (cntr < 2)
            {
                string Reg2;
                Reg2 = Text_Box.Text;
                if (Text_Box.Text == Reg2)
                {
                    Text_Box.Text = "";


                }

            }

        }
        void Save_On_Focus_(object sender, KeyEventArgs e)
        {

            try
            {
                if (sender.GetType() == typeof(TextBox))
                {





                    var val = ((TextBox)sender).Text.ToString();
                    var Tag = ((TextBox)sender).Tag.ToString();
                    var Description = ((TextBox)sender).ToolTip.ToString();

                    TextBox Text_Box = (TextBox)sender;
                    string[] Split = Tag.Split("|");
                    string type = Split[0];
                    string name = Split[1];
                    string ARG = Split[2];


                    if (val != null)
                    {

                        switch (type)
                        {

                            case "STRING":
                                if (e.Key == Key.Return)
                                {
                                    if (ARG != null && ARG != "" && ARG == "CONVAR")
                                    {
                                        Write_convar_To_File(name, val, Description, true, Convar_File);
                                        GC.Collect();
                                        Text_Box.Foreground = Brushes.White;

                                    }
                                    else
                                    {
                                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                                        Write_Startup_Arg_To_File(name, val, false, true, Ns_dedi_File);

                                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                                        Text_Box.Foreground = Brushes.White;


                                    }



                                }
                                break;
                            case "STRINGQ":
                                if (e.Key == Key.Return)
                                {
                                    if (ARG != null && ARG != "" && ARG == "CONVAR")
                                    {
                                        Write_convar_To_File(name, val, Description, true, Convar_File);
                                        Text_Box.Foreground = Brushes.White;

                                        GC.Collect();

                                    }
                                    else
                                    {
                                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                                        Write_Startup_Arg_To_File(name, val, false, true, Ns_dedi_File);
                                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                                        Text_Box.Foreground = Brushes.White;



                                    }



                                }
                                break;
                            case "INT":
                                if (e.Key == Key.Return)
                                {
                                    if (ARG != null && ARG != "" && ARG == "CONVAR")
                                    {
                                        Write_convar_To_File(name, val, Description, false, Convar_File);
                                        Text_Box.Foreground = Brushes.White;

                                    }
                                    else
                                    {
                                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                                        Write_Startup_Arg_To_File(name, val, false, true, Ns_dedi_File);
                                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                                        Text_Box.Foreground = Brushes.White;



                                    }



                                }
                                break;
                            case "FLOAT":
                                if (e.Key == Key.Return)
                                {
                                    if (ARG != null && ARG != "" && ARG == "CONVAR")
                                    {
                                        Write_convar_To_File(name, val, Description, false, Convar_File);
                                        Text_Box.Foreground = Brushes.White;

                                    }
                                    else
                                    {
                                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                                        Write_Startup_Arg_To_File(name, val, true, true, Ns_dedi_File);
                                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                                        GC.Collect();
                                        Text_Box.Foreground = Brushes.White;


                                    }



                                }
                                break;
                            case "PORT":
                                if (ARG != null && ARG != "" && ARG == "CONVAR")
                                {
                                    if (val.Count() > 5)
                                    {
                                        Send_Warning_Notif(GetTextResource("NOTIF_WARN_PORT_TOO_lONG"));
                                        Text_Box.Background = Brushes.Red;
                                    }
                                    else
                                    {
                                        Text_Box.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF4C4C4C");

                                        if (e.Key == Key.Return)
                                        {

                                            if (IsPort(val) == true && val.Count() < 6)
                                            {
                                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                                                Text_Box.Foreground = Brushes.White;
                                                Write_convar_To_File(name, val, Description, false, Convar_File);

                                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                                                Text_Box.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF4C4C4C");


                                            }
                                            else
                                            {
                                                if (val == null || val == "")
                                                {

                                                    Write_convar_To_File(name, val, Description, false, Convar_File);

                                                }
                                                else
                                                {
                                                    Send_Warning_Notif(GetTextResource("NOTIF_WARN_ERROR_AT") + name + "]");
                                                    Text_Box.Background = Brushes.Red;
                                                    Text_Box.Text = null;

                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (val.Count() > 5)
                                    {
                                        Send_Warning_Notif(GetTextResource("NOTIF_WARN_PORT_TOO_lONG"));
                                        Text_Box.Background = Brushes.Red;
                                    }
                                    else
                                    {
                                        Text_Box.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF4C4C4C");

                                        if (e.Key == Key.Return)
                                        {

                                            if (IsPort(val) == true && val.Count() < 6)
                                            {
                                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                                                Write_Startup_Arg_To_File(name, val, false, true, Ns_dedi_File);
                                                Text_Box.Foreground = Brushes.White;

                                                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                                                Text_Box.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF4C4C4C");


                                            }
                                            else
                                            {
                                                if (val == null || val == "")
                                                {

                                                    Send_Warning_Notif(GetTextResource("NOTIF_WARN_GROUP_CVAR_EMTPY_VALUE") + name + GetTextResource("NOTIF_WARN_GROUP_CVAR_REMOVED"));
                                                    Write_Startup_Arg_To_File(name, val, false, true, Ns_dedi_File);

                                                }
                                                else
                                                {
                                                    Send_Warning_Notif(GetTextResource("NOTIF_WARN_GROUP_CVAR_ERROR_AT") + name + "]");
                                                    Text_Box.Background = Brushes.Red;
                                                    Text_Box.Text = null;

                                                }

                                            }
                                        }
                                    }
                                }
                                break;

                        }





                    }
                }

            }
            catch (Exception ex)
            {
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_WRITING_INPUT_FAILED"));
                Write_To_Log(ErrorManager(ex));

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {


        }
        public class Arg_Set
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Default { get; set; }
            public string Tag { get; set; }
            public string regex { get; set; }
            public string ARG { get; set; }

            public string Description { get; set; }
            public string[] List { get; set; }

        }
        bool Verify_List(List<string> Good_Words, string Input)
        {
            return Good_Words.Contains(Input);

        }
        private ArrayList Load_Args()
        {

            ArrayList Arg_List = new ArrayList();

            using (StreamReader r = new StreamReader(@"D:\Development Northstar AmVCX C++ branch 19023 ID 44\VTOL\Resources\Test_Args_1.json"))
            {
                string json = r.ReadToEnd();
                // Send_Success_Notif(json);
                Server_Json = Server_Setup.FromJson(json);
            }

            foreach (var items in Server_Json.Startup_Arguments)
            {
                if (items.Type == "GAME_MODE")
                {
                    foreach (var item in items.List)
                    {

                        Game_Modes_List.Add(item);
                    }

                }

                Arg_List.Add(new Arg_Set
                {
                    Name = items.Name,
                    Type = items.Type,
                    Default = items.Default,
                    Description = items.Description_Tooltip,
                    List = items.List,
                    Tag = items.Type + "|" + items.Name + "|" + items.ARG,



                });
                DataContext = this;

            }


            return Arg_List;

        }

        private ArrayList Convar_Args()
        {

            ArrayList Arg_List = new ArrayList();

            using (StreamReader r = new StreamReader(@"D:\Development Northstar AmVCX C++ branch 19023 ID 44\VTOL\Resources\Test_Args_1.json"))
            {
                string json = r.ReadToEnd();
                // Send_Success_Notif(json);
                Server_Json = Server_Setup.FromJson(json);
            }

            foreach (var items in Server_Json.Convar_Arguments)
            {
                if (items.Type == "GAME_MODE")
                {
                    foreach (var item in items.List)
                    {

                        Game_Modes_List.Add(item);
                    }

                }

                Arg_List.Add(new Arg_Set
                {
                    Name = items.Name,
                    Type = items.Type,
                    Default = items.Default,
                    Description = items.Description_Tooltip,
                    List = items.List,
                    Tag = items.Type + "|" + items.Name + "|" + items.ARG,



                });
                DataContext = this;

            }
            return Arg_List;

        }

        private void Disabled_ListBox_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //            Send_Error_Notif("right click");

        }

        private void Mod_Panel_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off 
                try
                {

                    foreach (string file in files)
                    {
                        string path = file;
                        if (path == null || !File.Exists(file))
                        {

                            Send_Error_Notif(GetTextResource("NOTIF_ERROR_INVALID_MOD_ZIP_PATH"));


                        }
                        else
                        {
                            string FolderName = path.Split(Path.DirectorySeparatorChar).Last();
                            Browse_For_MOD.Text = path;
                            ////Console.WriteLine(path);
                            //     //Console.WriteLine("The Folder Name is-" + FolderName + "\n\n");
                            Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_RECEIVED_DASH") + file);

                            Unpack_To_Location_Custom(path, Current_Install_Folder + @"\R2Northstar\mods");
                            Call_Mods_From_Folder();

                            ApplyDataBinding();
                        }
                    }

                }
                catch (Exception ex)
                {
                    Drag_Drop_Overlay_Skins.Visibility = Visibility.Hidden;

                    Send_Error_Notif(GetTextResource("NOTIF_ERROR_FILE_PATH_ISSUE_REBROWSE"));
                    Write_To_Log(ErrorManager(ex));
                }
            }
            Drag_Drop_Overlay.Visibility = Visibility.Hidden;

        }

        private void Mod_Panel_DragOver(object sender, DragEventArgs e)
        {

        }

        private void Mod_Panel_DragLeave(object sender, DragEventArgs e)
        {
            Drag_Drop_Overlay.Visibility = Visibility.Hidden;

        }

        private void Mod_Panel_DragEnter(object sender, DragEventArgs e)
        {
            Drag_Drop_Overlay.Visibility = Visibility.Visible;
        }

        private void skins_Panel_Drop(object sender, DragEventArgs e)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (Directory.Exists(Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR"))
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                    bitmap.EndInit();
                    Diffuse_IMG.Source = bitmap;


                    Glow_IMG.Source = bitmap;

                    Directory.Delete(Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR", true);
                    GC.Collect();
                }
                catch (Exception ef)
                {
                    Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

                    Write_To_Log(ErrorManager(ef));
                }




            }
            if (Directory.Exists(Current_Install_Folder + @"\Thumbnails"))
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                    bitmap.EndInit();
                    Diffuse_IMG.Source = bitmap;


                    Glow_IMG.Source = bitmap;

                    Directory.Delete(Current_Install_Folder + @"\Thumbnails", true);
                    GC.Collect();
                }
                catch (Exception ef)
                {
                    Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

                    Write_To_Log(ErrorManager(ef));
                }

            }
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {

                    // Note that you can have more than one file.
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);


                    Skin_Temp_Loc = files[0];
                    if (Skin_Temp_Loc == null || !File.Exists(Skin_Temp_Loc))
                    {

                        Send_Error_Notif(GetTextResource("NOTIF_ERROR_INVALID_MOD_ZIP_PATH"));
                        return;

                    }
                    else
                    {
                        Skin_Path_Box.Text = Skin_Temp_Loc;
                        // Send_Success_Notif("\nSkin Found!");
                        if (ZipHasFile(".dds", Skin_Temp_Loc))
                        {
                            Send_Success_Notif(GetTextResource("NOTIF_SUCCESS_COMPATIBLE_SKIN_FOUND"));
                            Compat_Indicator.Fill = Brushes.LimeGreen;
                            Install_Skin_Bttn.IsEnabled = true;
                            //   var directory = new DirectoryInfo(root);
                            // var myFile = (from f in directory.GetFiles()orderby f.LastWriteTime descending select f).First();
                            if (Directory.Exists(Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR"))
                            {
                                Skin_Path = Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR";
                                ZipFile.ExtractToDirectory(Skin_Temp_Loc, Skin_Path, Encoding.GetEncoding("GBK"), true);

                            }
                            else
                            {

                                Directory.CreateDirectory(Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR");
                                Skin_Path = Current_Install_Folder + @"\Skins_Unpack_Mod_MNGR";

                                ZipFile.ExtractToDirectory(Skin_Temp_Loc, Skin_Path, Encoding.GetEncoding("GBK"));

                            }
                        }
                        else
                        {
                            Send_Error_Notif(GetTextResource("NOTIF_ERROR_SKIN_INCOMPATIBLE"));
                            Compat_Indicator.Fill = Brushes.Red;
                            Install_Skin_Bttn.IsEnabled = false;

                        }


                        //Console.WriteLine(Skin_Temp_Loc);
                        String Thumbnail = Current_Install_Folder + @"\Thumbnails\";
                        if (Directory.Exists(Thumbnail))
                        {
                            //DirectoryInfo dir = new DirectoryInfo(Thumbnail);
                            var Serached = SearchAccessibleFiles(Skin_Path, "col");
                            var firstOrDefault_Col = Serached.FirstOrDefault();
                            if (!Serached.Any())
                            {
                                throw new InvalidOperationException();
                            }
                            else
                            {
                                if (File.Exists(firstOrDefault_Col))
                                {
                                    String col = Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png";
                                    //Console.WriteLine(firstOrDefault_Col);
                                    if (File.Exists(col))
                                    {

                                        DDSImage img_1 = new DDSImage(firstOrDefault_Col);
                                        img_1.Save(Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png");
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(col);
                                        bitmap.EndInit();
                                        Diffuse_IMG.Source = bitmap;
                                    }
                                    else
                                    {
                                        //Console.WriteLine(col);
                                        DDSImage img_1 = new DDSImage(firstOrDefault_Col);

                                        img_1.Save(col);

                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(col);
                                        bitmap.EndInit();
                                        Diffuse_IMG.Source = bitmap;

                                    }

                                }
                                else
                                {
                                    BitmapImage bitmap = new BitmapImage();
                                    bitmap.BeginInit();
                                    bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                                    bitmap.EndInit();
                                    Diffuse_IMG.Source = bitmap;

                                }


                            }

                            var Serached_ = SearchAccessibleFiles(Skin_Path, "ilm");
                            var firstOrDefault_ilm = Serached_.FirstOrDefault();
                            if (!Serached.Any())
                            {
                                throw new InvalidOperationException();
                            }
                            else
                            {
                                if (File.Exists(firstOrDefault_ilm))
                                {
                                    if (File.Exists(firstOrDefault_ilm + ".png"))
                                    {

                                        //Console.WriteLine(firstOrDefault_ilm);
                                        // Image Image_2 = new Bitmap(Thumbnail+Path.GetFileName(firstOrDefault_ilm)+".png");
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                        bitmap.EndInit();
                                        Glow_IMG.Source = bitmap;
                                    }
                                    else
                                    {

                                        DDSImage img_2 = new DDSImage(firstOrDefault_ilm);
                                        img_2.Save(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");

                                        //Image Image_2 = new Bitmap(Thumbnail+Path.GetFileName(firstOrDefault_ilm)+".png");
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                        bitmap.EndInit();
                                        Glow_IMG.Source = bitmap;
                                    }
                                }
                                else
                                {
                                    // Image Image_1 = new Bitmap(Directory.GetCurrentDirectory()+@"\No_Texture.jpg");
                                    BitmapImage bitmap = new BitmapImage();
                                    bitmap.BeginInit();
                                    bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                                    bitmap.EndInit();
                                    Glow_IMG.Source = bitmap;
                                }

                            }




                        }

                        else
                        {

                            Directory.CreateDirectory(Thumbnail);

                            //DirectoryInfo dir = new DirectoryInfo(Thumbnail);
                            var Serached = SearchAccessibleFiles(Skin_Path, "col");
                            var firstOrDefault_Col = Serached.FirstOrDefault();
                            if (!Serached.Any())
                            {
                                throw new InvalidOperationException();
                            }
                            else
                            {
                                if (File.Exists(firstOrDefault_Col))
                                {
                                    //Console.WriteLine(firstOrDefault_Col);
                                    if (File.Exists(firstOrDefault_Col + ".png"))
                                    {

                                        //   Image Image_1 = new Bitmap(Thumbnail+Path.GetFileName(firstOrDefault_Col)+".png");
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png");
                                        bitmap.EndInit();
                                        Diffuse_IMG.Source = bitmap;
                                    }
                                    else
                                    {
                                        DDSImage img_1 = new DDSImage(firstOrDefault_Col);
                                        img_1.Save(Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png");
                                        // Image Image_1 = new Bitmap(Thumbnail+Path.GetFileName(firstOrDefault_Col)+".png");
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_Col) + ".png");
                                        bitmap.EndInit();
                                        Diffuse_IMG.Source = bitmap;

                                    }

                                }
                                else
                                {
                                    // Image Image_1 = new Bitmap(Directory.GetCurrentDirectory()+@"\No_Texture.jpg");
                                    BitmapImage bitmap = new BitmapImage();
                                    bitmap.BeginInit();
                                    bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                                    bitmap.EndInit();
                                    Diffuse_IMG.Source = bitmap;

                                }


                            }

                            var Serached_ = SearchAccessibleFiles(Skin_Path, "ilm");
                            var firstOrDefault_ilm = Serached_.FirstOrDefault();
                            if (!Serached.Any())
                            {
                                throw new InvalidOperationException();
                            }
                            else
                            {
                                if (File.Exists(firstOrDefault_ilm))
                                {
                                    if (File.Exists(firstOrDefault_ilm + ".png"))
                                    {

                                        //Console.WriteLine(firstOrDefault_ilm);
                                        //    Image Image_2 = new Bitmap(Thumbnail+Path.GetFileName(firstOrDefault_ilm)+".png");
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                        bitmap.EndInit();
                                        Glow_IMG.Source = bitmap;
                                    }
                                    else
                                    {

                                        DDSImage img_2 = new DDSImage(firstOrDefault_ilm);
                                        img_2.Save(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                        // Image Image_2 = new Bitmap(Thumbnail+Path.GetFileName(firstOrDefault_ilm)+".png");
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.UriSource = new Uri(Thumbnail + Path.GetFileName(firstOrDefault_ilm) + ".png");
                                        bitmap.EndInit();
                                        Glow_IMG.Source = bitmap;
                                    }
                                }
                                else
                                {
                                    //  Image Image_1 = new Bitmap(Directory.GetCurrentDirectory()+@"\No_Texture.jpg");
                                    BitmapImage bitmap = new BitmapImage();
                                    bitmap.BeginInit();
                                    bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                                    bitmap.EndInit();
                                    Glow_IMG.Source = bitmap;

                                }

                            }





                        }


                        //   Import_Skin_Bttn.Enabled=false;

                    }


                    Drag_Drop_Overlay_Skins.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                bitmap.EndInit();
                Diffuse_IMG.Source = bitmap;

                Glow_IMG.Source = bitmap;
                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));
                Drag_Drop_Overlay_Skins.Visibility = Visibility.Hidden;

            }




        }

        private void skins_Panel_DragLeave(object sender, DragEventArgs e)
        {
            Drag_Drop_Overlay_Skins.Visibility = Visibility.Hidden;
        }

        private void skins_Panel_DragEnter(object sender, DragEventArgs e)
        {
            Drag_Drop_Overlay_Skins.Visibility = Visibility.Visible;

        }





        private void Help_Button_Click(object sender, RoutedEventArgs e)
        {
            string val = @"https://github.com/BigSpice/VTOL/blob/master/README.md";

            Send_Info_Notif(GetTextResource("NOTIF_INFO_OPENING") + val);
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = val,
                UseShellExecute = true
            });
        }


        private async void CheckComboBox_Initialized(object sender, EventArgs e)
        {

            if (sender.GetType() == typeof(HandyControl.Controls.CheckComboBox))
            {
                HandyControl.Controls.CheckComboBox comboBox = (HandyControl.Controls.CheckComboBox)sender;
                var Var = ((HandyControl.Controls.CheckComboBox)sender).Tag.ToString();

                string[] Split = Var.Split("|");
                string type = Split[0];
                string name = Split[1];
                string ARG = Split[2];
                // string list = String.Join(" ", comboBox.SelectedItems.Cast<String>().ToArray());
                if (ARG == "CONVAR")
                {
                    if (type == "LIST")
                    {
                        string import = null;
                        this.Dispatcher.Invoke(() =>
                        {
                            import = Read_Convar_args(name, Convar_File);
                        });
                        if (import != null)
                        {
                            import = import.Replace("\"", "").Replace(name, "");
                            int index = import.IndexOf("//");
                            if (index >= 0)
                                import = import.Substring(0, index);
                            import = import.Trim();
                            string[] import_split = import.Split(",");
                            foreach (string item in import_split)
                            {
                                comboBox.SelectedItems.Add(item);
                            }

                            comboBox.Foreground = Brushes.White;

                        }
                        else
                        {
                            comboBox.Foreground = Brushes.Gray;

                        }

                    }
                    else
                    {


                        //   Send_Success_Notif("Convar");
                        //      Read_Convar_args(name, Convar_File);
                        string import = null;
                        this.Dispatcher.Invoke(() =>
                        {
                            import = Read_Convar_args(name, Convar_File);
                        });
                        if (import != null)
                        {
                            import = import.Replace("\"", "").Replace(name, "");
                            int index = import.IndexOf("//");
                            if (index >= 0)
                                import = import.Substring(0, index);

                            string[] import_split = import.Split(" ");
                            foreach (string item in import_split)
                            {
                                comboBox.SelectedItems.Add(item);


                                Send_Error_Notif(item);
                            }
                            comboBox.Foreground = Brushes.White;

                        }
                        else
                        {
                            comboBox.Foreground = Brushes.Gray;

                        }
                    }
                }


                else
                {

                    //Send_Info_Notif(Var);

                    // string list = String.Join(" ", comboBox.SelectedItems.Cast<String>().ToArray());
                    string import = null;
                    this.Dispatcher.Invoke(() =>
                    {
                        import = Read_Startup_args(name);
                    });
                    if (import != null)
                    {
                        import.Replace(name.Trim(new Char[] { '-', '+' }), "");
                        string[] import_split = import.Split(" ");
                        foreach (string item in import_split)
                        {
                            comboBox.SelectedItems.Add(item);

                        }

                        comboBox.Foreground = Brushes.White;

                    }
                    else
                    {
                        comboBox.Foreground = Brushes.Gray;

                    }
                }
            }
            else if (sender.GetType() == typeof(ComboBox))
            {
                ComboBox comboBox = (ComboBox)sender;
                var Var = ((ComboBox)sender).Tag.ToString();

                string[] Split = Var.Split("|");
                string type = Split[0];
                string name = Split[1];
                string ARG = Split[2];
                if (ARG == "CONVAR")
                {
                    if (type == "BOOL")
                    {
                        string import = null;
                        this.Dispatcher.Invoke(() =>
                        {
                            import = Read_Convar_args(name, Convar_File);
                        });
                        if (import != null)
                        {
                            import = import.Replace("\"", "").Replace(name, "");
                            int index = import.IndexOf("//");
                            if (index >= 0)
                                import = import.Substring(0, index);
                            comboBox.SelectedIndex = Convert.ToInt32(import);
                            comboBox.Foreground = Brushes.White;

                        }
                        else
                        {
                            comboBox.Foreground = Brushes.Gray;

                        }

                    }
                    if (type == "ONE_SELECT")
                    {
                        string import = null;
                        this.Dispatcher.Invoke(() =>
                        {
                            import = Read_Convar_args(name, Convar_File);
                        });
                        if (import != null)
                        {
                            import = import.Replace("\"", "").Replace(name, "");
                            int index = import.IndexOf("//");
                            if (index >= 0)
                            {
                                import = import.Substring(0, index);
                            }

                            comboBox.SelectedValue = import.Trim();
                            comboBox.Foreground = Brushes.White;
                        }
                        else
                        {
                            comboBox.Foreground = Brushes.Gray;

                        }

                    }


                }
            }

            Started_Selection = false;
        }

        private void TextBox_Initialized(object sender, EventArgs e)
        {
            try
            {
                var val = ((TextBox)sender).Text.ToString();
                var Tag = ((TextBox)sender).Tag.ToString();
                TextBox Text_Box = (TextBox)sender;
                string[] Split = Tag.Split("|");
                string type = Split[0];
                string name = Split[1];
                string ARG = Split[2];

                string import = null;
                if (ARG == "CONVAR")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        import = Read_Convar_args(name, Convar_File);

                    });
                    if (import != null)
                    {
                        Text_Box.Foreground = Brushes.White;

                        if (type == "STRING")
                        {
                            //  import = import.Replace(name.Trim(new Char[] { '-', '+' }), "");
                            import = import.Replace("\"", "").Replace(name, "");
                            int index = import.IndexOf("//");
                            if (index >= 0)
                                import = import.Substring(0, index);

                            Text_Box.Text = import.Trim();
                            //  Send_Warning_Notif(import);


                        }
                        else
                        {
                            //  import = import.Replace(name.Trim(new Char[] { '-', '+' }), "");
                            import = import.Replace("\"", "").Replace(name, "");
                            int index = import.IndexOf("//");
                            if (index >= 0)
                                import = import.Substring(0, index);


                            Text_Box.Text = import.Trim();


                        }






                    }
                    else
                    {

                        Text_Box.Foreground = Brushes.Gray;
                    }
                }
                else
                {


                    this.Dispatcher.Invoke(() =>
                    {
                        import = Read_Startup_args(name);
                    });
                    if (import != null)
                    {
                        Text_Box.Foreground = Brushes.White;

                        if (type == "STRINGQ")
                        {
                            Send_Info_Notif(import);
                            //  import = import.Replace(name.Trim(new Char[] { '-', '+' }), "");
                            import = import.Replace("\"", "").Replace(name, "");



                            Text_Box.Text = import.Trim();
                            //  Send_Warning_Notif(import);


                        }
                        else
                        {
                            import.Replace(name.Trim(new Char[] { '-', '+' }), "");
                            string[] import_split = import.Split(" ");
                            for (int i = 1; i < import_split.Length; i++)
                            {
                                Text_Box.Text = import_split[1].Trim();
                            }
                        }






                    }
                    else
                    {

                        Text_Box.Foreground = Brushes.Gray;
                    }
                }
            }
            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }

        }

        private void UI_List_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void S(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)

            {

                Scoller.LineDown();

            }

            else

            {

                Scoller.LineUp();

            }
        }

        private void Convar_Arguments_UI_List_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)

            {

                Scoller.LineDown();

            }

            else

            {

                Scoller.LineUp();

            }
        }

        private void ComboBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Started_Selection = true;

        }



        private void Load_Bt_(object sender, RoutedEventArgs e)
        {
            try
            {


                if (File.Exists(Current_Install_Folder + @"\VTOL_Dedicated_Workspace\autoexec_ns_server.cfg"))
                {
                    File.Delete(Current_Install_Folder + @"\VTOL_Dedicated_Workspace\autoexec_ns_server.cfg");
                }

                if (Directory.Exists(Current_Install_Folder))
                {

                    Convar_File = GetFile(Current_Install_Folder, "autoexec_ns_server.cfg").First();
                    Ns_dedi_File = GetFile(Current_Install_Folder, "ns_startup_args_dedi.txt").First();
                }
                if (Convar_File != null || Ns_dedi_File != null)
                {
                    if (File.Exists(Ns_dedi_File) && File.Exists(Convar_File))
                    {
                        Startup_Arguments_UI_List.ItemsSource = Load_Args();
                        Convar_Arguments_UI_List.ItemsSource = Convar_Args();
                        Started_Selection = false;

                        Load_Bt.Content = "Reload Arguments";
                        Check_Args();


                    }
                    else
                    {
                        Send_Error_Notif(GetTextResource("NOTIF_ERROR_SUGGEST_REBROWSE"));

                        return;
                    }
                }
                else
                {
                    Send_Error_Notif(GetTextResource("NOTIF_ERROR_SUGGEST_REBROWSE"));
                    Send_Error_Notif("Could Not Find Dedicated arg Files!");

                    return;
                }

            }
            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }


        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Mod = null;
                if (Disabled_ListBox.SelectedItem != null && Disabled_ListBox.IsMouseOver == true && Enabled_ListBox.IsMouseOver == false)
                {
                    Mod = (Disabled_ListBox.SelectedItem.ToString());

                }
                else if (Enabled_ListBox.SelectedItem != null && Disabled_ListBox.IsMouseOver == false && Enabled_ListBox.IsMouseOver == true)
                {
                    Mod = (Enabled_ListBox.SelectedItem.ToString());


                }
                else
                {
                    Mod = null;
                }
                if (Mod != null)
                {

                    if (Directory.Exists(Current_Install_Folder + @"\R2Northstar\mods\" + Mod))
                    {
                        Process.Start("explorer.exe", Current_Install_Folder + @"\R2Northstar\mods\" + Mod);
                    }
                    else
                    {

                        Process.Start("explorer.exe", Current_Install_Folder + @"\R2Northstar\mods\");

                    }
                }
                else
                {

                    if (Directory.Exists(Current_Install_Folder + @"\R2Northstar\mods\"))
                    {
                        Process.Start("explorer.exe", Current_Install_Folder + @"\R2Northstar\mods\");
                    }
                    else
                    {
                        return;
                    }

                }

            }

            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }

        }

        private void Cheat_Sheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Send_Info_Notif(GetTextResource("NOTIF_INFO_OPENING_WIKI"));
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = @"https://r2northstar.gitbook.io/r2northstar-wiki/hosting-a-server-with-northstar/dedicated-server#gamemodes",
                    UseShellExecute = true
                });
            }

            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {

        }



        private void Language_Selection_SelectionChanged(object sender, SelectionChangedEventArgs e) //TODO: fill in the correct xaml file names to each language
        {
            if (English.IsSelected == true)
            {

                ChangeLanguageTo("en");

            }
            if (French.IsSelected == true)
            {

                ChangeLanguageTo("fr");

            }
            if (German.IsSelected == true)
            {

                ChangeLanguageTo("de");

            }

            if (Italian.IsSelected == true)
            {
                ChangeLanguageTo("it");

            }
            /*
           if (Japanese.IsSelected == true)
           {

               ChangeLanguageTo("en");

           }
           if (Portugese.IsSelected == true)
           {
               ChangeLanguageTo("en");

           }
           if (Russian.IsSelected == true)
           {
               ChangeLanguageTo("en");

           }
           */
            if (Chinese.IsSelected == true)
            {
                ChangeLanguageTo("cn");

            }
            if (Korean.IsSelected == true)
            {
                ChangeLanguageTo("kr");

            }

        }

        private void Language_Selection_MouseEnter(object sender, MouseEventArgs e)
        {
            Language_Selection.BorderBrush = Brushes.White;
        }

        private void Language_Selection_MouseLeave(object sender, MouseEventArgs e)
        {
            Language_Selection.BorderBrush = Brushes.Black;

        }
        void Check_Tabs(bool hard_reset = false, bool Search_ = false, string SearchQuery = "#")
        {
            this.Dispatcher.Invoke(() =>
            {
                Test_List.Background.Opacity = 0;
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            });
            //IsLoading_Panel.Visibility = Visibility.Visible;

            //Send_Success_Notif(Convert.ToString((Sections_Tabs.SelectedItem as TabItem).Header));


            switch (Convert.ToString((Sections_Tabs.SelectedItem as TabItem).Header))
            {
                case "All":
                    Thunderstore_Parse(hard_reset, "None", Search_, SearchQuery);

                    break;
                case "Skins":
                    Thunderstore_Parse(hard_reset, "Skins", Search_, SearchQuery);

                    break;
                case "Menu Mods":
                    Thunderstore_Parse(hard_reset, "Custom Menus", Search_, SearchQuery);

                    break;
                case "DDS Skins":
                    Thunderstore_Parse(hard_reset, "DDS", Search_, SearchQuery);

                    break;
                case "Server Mods":
                    Thunderstore_Parse(hard_reset, "Server-side", Search_, SearchQuery);

                    break;
                case "Client Mods":
                    Thunderstore_Parse(hard_reset, "Client-side", Search_, SearchQuery);

                    break;
                default:
                    Thunderstore_Parse(hard_reset, "None", Search_, SearchQuery);
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                    break;


            }
            this.Dispatcher.Invoke(() =>
            {

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            });
            //IsLoading_Panel.Visibility = Visibility.Hidden;

        }

        private void Sections_Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (Finished_Init == false)
            {

                Check_Tabs(true);
            }
            else
            {

                Check_Tabs(false);

            }


        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public class InstalledApplications
        {
            public static string GetApplictionInstallPath(string nameOfAppToFind)
            {
                string installedPath;
                string keyName;

                // search in: CurrentUser
                keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                installedPath = ExistsInSubKey(Registry.CurrentUser, keyName, "DisplayName", nameOfAppToFind);
                if (!string.IsNullOrEmpty(installedPath))
                {
                    return installedPath;
                }

                // search in: LocalMachine_32
                keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                installedPath = ExistsInSubKey(Registry.LocalMachine, keyName, "DisplayName", nameOfAppToFind);
                if (!string.IsNullOrEmpty(installedPath))
                {
                    return installedPath;
                }

                // search in: LocalMachine_64
                keyName = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
                installedPath = ExistsInSubKey(Registry.LocalMachine, keyName, "DisplayName", nameOfAppToFind);
                if (!string.IsNullOrEmpty(installedPath))
                {
                    return installedPath;
                }

                return string.Empty;
            }

            public static void GetInstalledApps()

            {
                string x = "";
                string appPATH = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
                using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(appPATH))
                {
                    foreach (string skName in rk.GetSubKeyNames())
                    {
                        using (RegistryKey sk = rk.OpenSubKey(skName))
                        {
                            try
                            {

                                //Get App Name
                                var displayName = sk.GetValue("DisplayName");
                                //Get App Size
                                var size = sk.GetValue("EstimatedSize");

                                string item;
                                if (displayName != null)
                                {
                                    if (size != null)
                                        item = displayName.ToString();
                                    else
                                    {
                                        item = displayName.ToString();
                                        if (item.Contains(""))
                                            x = x + displayName.ToString() + "\n";
                                        // MessageBox.Show(displayName.ToString());

                                    }

                                }
                            }
                            catch (Exception ex)
                            {




                            }
                        }
                    }
                    //File.WriteAllText(@"C:\VTOL\ALLApps.txt",x);
                }

            }

            private static string ExistsInSubKey(RegistryKey root, string subKeyName, string attributeName, string nameOfAppToFind)
            {
                RegistryKey subkey;
                string displayName;

                using (RegistryKey key = root.OpenSubKey(subKeyName))
                {
                    if (key != null)
                    {
                        foreach (string kn in key.GetSubKeyNames())
                        {
                            using (subkey = key.OpenSubKey(kn))
                            {
                                displayName = subkey.GetValue(attributeName) as string;
                                if (nameOfAppToFind.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                                {
                                    return subkey.GetValue("InstallLocation") as string;
                                }
                            }
                        }
                    }
                }
                return string.Empty;
            }
        }
        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        async Task Run_Origin()
        {
            try
            {
                if (Check_Process_Running("EABackgroundService", true) == true)
                {

                    Process[] runingProcess = Process.GetProcesses();
                    string[] origin = {
                        "QtWebEngineProcess",
                        "OriginLegacyCompatibility",
                        "EADesktop",
                        "EABackgroundService",
                        "EALauncher",
                        "Link2EA",
                        "EALocalHostSvc",
                        "EAGEP"};

                    for (int i = 0; i < runingProcess.Length; i++)
                    {
                        foreach (var x in origin)
                        {
                            // compare equivalent process by their name

                            if (runingProcess[i].ProcessName == x)
                            {
                                try
                                {
                                    //kill running process
                                    runingProcess[i].Kill();
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                        }


                    }
                    Thread.Sleep(3000);

                }
                else
                {
                    if (IsAdministrator() == false)
                    {

                        System.Windows.MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo { Message = "Not Running In Administrator Mode. Would you like to eleveate the application?", Caption = "ERROR!", Button = MessageBoxButton.YesNo, IconBrushKey = ResourceToken.AccentBrush, IconKey = ResourceToken.AskGeometry, StyleKey = "MessageBoxCustom" });
                        if (result == System.Windows.MessageBoxResult.Yes)
                        {
                            if (File.Exists((Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", "")).Trim() + @"\VTOL.exe"))
                            {
                                // this.Close();

                                ProcessStartInfo info = new ProcessStartInfo((System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", "")).Trim() + @"\VTOL.exe");
                                info.UseShellExecute = true;
                                info.Verb = "runas";
                                Process.Start(info);
                                App.Current.Shutdown();

                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    
                }









                string location = InstalledApplications.GetApplictionInstallPath("Origin") + @"\Origin.exe";
                if (File.Exists(location))
                {
                    Send_Success_Notif("Starting Origin Client Service!");
                    Indicator_Origin_Client.Visibility = Visibility.Hidden;


                    ProcessStartInfo procStartInfo = new ProcessStartInfo();
                    Process process = new Process();
                    procStartInfo.FileName = location;
                    procStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(NSExe);


                    // procStartInfo.Arguments = args;

                    process.StartInfo = procStartInfo;

                    process.Start();
                    int id = process.Id;
                    pid = id;
                    Process tempProc = Process.GetProcessById(id);
                    // this.Visible = false;
                    // Thread.Sleep(5000);
                    // tempProc.WaitForExit();
                    // this.Visible = true;


                    // Process process = Process.Start(NSExe, Arg_Box.Text);
                    process.Close();

                    Origin_Client_Running = Check_Process_Running("OriginClientService");
                    while (!Origin_Client_Running)
                    {


                        Origin_Client_Running = Check_Process_Running("OriginClientService");

                        Thread.Sleep(1000);
                    }
                    if (Origin_Client_Running == true)
                    {
                        Origin_Client_Status.Fill = Brushes.LimeGreen;
                        Indicator_Origin_Client.Visibility = Visibility.Hidden;


                    }
                }
                else
                {
                    Origin_Client_Status.Fill = Brushes.Red;
                    Indicator_Origin_Client.Visibility = Visibility.Visible;

                    Send_Warning_Notif("Could not Find EA Origin Install, Please Start Manually, Or Repair your installation!.");

                }
            }
            catch (Exception ex)
            {

                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));
            }

        }

     
        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }
        private string Find_Folder(string searchQuery, string folderPath)
        {
            searchQuery = "*" + searchQuery + "*";

            var directory = new DirectoryInfo(folderPath);

            var directories = directory.GetDirectories(searchQuery, SearchOption.AllDirectories);
            return directories[0].ToString();
        }
        private void Delete_Menu_Context_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Disabled_ListBox.SelectedItem != null && Disabled_ListBox.IsMouseOver == true && Enabled_ListBox.IsMouseOver == false)
                {
                    string Mod = (Disabled_ListBox.SelectedItem.ToString());
                    string FolderDir = Find_Folder(Mod, Current_Install_Folder + @"\R2Northstar\mods");
                    if (Directory.Exists(FolderDir))
                    {
                        System.Windows.MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo { Message = "Are You Sure You Want To Delete The Mod - " + Mod + " ?", Caption = "WARNING!", Button = MessageBoxButton.YesNo, IconBrushKey = ResourceToken.AccentBrush, IconKey = ResourceToken.AskGeometry, StyleKey = "MessageBoxCustom" });
                        if (result == System.Windows.MessageBoxResult.Yes)
                        {
                            Directory.Delete(FolderDir, true);

                            if (!Directory.Exists(FolderDir))
                            {
                                Send_Success_Notif("Successfully Deleted - " + Mod);
                                Call_Mods_From_Folder();

                            }
                            else
                            {
                                Send_Error_Notif("Could not Delete! - " + Mod);
                                Call_Mods_From_Folder();

                            }

                        }

                    }

                }
                else if (Enabled_ListBox.SelectedItem != null && Disabled_ListBox.IsMouseOver == false && Enabled_ListBox.IsMouseOver == true)
                {
                    string Mod = (Enabled_ListBox.SelectedItem.ToString());
                    string FolderDir = Find_Folder(Mod, Current_Install_Folder + @"\R2Northstar\mods");
                    if (Directory.Exists(FolderDir))
                    {
                        System.Windows.MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo { Message = "Are You Sure You Want To Delete The Mod - " + Mod + " ?", Caption = "WARNING!", Button = MessageBoxButton.YesNo, IconBrushKey = ResourceToken.AccentBrush, IconKey = ResourceToken.AskGeometry, StyleKey = "MessageBoxCustom" });
                        if (result == System.Windows.MessageBoxResult.Yes)
                        {
                            Directory.Delete(FolderDir, true);

                            if (!Directory.Exists(FolderDir))
                            {
                                Send_Success_Notif("Successfully Deleted - " + Mod);
                                Call_Mods_From_Folder();

                            }
                            else
                            {
                                Send_Error_Notif("Could not Delete! - " + Mod);
                                Call_Mods_From_Folder();

                            }

                        }

                    }




                }
            }
            catch (Exception ex)
            {
                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }
        }

        private void MOD_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            //  Current_Install_Folder + @"\R2Northstar\mods";
            Delete_Menu_Context.IsHitTestVisible = false;
            Delete_Menu_Context.Foreground = Brushes.Gray;

            if (Disabled_ListBox.SelectedItem != null && Disabled_ListBox.IsMouseOver == true && Enabled_ListBox.IsMouseOver == false)
            {
                Delete_Menu_Context.IsHitTestVisible = true;
                Delete_Menu_Context.Foreground = Brushes.White;
            }
            else if (Enabled_ListBox.SelectedItem != null && Disabled_ListBox.IsMouseOver == false && Enabled_ListBox.IsMouseOver == true)
            {
                Delete_Menu_Context.IsHitTestVisible = true;
                Delete_Menu_Context.Foreground = Brushes.White;
            }

            else
            {
                Delete_Menu_Context.IsHitTestVisible = false;
                Delete_Menu_Context.Foreground = Brushes.Gray;


            }
            Context_Menu_Mod_Mngs.ContextMenu.Items.Refresh();

            Delete_Menu_Context.Refresh();

        }

        private void Disabled_ListBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Disabled_ListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            //  Disabled_ListBox.SelectedItem= null;


        }

        private void Enabled_ListBox_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void Enabled_ListBox_LostFocus(object sender, RoutedEventArgs e)
        {
        }



        private void Disabled_ListBox_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void Enabled_ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Disabled_ListBox.SelectedItem != null)
            {
                Disabled_ListBox.UnselectAll();
                Disabled_ListBox.SelectedValue = null;


            }
        }

        private void Disabled_ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Enabled_ListBox.SelectedItem != null)
            {
                Enabled_ListBox.SelectedValue = null;
                Enabled_ListBox.UnselectAll();

            }
        }

        private void Disabled_ListBox_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
        }

        private void SortMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Sort_Lists == true)
                {
                    Sort_Img_Source.Source = new BitmapImage(new Uri(@"/Resources/Sort_Off.png", UriKind.Relative));
                    Sort_Lists = false;
                    Mod_Directory_List_Active.Sort();
                    Mod_Directory_List_InActive.Sort();
                    Enabled_ListBox.Items.IsLiveSorting = true;
                    Disabled_ListBox.Items.IsLiveSorting = true;
                    Properties.Settings.Default.Sort_Mods = false;
                    Properties.Settings.Default.Save();
                    Sort_Lists = Properties.Settings.Default.Sort_Mods;

                    Call_Mods_From_Folder();

                }
                else
                {
                    Sort_Img_Source.Source = new BitmapImage(new Uri(@"/Resources/Sort_On.png", UriKind.Relative));
                    Enabled_ListBox.Items.IsLiveSorting = false;
                    Disabled_ListBox.Items.IsLiveSorting = false;

                    Sort_Lists = true;
                    Properties.Settings.Default.Sort_Mods = true;
                    Properties.Settings.Default.Save();
                    Sort_Lists = Properties.Settings.Default.Sort_Mods;

                    Call_Mods_From_Folder();

                }
            }
            catch (Exception ex)
            {
                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }
        }
        private string _filePath;

        public void ZIP_LIST(List<string> filesToZip, string sZipFileName, bool deleteExistingZip = true)
        {
            if (filesToZip.Count > 0)
            {
                if (File.Exists(filesToZip[0]))
                {

                    // Get the first file in the list so we can get the root directory
                    string strRootDirectory = Path.GetDirectoryName(filesToZip[0]);

                    // Set up a temporary directory to save the files to (that we will eventually zip up)
                    DirectoryInfo dirTemp = Directory.CreateDirectory(strRootDirectory + "/" + DateTime.Now.ToString("yyyyMMddhhmmss"));

                    // Copy all files to the temporary directory
                    foreach (string strFilePath in filesToZip)
                    {
                        if (!File.Exists(strFilePath))

                        {
                            Send_Error_Notif("Failed!");
                            Write_To_Log("file does not exist" + Ns_dedi_File + "   " + Convar_File);

                            return;
                            // throw new Exception(string.Format("File {0} does not exist", strFilePath));
                        }
                        string strDestinationFilePath = Path.Combine(dirTemp.FullName, Path.GetFileName(strFilePath));
                        File.Copy(strFilePath, strDestinationFilePath);
                    }

                    // Create the zip file using the temporary directory
                    if (!sZipFileName.EndsWith(".zip")) { sZipFileName += ".zip"; }
                    string strZipPath = Path.Combine(strRootDirectory, sZipFileName);
                    if (deleteExistingZip == true && File.Exists(strZipPath)) { File.Delete(strZipPath); }
                    ZipFile.CreateFromDirectory(dirTemp.FullName, strZipPath, CompressionLevel.Fastest, false);

                    // Delete the temporary directory
                    dirTemp.Delete(true);

                    _filePath = strZipPath;
                }
                else
                {
                    Send_Error_Notif("Failed!");
                    Write_To_Log("file does not exist" + Ns_dedi_File + "   " + Convar_File);
                    return;
                }
            }
            else
            {
                Send_Error_Notif("You must specify at least one file to zip.");
                return;
            }
        }

        private void Export_Server_Config_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Directory.Exists(Current_Install_Folder + "/VTOL_Dedicated_Workspace"))
                {
                    if (File.Exists(Ns_dedi_File) && File.Exists(Convar_File))
                    {
                        List<string> files = new List<string>();
                        files.Add(Ns_dedi_File);
                        files.Add(Convar_File);

                        ZIP_LIST(files, Current_Install_Folder + @"\VTOL_Dedicated_Workspace/Exported_Config.zip", true);

                        Send_Success_Notif("Exported to " + Current_Install_Folder + @"\VTOL_Dedicated_Workspace/Exported_Config.zip" + " Sucessfully!");



                    }
                    else
                    {
                        Send_Error_Notif(GetTextResource("NOTIF_ERROR_SUGGEST_REBROWSE"));
                        return;
                    }
                }
                else
                {
                    Directory.CreateDirectory(Current_Install_Folder + @"\VTOL_Dedicated_Workspace");

                    if (File.Exists(Ns_dedi_File) && File.Exists(Convar_File))
                    {
                        List<string> files = new List<string>();
                        files.Add(Path.GetDirectoryName(Ns_dedi_File).ToString());
                        files.Add(Path.GetDirectoryName(Convar_File).ToString());

                        ZIP_LIST(files, Current_Install_Folder + @"\VTOL_Dedicated_Workspace/Exported_Config.zip", true);
                        Send_Success_Notif("Exported to " + Current_Install_Folder + @"\VTOL_Dedicated_Workspace/Exported_Config.zip" + " Sucessfully!");





                    }
                    else
                    {
                        Send_Error_Notif(GetTextResource("LOAD"));
                        Send_Error_Notif(GetTextResource("NOTIF_ERROR_SUGGEST_REBROWSE"));
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }
        }

        private void Import_Server_Config_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string zipPath = null;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    zipPath = openFileDialog.FileName;
                }
                else
                {
                    return;
                }

                if (Directory.Exists(Current_Install_Folder + @"\VTOL_Dedicated_Workspace"))
                {
                    string extractPath = Path.GetFullPath(Current_Install_Folder + @"\VTOL_Dedicated_Workspace");
                    if (File.Exists(Current_Install_Folder + @"\VTOL_Dedicated_Workspace\autoexec_ns_server.cfg"))
                    {
                        File.Delete(Current_Install_Folder + @"\VTOL_Dedicated_Workspace\autoexec_ns_server.cfg");
                    }

                    if (!extractPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                        extractPath += Path.DirectorySeparatorChar;







                    if (zipPath != null)
                    {

                        using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                if (entry.FullName.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase) || entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                                {
                                    // Gets the full path to ensure that relative segments are removed.
                                    string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));

                                    // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                                    // are case-insensitive.
                                    if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal))
                                    {
                                        string f = Find_Folder("Northstar.CustomServers", Current_Install_Folder).ToString();

                                        if (Directory.Exists(f))
                                        {
                                            string c = GetFile(f, "autoexec_ns_server.cfg").First();

                                            if (entry.FullName.EndsWith(".cfg"))
                                            {
                                                entry.ExtractToFile(c, true);


                                            }

                                        }
                                        if (File.Exists(GetFile(Current_Install_Folder, "ns_startup_args_dedi.txt").First()))
                                        {
                                            string d = GetFile(Current_Install_Folder, "ns_startup_args_dedi.txt").First();

                                            if (entry.FullName.EndsWith(".txt"))
                                            {
                                                entry.ExtractToFile(d, true);

                                            }


                                        }

                                    }
                                }
                                else
                                {

                                    Send_Error_Notif("Error! Cannot see valid Server config files in the zip!");
                                    return;
                                }
                            }
                        }
                    }
                    Convar_File = GetFile(Current_Install_Folder, "autoexec_ns_server.cfg").First();
                    Ns_dedi_File = GetFile(Current_Install_Folder, "ns_startup_args_dedi.txt").First();
                    // HandyControl.Controls.MessageBox.Show(new MessageBoxInfo { Message = "Please Select the Northstar Dedicated Import as well.", Caption = "PROMPT!", Button = MessageBoxButton.OK, IconBrushKey = ResourceToken.AccentBrush, IconKey = ResourceToken.AskGeometry, StyleKey = "MessageBoxCustom" });
                }
                else
                {
                    Directory.CreateDirectory(Current_Install_Folder + @"\VTOL_Dedicated_Workspace");
                    string extractPath = Path.GetFullPath(Current_Install_Folder + @"\VTOL_Dedicated_Workspace");



                    if (!extractPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                        extractPath += Path.DirectorySeparatorChar;





                    if (zipPath != null)
                    {


                        using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                if (entry.FullName.EndsWith(".cfg", StringComparison.OrdinalIgnoreCase) || entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                                {
                                    // Gets the full path to ensure that relative segments are removed.
                                    string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));

                                    // Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
                                    // are case-insensitive.
                                    if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal))
                                    {
                                        string f = Find_Folder("Northstar.CustomServers", Current_Install_Folder).ToString();

                                        if (Directory.Exists(f))
                                        {
                                            string c = GetFile(f, "autoexec_ns_server.cfg").First();

                                            if (entry.FullName.EndsWith(".cfg"))
                                            {
                                                entry.ExtractToFile(c, true);


                                            }

                                        }
                                        if (File.Exists(GetFile(Current_Install_Folder, "ns_startup_args_dedi.txt").First()))
                                        {
                                            string d = GetFile(Current_Install_Folder, "ns_startup_args_dedi.txt").First();

                                            if (entry.FullName.EndsWith(".txt"))
                                            {
                                                entry.ExtractToFile(d, true);

                                            }


                                        }

                                    }
                                }
                                else
                                {

                                    Send_Error_Notif(GetTextResource("SERVER_CFG_FAILED"));
                                    return;
                                }
                            }
                        }
                    }
                    // HandyControl.Controls.MessageBox.Show(new MessageBoxInfo { Message = "Please Select the Northstar Dedicated Import as well.", Caption = "PROMPT!", Button = MessageBoxButton.OK, IconBrushKey = ResourceToken.AccentBrush, IconKey = ResourceToken.AskGeometry, StyleKey = "MessageBoxCustom" });
                    if (Directory.Exists(Current_Install_Folder))
                    {
                        Ns_dedi_File = GetFile(Current_Install_Folder, "ns_startup_args_dedi.txt").First();

                        if (Directory.Exists(Current_Install_Folder + @"\R2Northstar\mods"))
                        {
                            Convar_File = GetFile(Current_Install_Folder, "autoexec_ns_server.cfg").First();

                        }
                    }





                }
                if (File.Exists(Ns_dedi_File) && File.Exists(Convar_File))
                {
                    Startup_Arguments_UI_List.ItemsSource = Load_Args();
                    Convar_Arguments_UI_List.ItemsSource = Convar_Args();
                    Started_Selection = false;

                    Load_Bt.Content = "Reload Arguments";
                    Check_Args();

                    Send_Success_Notif("Imported and Applied!");
                }
                else
                {
                    Send_Error_Notif(GetTextResource("NOTIF_ERROR_SUGGEST_REBROWSE"));
                    return;
                }
            }
            catch (Exception ex)
            {
                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }




        private void DataGridSettings_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void DataGridSettings_SelectedObjectChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
        }

        private void DataGridSettings_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void DataGridSettings_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void TextBox_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

            
            if (e.Key == Key.Enter)
            {
                Send_Success_Notif("Changed Repo URL From " + Current_REPO_URL + " -To- " + Repo_URl.Text.ToString());
                Current_REPO_URL = Repo_URl.Text.ToString();
                Properties.Settings.Default.Current_REPO_URL = Repo_URl.Text.ToString();
                Properties.Settings.Default.Save();
                    User_Settings_Vars.RepoUrl = Current_REPO_URL;
                    string User_Settings_Json_Strings = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
                    using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
                    {
                        StreamWriter.WriteLine(User_Settings_Json_Strings);
                        StreamWriter.Close();
                    }

                }
            }
            catch (Exception ef)
            {
                Write_To_Log(ErrorManager(ef));

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_CONTACT"));
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (Disabled_ListBox.SelectedItem != null  && Enabled_ListBox.IsMouseOver == false)
                {
                    string Mod = (Disabled_ListBox.SelectedItem.ToString());
                    string FolderDir = Find_Folder(Mod, Current_Install_Folder + @"\R2Northstar\mods");
                  
                    if (Directory.Exists(FolderDir))
                    {


                            string mod_Json = FindFirstFile(FolderDir, "mod.json");


                        if (mod_Json != null && File.Exists(mod_Json))
                            {
                                var myJsonString = File.ReadAllText(mod_Json);
                                var myJObject = JObject.Parse(myJsonString);

                                string Output = "Name: " + myJObject.SelectToken("Name").Value<string>() + Environment.NewLine + "Description: " + myJObject.SelectToken("Description").Value<string>() + Environment.NewLine + "Version: " + myJObject.SelectToken("Version").Value<string>();

                                System.Windows.MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo { Message = Output, Caption = "INFO", Button = MessageBoxButton.OK, IconBrushKey = ResourceToken.AccentBrush, IconKey = ResourceToken.AskGeometry, StyleKey = "MessageBoxCustom" });

                            }

                        


                    }
                  
                        

                    

                }
                else if (Enabled_ListBox.SelectedItem != null  && Disabled_ListBox.IsMouseOver == false)
                {

                    string Mod = (Enabled_ListBox.SelectedItem.ToString());
                    string FolderDir = Find_Folder(Mod, Current_Install_Folder + @"\R2Northstar\mods");
                
                    if (Directory.Exists(FolderDir))
                    {


                       
                            string mod_Json = FindFirstFile(FolderDir, "mod.json");

                        if (mod_Json != null && File.Exists(mod_Json))
                        {
                            var myJsonString = File.ReadAllText(mod_Json);
                                var myJObject = JObject.Parse(myJsonString);
                                string Output = "Name:  " + myJObject.SelectToken("Name").Value<string>() + Environment.NewLine + "Description:  " + myJObject.SelectToken("Description").Value<string>() + Environment.NewLine + "Version:  " + myJObject.SelectToken("Version").Value<string>();

                                System.Windows.MessageBoxResult result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo { Message = Output, Caption = "INFO", Button = MessageBoxButton.OK, IconBrushKey = ResourceToken.AccentBrush, IconKey = ResourceToken.AskGeometry, StyleKey = "MessageBoxCustom" });

                            }




                    }
                 

                }
            }
            catch (Exception ex)
            {
                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }
        }

        private void ColorPicker_Accent_Confirmed(object sender, FunctionEventArgs<Color> e)
        {
            try
            {
                // Color color = (Color)ColorConverter.ConvertFromString(ColorPicker_Accent.SelectedBrush.Color.ToString());
                //   Send_Info_Notif(color.GetBrightness().ToString());
                if (File.Exists(@"C:\ProgramData\VTOL_DATA\VARS\Theme.txt"))
                {

                    string[] lines = System.IO.File.ReadAllLines(@"C:\ProgramData\VTOL_DATA\VARS\Theme.txt");
                    if (lines.Length > 0)
                    {
                        lines = lines.ToArray();
                        lines[0] = ColorPicker_Accent.SelectedBrush.Color.ToString().Trim();
                        // var newLines = new string[] { ColorPicker_Accent.SelectedBrush.Color.ToString().Trim() };


                        System.IO.File.WriteAllLines(@"C:\ProgramData\VTOL_DATA\VARS\Theme.txt", lines);
                        Accent_Color = (SolidColorBrush)new BrushConverter().ConvertFrom(ColorPicker_Accent.SelectedBrush.Color.ToString());
                        Send_Success_Notif("Saved The Color And Applied!");

                        this.Resources["Button_BG"] = (SolidColorBrush)new BrushConverter().ConvertFrom(ColorPicker_Accent.SelectedBrush.Color.ToString());


                    }

                }
                else
                {
                    if (ColorPicker_Accent.SelectedBrush.Color != null)
                    {
                        if (isValidHexaCode(ColorPicker_Accent.SelectedBrush.Color.ToString()))
                        {
                            this.Resources["Button_BG"] = (SolidColorBrush)new BrushConverter().ConvertFrom(ColorPicker_Accent.SelectedBrush.Color.ToString());
                            User_Settings_Vars.Theme = ColorPicker_Accent.SelectedBrush.Color.ToString().Trim();
                            string User_Settings_Json_Strings = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
                            using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
                            {
                                StreamWriter.WriteLine(User_Settings_Json_Strings);
                                StreamWriter.Close();
                            }
                            Send_Success_Notif("Saved The Color And Applied!");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }

        }

        private void ColorPicker_Border_Confirmed(object sender, FunctionEventArgs<Color> e)
        {
            try
            {
                if (File.Exists(@"C:\ProgramData\VTOL_DATA\VARS\Theme.txt"))
                {

                    string[] lines = System.IO.File.ReadAllLines(@"C:\ProgramData\VTOL_DATA\VARS\Theme.txt");
                    if (lines.Length > 0)
                    {
                        lines = lines.ToArray();
                        var newLines = new string[] { lines[0], ColorPicker_Border.SelectedBrush.Color.ToString().Trim() };



                        System.IO.File.WriteAllLines(@"C:\ProgramData\VTOL_DATA\VARS\Theme.txt", newLines);
                        Border_Color = (SolidColorBrush)new BrushConverter().ConvertFrom(ColorPicker_Border.SelectedBrush.Color.ToString().Trim());
                        Send_Success_Notif("Saved The Color And Applied!");


                    }

                }
                else
                {
                    if (ColorPicker_Border.SelectedBrush.Color != null)
                    {
                        if (isValidHexaCode(ColorPicker_Border.SelectedBrush.Color.ToString()))
                        {
                            User_Settings_Vars.Theme = ColorPicker_Accent.SelectedBrush.Color.ToString().Trim();
                            string User_Settings_Json_Strings = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
                            using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
                            {
                                StreamWriter.WriteLine(User_Settings_Json_Strings);
                                StreamWriter.Close();
                            }
                            Send_Success_Notif("Saved The Color And Applied!");

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Write_To_Log(ErrorManager(ex));
                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_LOG"));

            }
        }

        private void AuthorBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

            
            if (e.Key == Key.Enter)
            {
                Send_Success_Notif("Changed Author URL From " + Author_Used + " -To- " + AuthorBox.Text.ToString());
                Author_Used = AuthorBox.Text.ToString();
                Properties.Settings.Default.Author = AuthorBox.Text.ToString();
                Properties.Settings.Default.Save();
                    User_Settings_Vars.Author = Author_Used;
                    string User_Settings_Json_Strings = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
                    using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
                    {
                        StreamWriter.WriteLine(User_Settings_Json_Strings);
                        StreamWriter.Close();
                    }
                }
            }
            catch (Exception ef)
            {
                Write_To_Log(ErrorManager(ef));

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_CONTACT"));
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Master_ServerBox_KeyDown(object sender, KeyEventArgs e)
        {
            try { 
            if (e.Key == Key.Enter)
            {
                Send_Success_Notif("Changed Repo URL From " + MasterServer_URL + " -To- " + Master_ServerBox.Text.ToString());
                MasterServer_URL = Master_ServerBox.Text.ToString();
                Properties.Settings.Default.Repo = Master_ServerBox.Text.ToString();
                Properties.Settings.Default.Save();
                    User_Settings_Vars.MasterServerUrl = MasterServer_URL;
                    string User_Settings_Json_Strings = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
                    using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
                    {
                        StreamWriter.WriteLine(User_Settings_Json_Strings);
                        StreamWriter.Close();
                    }
                }
        }
            catch (Exception ef)
            {
                Write_To_Log(ErrorManager(ef));

        Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_CONTACT"));
            }
}

        private void RepoBox_KeyDown(object sender, KeyEventArgs e)
        {
            try { 
            if (e.Key == Key.Enter)
            {
                Send_Success_Notif("Changed Repo URL From " + Repo_Used + " -To- " + RepoBox.Text.ToString());
                Repo_Used = RepoBox.Text.ToString();
                Properties.Settings.Default.Repo = RepoBox.Text.ToString();
                Properties.Settings.Default.Save();
                    User_Settings_Vars.Repo = Repo_Used;
                    string User_Settings_Json_Strings = Newtonsoft.Json.JsonConvert.SerializeObject(User_Settings_Vars);
                    using (var StreamWriter = new StreamWriter(DocumentsFolder + @"\VTOL_DATA\Settings\User_Settings.Json", false))
                    {
                        StreamWriter.WriteLine(User_Settings_Json_Strings);
                        StreamWriter.Close();
                    }
                }
            }
            catch (Exception ef)
            {
                Write_To_Log(ErrorManager(ef));

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_CONTACT"));
            }
        }

        private void SearchBar_KeyDown(object sender, KeyEventArgs e)
        {
            try { 
            if (e.Key == Key.Enter)
            {
                if (SearchBar.Text != null && SearchBar.Text != "" && SearchBar.Text != " ")
                {
                    Test_List.ItemsSource = null;

                    Check_Tabs(true, true, SearchBar.Text);





                    if (Test_List.Items.Count <= 0)
                    {
                        Test_List.Background.Opacity = 0.01;
                    }
                    else
                    {
                        Test_List.Background.Opacity = 0;

                    }
                }
                else
                {




                    Check_Tabs(true, false, SearchBar.Text);

                }
            }
        }
            catch (Exception ef)
            {
                Write_To_Log(ErrorManager(ef));

        Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_CONTACT"));
            }
}
        public string Current_Mod_To_Pack;
        private void Locate_Zip_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                Current_Mod_To_Pack = openFileDialog.FileName;
                if (!File.Exists(Current_Mod_To_Pack))
                {

                    Send_Error_Notif(GetTextResource("NOTIF_ERROR_INVALID_ZIP_PATH"));
                    Zip_Box.Background = Brushes.IndianRed;

                    return;

                }
                else
                {
                    if (Path.GetExtension(Current_Mod_To_Pack).Contains("zip") || Path.GetExtension(Current_Mod_To_Pack).Contains("Zip"))
                    {
                        Send_Success_Notif("Valid Zip Found!");
                        Zip_Box.Text = Current_Mod_To_Pack;
                        Zip_Box.Background = Brushes.White;

                    }
                }
            }

            }
            catch (Exception ef)
            {
                Write_To_Log(ErrorManager(ef));

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_CONTACT"));
            }
        }
        public string Mod_Icon_Path;
        private void Locate_Icon_Click(object sender, RoutedEventArgs e)
        {
            try {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Png files (*.png)|*.png|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                Mod_Icon_Path = openFileDialog.FileName;
                if (!File.Exists(Mod_Icon_Path))
                {

                    Send_Error_Notif(GetTextResource("Not A Valid PNG Image!"));

                    return;

                }
                else
                {
                    if (Path.GetExtension(Mod_Icon_Path).Contains("png"))
                    {
                        int imgwidth;
                        int imgheight;

                        using (var image = SixLabors.ImageSharp.Image.Load(Mod_Icon_Path))
                        {
                            imgwidth = image.Width;
                            imgheight = image.Height;
                        }

                        if (imgwidth == 256 && imgheight == 256)
                        {

                            Send_Success_Notif("Valid Image Found at - " + Mod_Icon_Path);
                            BitmapImage Mod_Icon = new BitmapImage();
                            Mod_Icon.BeginInit();

                            Mod_Icon.UriSource = new Uri(Mod_Icon_Path);
                            Mod_Icon.EndInit();

                            Icon_Image.Source = Mod_Icon;

                        }
                        else
                        {
                            Send_Warning_Notif("Invalid Image Size!. Must be 256x256!");
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                            bitmap.EndInit();
                            Icon_Image.Source = bitmap;
                            return;

                        }

                    }
                    else
                    {
                        Send_Warning_Notif("That was not a proper PNG!");
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                        bitmap.EndInit();
                        Icon_Image.Source = bitmap;
                        return;
                    }
                }
            }
            }
            catch (Exception ef)
            {
                Write_To_Log(ErrorManager(ef));

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_CONTACT"));
            }
        }
        public string Current_Output_Dir;
        private void Output_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult res = dialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                Current_Output_Dir = dialog.SelectedPath;
                if (!Directory.Exists(Current_Output_Dir))
                {
                    Send_Error_Notif("Not An Output Directory!");
                    Output_Box.Background = Brushes.IndianRed;

                    return;

                }
                else
                {
                    Output_Box.Text = Current_Output_Dir;
                    Output_Box.Background = Brushes.White;

                }
            }
        }
        void create_Manifest(string Output_Folder)
        {
            try { 
            if (Mod_name.Text == null && Mod_name.Text == "" && Mod_version_number.Text == null && Mod_version_number.Text == "" && Mod_website_url.Text == null && Mod_website_url.Text == "" && Mod_description.Text == null && Mod_description.Text == "" && Mod_dependencies.Text == null && Mod_dependencies.Text == "")
            {
                Send_Warning_Notif("One of the Manifest Inputs are Empty!");
                return;

            }
            else
            {
                    string Output = "";
                    if(Mod_dependencies.Text != null || Mod_dependencies.Text != "" ){

                         Output = @"{
    ""name"": " + '\u0022' + Mod_name.Text.Trim() + '\u0022' + @",
    ""version_number"":" + '\u0022' + Mod_version_number.Text.Trim() + '\u0022' + @",
    ""website_url"": " + '\u0022' + Mod_website_url.Text.Trim() + '\u0022' + @",
    ""description"": " + '\u0022' + Mod_description.Text.Trim() + '\u0022' + @",
    ""dependencies"": [" + '\u0022' + Mod_dependencies.Text+ '\u0022' + "]" +
       "\n}";
                    }
                    else {
                         Output = @"{
    ""name"": " + '\u0022' + Mod_name.Text.Trim() + '\u0022' + @",
    ""version_number"":" + '\u0022' + Mod_version_number.Text.Trim() + '\u0022' + @",
    ""website_url"": " + '\u0022' + Mod_website_url.Text.Trim() + '\u0022' + @",
    ""description"": " + '\u0022' + Mod_description.Text.Trim() + '\u0022' + @",
    ""dependencies"":" + '\u0022' + "[" + '\u0022' + "northstar-Northstar-" + Properties.Settings.Default.Version.Remove(0, 1) + '\u0022' + "]" +
                "\n}";
                    }
                saveAsyncFile(Output, Output_Folder + "/" + "manifest.json", false, false);

            }
                //   MessageBox.Show(Properties.Settings.Default.Version.Remove(0,1));
            }
            catch (Exception ef)
            {
                Write_To_Log(ErrorManager(ef));

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_CONTACT"));
            }
        }
        private void Save_Mod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(Current_Mod_To_Pack))
                {
                    if (File.Exists(Mod_Icon_Path))
                    {
                        if (Directory.Exists(Current_Output_Dir))
                        {
                            FileInfo Dir = new FileInfo(Current_Mod_To_Pack);
                            Directory.CreateDirectory(Current_Output_Dir + "/" + Mod_name.Text.Trim());
                            if (Directory.Exists(Current_Output_Dir + "/" + Mod_name.Text.Trim()))
                            {
                                File.Copy(Mod_Icon_Path, Current_Output_Dir + "/" + Mod_name.Text.Trim() + "/" + "icon.png", true);
                                create_Manifest(Current_Output_Dir + "/" + Mod_name.Text.Trim());
                                TextRange Description = new TextRange(
                                // TextPointer to the start of content in the RichTextBox.
                                Description_Box.Document.ContentStart,
                                // TextPointer to the end of content in the RichTextBox.
                                Description_Box.Document.ContentEnd);
                                saveAsyncFile(Description.Text, Current_Output_Dir + "/" + Mod_name.Text.Trim() + "/" + "README.md", false, false);
                                if (Skin_Mod_Pack_Check.IsChecked == true)
                                {
                                    if (!Directory.Exists(Current_Output_Dir + "/" + Mod_name.Text.Trim() + "/" + "mods" + "/"))
                                    {
                                        Directory.CreateDirectory(Current_Output_Dir + "/" + Mod_name.Text.Trim() + "/" + "mods" + "/");
                                    }
                                    File.Copy(Current_Mod_To_Pack, Current_Output_Dir + "/" + Mod_name.Text.Trim() + "/" + "mods" + "/" + Dir.Name, true);


                                }
                                else
                                {
                                    if (!Directory.Exists(Current_Output_Dir + "/" + Mod_name.Text.Trim() + "/" + "mods" + "/"))
                                    {
                                        Directory.CreateDirectory(Current_Output_Dir + "/" + Mod_name.Text.Trim() + "/" + "mods" + "/");

                                    }
                                    ZipFile.ExtractToDirectory(Current_Mod_To_Pack, Current_Output_Dir + "/" + Mod_name.Text.Trim() + "/" + "mods" + "/", true);


                                }

                                if (File.Exists(Current_Output_Dir + "/" + Mod_name.Text.Trim() + ".zip"))
                                {
                                    File.Delete(Current_Output_Dir + "/" + Mod_name.Text.Trim() + ".zip");
                                    ZipFile.CreateFromDirectory(Current_Output_Dir + "/" + Mod_name.Text.Trim(), Current_Output_Dir + "/" + Mod_name.Text.Trim() + ".zip");

                                }
                                else
                                {
                                    ZipFile.CreateFromDirectory(Current_Output_Dir + "/" + Mod_name.Text.Trim(), Current_Output_Dir + "/" + Mod_name.Text.Trim() + ".zip");

                                }

                            }
                        }
                        else
                        {
                            Send_Warning_Notif("No Valid Output Path Found!");
                            Output_Box.Background = Brushes.IndianRed;

                            return;

                        }

                        if (File.Exists(Current_Output_Dir + "/" + Mod_name.Text.Trim() + ".zip"))
                        {
                            Directory.Delete(Current_Output_Dir + "/" + Mod_name.Text.Trim(), true);
                        }
                        Send_Success_Notif("Successfully Packed all items to -" + Current_Output_Dir + "/" + Mod_name.Text.Trim() + ".zip");
                    }
                    else
                    {
                        Send_Warning_Notif("No Valid Mod ICON Found!");
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(@"pack://application:,,,/Resources/NO_TEXTURE.png");
                        bitmap.EndInit();
                        Icon_Image.Source = bitmap;
                        return;
                    }


                }
                else
                {
                    Send_Warning_Notif("No Valid Mod Zip Found!");
                    Output_Box.Background = Brushes.IndianRed;

                    return;


                }
            }
            catch (Exception ef)
            {
                Write_To_Log(ErrorManager(ef));

                Send_Fatal_Notif(GetTextResource("NOTIF_FATAL_COMMON_CONTACT"));
            }
        }

        private void Skin_Mod_Pack_Check_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.PackageAsSkin = true;
            Properties.Settings.Default.Save();
        }

        private void Skin_Mod_Pack_Check_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.PackageAsSkin = false;
            Properties.Settings.Default.Save();
        }

        private void SearchBar_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBar.Text = "";
        }

        private void SearchBar_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchBar.Text = "Search";

        }
        private List<string> autoSuggestionList = new List<string>();

        public List<string> AutoSuggestionList
        {
            get { return this.autoSuggestionList; }
            set { this.autoSuggestionList = value; }
        }
        private void OpenAutoSuggestionBox()
        {
            try
            {
                autoSuggestionList.Add("ns_should_log_all_clientcommands");
                // Enable.  
                this.autoListPopup.Visibility = Visibility.Visible;
                this.autoListPopup.IsOpen = true;
                this.autoList.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                // Info.  
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Write(ex);
            }
        }
        private void CloseAutoSuggestionBox()
        {
            try
            {
                // Enable.  
                this.autoListPopup.Visibility = Visibility.Collapsed;
                this.autoListPopup.IsOpen = false;
                this.autoList.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                // Info.  
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Write(ex);
            }
        }
        private void AutoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Verification.  
                if (this.autoList.SelectedIndex <= -1)
                {
                    // Disable.  
                    this.CloseAutoSuggestionBox();

                    // Info.  
                    return;
                }

                // Disable.  
                this.CloseAutoSuggestionBox();

                // Settings.  
                this.Arg_Box.Text = this.Arg_Box.Text  + " "+ this.autoList.SelectedItem.ToString() + " ";
                this.autoList.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                // Info.  
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Write(ex);
            }
        }

    
    private void Arg_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                // Verification.  
                if (string.IsNullOrEmpty(this.Arg_Box.Text))
                {
                    // Disable.  
                //    this.CloseAutoSuggestionBox();

                    // Info.  
                    return;
                }

                // Enable.  
             //   this.OpenAutoSuggestionBox();

                // Settings.  
                //this.autoList.ItemsSource = this.AutoSuggestionList.Where(p => p.ToLower().Contains(this.Arg_Box.Text.ToLower())).ToList();
              //  this.autoList.ItemsSource = this.AutoSuggestionList;
            }
            catch (Exception ex)
            {
                // Info.  
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Write(ex);
            }
        }
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
        private void VTOL_Loaded(object sender, RoutedEventArgs e)
        {
           // Loaded_ = true;
            //this.Topmost = true;
        }

        private void VTOL_LayoutUpdated(object sender, EventArgs e)
        {


        }

        private void Banner_Image_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }

}


