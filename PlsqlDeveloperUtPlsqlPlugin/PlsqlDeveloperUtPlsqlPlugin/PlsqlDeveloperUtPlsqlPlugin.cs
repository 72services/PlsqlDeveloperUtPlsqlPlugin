using System;
using RGiesecke.DllExport;
using System.Runtime.InteropServices;

namespace PlsqlDeveloperUtPlsqlPlugin
{

    //TODO: declare delegates for PL/SQL Developer callbacks, for instance
    //delegate void IdeCreateWindow(int windowType, string text, [MarshalAs(UnmanagedType.Bool)] bool execute);

    public class PlsqlDeveloperUtPlsqlPlugin
    {
        private const string PLUGIN_NAME = "utPLSQL Plugin";
        private const int PLUGIN_MENU_INDEX = 72;

        private static PlsqlDeveloperUtPlsqlPlugin plugin;
        private int pluginId;

        //TODO: declare private delegate variable (not necessarilly static), for instance
        //private static IdeCreateWindow createWindowCallback;

        private PlsqlDeveloperUtPlsqlPlugin(int id)
        {
            pluginId = id;
        }

        #region DLL exported API
        [DllExport("IdentifyPlugIn", CallingConvention = CallingConvention.Cdecl)]
        public static string IdentifyPlugIn(int id)
        {
            if (plugin == null)
            {
                plugin = new PlsqlDeveloperUtPlsqlPlugin(id);
            }
            return PLUGIN_NAME;
        }

        [DllExport("RegisterCallback", CallingConvention = CallingConvention.Cdecl)]
        public static void RegisterCallback(int index, IntPtr function)
        {
            //TODO: register pointers to PL/SQL Developer callbacks you need, for instance
            //createWindowCallback = (IdeCreateWindow)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeCreateWindow));
        }

        [DllExport("OnMenuClick", CallingConvention = CallingConvention.Cdecl)]
        public static void OnMenuClick(int index)
        {
            if (index == PLUGIN_MENU_INDEX)
            {
                About about = new About();
                about.Show(plugin);
            }
        }

        [DllExport("CreateMenuItem", CallingConvention = CallingConvention.Cdecl)]
        public static string CreateMenuItem(int index)
        {
            if (index == PLUGIN_MENU_INDEX)
            {
                return "Tools / utPLSQL";
            }
            else
            {
                return "";
            }
        }

        [DllExport("About", CallingConvention = CallingConvention.Cdecl)]
        public static string About()
        {
            //TODO: create about dialog
            return "utPLSQL Plugin";
        }
        #endregion
    }
}
